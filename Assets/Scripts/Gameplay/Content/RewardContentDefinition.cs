using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Content
{
    public sealed class RewardContentDefinition
    {
        public RewardId Id { get; }
        public string DisplayNameKey { get; }
        public CurrencyId CurrencyId { get; }
        public int Amount { get; }

        public RewardContentDefinition(RewardId id, string displayNameKey, CurrencyId currencyId, int amount)
        {
            Id = id;
            DisplayNameKey = displayNameKey ?? string.Empty;
            CurrencyId = currencyId;
            Amount = amount;
        }
    }
}
