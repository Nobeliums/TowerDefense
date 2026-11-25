using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.SceneManagment;

namespace _Project.Develop.Runtime.UI.Core.Popups
{
	public class WinPopupPresenter : PopupBasePresenter
	{
		private readonly WinPopupView _view;
		private readonly SceneSwitcherService _sceneSwitcher;

		public WinPopupPresenter(
			WinPopupView view,
			ICoroutinePerformer coroutinePerformer,
			SceneSwitcherService sceneSwitcher) : base(coroutinePerformer)
		{
			_view = view;
			_sceneSwitcher = sceneSwitcher;
		}

		protected override PopupBaseView View => _view;

		public override void Initialize()
		{
			base.Initialize();

			_view.GameEndButton.ClickEvent += OnGameEndClicked;
		}

		public override void Dispose()
		{
			base.Dispose();

			_view.GameEndButton.ClickEvent -= OnGameEndClicked;
		}

		private void OnGameEndClicked()
			=> _coroutinePerformer.StartPerform(_sceneSwitcher.ProcessSceneSwitchTo(Scenes.MainMenu));
	}
}
