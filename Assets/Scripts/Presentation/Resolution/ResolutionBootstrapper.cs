using UnityEngine;
using UnityEngine.Localization.Components;
using TowerOblivion.Gameplay;

namespace TowerOblivion.Presentation.Resolution
{
    public sealed class ResolutionBootstrapper : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _progressionLocalizer;

        private void Start()
        {
            var global = GlobalBootstrapper.Instance;
            if (global == null || global.ActiveRun == null) return;

            if (global.ContentService == null || global.ContentService.Catalogs == null) return;

            // In a real flow, this would pull from the actual resolved run state (e.g. meta state updates).
            // For the placeholder slice, we read from the consequence/reward definitions from the run's last encounter.
            if (global.ContentService.Catalogs.Rooms.TryGet(global.ActiveRun.ActiveRoomId, out var roomDef) && roomDef.EncounterIds.Count > 0)
            {
                var encounterId = roomDef.EncounterIds[0];
                if (global.ContentService.Catalogs.Encounters.TryGet(encounterId, out var encounterDef) && encounterDef.RewardIds.Count > 0)
                {
                    var rewardId = encounterDef.RewardIds[0];
                    if (global.ContentService.Catalogs.Rewards.TryGet(rewardId, out var rewardDef))
                    {
                        if (_progressionLocalizer != null)
                        {
                            _progressionLocalizer.StringReference.SetReference("UI", "Resolution.Label.Progression");
                            _progressionLocalizer.StringReference.Arguments = new object[] { rewardDef.Amount, rewardDef.CurrencyId.Value };
                            _progressionLocalizer.RefreshString();
                        }
                    }
                }
            }
        }
    }
}
