using System;
using _Project.Develop.Runtime.Gameplay.Configs.Effects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class ExplosionEffectSpawner : IDisposable
	{
		private readonly ExplosionService _explosionService;
		private readonly ExplosionEffectConfig _config;

		public ExplosionEffectSpawner(ExplosionService explosionService, ExplosionEffectConfig config)
		{
			_explosionService = explosionService;
			_config = config;

			_explosionService.Exploded += OnExploded;
		}

		private void OnExploded(Vector3 position, float radius)
		{
			ParticleSystem effect = Object.Instantiate(_config.Prefab, position, Quaternion.identity);
			effect.transform.localScale = Vector3.one * _config.Scale;
		}

		public void Dispose()
		{
			_explosionService.Exploded -= OnExploded;
		}
	}
}
