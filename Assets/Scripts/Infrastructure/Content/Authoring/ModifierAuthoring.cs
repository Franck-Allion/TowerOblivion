using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "ModifierAuthoring", menuName = "TowerOblivion/Content/Modifier Authoring")]
    public sealed class ModifierAuthoring : ScriptableObject
    {
        [SerializeField] internal string _id;
        public string Id => _id;

        [SerializeField] internal string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [SerializeField] internal int _magnitude;
        public int Magnitude => _magnitude;
    }
}
