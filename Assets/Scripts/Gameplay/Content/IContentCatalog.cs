using System.Collections.Generic;

namespace TowerOblivion.Gameplay.Content
{
    public interface IContentCatalog<TId, TDefinition>
    {
        bool TryGet(TId id, out TDefinition definition);
        IReadOnlyList<TDefinition> GetAll();
    }
}
