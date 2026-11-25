using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Configs.Effects
{
	[CreateAssetMenu(fileName = "ExplosionEffectConfig", menuName = "Project/Effects/ExplosionEffectConfig")]
	public class ExplosionEffectConfig : ScriptableObject
	{
		[field: SerializeField]
		public ParticleSystem Prefab { get; private set; }

		[field: SerializeField, Min(0.01f)]
		public float Scale { get; private set; } = 1f;
	}
}
