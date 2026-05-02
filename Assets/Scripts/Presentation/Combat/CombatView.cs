using TMPro;
using UnityEngine;

namespace TowerOblivion.Presentation.Combat
{
    public sealed class CombatView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _encounterNameLabel;

        public void Initialize(TextMeshProUGUI encounterNameLabel)
        {
            _encounterNameLabel = encounterNameLabel;
        }

        public void SetEncounterName(string encounterName)
        {
            if (string.IsNullOrEmpty(encounterName))
            {
                return;
            }

            if (_encounterNameLabel != null)
            {
                _encounterNameLabel.text = encounterName;
            }
        }
    }
}
