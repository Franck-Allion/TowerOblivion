using UnityEngine;
using UnityEngine.Localization.Components;
using TowerOblivion.Gameplay;

namespace TowerOblivion.Presentation.Reward
{
    public sealed class RewardBootstrapper : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _titleLocalizer;
        [SerializeField] private LocalizeStringEvent _effectLocalizer;

        private void Start()
        {
            var global = GlobalBootstrapper.Instance;
            if (global == null || global.ActiveRun == null) return;

            if (global.ContentService == null || global.ContentService.Catalogs == null) return;

            if (global.ContentService.Catalogs.Rooms.TryGet(global.ActiveRun.ActiveRoomId, out var roomDef) && roomDef.EncounterIds.Count > 0)
            {
                var encounterId = roomDef.EncounterIds[0];
                if (global.ContentService.Catalogs.Encounters.TryGet(encounterId, out var encounterDef) && encounterDef.RewardIds.Count > 0)
                {
                    var rewardId = encounterDef.RewardIds[0];
                    if (global.ContentService.Catalogs.Rewards.TryGet(rewardId, out var rewardDef))
                    {
                        if (_titleLocalizer != null)
                        {
                            _titleLocalizer.StringReference.SetReference("UI", rewardDef.DisplayNameKey);
                            _titleLocalizer.RefreshString();
                        }

                        if (_effectLocalizer != null)
                        {
                            _effectLocalizer.StringReference.SetReference("UI", "Reward.Label.Effect");
                            _effectLocalizer.StringReference.Arguments = new object[] { rewardDef.Amount, rewardDef.CurrencyId.Value };
                            _effectLocalizer.RefreshString();
                        }
                    }
                }
            }
        }
    }
}
