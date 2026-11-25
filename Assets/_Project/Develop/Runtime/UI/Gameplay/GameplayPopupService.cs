using _Project.Develop.Runtime.UI.Core.Popups;
using _Project.Develop.Runtime.UI.Presenters;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Core.Gameplay
{
	public class GameplayPopupService : PopupService
	{
		private GameplayCanvasRoot _gameplayCanvasRoot;
		private GameplayPresenterFactory _gameplayPresenterFactory;
		
		public GameplayPopupService(
			ProjectPresenterFactory projectPresenterFactory,
			ViewFactory viewFactory,
			GameplayCanvasRoot gameplayCanvasRoot,
			GameplayPresenterFactory gameplayPresenterFactory) : base(projectPresenterFactory, viewFactory)
		{
			_gameplayCanvasRoot = gameplayCanvasRoot;
			_gameplayPresenterFactory = gameplayPresenterFactory;
		}

		protected override Transform PopupLayer => _gameplayCanvasRoot.Popups;

		public WinPopupPresenter CreateWinPopup()
		{
			WinPopupView view = _viewFactory.Create<WinPopupView>(ViewIds.WinResultPopupView, PopupLayer);

			WinPopupPresenter presenter = _gameplayPresenterFactory.CreateWinPopupPresenter(view);
			
			OnPopupCreated(presenter, view);
			
			return presenter;
		}

		public DefeatPopupPresenter CreateDefeatPopup()
		{
			DefeatPopupView view = _viewFactory.Create<DefeatPopupView>(ViewIds.DefeatResultPopupView, PopupLayer);

			DefeatPopupPresenter presenter = _gameplayPresenterFactory.CreateDefeatPopupPresenter(view);

			OnPopupCreated(presenter, view);

			return presenter;
		}
	}
}