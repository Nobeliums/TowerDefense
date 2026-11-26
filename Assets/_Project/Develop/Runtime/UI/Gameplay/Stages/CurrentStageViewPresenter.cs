using System;
using _Project.Develop.Runtime.Gameplay.Features.StageFeature;
using _Project.Develop.Runtime.UI.Presenters;

namespace _Project.Develop.Runtime.UI.Core.Gameplay.Stages
{
	public class CurrentStageViewPresenter : IPresenter
	{
		private const string StageFormat = "Stage: {0}/{1}";

		private readonly GameplayView _view;
		private readonly StageProviderService _stageProviderService;

		private IDisposable _currentStageNumberDisposable;

		public CurrentStageViewPresenter(GameplayView view, StageProviderService stageProviderService)
		{
			_view = view;
			_stageProviderService = stageProviderService;
		}

		public void Initialize()
		{
			_currentStageNumberDisposable = _stageProviderService.CurrentStageNumber.Subscribe(OnCurrentStageNumberChanged);
			UpdateStageText(_stageProviderService.CurrentStageNumber.Value);
		}

		public void Dispose()
		{
			_currentStageNumberDisposable?.Dispose();
		}

		private void OnCurrentStageNumberChanged(int oldValue, int newValue)
			=> UpdateStageText(newValue);

		private void UpdateStageText(int stageNumber)
			=> _view.SetCurrentWave(string.Format(StageFormat, stageNumber, _stageProviderService.StageCount));
	}
}
