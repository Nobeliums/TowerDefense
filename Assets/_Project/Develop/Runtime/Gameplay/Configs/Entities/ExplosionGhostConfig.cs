using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Configs.Entities
{
	[CreateAssetMenu(fileName = "ExplosionGhostConfig", menuName = "Project/Enemies/ExplosionGhostConfig")]
	public class ExplosionGhostConfig : EntityConfig
	{
		[field: SerializeField]
		public MovementConfig MovementConfig { get; private set; }
		
		[field: SerializeField]
		public HealthConfig HealthConfig { get; private set; }
		
		[field: SerializeField]
		public ExplosionConfig ExplosionConfig { get; private set; }
		
		[field: SerializeField]
		public float ExplosionTriggerDistance { get; private set; }
	}
}