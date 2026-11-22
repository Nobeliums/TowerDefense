using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Meta.Features.LevelSelection;
using _Project.Develop.Runtime.UI.Presenters;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.SceneManagment;

namespace _Project.Develop.Runtime.UI.Core.MainMenu
{
	public class MainMenuPresenter : IPresenter
	{
		private MainMenuView _mainMenuView;

		private ProjectPresenterFactory _factory;
		private MainMenuPopupService _popupService;
		private RandomLevelService _randomLevelService;
		private SceneSwitcherService _sceneSwitcherService;
		private ICoroutinePerformer _coroutinePerformer;

		private List<IPresenter> _children = new();

		private bool _isGameStarting;

		public MainMenuPresenter(
			MainMenuView mainMenuView,
			ProjectPresenterFactory factory,
			MainMenuPopupService popupService,
			RandomLevelService randomLevelService,
			SceneSwitcherService sceneSwitcherService,
			ICoroutinePerformer coroutinePerformer)
		{
			_mainMenuView = mainMenuView;
			_factory = factory;
			_popupService = popupService;
			_randomLevelService = randomLevelService;
			_sceneSwitcherService = sceneSwitcherService;
			_coroutinePerformer = coroutinePerformer;
		}

		public void Initialize()
		{
			CreateHorizontalGameStatisticsView();

			_mainMenuView.StartGameClick += StartRandomLevel;
			_mainMenuView.ShopOpenButtonClick += OpenShopPopup;

			foreach(var child in _children)
				child.Initialize();
		}

		public void Dispose()
		{
			_mainMenuView.StartGameClick -= StartRandomLevel;
			_mainMenuView.ShopOpenButtonClick -= OpenShopPopup;

			foreach(var child in _children)
				child.Dispose();

			_children.Clear();
		}

		private void CreateHorizontalGameStatisticsView()
		{
			HorizontalGameStatisticsViewPresenter gameStatisticsPresenter = _factory
				.CreateHorizontalGameStatisticsViewPresenter(_mainMenuView.WalletView, _mainMenuView.GameStatisticsView);

			_children.Add(gameStatisticsPresenter);
		}

		private void StartRandomLevel()
		{
			if (_isGameStarting)
				return;

			_isGameStarting = true;

			GameplayInputArgs args = new GameplayInputArgs(_randomLevelService.GetRandomLevelNumber());

			_coroutinePerformer.StartPerform(_sceneSwitcherService.ProcessSceneSwitchTo(Scenes.Gameplay, args));
		}

		private void OpenShopPopup()
		{
			_popupService.CreateShopPopup();
		}
	}
}
