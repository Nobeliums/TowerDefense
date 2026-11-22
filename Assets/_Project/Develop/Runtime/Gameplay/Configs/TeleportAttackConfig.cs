using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Configs
{
	[CreateAssetMenu(menuName = "Project/Common/TeleportAttackConfig", fileName = "TeleportAttackConfig", order = 0)]
	public class TeleportAttackConfig : ScriptableObject
	{
		[field: SerializeField, Min(0)]
		public float Damage { get; private set; }

		[field: SerializeField, Min(0)]
		public float TeleportTakeDamageRadius { get; private set; }
	}
}