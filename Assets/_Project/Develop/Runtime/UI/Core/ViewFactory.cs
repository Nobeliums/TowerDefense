using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.UI.View;
using _Project.Develop.Runtime.Utilities.ResourcesManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Develop.Runtime.UI.Core
{
	public class ViewFactory
	{
		private ResourceLoadService _resourceLoadService;

		private readonly Dictionary<string, string> _viewIdToResourcesPath = new()
		{
			{ViewIds.CurrencyView, "Prefabs/UI/Common/CurrencyView"},
			{ViewIds.MainMenuView, "Prefabs/UI/MainMenu/MainMenuView"},
			{ViewIds.GameStatTextView, "Prefabs/UI/Common/TextBoxView"},
			{ViewIds.SelectCharsGameButtonView, "Prefabs/UI/MainMenu/SelectLevelNumberView"},
			{ViewIds.SelectGameCharsPopupView, "Prefabs/UI/MainMenu/SelectLevelNumberPopup"},
			{ViewIds.ShopItemBuyButtonView, "Prefabs/UI/MainMenu/ShopItemBuy"},
			{ViewIds.ShopPopupView, "Prefabs/UI/MainMenu/ShopPopup"},
			{ViewIds.GameplayView, "Prefabs/UI/Gameplay/GameplayView"},
			{ViewIds.WinResultPopupView, "Prefabs/UI/Gameplay/WinResultPopup"},
			{ViewIds.DefeatResultPopupView, "Prefabs/UI/Gameplay/DefeatResultPopup"},
			{ViewIds.TowerHealthBar, "Prefabs/UI/Gameplay/Bars/TowerHealthBar"},
			{ViewIds.EnemyHealthBar, "Prefabs/UI/Gameplay/Bars/EnemyHealthBar"}
		};

		public ViewFactory(ResourceLoadService resourceLoadService)
		{
			_resourceLoadService = resourceLoadService;
		}

		public TView Create<TView>(string viewId, Transform parent = null) where TView : MonoBehaviour, IView
		{
			if (_viewIdToResourcesPath.TryGetValue(viewId, out var resourcePath))
			{
				TView prefab = _resourceLoadService.Load<TView>(resourcePath);
	
				TView viewIntance = Object.Instantiate(prefab, parent);
				
				return viewIntance;
			}
			
			throw new ArgumentException($"Unknown view id '{viewId}'");
		}

		public void Release<TView>(TView view) where TView : MonoBehaviour, IView
		{
			Object.Destroy(view.gameObject);
		}
	}
}
