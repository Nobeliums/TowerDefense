using _Project.Develop.Runtime.Gameplay.Features.StageFeature;
using _Project.Develop.Runtime.Utilities.Input;
using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.States
{
	public class StageProcessState : State, IUpdatebleState
	{
		private StageProviderService _stageProviderService;
		private IInputHandler _inputHandler;

		public StageProcessState(StageProviderService stageProviderService, IInputHandler inputHandler)
		{
			_stageProviderService = stageProviderService;
			_inputHandler = inputHandler;
		}

		public override void Enter()
		{
			base.Enter();

			_stageProviderService.SwitchToNext();
			_stageProviderService.StartCurrent();
		}

		public void Update(float deltaTime)
		{
			_stageProviderService.UpdateCurrent(deltaTime);
			_inputHandler.Update(deltaTime);
		}

		public override void Exit()
		{
			base.Exit();

			_stageProviderService.CleanupCurrent();
		}
	}
}
