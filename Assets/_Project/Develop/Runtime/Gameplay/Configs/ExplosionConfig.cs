using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Configs
{
	[CreateAssetMenu(fileName = "ExplosionConfig", menuName = "Project/ExplosionConfig")]
	public class ExplosionConfig : ScriptableObject
	{
		[field: SerializeField]
		public float ExplosionRadius  { get;  private set; }
		
		[field: SerializeField]
		public float Damage { get; private set; }
	}
}