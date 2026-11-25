using _Project.Develop.Runtime.UI.View;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Core.Gameplay.Bars
{
	public class EntitiesHealthDisplay : ElementsListView<BarWithTextView>
	{
		private Camera _camera;

		private void Awake()
		{
			_camera = Camera.main;
		}

		public void UpdatePositionFor(BarWithTextView bar, Vector3 worldPosition)
		{
			Vector3 screenPoint = _camera.WorldToScreenPoint(worldPosition);

			bar.transform.position = screenPoint;
		}
	}
}
