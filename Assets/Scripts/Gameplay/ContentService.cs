namespace TowerOblivion.Gameplay
{
    public sealed class ContentService
    {
        public IRoomLoader RoomLoader { get; }

        public ContentService(IRoomLoader roomLoader)
        {
            RoomLoader = roomLoader;
        }
    }
}
