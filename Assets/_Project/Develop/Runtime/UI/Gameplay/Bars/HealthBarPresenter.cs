using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.TeamsFeutere;
using _Project.Develop.Runtime.UI.Presenters;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Core.Gameplay.Bars
{
	public class HealthBarPresenter : IPresenter
	{
		private readonly BarWithTextView _view;
		private readonly Entity _entity;

		private readonly IReadOnlyReactiveVariable<float> _health;
		private readonly IReadOnlyReactiveVariable<float> _maxHealth;
		private readonly IReadOnlyReactiveVariable<Teams> _team;

		private readonly List<IDisposable> _disposables = new();

		public HealthBarPresenter(
			BarWithTextView view,
			Entity entity)
		{
			_view = view;
			_entity = entity;
			_health = _entity.CurrentHealth;
			_maxHealth = _entity.MaxHealth;
			_team = _entity.Team;
		}

		public void Initialize()
		{
			_disposables.Add(_health.Subscribe(OnHealthChanged));
			_disposables.Add(_maxHealth.Subscribe(OnMaxHealthChanged));
			_disposables.Add(_team.Subscribe(OnTeamChanged));

			UpdateValue();
			UpdateColor(_team.Value);
		}

		public void Dispose()
		{
			foreach (IDisposable disposable in _disposables)
				disposable.Dispose();

			_disposables.Clear();
		}

		private void OnHealthChanged(float oldValue, float newValue) => UpdateValue();

		private void OnMaxHealthChanged(float oldValue, float newValue) => UpdateValue();

		private void OnTeamChanged(Teams oldValue, Teams newValue) => UpdateColor(newValue);

		private void UpdateValue()
		{
			float normalized = _maxHealth.Value > 0f ? _health.Value / _maxHealth.Value : 0f;

			_view.UpdateText(_health.Value.ToString("0"));
			_view.UpdateSlider(Mathf.Clamp01(normalized));
		}

		private void UpdateColor(Teams team)
		{ 
			if (team == Teams.Player)
				_view.SetFillerColor(Color.green);
			else if (team == Teams.Enemy)
				_view.SetFillerColor(Color.red);
		}
	}
}
