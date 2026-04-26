using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "RewardAuthoring", menuName = "TowerOblivion/Content/Reward Authoring")]
    public sealed class RewardAuthoring : ScriptableObject
    {
        [SerializeField] internal string _id;
        public string Id => _id;

        [SerializeField] internal string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [SerializeField] internal string _currencyId;
        public string CurrencyId => _currencyId;

        [SerializeField] internal int _amount;
        public int Amount => _amount;
    }
}
