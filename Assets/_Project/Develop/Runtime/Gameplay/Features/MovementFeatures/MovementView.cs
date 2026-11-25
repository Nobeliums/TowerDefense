using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.MovementFeuture
{
	public class MovementView : EntityView
	{
		private readonly int IsMovingKey = Animator.StringToHash("IsWalking");

		[SerializeField] private Animator _animator;

		private ReactiveVariable<bool> _isMoving;
		private IDisposable _isMovingDisposable;

		private void OnValidate()
		{
			if (_animator == null)
				_animator = GetComponent<Animator>();
		}

		protected override void OnEntityStartedWork(Entity entity)
		{
			_isMoving = entity.IsMoving;

			_isMovingDisposable = _isMoving.Subscribe(OnIsMovingChanged);
			UpdateIsMovingKey(_isMoving.Value);
		}

		public override void Cleanup(Entity entity)
		{
			base.Cleanup(entity);

			_isMovingDisposable?.Dispose();
		}

		private void OnIsMovingChanged(bool oldValue, bool newValue)
			=> UpdateIsMovingKey(newValue);

		private void UpdateIsMovingKey(bool value)
			=> _animator.SetBool(IsMovingKey, value);
	}
}
