using System;
using TowerOblivion.Core;

using TowerOblivion.Gameplay.Progression;
namespace TowerOblivion.Gameplay.RunGeneration
{
    [Serializable]
    public sealed class RunResourceState
    {
        public CurrencyId ResourceId { get; set; }
        public int Amount { get; set; }
    }
}
