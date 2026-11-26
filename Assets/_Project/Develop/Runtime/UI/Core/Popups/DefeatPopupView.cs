using System.Collections.Generic;
using _Project.Develop.Runtime.UI.View;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Develop.Runtime.UI.Core.Popups
{
	public class DefeatPopupView : PopupBaseView
	{
		[SerializeField] private TextBoxView _gameResult;
		[SerializeField] private ButtonView _mainMenuButton;
		[SerializeField] private ButtonView _restartGameButton;
		[SerializeField] private List<RectTransform> _stars;

		[Header("Stars animation")]
		[SerializeField] private float _starsStartDelay = 0.2f;
		[SerializeField] private float _starInterval = 0.25f;
		[SerializeField] private float _starFallDuration = 0.6f;
		[SerializeField] private float _starFallHeight = 300f;
		[SerializeField] private float _starTiltAngle = 15f;
		[SerializeField] private float _starShakeStrength = 12f;
		[SerializeField] private float _starShakeDuration = 0.3f;

		private Sequence _starsSequence;

		public ButtonView MainMenuButton => _mainMenuButton;
		public ButtonView RestartGameButton => _restartGameButton;

		protected override void OnPostShow()
		{
			base.OnPostShow();

			PlayStarsAnimation();
		}

		protected override void OnPreHide()
		{
			base.OnPreHide();

			_starsSequence?.Kill(true);
		}

		private void OnDestroy() => _starsSequence?.Kill();

		private void PlayStarsAnimation()
		{
			_starsSequence?.Kill(true);
			_starsSequence = DOTween.Sequence().SetLink(gameObject);

			for (int i = 0; i < _stars.Count; i++)
			{
				RectTransform star = _stars[i];
				Vector2 targetPosition = star.anchoredPosition;
				float tilt = i % 2 == 0 ? _starTiltAngle : -_starTiltAngle;

				star.localScale = Vector3.one;
				star.localEulerAngles = Vector3.zero;
				star.anchoredPosition = targetPosition + Vector2.up * _starFallHeight;

				float startTime = _starsStartDelay + i * _starInterval;
				float landTime = startTime + _starFallDuration;

				_starsSequence.Insert(startTime, star.DOAnchorPos(targetPosition, _starFallDuration).SetEase(Ease.OutBounce));
				_starsSequence.Insert(startTime, star.DOLocalRotate(new Vector3(0f, 0f, tilt), _starFallDuration).SetEase(Ease.OutQuad));
				_starsSequence.Insert(landTime, star.DOShakeAnchorPos(_starShakeDuration, _starShakeStrength, 20, 90f, false, true));
			}
		}
	}
}
