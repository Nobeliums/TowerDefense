using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Features.Gamecycle;
using _Project.Develop.Runtime.Gameplay.Features.StageFeature;
using _Project.Develop.Runtime.UI.Core.Gameplay.Bars;
using _Project.Develop.Runtime.UI.Core.Gameplay.Stages;
using _Project.Develop.Runtime.UI.Presenters;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Core.Gameplay
{
	public class GameplayPresenter : IPresenter
	{
		private readonly GameplayView _gameplayView;
		private readonly GameplayPresenterFactory _factory;

		private readonly GameplayCycle _gameplayCycle;
		private readonly GameplayPopupService _gameplayPopupService;
		private readonly WaveStartRequestService _waveStartRequestService;
		private readonly GameResultHandler _gameResultHandler;

		private readonly List<IDisposable> _disposables = new();
		private readonly List<IPresenter> _children = new();

		private EntitiesHealthDisplayPresenter _entitiesHealthDisplayPresenter;

		public GameplayPresenter(
			GameplayView gameplayView,
			GameplayPresenterFactory factory,
			GameplayCycle gameplayCycle,
			GameplayPopupService gameplayPopupService,
			WaveStartRequestService waveStartRequestService,
			GameResultHandler gameResultHandler)
		{
			_gameplayView = gameplayView;
			_factory = factory;
			_gameplayCycle = gameplayCycle;
			_gameplayPopupService = gameplayPopupService;
			_waveStartRequestService = waveStartRequestService;
			_gameResultHandler = gameResultHandler;
		}

		public void Initialize()
		{
			_gameplayView.StartWaveClick += OnStartWaveClicked;
			_gameResultHandler.Won += OnWon;
			_gameResultHandler.Defeated += OnDefeated;

			_gameplayView.SetStartWaveButtonActive(_waveStartRequestService.IsNewWaveStartAvailable.Value);
			_disposables.Add(_waveStartRequestService.IsNewWaveStartAvailable.Subscribe(OnNewWaveStartAvailableChanged));

			CreateCurrentStageViewPresenter();
			CreateEntitiesHealthDisplayPresenter();

			foreach (IPresenter child in _children)
				child.Initialize();
		}

		public void LateUpdate()
		{
			_entitiesHealthDisplayPresenter?.LateUpdate();
		}

		public void Dispose()
		{
			_gameplayView.StartWaveClick -= OnStartWaveClicked;
			_gameResultHandler.Won -= OnWon;
			_gameResultHandler.Defeated -= OnDefeated;

			foreach (IDisposable disposable in _disposables)
			{
				disposable.Dispose();
			}

			_disposables.Clear();

			foreach (IPresenter child in _children)
				child.Dispose();

			_children.Clear();
		}

		private void CreateCurrentStageViewPresenter()
		{
			CurrentStageViewPresenter currentStagePresenter = _factory
				.CreateCurrentStageViewPresenter(_gameplayView);

			_children.Add(currentStagePresenter);
		}

		private void CreateEntitiesHealthDisplayPresenter()
		{
			_entitiesHealthDisplayPresenter = _factory
				.CreateEntitiesHealthDisplayPresenter(_gameplayView.HealthDisplay);

			_children.Add(_entitiesHealthDisplayPresenter);
		}

		private void OnWon() => _gameplayPopupService.CreateWinPopup();

		private void OnDefeated() => _gameplayPopupService.CreateDefeatPopup();

		private void OnStartWaveClicked() => _waveStartRequestService.Request();

		private void OnNewWaveStartAvailableChanged(bool oldValue, bool isAvailable)
			=> _gameplayView.SetStartWaveButtonActive(isAvailable);
	}
}
