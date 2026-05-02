using System;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Combat;
using TowerOblivion.Gameplay.Content;
using UnityEngine;

namespace TowerOblivion.Presentation.Combat
{
    public sealed class CombatPresenter : MonoBehaviour
    {
        [SerializeField] private CombatView _view;

        private IEventBus _eventBus;
        private IContentCatalog<EncounterId, EncounterContentDefinition> _encounterCatalog;
        private IDisposable _subscription;

        public void Initialize(IEventBus eventBus, IContentCatalog<EncounterId, EncounterContentDefinition> encounterCatalog)
        {
            _subscription?.Dispose();
            _eventBus = eventBus;
            _encounterCatalog = encounterCatalog;
            _subscription = _eventBus.Subscribe<CombatStarted>(OnCombatStarted);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnCombatStarted(CombatStarted evt)
        {
            if (_view == null) return;

            if (_encounterCatalog.TryGet(evt.State.EncounterId, out var encounterDef))
            {
                // Placeholder for Localization: using DisplayNameKey directly for now
                _view.SetEncounterName(encounterDef.DisplayNameKey);
            }
        }

        public void InitializeView(CombatView view)
        {
            _view = view;
        }
    }
}
