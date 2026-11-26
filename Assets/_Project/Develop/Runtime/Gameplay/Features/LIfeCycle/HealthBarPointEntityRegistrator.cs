using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.LIfeCycle
{
	public class HealthBarPointEntityRegistrator : MonoEntityRegistrator
	{
		[SerializeField] private Transform _healthBarPoint;

		public override void Register(Entity entity)
		{
			entity.AddHealthBarPoint(_healthBarPoint);
		}
	}
}
