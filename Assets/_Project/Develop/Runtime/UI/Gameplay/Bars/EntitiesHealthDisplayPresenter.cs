using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.LIfeCycle;
using _Project.Develop.Runtime.Gameplay.Features.TeamsFeutere;
using _Project.Develop.Runtime.Gameplay.Features.Tower;
using _Project.Develop.Runtime.UI.Presenters;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Core.Gameplay.Bars
{
	public class EntitiesHealthDisplayPresenter : IPresenter
	{
		private readonly EntitiesHealthDisplay _display;
		private readonly EntitiesLifeContext _entitiesLifeContext;
		private readonly ViewFactory _viewFactory;
		private readonly GameplayPresenterFactory _presenterFactory;

		private readonly Dictionary<Entity, EntityHealthBarInfo> _entityToHealthBar = new();

		public EntitiesHealthDisplayPresenter(
			EntitiesHealthDisplay display,
			EntitiesLifeContext entitiesLifeContext,
			ViewFactory viewFactory,
			GameplayPresenterFactory presenterFactory)
		{
			_display = display;
			_entitiesLifeContext = entitiesLifeContext;
			_viewFactory = viewFactory;
			_presenterFactory = presenterFactory;
		}

		public void Initialize()
		{
			_entitiesLifeContext.Added += OnEntityAdded;
			_entitiesLifeContext.Released += OnEntityReleased;

			foreach (Entity entity in _entitiesLifeContext.Entities)
				OnEntityAdded(entity);
		}

		public void Dispose()
		{
			_entitiesLifeContext.Added -= OnEntityAdded;
			_entitiesLifeContext.Released -= OnEntityReleased;

			foreach (Entity entity in new List<Entity>(_entityToHealthBar.Keys))
				RemoveHealthBarFor(entity);
		}

		public void LateUpdate()
		{
			foreach (EntityHealthBarInfo info in _entityToHealthBar.Values)
				_display.UpdatePositionFor(info.View, info.HealthBarPoint.position);
		}

		private void OnEntityAdded(Entity entity)
		{
			if (CanShowHealthBar(entity) == false)
				return;

			if (_entityToHealthBar.ContainsKey(entity))
				return;

			string viewId = entity.HasComponent<IsTower>() ? ViewIds.TowerHealthBar : ViewIds.EnemyHealthBar;

			BarWithTextView view = _viewFactory.Create<BarWithTextView>(viewId);
			_display.Add(view);

			HealthBarPresenter presenter = _presenterFactory.CreateHealthBarPresenter(view, entity);
			presenter.Initialize();

			IDisposable removeReason = entity.IsDead.Subscribe((oldValue, isDead) =>
			{
				if (isDead)
					RemoveHealthBarFor(entity);
			});

			Transform healthBarPoint = entity.HealthBarPoint;

			_entityToHealthBar.Add(entity, new EntityHealthBarInfo(removeReason, healthBarPoint, presenter, view));

			_display.UpdatePositionFor(view, healthBarPoint.position);
		}

		private void OnEntityReleased(Entity entity) => RemoveHealthBarFor(entity);

		private void RemoveHealthBarFor(Entity entity)
		{
			if (_entityToHealthBar.TryGetValue(entity, out EntityHealthBarInfo info) == false)
				return;

			info.RemoveReason.Dispose();
			info.Presenter.Dispose();

			_display.Remove(info.View);
			_viewFactory.Release(info.View);

			_entityToHealthBar.Remove(entity);
		}

		private bool CanShowHealthBar(Entity entity)
		{
			return entity.HasComponent<CurrentHealth>()
			       && entity.HasComponent<MaxHealth>()
			       && entity.HasComponent<Team>()
			       && entity.HasComponent<HealthBarPoint>()
			       && entity.HasComponent<IsDead>()
			       && entity.IsDead.Value == false;
		}

		private class EntityHealthBarInfo
		{
			public EntityHealthBarInfo(
				IDisposable removeReason,
				Transform healthBarPoint,
				HealthBarPresenter presenter,
				BarWithTextView view)
			{
				RemoveReason = removeReason;
				HealthBarPoint = healthBarPoint;
				Presenter = presenter;
				View = view;
			}

			public IDisposable RemoveReason { get; }
			public Transform HealthBarPoint { get; }
			public HealthBarPresenter Presenter { get; }
			public BarWithTextView View { get; }
		}
	}
}
