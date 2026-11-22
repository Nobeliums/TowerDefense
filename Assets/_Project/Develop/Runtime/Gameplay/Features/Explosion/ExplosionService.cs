using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using _Project.Develop.Runtime.Gameplay.Features.TeamsFeutere;
using _Project.Develop.Runtime.Utilities;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class ExplosionService
	{
		private Buffer<Collider> _colliderBuffer = new(64);
		private HashSet<Entity> _damagedEntities = new HashSet<Entity>();

		private CollidersRegistryService _collidersRegistryService;

		public ExplosionService(CollidersRegistryService collidersRegistryService)
		{
			_collidersRegistryService = collidersRegistryService;
		}

		public void CreateExplosion(Vector3 position,  float radius, float damage, LayerMask layerMask, Teams sourceTeam)
		{
			_colliderBuffer.Count = Physics.OverlapSphereNonAlloc(position, radius, _colliderBuffer.Items, layerMask,
				QueryTriggerInteraction.Ignore);

			if (_colliderBuffer.Count == 0)
				return;
			
			ApplyExplosionDamage(damage, sourceTeam);
		}

		private void ApplyExplosionDamage(float damage, Teams sourceTeam)
		{
			_damagedEntities.Clear();
			
			for (int i = 0; i < _colliderBuffer.Count; i++)
			{
				Entity entity = _collidersRegistryService.GetBy(_colliderBuffer.Items[i]);
				
				if (entity == null || _damagedEntities.Add(entity) == false)
					continue;
				
				if (entity.HasComponent<CanApplyDamage>() == false)
					continue;
				
				if (entity.HasComponent<Team>() && entity.Team.Value == sourceTeam)
					continue;

				if (entity.TryGetTakeDamageRequest(out ReactiveEvent<float> takeDamageRequest))
					takeDamageRequest.Invoke(damage);
			}
		}
	}
}