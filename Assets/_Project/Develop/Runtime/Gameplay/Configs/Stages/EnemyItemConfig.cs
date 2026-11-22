using System;
using _Project.Develop.Runtime.Gameplay.Configs.Entities;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Configs.Stages
{
	[Serializable]
	public class EnemyItemConfig
	{
		[field: SerializeField]
		public EntityConfig EntityConfig { get; private set; }

		[field: SerializeField, Min(1)]
		public int Count { get; private set; } = 1;
	}
}
