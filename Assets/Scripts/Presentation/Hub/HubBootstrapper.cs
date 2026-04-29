using System;
using UnityEngine;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.Persistence;
using TowerOblivion.Gameplay.RunGeneration;
using TowerOblivion.Infrastructure.Persistence;

namespace TowerOblivion.Presentation.Hub
{
    public sealed class HubBootstrapper : MonoBehaviour
    {
        [SerializeField] private HubPresenter _hubPresenter;
        [SerializeField] private HubSceneLoader _sceneLoader;
        
        private HubStateOrchestrator _orchestrator;
        private IEventBus _eventBus;

        private void Awake()
        {
            // AAA standard: Ensure localization is fully initialized before any OnEnable/Start
            // to avoid English-to-translated flicker on the first frame.
            UnityEngine.Localization.Settings.LocalizationSettings.InitializationOperation.WaitForCompletion();
        }

        private void Start()
        {
            InitializeHub();
        }

        private void InitializeHub()
        {
            // 1. Setup Infrastructure
            var savePath = Application.persistentDataPath;
            var contentVersion = new ContentVersion("content.0.1.0");
            var validator = new SaveDataValidator(contentVersion);
            var repository = new JsonSaveRepository(savePath, validator);
            var saveService = new SaveService(repository);
            
            _eventBus = new SimpleEventBus();
            _orchestrator = new HubStateOrchestrator(_eventBus, contentVersion);

            // 2. Setup Scene Loader
            if (_sceneLoader == null)
            {
                _sceneLoader = gameObject.AddComponent<HubSceneLoader>();
            }
            _sceneLoader.Bind(_eventBus);

            // 3. Handle Start Run Request
            _eventBus.Subscribe<StartRunRequested>(_ => 
            {
                _orchestrator.CreateNewRun(new RunId($"run.{Guid.NewGuid()}"), new RunSeed(UnityEngine.Random.Range(0, int.MaxValue)));
            });

            // 4. Load or Create Save
            var loadResult = saveService.Load(SaveSlot.Primary);
            SaveSnapshotV1 snapshot;
            
            if (loadResult.Result.IsSuccess && loadResult.HasSnapshot)
            {
                snapshot = loadResult.Snapshot;
            }
            else
            {
                // Create a dummy starting profile if no save exists
                snapshot = SaveSnapshotFactory.CreateDefault(contentVersion, new SaveVersion(1), DateTime.UtcNow);
                // Add some dummy progression data for visual verification
                snapshot.PlayerProfile.UnlockedSouvenirIds.Add(new SouvenirId("souvenir.broken_laurel"));
                snapshot.PlayerProfile.Level = 1;
            }

            // 5. Initialize Presenter
            if (_hubPresenter != null)
            {
                _hubPresenter.Initialize(snapshot.PlayerProfile, snapshot.Metadata, _eventBus);
            }

            // 6. Finalize Entry
            _orchestrator.EnterHub();
        }
    }
}
