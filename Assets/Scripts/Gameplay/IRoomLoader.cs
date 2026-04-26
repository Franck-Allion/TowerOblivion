using TowerOblivion.Core;

using TowerOblivion.Gameplay.Content;
namespace TowerOblivion.Gameplay
{
    public interface IRoomLoader
    {
        Result LoadRoom(RoomId roomId);
    }
}
