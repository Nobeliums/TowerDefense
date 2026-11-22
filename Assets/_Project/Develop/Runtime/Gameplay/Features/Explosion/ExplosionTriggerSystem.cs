using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Gameplay.Features.TeamsFeutere;
using _Project.Develop.Runtime.Gameplay.Features.Tower;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class ExplosionTriggerSystem : IInitializableSystem, IUpdatableSystem
	{
		private ReactiveVariable<float> _explosionTriggerDistance;
		private ReactiveVariable<float> _explosionRadius;
		private ReactiveVariable<float> _explosionDamage;
		private ReactiveVariable<bool> _isDetonated;
		
		private ReactiveVariable<Entity> _target;
		
		private Transform _entityTransform;
		private Entity _entity;

		private readonly LayerMask _explosionLayer;
		private readonly ExplosionService _explosionService;

		public ExplosionTriggerSystem(LayerMask explosionLayer, ExplosionService explosionService)
		{
			_explosionLayer = explosionLayer;
			_explosionService = explosionService;
		}

		public void OnInit(Entity entity)
		{
			_explosionTriggerDistance = entity.ExplosionTriggerDistance;
			_explosionDamage = entity.ExplosionDamage;
			_explosionRadius = entity.ExplosionRadius;
			_isDetonated = entity.IsDetonated;

			_target = entity.CurrentTarget;

			_entityTransform = entity.TransformComponent;
			_entity = entity;
		}

		public void OnUpdate(float dt)
		{
			if (_isDetonated.Value || _target.Value == null)
				return;

			if (DistanceToTarget() <= _explosionTriggerDistance.Value)
			{
				_isDetonated.Value = true;
				_explosionService.CreateExplosion(
					_entityTransform.position,
					_explosionRadius.Value,
					_explosionDamage.Value,
					_explosionLayer,
					_entity.Team.Value);
			}
		}

		private float DistanceToTarget()
			=> Vector3.Distance(_target.Value.TransformComponent.position, _entityTransform.position);
	}
}