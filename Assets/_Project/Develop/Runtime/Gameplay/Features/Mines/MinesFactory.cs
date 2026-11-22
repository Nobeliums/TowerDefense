using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.AI;
using _Project.Develop.Runtime.Gameplay.Features.TeamsFeutere;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Mines
{
	public class MinesFactory
	{
		private readonly EntitiesFactory _entitiesFactory;
		private readonly BrainsFactory _brainsFactory;
		private readonly EntitiesLifeContext _entitiesLifeContext;

		public MinesFactory(DIContainer container)
		{
			_entitiesFactory = container.Resolve<EntitiesFactory>();
			_brainsFactory = container.Resolve<BrainsFactory>();
			_entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
		}

		public Entity Create(Vector3 position, MineConfig config)
		{
			Entity mine = _entitiesFactory.CreateMineEntity(position, config);

			mine.AddTeam(new ReactiveVariable<Teams>(Teams.Player));

			_entitiesLifeContext.Add(mine);

			_brainsFactory.CreateMineBrain(mine);

			return mine;
		}
	}
}
