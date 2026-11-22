using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Gameplay.Features.Explosion;
using _Project.Develop.Runtime.Gameplay.Features.TeamsFeutere;
using _Project.Develop.Runtime.Utilities;
using _Project.Develop.Runtime.Utilities.Input;
using UnityEngine.EventSystems;

namespace _Project.Develop.Runtime.Gameplay.Features.GameplayInputHandlers
{
	public class PlayerExplosionInputHandler : IInputHandler
	{
		private ExplosionService _explosionService;
		private IInputService _inputService;
		private ExplosionConfig _config;
		
		public PlayerExplosionInputHandler(ExplosionService explosionService, IInputService inputService, ExplosionConfig config)
		{
			_explosionService = explosionService;
			_inputService = inputService;
			_config = config;
		}

		public void Update(float deltaTime)
		{
			if (_inputService.IsAttackButtonPressed && EventSystem.current.IsPointerOverGameObject() == false)
				_explosionService.CreateExplosion(
					_inputService.CursorPosition,
					_config.ExplosionRadius,
					_config.Damage,
					Layers.CharacterMask,
					Teams.Player);
		}
	}
}