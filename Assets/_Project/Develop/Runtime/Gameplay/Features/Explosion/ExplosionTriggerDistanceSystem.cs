using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class ExplosionTriggerDistanceSystem : IInitializableSystem
	{
		private ReactiveVariable<float> _explosionTriggerDistance;
		private ReactiveVariable<float> _explosionRadius;
		private ReactiveVariable<float> _explosionDamage;
		
		
		
		public void OnInit(Entity entity)
		{
			_explosionTriggerDistance = entity.ExplosionTriggerDistance;
			_explosionDamage = entity.ExplosionDamage;
			_explosionRadius = entity.ExplosionDamage;
		}
	}
}