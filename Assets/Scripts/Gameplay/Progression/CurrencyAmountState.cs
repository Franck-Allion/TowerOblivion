using System;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Progression
{
    [Serializable]
    public class CurrencyAmountState
    {
        public CurrencyId CurrencyId { get; set; }
        public int Amount { get; set; }
    }
}
