using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Configs.Stages;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.Enemies;
using _Project.Develop.Runtime.Gameplay.Features.Tower;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Develop.Runtime.Gameplay.Features.StageFeature
{
	public class ClearAllEnemiesStage : IStage
	{
		private const float MaxDegree = 360f;
		
		private ClearAllEnemiesStageConfig _config;
		private EnemiesFactory _enemiesFactory;
		private EntitiesLifeContext _entitiesLifeContext;
		private TowerHolderService _towerHolderService;

		private ReactiveEvent _completed;

		private bool _inProcess;

		private Dictionary<Entity, IDisposable> _enemies = new();

		public ClearAllEnemiesStage(
			ClearAllEnemiesStageConfig config,
			EnemiesFactory enemiesFactory,
			EntitiesLifeContext entitiesLifeContext,
			TowerHolderService towerHolderService)
		{
			_config = config;
			_enemiesFactory = enemiesFactory;
			_entitiesLifeContext = entitiesLifeContext;
			_towerHolderService = towerHolderService;
			_completed = new ReactiveEvent();
		}

		public IReadOnlyReactiveEvent Completed => _completed;

		public void Start()
		{
			if (_inProcess)
				throw new InvalidOperationException("Game mode is already started");

			SpawnEnemies();
			_inProcess = true;
		}

		public void Update(float deltaTime)
		{
			if (_inProcess == false)
				return;
			
			if (_enemies.Count == 0)
				ProcessEnd();
		}

		public void Cleanup()
		{
			foreach (KeyValuePair<Entity, IDisposable> pair in _enemies)
			{
				pair.Value.Dispose();
				_entitiesLifeContext.Release(pair.Key);
			}

			_enemies.Clear();
			_inProcess = false;
		}

		private void ProcessEnd()
		{
			_inProcess = false;
			_completed?.Invoke();
		}

		private void SpawnEnemies()
		{
			Vector3 center = _towerHolderService.Tower.TransformComponent.position;

			foreach (EnemyItemConfig enemyItem in _config.Enemies)
				for (int i = 0; i < enemyItem.Count; i++)
					SpawnEnemy(enemyItem, GetRandomPointOnCircle(center, _config.SpawnRadius));
		}

		private Vector3 GetRandomPointOnCircle(Vector3 center, float radius)
		{
			float angle = Random.Range(0f, MaxDegree) * Mathf.Deg2Rad;

			return center + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
		}

		private void SpawnEnemy(EnemyItemConfig enemyItem, Vector3 position)
		{
			Entity spawnedEnemy = _enemiesFactory.Create(position, enemyItem.EntityConfig);

			IDisposable subscription = spawnedEnemy.IsDead.Subscribe((oldValue, isDead) =>
			{
				if (isDead)
				{
					_enemies[spawnedEnemy].Dispose();
					_enemies.Remove(spawnedEnemy);
				}
			});

			_enemies.Add(spawnedEnemy, subscription);
		}

		public void Dispose()
		{
			foreach (KeyValuePair<Entity, IDisposable> pair in _enemies)
				pair.Value.Dispose();

			_enemies.Clear();
			_inProcess = false;
		}
	}
}
