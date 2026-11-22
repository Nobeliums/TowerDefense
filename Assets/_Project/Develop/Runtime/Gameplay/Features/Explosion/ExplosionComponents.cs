using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.Explosion
{
	public class ExplosionTriggerDistance : IEntityComponent
	{
		public ReactiveVariable<float> Value;
	}

	public class ExplosionRadius : IEntityComponent
	{
		public ReactiveVariable<float> Value;
	}

	public class ExplosionDamage : IEntityComponent
	{
		public ReactiveVariable<float> Value;
	}

	public class IsDetonated : IEntityComponent
	{
		public ReactiveVariable<bool> Value;
	}
}