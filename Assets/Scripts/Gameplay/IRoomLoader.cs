using TowerOblivion.Core;

namespace TowerOblivion.Gameplay
{
    public interface IRoomLoader
    {
        Result LoadRoom(RoomId roomId);
    }
}
