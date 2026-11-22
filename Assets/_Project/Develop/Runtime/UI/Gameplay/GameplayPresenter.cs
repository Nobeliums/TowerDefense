using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Features.Gamecycle;
using _Project.Develop.Runtime.Gameplay.Features.StageFeature;
using _Project.Develop.Runtime.UI.Presenters;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Core.Gameplay
{
	public class GameplayPresenter : IPresenter
	{
		private readonly GameplayView _gameplayView;

		private readonly GameplayCycle _gameplayCycle;
		private readonly GameplayPopupService _gameplayPopupService;
		private readonly WaveStartRequestService _waveStartRequestService;

		private readonly List<IDisposable> _disposables = new();

		public GameplayPresenter(
			GameplayView gameplayView,
			GameplayCycle gameplayCycle,
			GameplayPopupService gameplayPopupService,
			WaveStartRequestService waveStartRequestService)
		{
			_gameplayView = gameplayView;
			_gameplayCycle = gameplayCycle;
			_gameplayPopupService = gameplayPopupService;
			_waveStartRequestService = waveStartRequestService;
		}

		public void Initialize()
		{
			_gameplayView.StartWaveClick += OnStartWaveClicked;

			_gameplayView.SetStartWaveButtonActive(_waveStartRequestService.IsNewWaveStartAvailable.Value);
			_disposables.Add(_waveStartRequestService.IsNewWaveStartAvailable.Subscribe(OnNewWaveStartAvailableChanged));
		}

		public void Dispose()
		{
			_gameplayView.StartWaveClick -= OnStartWaveClicked;

			foreach (IDisposable disposable in _disposables)
			{
				disposable.Dispose();
			}

			_disposables.Clear();
		}

		private void OnStartWaveClicked() => _waveStartRequestService.Request();

		private void OnNewWaveStartAvailableChanged(bool oldValue, bool isAvailable)
			=> _gameplayView.SetStartWaveButtonActive(isAvailable);
	}
}
