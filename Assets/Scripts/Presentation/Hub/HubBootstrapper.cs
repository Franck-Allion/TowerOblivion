using System;
using UnityEngine;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.Persistence;
using TowerOblivion.Gameplay.RunGeneration;
using TowerOblivion.Presentation.Hub;

namespace TowerOblivion.Presentation.Hub
{
    public sealed class HubBootstrapper : MonoBehaviour
    {
        [SerializeField] private HubPresenter _hubPresenter;

        private HubStateOrchestrator _hubOrchestrator;
        private IEventBus _eventBus;
        private GameStateOrchestrator _gameStateOrchestrator;
        private IDisposable _startRunSubscription;

        private void Start()
        {
            InitializeHub();
        }

        private void OnDestroy()
        {
            _startRunSubscription?.Dispose();
        }

        private void InitializeHub()
        {
            var global = GlobalBootstrapper.Instance;
            if (global == null)
            {
                Debug.LogError("[HubBootstrapper] GlobalBootstrapper not found. Ensure the application starts from a scene with GlobalBootstrapper or it is lazily created.");
                return;
            }

            _eventBus = global.EventBus;
            _gameStateOrchestrator = global.GameStateOrchestrator;

            _hubOrchestrator = new HubStateOrchestrator(_eventBus, global.ContentVersion);

            // 1. Handle Start Run Request (Transitions the Mode)
            _startRunSubscription?.Dispose();
            _startRunSubscription = _eventBus.Subscribe<StartRunRequested>(_ =>
            {
                _hubOrchestrator.CreateNewRun(new RunId($"run.{Guid.NewGuid()}"), new RunSeed(UnityEngine.Random.Range(0, int.MaxValue)));

                // Integration: Request high-level mode transition
                _gameStateOrchestrator.TransitionTo(GameModeId.Exploration);
            });

            // 2. Load or Create Save
            var loadResult = global.SaveService.Load(SaveSlot.Primary);
            SaveSnapshotV1 snapshot;

            if (loadResult.Result.IsSuccess && loadResult.HasSnapshot)
            {
                snapshot = loadResult.Snapshot;
            }
            else
            {
                snapshot = SaveSnapshotFactory.CreateDefault(global.ContentVersion, new SaveVersion(1), DateTime.UtcNow);
                snapshot.PlayerProfile.UnlockedSouvenirIds.Add(new SouvenirId("souvenir.broken_laurel"));      
                snapshot.PlayerProfile.Level = 1;
            }

            // 3. Initialize Presenter
            if (_hubPresenter != null)
            {
                _hubPresenter.Initialize(snapshot.PlayerProfile, snapshot.Metadata, _eventBus);
            }

            // 4. Finalize Entry
            _hubOrchestrator.EnterHub();
        }
    }
}
