using System;
using _Project.Develop.Runtime.UI.View;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Develop.Runtime.UI.Core.Gameplay
{
	public class GameplayView : MonoBehaviour, IView
	{
		public event Action StartWaveClick;

		[SerializeField] private TextBoxView _targetPhrase;
		[SerializeField] private TextBoxView _playerPhrase;
		[SerializeField] private Button _startWaveButton;

		private void Awake() => _startWaveButton.onClick.AddListener(OnStartWaveButtonClicked);

		private void OnDestroy() => _startWaveButton.onClick.RemoveListener(OnStartWaveButtonClicked);

		public void SetTargetPhrase(string targetPhrase) =>  _targetPhrase.SetText(targetPhrase);
		public void SetPlayerPhrase(string playerPhrase) =>  _playerPhrase.SetText(playerPhrase);

		public void SetStartWaveButtonActive(bool isActive) => _startWaveButton.gameObject.SetActive(isActive);

		private void OnStartWaveButtonClicked() => StartWaveClick?.Invoke();
	}
}
