using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Content;

namespace TowerOblivion.Infrastructure.Content
{
    public sealed class RoomLoader : IRoomLoader
    {
        private readonly IContentCatalog<RoomId, RoomContentDefinition> _catalog;
        private readonly RoomStateOrchestrator _orchestrator;

        public RoomLoader(
            IContentCatalog<RoomId, RoomContentDefinition> catalog,
            RoomStateOrchestrator orchestrator)
        {
            _catalog = catalog;
            _orchestrator = orchestrator;
        }

        public Result LoadRoom(RoomId roomId)
        {
            return _orchestrator.LoadRoom(roomId);
        }
    }
}
