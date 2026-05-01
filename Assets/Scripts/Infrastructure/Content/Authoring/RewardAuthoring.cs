using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "RewardAuthoring", menuName = "TowerOblivion/Content/Reward Authoring")]
    public sealed class RewardAuthoring : ScriptableObject
    {
        [SerializeField] private string _id;
        public string Id => _id;

        [SerializeField] private string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [SerializeField] private string _currencyId;
        public string CurrencyId => _currencyId;

        [SerializeField] private int _amount;
        public int Amount => _amount;

        internal void SetData(string id, string displayNameKey, string currencyId, int amount)
        {
            _id = id;
            _displayNameKey = displayNameKey;
            _currencyId = currencyId;
            _amount = amount;
        }
    }
}
