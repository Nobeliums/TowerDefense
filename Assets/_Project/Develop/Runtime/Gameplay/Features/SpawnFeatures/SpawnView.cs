using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.SpawnFeatures
{
	public class SpawnView : EntityView
	{
		private readonly int SpawningProcessKey = Animator.StringToHash("InSpawnProcess");

		[SerializeField] private Animator _animator;
		[SerializeField] private GameObject _spawnEffectPrefab;

		private ReactiveVariable<bool> _inSpawnProcess;
		private IDisposable _inSpawnProcessDisposable;
		private Transform _entityTransform;

		private void OnValidate()
		{
			_animator ??= GetComponent<Animator>();
		}

		protected override void OnEntityStartedWork(Entity entity)
		{
			_inSpawnProcess = entity.InSpawnProcess;
			_entityTransform = entity.TransformComponent;

			_inSpawnProcessDisposable = _inSpawnProcess.Subscribe(OnInSpawnProcessChanged);
			UpdateSpawnProcessKey(_inSpawnProcess.Value);
		}

		public override void Cleanup(Entity entity)
		{
			base.Cleanup(entity);

			_inSpawnProcessDisposable?.Dispose();
		}

		private void OnInSpawnProcessChanged(bool oldValue, bool newValue)
			=> UpdateSpawnProcessKey(newValue);

		private void UpdateSpawnProcessKey(bool value)
		{
			_animator.SetBool(SpawningProcessKey, value);

			if (value)
				Instantiate(_spawnEffectPrefab, _entityTransform.position, Quaternion.identity);
		}
	}
}
