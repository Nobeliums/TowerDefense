using System;
using _Project.Develop.Runtime.UI.Core.Gameplay.Bars;
using _Project.Develop.Runtime.UI.View;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Develop.Runtime.UI.Core.Gameplay
{
	public class GameplayView : MonoBehaviour, IView
	{
		public event Action StartWaveClick;

		[SerializeField] private Button _startWaveButton;
		[SerializeField] private EntitiesHealthDisplay _healthText;
		[SerializeField] private IconTextView _currentStageView;
		[SerializeField] private IconTextView _goldView;

		public EntitiesHealthDisplay HealthDisplay => _healthText;
		public IconTextView GoldView => _goldView;

		private void Awake() => _startWaveButton.onClick.AddListener(OnStartWaveButtonClicked);

		private void OnDestroy() => _startWaveButton.onClick.RemoveListener(OnStartWaveButtonClicked);

		public void SetStartWaveButtonActive(bool isActive) => _startWaveButton.gameObject.SetActive(isActive);

		public void SetCurrentWave(string waveInfo) => _currentStageView.SetText(waveInfo);

		private void OnStartWaveButtonClicked() => StartWaveClick?.Invoke();
	}
}
