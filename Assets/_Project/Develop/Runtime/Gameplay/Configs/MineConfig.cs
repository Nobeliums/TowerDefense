using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Configs
{
	[CreateAssetMenu(fileName = "MineConfig", menuName = "Project/MineConfig")]
	public class MineConfig : ScriptableObject
	{
		[field: SerializeField, Min(0)]
		public int Price { get; private set; }

		[field: SerializeField, Min(0f)]
		public float TriggerDistance { get; private set; }

		[field: SerializeField]
		public ExplosionConfig ExplosionConfig { get; private set; }
	}
}
