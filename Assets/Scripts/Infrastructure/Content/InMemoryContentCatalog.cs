using System;
using System.Collections.Generic;
using TowerOblivion.Gameplay.Content;

namespace TowerOblivion.Infrastructure.Content
{
    public sealed class InMemoryContentCatalog<TId, TDefinition> : IContentCatalog<TId, TDefinition>
    {
        private readonly Dictionary<TId, TDefinition> _definitionsById;
        private readonly IReadOnlyList<TDefinition> _allDefinitions;

        public InMemoryContentCatalog(IReadOnlyList<TDefinition> definitions, Func<TDefinition, TId> idSelector)
        {
            if (idSelector == null)
            {
                throw new ArgumentNullException(nameof(idSelector));
            }

            _allDefinitions = definitions ?? new List<TDefinition>();
            _definitionsById = new Dictionary<TId, TDefinition>();

            for (var i = 0; i < _allDefinitions.Count; i++)
            {
                var definition = _allDefinitions[i];
                var id = idSelector(definition);
                if (_definitionsById.ContainsKey(id))
                {
                    throw new ArgumentException($"Duplicate content ID detected in catalog: {id}");
                }
                _definitionsById.Add(id, definition);
            }
        }

        public bool TryGet(TId id, out TDefinition definition)
        {
            return _definitionsById.TryGetValue(id, out definition);
        }

        public IReadOnlyList<TDefinition> GetAll()
        {
            return _allDefinitions;
        }
    }
}
