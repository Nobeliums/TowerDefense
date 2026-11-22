using _Project.Develop.Runtime.Gameplay.Features.StageFeature;
using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.States
{
	public class PreparationState : State, IUpdatebleState
	{
		private readonly WaveStartRequestService _waveStartRequestService;

		public PreparationState(WaveStartRequestService waveStartRequestService)
		{
			_waveStartRequestService = waveStartRequestService;
		}

		public override void Enter()
		{
			base.Enter();

			_waveStartRequestService.Open();
		}

		public void Update(float deltaTime)
		{
		}

		public override void Exit()
		{
			base.Exit();

			_waveStartRequestService.Close();
		}
	}
}
