using _Project.Develop.Runtime.Gameplay.Features.StageFeature;
using _Project.Develop.Runtime.Utilities.Input;
using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.States
{
	public class PreparationState : State, IUpdatebleState
	{
		private readonly WaveStartRequestService _waveStartRequestService;
		private readonly IInputHandler _inputHandler;

		public PreparationState(WaveStartRequestService waveStartRequestService, IInputHandler inputHandler)
		{
			_waveStartRequestService = waveStartRequestService;
			_inputHandler = inputHandler;
		}

		public override void Enter()
		{
			base.Enter();

			_waveStartRequestService.Open();
		}

		public void Update(float deltaTime)
		{
			_inputHandler.Update(deltaTime);
		}

		public override void Exit()
		{
			base.Exit();

			_waveStartRequestService.Close();
		}
	}
}
