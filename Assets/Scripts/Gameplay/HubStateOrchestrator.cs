using System;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.RunGeneration;
using TowerOblivion.Gameplay.Content;

namespace TowerOblivion.Gameplay
{
    public sealed class HubStateOrchestrator
    {
        private readonly IEventBus _eventBus;
        private readonly ContentVersion _contentVersion;

        public HubStateOrchestrator(IEventBus eventBus, ContentVersion contentVersion)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _contentVersion = contentVersion;
        }

        public void EnterHub()
        {
            _eventBus.Publish(new HubEntered());
        }

        public RunState CreateNewRun(RunId runId, RunSeed seed)
        {
            var runState = new RunState
            {
                RunId = runId,
                Seed = seed,
                ContentVersion = _contentVersion,
                CurrentFloorIndex = 0,
                ActiveRoomId = new RoomId("room.entry") // First exploration room
            };

            // Fact: The run has started
            _eventBus.Publish(new RunStarted(runState));

            return runState;
        }
    }
}
