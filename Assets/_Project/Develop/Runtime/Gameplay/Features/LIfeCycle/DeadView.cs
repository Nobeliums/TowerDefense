using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.LIfeCycle
{
	public class DeadView : EntityView
	{
		private readonly int IsDeadKey = Animator.StringToHash("IsDead");

		[SerializeField] private Animator _animator;

		private ReactiveVariable<bool> _isDead;
		private IDisposable _isDeadDisposable;

		private void OnValidate()
		{
			if (_animator == null)
				_animator = GetComponent<Animator>();
		}

		protected override void OnEntityStartedWork(Entity entity)
		{
			_isDead = entity.IsDead;

			_isDeadDisposable = _isDead.Subscribe(OnIsDeadChanged);
			UpdateIsDeadKey(_isDead.Value);
		}

		public override void Cleanup(Entity entity)
		{
			base.Cleanup(entity);

			_isDeadDisposable?.Dispose();
		}

		private void OnIsDeadChanged(bool oldValue, bool newValue)
			=> UpdateIsDeadKey(newValue);

		private void UpdateIsDeadKey(bool value)
			=> _animator.SetBool(IsDeadKey, value);
	}
}
