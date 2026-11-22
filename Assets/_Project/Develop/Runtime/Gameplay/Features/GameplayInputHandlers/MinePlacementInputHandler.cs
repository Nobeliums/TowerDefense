using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Gameplay.Features.Mines;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Utilities.Input;
using UnityEngine.EventSystems;

namespace _Project.Develop.Runtime.Gameplay.Features.GameplayInputHandlers
{
	public class MinePlacementInputHandler : IInputHandler
	{
		private const WalletCurrencyType MineCurrency = WalletCurrencyType.Gold;

		private readonly MinesFactory _minesFactory;
		private readonly WalletService _walletService;
		private readonly IInputService _inputService;
		private readonly MineConfig _config;

		public MinePlacementInputHandler(
			MinesFactory minesFactory,
			WalletService walletService,
			IInputService inputService,
			MineConfig config)
		{
			_minesFactory = minesFactory;
			_walletService = walletService;
			_inputService = inputService;
			_config = config;
		}

		public void Update(float deltaTime)
		{
			if (_inputService.IsAttackButtonPressed == false)
				return;

			if (EventSystem.current.IsPointerOverGameObject())
				return;

			if (_walletService.IsEnough(_config.Price, MineCurrency) == false)
				return;

			_walletService.Debit(_config.Price, MineCurrency);
			_minesFactory.Create(_inputService.CursorPosition, _config);
		}
	}
}
