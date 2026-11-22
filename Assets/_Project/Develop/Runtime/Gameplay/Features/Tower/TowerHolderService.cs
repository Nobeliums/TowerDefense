using System;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.Tower
{
	public class TowerHolderService : IInitializable, IDisposable
	{
		private readonly EntitiesLifeContext _entitiesLifeContext;
		private readonly ReactiveEvent<Entity> _towerRegistered = new();

		private Entity _tower;

		public TowerHolderService(EntitiesLifeContext entitiesLifeContext)
		{
			_entitiesLifeContext = entitiesLifeContext;
		}

		public IReadOnlyReactiveEvent<Entity> TowerRegistered => _towerRegistered;

		public Entity Tower => _tower;

		public void Initialize()
		{
			_entitiesLifeContext.Added += OnEntityAdded;
		}

		public void Dispose()
		{
			_entitiesLifeContext.Added -= OnEntityAdded;
		}

		private void OnEntityAdded(Entity entity)
		{
			if (entity.HasComponent<IsTower>() == false)
				return;

			_entitiesLifeContext.Added -= OnEntityAdded;
			_tower = entity;
			_towerRegistered.Invoke(_tower);
		}
	}
}
