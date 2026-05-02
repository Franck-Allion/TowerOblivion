using UnityEngine;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Combat;
using TowerOblivion.Gameplay.RunGeneration;

namespace TowerOblivion.Presentation.Combat
{
    public sealed class CombatBootstrapper : MonoBehaviour
    {
        private CombatStateOrchestrator _combatOrchestrator;
        
        private void Start()
        {
            var global = GlobalBootstrapper.Instance;
            if (global == null || global.ActiveRun == null)
            {
                Debug.LogError("[CombatBootstrapper] Cannot start combat: No active run state found.");
                return;
            }

            if (global.ContentService == null || global.ContentService.Catalogs == null)
            {
                Debug.LogError("[CombatBootstrapper] ContentService or Catalogs missing.");
                return;
            }

            var presenter = GetComponent<CombatPresenter>();
            if (presenter != null)
            {
                presenter.Initialize(global.EventBus, global.ContentService.Catalogs.Encounters);
            }

            // For the stub, get the current room and its first encounter
            if (!global.ContentService.Catalogs.Rooms.TryGet(global.ActiveRun.ActiveRoomId, out var roomDef) || roomDef.EncounterIds.Count == 0)
            {
                Debug.LogError($"[CombatBootstrapper] Invalid room or missing encounter for room {global.ActiveRun.ActiveRoomId}");
                return;
            }

            var encounterId = roomDef.EncounterIds[0];
            if (!global.ContentService.Catalogs.Encounters.TryGet(encounterId, out var encounterDef))
            {
                Debug.LogError($"[CombatBootstrapper] Encounter {encounterId} not found in catalog.");
                return;
            }

            _combatOrchestrator = new CombatStateOrchestrator(global.EventBus);
            _combatOrchestrator.StartCombat(encounterDef, global.ActiveRun, global.ActiveRun.Seed);
        }
    }
}
