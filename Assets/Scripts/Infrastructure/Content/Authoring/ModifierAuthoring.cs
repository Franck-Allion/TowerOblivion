using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "ModifierAuthoring", menuName = "TowerOblivion/Content/Modifier Authoring")]
    public sealed class ModifierAuthoring : ScriptableObject
    {
        [SerializeField] private string _id;
        public string Id => _id;

        [SerializeField] private string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [SerializeField] private int _magnitude;
        public int Magnitude => _magnitude;

        internal void SetData(string id, string displayNameKey, int magnitude)
        {
            _id = id;
            _displayNameKey = displayNameKey;
            _magnitude = magnitude;
        }
    }
}
