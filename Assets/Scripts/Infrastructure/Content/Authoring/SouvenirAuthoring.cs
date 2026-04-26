using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "SouvenirAuthoring", menuName = "TowerOblivion/Content/Souvenir Authoring")]
    public sealed class SouvenirAuthoring : ScriptableObject
    {
        [SerializeField] internal string _id;
        public string Id => _id;

        [SerializeField] internal string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [SerializeField] internal int _baseLife;
        public int BaseLife => _baseLife;
    }
}
