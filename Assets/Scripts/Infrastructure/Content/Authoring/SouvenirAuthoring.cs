using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "SouvenirAuthoring", menuName = "TowerOblivion/Content/Souvenir Authoring")]
    public sealed class SouvenirAuthoring : ScriptableObject
    {
        [SerializeField] private string _id;
        public string Id => _id;

        [SerializeField] private string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [SerializeField] private int _baseLife;
        public int BaseLife => _baseLife;

        internal void SetData(string id, string displayNameKey, int baseLife)
        {
            _id = id;
            _displayNameKey = displayNameKey;
            _baseLife = baseLife;
        }
    }
}
