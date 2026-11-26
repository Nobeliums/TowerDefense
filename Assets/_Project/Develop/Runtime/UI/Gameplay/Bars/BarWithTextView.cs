using _Project.Develop.Runtime.UI.View;
using TMPro;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Core.Gameplay.Bars
{
	public class BarWithTextView : MonoBehaviour, IView
	{
		[SerializeField] private BarView _bar;
		[SerializeField] private TMP_Text _text;
		
		public void UpdateText(string text) =>_text.text = text;
		
		public void UpdateSlider(float sliderValue) => _bar.SetValue(sliderValue);

		public void SetFillerColor(Color color) => _bar.SetFillerColor(color);
	}
}