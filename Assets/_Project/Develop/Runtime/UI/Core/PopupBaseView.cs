using System;
using _Project.Develop.Runtime.UI.View;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Develop.Runtime.UI.Core
{
	public class PopupBaseView : MonoBehaviour, IShowableView
	{
		public event Action CloseRequest;
		
		[SerializeField] private CanvasGroup _group;
		[SerializeField] private Button _closeButton;

		[Header("Animation")]
		[SerializeField] private RectTransform _content;
		[SerializeField] private float _showDuration = 0.25f;
		[SerializeField] private float _hideDuration = 0.15f;
		[SerializeField] private float _hiddenScale = 0.85f;
		
		private Sequence _animationSequence;

		private Transform Content => _content != null ? _content : transform;

		private void Awake()
		{
			_group.alpha = 0.0f;
		}

		public Tween Show()
		{
			OnPreShow();

			_animationSequence?.Kill();

			_group.alpha = 0.0f;
			Content.localScale = Vector3.one * _hiddenScale;

			_animationSequence = DOTween.Sequence()
				.Join(_group.DOFade(1.0f, _showDuration).SetEase(Ease.OutQuad))
				.Join(Content.DOScale(1.0f, _showDuration).SetEase(Ease.OutBack))
				.SetLink(gameObject);

			OnPostShow();

			return _animationSequence;
		}
		
		public Tween Hide()
		{
			OnPreHide();

			_animationSequence?.Kill();

			_animationSequence = DOTween.Sequence()
				.Join(_group.DOFade(0.0f, _hideDuration).SetEase(Ease.InQuad))
				.Join(Content.DOScale(_hiddenScale, _hideDuration).SetEase(Ease.InBack))
				.SetLink(gameObject);
	
			OnPostHide();
			
			return _animationSequence;
		}

		public void OnCloseButtonClick() => CloseRequest?.Invoke();

		protected virtual void OnPreShow()
		{
			
		}

		protected virtual void OnPostShow()
		{
			
		}

		protected virtual void OnPreHide()
		{
			
		}

		protected virtual void OnPostHide()
		{
			
		}
	}
}