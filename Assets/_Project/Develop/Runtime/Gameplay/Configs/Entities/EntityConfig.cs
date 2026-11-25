using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Configs.Entities
{
	public abstract class EntityConfig : ScriptableObject
	{
		[field: SerializeField, Min(0f)]
		public float SpawnProcessInitialTime { get; private set; } = 1f;
	}
}
