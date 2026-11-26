using UnityEngine;

namespace _Project.Develop.Runtime.UI.Core
{
	[RequireComponent(typeof(RectTransform))]
	public class SafeAreaFitter : MonoBehaviour
	{
		private RectTransform _rectTransform;

		private Rect _lastSafeArea;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
			UpdateSafeArea();
		}

		private void Update()
		{
			if (Screen.safeArea != _lastSafeArea)
				UpdateSafeArea();
		}

		private void UpdateSafeArea()
		{
			Rect safeArea = Screen.safeArea;

			_lastSafeArea = safeArea;

			Vector2 anchorMin = safeArea.position;
			Vector2 anchorMax = safeArea.position + safeArea.size;

			anchorMin.x /= Screen.width;
			anchorMin.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;

			_rectTransform.anchorMin = anchorMin;
			_rectTransform.anchorMax = anchorMax;
		}
	}
}
