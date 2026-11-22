using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Common;
using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.AI.States;
using _Project.Develop.Runtime.Gameplay.Features.Attack;
using _Project.Develop.Runtime.Gameplay.Features.Tower;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.ConfigManagement;
using _Project.Develop.Runtime.Utilities.Reactive;
using _Project.Develop.Runtime.Utilities.TimerService;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.AI
{
	public class BrainsFactory
	{
		private readonly DIContainer _container;
		private readonly TimerServiceFactory _timerServiceFactory;
		private readonly AIBrainsContext _aiBrainsContext;
		private readonly EntitiesLifeContext _entitiesLifeContext;
		private readonly TowerHolderService _towerHolderService;

		public BrainsFactory(DIContainer container)
		{
			_container = container;
			
			_timerServiceFactory = _container.Resolve<TimerServiceFactory>();
			_aiBrainsContext = _container.Resolve<AIBrainsContext>();
			_entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
			_towerHolderService = container.Resolve<TowerHolderService>();
		}

		public StateMachineBrain CreateGhostBrain(Entity entity)
		{
			AIStateMachine stateMachine = CreateRandomMovementStateMachine(entity);

			StateMachineBrain brain = new StateMachineBrain(stateMachine);
			
			_aiBrainsContext.SetFor(entity, brain);
			
			return brain;
		}

		public StateMachineBrain CreateGhostTeleportToTargetBrain(Entity entity, ITargetSelector targetSelector)
		{
			IPointSelector pointSelector = new ToTargetDirectionPointSelector(entity.CurrentTarget, entity, entity.TeleportRadius);

			ReactiveVariable<float> currentEnergy = entity.CurrentEnergy;
			ReactiveVariable<float> maxEnergy = entity.MaxEnergy;
			ReactiveVariable<float> percentOfEnergyToTeleport = entity.MinPercentOfEnergyToTeleport;
			
			ICompositeCondition additionalToTeleportStateCondition = new  CompositeCondition()
				.AddCondition(new FuncCondition(() => currentEnergy.Value / maxEnergy.Value > percentOfEnergyToTeleport.Value / 100f))
				.AddCondition(new FuncCondition(() => entity.CurrentTarget.Value != null));

			AIStateMachine stateMachine = CreateTeleportStateMachine(entity, pointSelector, additionalToTeleportStateCondition);
			
			FindTargetState findTargetState = new FindTargetState(targetSelector, _entitiesLifeContext, entity);
			AIParallelState  parallelState = new AIParallelState(findTargetState, stateMachine);
			
			AIStateMachine rootStateMachine = new AIStateMachine();
			
			rootStateMachine.AddState(parallelState);
			
			StateMachineBrain brain = new StateMachineBrain(rootStateMachine);
			
			_aiBrainsContext.SetFor(entity, brain);

			return brain;
		}

		public StateMachineBrain CreateMineBrain(Entity entity)
		{
			FindTargetState findTargetState = new FindTargetState(
				new NearestDamageableTargetSelector(entity),
				_entitiesLifeContext,
				entity);

			AIStateMachine stateMachine = new AIStateMachine();

			stateMachine.AddState(findTargetState);

			StateMachineBrain brain = new StateMachineBrain(stateMachine);

			_aiBrainsContext.SetFor(entity, brain);

			return brain;
		}

		public StateMachineBrain CreateMoveToTowerBrain(Entity entity)
		{
			AIStateMachine moveStateMachine =  CreateMoveToTowerStateMachine(entity);
			
			StateMachineBrain brain = new StateMachineBrain(moveStateMachine);

			entity.CurrentTarget.Value = _towerHolderService.Tower;
			
			_aiBrainsContext.SetFor(entity, brain);

			return brain;
		}

		private AIStateMachine CreateMoveToTowerStateMachine(Entity entity)
		{
			List<IDisposable> disposables = new();

			MoveToTowerState movementState = new MoveToTowerState(_towerHolderService.Tower.TransformComponent, entity);

			EmptyState emptyState = new EmptyState();

			FuncCondition fromEmptyToMovementState =
				new FuncCondition(() => _towerHolderService.Tower.IsDead.Value == false);
			
			FuncCondition fromMovementToEmptyState = new FuncCondition(() => _towerHolderService.Tower.IsDead.Value);
			
			AIStateMachine stateMachine = new AIStateMachine();
			
			stateMachine.AddState(emptyState);
			stateMachine.AddState(movementState);
			
			stateMachine.AddTransition(emptyState, movementState, fromEmptyToMovementState);
			stateMachine.AddTransition(movementState, emptyState,  fromMovementToEmptyState);

			return stateMachine;
		}

		private AIStateMachine CreateTeleportStateMachine(Entity entity, IPointSelector pointSelector, ICompositeCondition additionalCondition = null)
		{
			List<IDisposable> disposables = new List<IDisposable>();
			
			EmptyState cooldownState = new EmptyState();

			TeleportState teleportState = new TeleportState(
				entity.InTeleportCooldown,
				entity.TeleportRequest,
				pointSelector,
				entity.TeleportEnergyCost,
				entity.SpendEnergyRequest);
			
			TimerService teleportCooldownTimer = _timerServiceFactory.Create(entity.TeleportCooldownInitialTime.Value);

			disposables.Add(teleportState.Entered.Subscribe(teleportCooldownTimer.Restart));
			disposables.Add(teleportCooldownTimer.TimerEnded.Subscribe(() => entity.InTeleportCooldown.Value = false));

			ICompositeCondition fromCooldownStateToTeleport = new CompositeCondition()
				.AddCondition(new FuncCondition(() => teleportCooldownTimer.IsOver))
				.AddCondition(entity.CanTeleport);

			ICompositeCondition fromTeleportStateToCooldown = new CompositeCondition()
				.AddCondition(new FuncCondition(() => entity.CanTeleport.Evaluate() == false));

			if (additionalCondition != null)
				fromCooldownStateToTeleport.AddCondition(additionalCondition);
			
			AIStateMachine behaviour = new AIStateMachine(disposables);
			
			behaviour.AddState(cooldownState);
			behaviour.AddState(teleportState);
			
			behaviour.AddTransition(teleportState, cooldownState, fromTeleportStateToCooldown);
			behaviour.AddTransition(cooldownState, teleportState, fromCooldownStateToTeleport);

			return behaviour;
		}

		private AIStateMachine CreateRandomMovementStateMachine(Entity entity)
		{
			List<IDisposable> disposables = new();
			
			RandomMovementState movementState = new RandomMovementState(
				entity,
				0.5f,
				30f);

			EmptyState emptyState = new EmptyState();

			TimerService movementTimer = _timerServiceFactory.Create(2f);
			disposables.Add(movementTimer);
			disposables.Add(movementState.Entered.Subscribe(movementTimer.Restart));
			
			TimerService idleTimer = _timerServiceFactory.Create(2f);
			disposables.Add(idleTimer);
			disposables.Add(emptyState.Entered.Subscribe(idleTimer.Restart));

			FuncCondition movementTimerEndCondition = new FuncCondition(() => movementTimer.IsOver);
			FuncCondition idleTimerEndCondition = new FuncCondition(() => idleTimer.IsOver);

			AIStateMachine stateMachine = new AIStateMachine(disposables);
			
			stateMachine.AddState(emptyState);
			stateMachine.AddState(movementState);
			
			stateMachine.AddTransition(movementState, emptyState, movementTimerEndCondition);
			stateMachine.AddTransition(emptyState, movementState, idleTimerEndCondition);
			
			return stateMachine;
		}
		
		private AIStateMachine CreateAutoAttackStateMachine(Entity entity)
		{
			RotateToTargetState rotateToTargetState = new RotateToTargetState(entity);
			AttackTriggerState attackTriggerState = new AttackTriggerState(entity);

			ICondition canAttack = entity.GetComponent<CanStartAttack>().Value;
			Transform transform = entity.GetComponent<TransformComponent>().Value;
			ReactiveVariable<Entity> currentTarget = entity.GetComponent<CurrentTarget>().Value;

			ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
				.AddCondition(canAttack)
				.AddCondition(new FuncCondition(() =>
				{
					Entity target =  currentTarget.Value;

					if (target == null)
						return false;
					
					float angleToTarget = Quaternion.Angle(
						transform.rotation,
						Quaternion.LookRotation(
							target.GetComponent<TransformComponent>().Value.position -  transform.position));

					return angleToTarget <= 3f;
				}));
			
			ReactiveVariable<bool> inAttackProcess = entity.GetComponent<InAttackProcess>().Value;

			ICondition fromAttackToRotateStateCondition = new FuncCondition(() => inAttackProcess.Value == false);

			AIStateMachine stateMachine = new AIStateMachine();
			
			stateMachine.AddState(rotateToTargetState);
			stateMachine.AddState(attackTriggerState);
			
			stateMachine.AddTransition(rotateToTargetState, attackTriggerState, fromRotateToAttackCondition);
			stateMachine.AddTransition(attackTriggerState, rotateToTargetState, fromAttackToRotateStateCondition);
			
			return stateMachine;
		}
	}
}