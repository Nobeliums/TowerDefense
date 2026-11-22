using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Configs
{
	[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Gameplay/PlayerConfig")]
	public class PlayerConfig : ScriptableObject
	{
		[field: SerializeField]
		public ExplosionConfig ExplosionConfig { get; private set; }
	}
}