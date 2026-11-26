using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.SceneManagment;

namespace _Project.Develop.Runtime.UI.Core.Popups
{
	public class DefeatPopupPresenter : PopupBasePresenter
	{
		private readonly DefeatPopupView _view;
		private readonly SceneSwitcherService _sceneSwitcher;
		private readonly GameplayInputArgs _gameplayArgs;

		private bool _isSwitching;

		public DefeatPopupPresenter(
			DefeatPopupView view,
			ICoroutinePerformer coroutinePerformer,
			SceneSwitcherService sceneSwitcher,
			GameplayInputArgs gameplayArgs) : base(coroutinePerformer)
		{
			_view = view;
			_sceneSwitcher = sceneSwitcher;
			_gameplayArgs = gameplayArgs;
		}

		protected override PopupBaseView View => _view;

		public override void Initialize()
		{
			base.Initialize();

			_view.MainMenuButton.ClickEvent += OnMainMenuClicked;
			_view.RestartGameButton.ClickEvent += OnRestartClicked;
		}

		public override void Dispose()
		{
			base.Dispose();

			_view.MainMenuButton.ClickEvent -= OnMainMenuClicked;
			_view.RestartGameButton.ClickEvent -= OnRestartClicked;
		}

		private void OnMainMenuClicked() => SwitchTo(Scenes.MainMenu);

		private void OnRestartClicked()
			=> SwitchTo(Scenes.Gameplay, new GameplayInputArgs(_gameplayArgs.LevelNumber));

		private void SwitchTo(string sceneName, IInputSceneArgs args = null)
		{
			if (_isSwitching)
				return;

			_isSwitching = true;
			_coroutinePerformer.StartPerform(_sceneSwitcher.ProcessSceneSwitchTo(sceneName, args));
		}
	}
}
