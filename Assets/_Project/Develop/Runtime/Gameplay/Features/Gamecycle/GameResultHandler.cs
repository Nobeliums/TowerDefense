using _Project.Develop.Runtime.Meta.Features.GameStatistics;
using _Project.Develop.Runtime.Meta.Features.LevelsProgression;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagment.DataProvider;

namespace _Project.Develop.Runtime.Features.Gamecycle
{
	public class GameResultHandler
	{
		private readonly GameStatisticsService _gameStatisticsService;
		private readonly PlayerDataProvider _playerDataProvider;
		private readonly ICoroutinePerformer _coroutinePerformer;
		private readonly WalletService _walletService;
		private readonly LevelsProgressionService _levelsProgressionService;
		private readonly int _levelNumber;
		private readonly int _winReward;

		public GameResultHandler(
			GameStatisticsService gameStatisticsService,
			PlayerDataProvider playerDataProvider,
			ICoroutinePerformer coroutinePerformer,
			WalletService walletService,
			LevelsProgressionService levelsProgressionService,
			int levelNumber,
			int winReward)
		{
			_gameStatisticsService = gameStatisticsService;
			_playerDataProvider = playerDataProvider;
			_coroutinePerformer = coroutinePerformer;
			_walletService = walletService;
			_levelsProgressionService = levelsProgressionService;
			_levelNumber = levelNumber;
			_winReward = winReward;
		}

		public void OnWin()
		{
			_gameStatisticsService.OnWinGame();
			_walletService.Add(_winReward, WalletCurrencyType.Gold);
			_levelsProgressionService.AddLevelToCompleted(_levelNumber);
			_coroutinePerformer.StartPerform(_playerDataProvider.SaveAsync());
		}

		public void OnDefeat()
		{
			_gameStatisticsService.OnLoseGame();
			_coroutinePerformer.StartPerform(_playerDataProvider.SaveAsync());
		}
	}
}
