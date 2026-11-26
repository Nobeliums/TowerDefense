using System.Collections.Generic;
using _Project.Develop.Runtime.UI.View;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Develop.Runtime.UI.Core.Popups
{
	public class WinPopupView : PopupBaseView
	{
		[SerializeField] private TextBoxView _gameResult;
		[SerializeField] private ButtonView _gameEndButton;
		[SerializeField] private List<RectTransform> _stars;

		[Header("Stars animation")]
		[SerializeField] private float _starsStartDelay = 0.15f;
		[SerializeField] private float _starInterval = 0.2f;
		[SerializeField] private float _starFlyDuration = 0.45f;
		[SerializeField] private float _starFlyOffset = 250f;
		[SerializeField] private float _starStartRotation = -180f;

		private Sequence _starsSequence;

		public ButtonView GameEndButton => _gameEndButton;

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

				star.localScale = Vector3.zero;
				star.localEulerAngles = new Vector3(0f, 0f, _starStartRotation);
				star.anchoredPosition = targetPosition + Vector2.up * _starFlyOffset;

				float startTime = _starsStartDelay + i * _starInterval;

				_starsSequence.Insert(startTime, star.DOAnchorPos(targetPosition, _starFlyDuration).SetEase(Ease.OutBack));
				_starsSequence.Insert(startTime, star.DOScale(1f, _starFlyDuration).SetEase(Ease.OutBack));
				_starsSequence.Insert(startTime, star.DOLocalRotate(Vector3.zero, _starFlyDuration).SetEase(Ease.OutCubic));
				_starsSequence.Insert(startTime + _starFlyDuration, star.DOPunchScale(Vector3.one * 0.2f, 0.25f, 6, 0.5f));
			}
		}
	}
}
