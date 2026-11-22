using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Configs.Stages;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Configs.Levels
{
	[CreateAssetMenu(menuName = "Project/Gameplay/Levels/LevelConfig", fileName = "LevelConfig", order = 0)]
	public class LevelConfig : ScriptableObject
	{
		[SerializeField]
		private List<StageConfig> _stages;

		[field: SerializeField, Min(1f)]
		public float TowerMaxHealth { get; private set; } = 100f;

		[field: SerializeField, Min(0)]
		public int WinReward { get; private set; }

		public IReadOnlyList<StageConfig> Stages => _stages;
	}
}
