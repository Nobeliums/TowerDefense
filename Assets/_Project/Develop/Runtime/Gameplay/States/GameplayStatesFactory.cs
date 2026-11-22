using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Configs.Stages;
using _Project.Develop.Runtime.Gameplay.Features.Tower;
using _Project.Develop.Runtime.Gameplay.Features.StageFeature;
using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Features.Gamecycle;
using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Gameplay.Features.Explosion;
using _Project.Develop.Runtime.Gameplay.Features.GameplayInputHandlers;
using _Project.Develop.Runtime.Gameplay.Features.Mines;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Meta.Features.LevelsProgression;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.ConfigManagement;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagment.DataProvider;
using _Project.Develop.Runtime.Utilities.Input;
using _Project.Develop.Runtime.Utilities.SceneManagment;

namespace _Project.Develop.Runtime.Gameplay.States
{
	public class GameplayStatesFactory
	{
		private readonly DIContainer _container;

		public GameplayStatesFactory(DIContainer container)
		{
			_container = container;
		}

		public PreparationState CreatePreparationState(IInputHandler inputHandler)
		{
			return new PreparationState(_container.Resolve<WaveStartRequestService>(), inputHandler);
		}

		public StageProcessState CreateStageProcessState(IInputHandler inputHandler)
		{
			return new StageProcessState(_container.Resolve<StageProviderService>(), inputHandler);
		}

		public WinState CreateWinState()
		{
			return new WinState(
				_container.Resolve<IInputService>(),
				_container.Resolve<SceneSwitcherService>(),
				_container.Resolve<ICoroutinePerformer>());
		}

		public GameplayStateMachine CreateCoreLoopState()
		{
			GameplayStateMachine stateMachine = new GameplayStateMachine();

			PlayerConfig playerConfig = _container.Resolve<ConfigService>().GetConfig<PlayerConfig>();

			IInputHandler playerExplosionInputHandler = new PlayerExplosionInputHandler(
				_container.Resolve<ExplosionService>(),
				_container.Resolve<IInputService>(),
				playerConfig.ExplosionConfig);

			IInputHandler minePlacementInputHandler = new MinePlacementInputHandler(
				_container.Resolve<MinesFactory>(),
				_container.Resolve<WalletService>(),
				_container.Resolve<IInputService>(),
				playerConfig.MineConfig);

			PreparationState preparationState = CreatePreparationState(minePlacementInputHandler);
			StageProcessState stageProcessState = CreateStageProcessState(playerExplosionInputHandler);

			stateMachine.AddState(preparationState);
			stateMachine.AddState(stageProcessState);

			WaveStartRequestService waveStartRequestService = _container.Resolve<WaveStartRequestService>();
			StageProviderService stageProviderService = _container.Resolve<StageProviderService>();

			ICompositeCondition toStageProcessCondition = new CompositeCondition();
			toStageProcessCondition.AddCondition(new FuncCondition(() => waveStartRequestService.IsRequested));
			toStageProcessCondition.AddCondition(new FuncCondition(() => stageProviderService.HasNextStage));

			ICondition toPreparationCondition = new FuncCondition(() => stageProviderService.CurrentStageResult.Value == StageResults.Completed);
			
			stateMachine.AddTransition(preparationState, stageProcessState, toStageProcessCondition);
			stateMachine.AddTransition(stageProcessState, preparationState, toPreparationCondition);

			return stateMachine;
		}

		public DefeatState CreateDefeatState()
		{
			return new DefeatState(
				_container.Resolve<IInputService>(),
				_container.Resolve<SceneSwitcherService>(),
				_container.Resolve<ICoroutinePerformer>());
		}

		public GameplayStateMachine CreateGameplayStateMachine(GameplayInputArgs args)
		{
			List<IDisposable> disposables = new List<IDisposable>();

			GameplayStateMachine stateMachine = new GameplayStateMachine(disposables);

			GameplayStateMachine coreLoopState = CreateCoreLoopState();
			WinState winState = CreateWinState();
			DefeatState defeatState = CreateDefeatState();

			GameResultHandler gameResultHandler = _container.Resolve<GameResultHandler>();

			disposables.Add(winState.Entered.Subscribe(gameResultHandler.OnWin));
			disposables.Add(defeatState.Entered.Subscribe(gameResultHandler.OnDefeat));

			stateMachine.AddState(coreLoopState);
			stateMachine.AddState(winState);
			stateMachine.AddState(defeatState);

			TowerHolderService towerHolderService = _container.Resolve<TowerHolderService>();

			ICondition toDefeatCondition = new FuncCondition(() =>
				towerHolderService.Tower != null && towerHolderService.Tower.IsDead.Value);

			stateMachine.AddTransition(coreLoopState, defeatState, toDefeatCondition);

			StageProviderService stageProviderService = _container.Resolve<StageProviderService>();

			ICompositeCondition toWinCondition = new CompositeCondition();
			toWinCondition.AddCondition(new FuncCondition(() => stageProviderService.CurrentStageResult.Value == StageResults.Completed));
			toWinCondition.AddCondition(new FuncCondition(() => stageProviderService.HasNextStage == false));

			stateMachine.AddTransition(coreLoopState, winState, toWinCondition);

			return stateMachine;
		}
	}
}
