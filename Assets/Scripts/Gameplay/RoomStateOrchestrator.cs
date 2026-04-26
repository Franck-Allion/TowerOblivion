using TowerOblivion.Core;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.RunGeneration;

namespace TowerOblivion.Gameplay
{
    public sealed class RoomStateOrchestrator 
    {
        private readonly RunState _runState;
        private readonly IEventBus _eventBus;
        private readonly IContentCatalog<RoomId, RoomContentDefinition> _roomCatalog;

        public RoomStateOrchestrator(
            RunState runState,
            IEventBus eventBus,
            IContentCatalog<RoomId, RoomContentDefinition> roomCatalog)
        {
            _runState = runState;
            _eventBus = eventBus;
            _roomCatalog = roomCatalog;
        }

        public Result LoadRoom(RoomId roomId)
        {
            if (!_roomCatalog.TryGet(roomId, out var roomDefinition))
            {
                return Result.Failure("exploration.room_not_found", $"Room with ID {roomId} not found in catalog.");
            }

            _runState.ActiveRoomId = roomId;
            
            if (!_runState.VisitedRoomIds.Contains(roomId))
            {
                _runState.VisitedRoomIds.Add(roomId);
            }
            
            _eventBus.Publish(new RoomLoaded(roomDefinition));

            return Result.Success();
        }
    }
}
