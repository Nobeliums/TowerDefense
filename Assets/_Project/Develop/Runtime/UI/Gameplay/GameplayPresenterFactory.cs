using _Project.Develop.Runtime.Features.Gamecycle;
using _Project.Develop.Runtime.Gameplay.Features.StageFeature;
using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.UI.Core.Gameplay.Bars;
using _Project.Develop.Runtime.UI.Core.Gameplay.Stages;
using _Project.Develop.Runtime.UI.Core.Popups;
using _Project.Develop.Runtime.UI.View;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagment.DataProvider;
using _Project.Develop.Runtime.Utilities.SceneManagment;

namespace _Project.Develop.Runtime.UI.Core.Gameplay
{
	public class GameplayPresenterFactory
	{
		private DIContainer _container;

		public GameplayPresenterFactory(DIContainer container)
		{
			_container = container;
		}

		public GameplayPresenter CreateGameplayPresenter(GameplayView gameplayView)
		{
			return new GameplayPresenter(gameplayView, 
				this,
				_container.Resolve<GameplayCycle>(), 
				_container.Resolve<GameplayPopupService>(),
				_container.Resolve<WaveStartRequestService>(),
				_container.Resolve<GameResultHandler>());
		}

		public CurrentStageViewPresenter CreateCurrentStageViewPresenter(GameplayView view)
			=> new CurrentStageViewPresenter(view, _container.Resolve<StageProviderService>());

		public EntitiesHealthDisplayPresenter CreateEntitiesHealthDisplayPresenter(EntitiesHealthDisplay display)
		{
			return new EntitiesHealthDisplayPresenter(
				display,
				_container.Resolve<EntitiesLifeContext>(),
				_container.Resolve<ViewFactory>(),
				this);
		}

		public HealthBarPresenter CreateHealthBarPresenter(BarWithTextView view, Entity entity)
			=> new HealthBarPresenter(view, entity);

		public WinPopupPresenter CreateWinPopupPresenter(WinPopupView view)
		{
			return new WinPopupPresenter(
				view,
				_container.Resolve<ICoroutinePerformer>(),
				_container.Resolve<SceneSwitcherService>());
		}

		public DefeatPopupPresenter CreateDefeatPopupPresenter(DefeatPopupView view)
		{
			return new DefeatPopupPresenter(
				view,
				_container.Resolve<ICoroutinePerformer>(),
				_container.Resolve<SceneSwitcherService>(),
				_container.Resolve<GameplayInputArgs>());
		}
	}
}