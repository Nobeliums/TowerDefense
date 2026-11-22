using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.StageFeature
{
	public class WaveStartRequestService
	{
		private readonly ReactiveVariable<bool> _isNewWaveStartAvailable = new();

		private bool _isRequested;

		public IReadOnlyReactiveVariable<bool> IsNewWaveStartAvailable => _isNewWaveStartAvailable;

		public bool IsRequested => _isRequested;

		public void Open()
		{
			_isRequested = false;
			_isNewWaveStartAvailable.Value = true;
		}

		public void Close()
		{
			_isNewWaveStartAvailable.Value = false;
		}

		public void Request()
		{
			if (_isNewWaveStartAvailable.Value == false)
				return;

			_isRequested = true;
		}
	}
}
