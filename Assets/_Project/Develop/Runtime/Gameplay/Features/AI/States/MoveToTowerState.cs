using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.AI.States
{
	public class MoveToTowerState : State, IUpdatebleState
	{
		private Transform _towerTransform;

		private Entity _sourceEntity;

		public MoveToTowerState(Transform towerTransform, Entity sourceEntity)
		{
			_towerTransform = towerTransform;
			_sourceEntity = sourceEntity;
		}

		public override void Enter()
		{
			base.Enter();

			_sourceEntity.MoveDirection.Value = GetDirectionToTower();
			_sourceEntity.RotateDirection.Value = GetDirectionToTower();
		}

		public void Update(float deltaTime)
		{
		}

		public override void Exit()
		{
			base.Exit();
			
			_sourceEntity.MoveDirection.Value = Vector3.zero;
		}

		private Vector3 GetDirectionToTower()
			=> (_towerTransform.position - _sourceEntity.TransformComponent.position).normalized;
	}
}