using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Configs
{
	[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Project/Player/PlayerConfig")]
	public class PlayerConfig : ScriptableObject
	{
		[field: SerializeField]
		public ExplosionConfig ExplosionConfig { get; private set; }

		[field: SerializeField]
		public MineConfig MineConfig { get; private set; }
	}
}