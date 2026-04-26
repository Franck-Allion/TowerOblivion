using TowerOblivion.Core;
using TowerOblivion.Gameplay.Content;

namespace TowerOblivion.Gameplay
{
    public sealed class RoomLoaded : IGameEvent
    {
        public RoomContentDefinition Room { get; }

        public RoomLoaded(RoomContentDefinition room)
        {
            Room = room;
        }
    }
}
