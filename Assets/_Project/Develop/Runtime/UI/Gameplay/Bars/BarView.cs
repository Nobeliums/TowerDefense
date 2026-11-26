using _Project.Develop.Runtime.UI.View;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Develop.Runtime.UI.Core.Gameplay.Bars
{
	public class BarView : MonoBehaviour, IView
	{
		[SerializeField] private Slider _slider;
		[SerializeField] private Image _filler;

		public void SetValue(float value) => _slider.value = value;

		public void SetFillerColor(Color color) => _filler.color = color;
	}
}
