using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Configs.Levels;
using _Project.Develop.Runtime.Meta.Features.LevelsProgression;
using UnityEngine;

namespace _Project.Develop.Runtime.Meta.Features.LevelSelection
{
	public class RandomLevelService
	{
		private const int FirstLevel = 1;

		private readonly LevelListConfig _levelListConfig;
		private readonly LevelsProgressionService _levelsProgressionService;

		public RandomLevelService(LevelListConfig levelListConfig, LevelsProgressionService levelsProgressionService)
		{
			_levelListConfig = levelListConfig;
			_levelsProgressionService = levelsProgressionService;
		}

		public int GetRandomLevelNumber()
		{
			List<int> uncompletedLevels = new List<int>();

			for (int levelNumber = FirstLevel; levelNumber <= _levelListConfig.Count; levelNumber++)
			{
				if (_levelsProgressionService.IsLevelCompleted(levelNumber) == false)
					uncompletedLevels.Add(levelNumber);
			}

			if (uncompletedLevels.Count == 0)
				return Random.Range(FirstLevel, _levelListConfig.Count + 1);

			return uncompletedLevels[Random.Range(0, uncompletedLevels.Count)];
		}
	}
}
