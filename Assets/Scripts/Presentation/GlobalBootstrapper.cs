using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Persistence;
using TowerOblivion.Infrastructure.Persistence;
using TowerOblivion.Presentation.SceneFlow;

namespace TowerOblivion.Presentation
{
    /// <summary>
    /// Global composition root for presentation services.
    /// Ensures core infrastructure (EventBus, Save, Orchestration) is available across scenes.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public sealed class GlobalBootstrapper : MonoBehaviour
    {
        private static GlobalBootstrapper _instance;
        public static GlobalBootstrapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    var existing = FindFirstObjectByType<GlobalBootstrapper>();
                    if (existing != null)
                    {
                        _instance = existing;
                    }
                    else
                    {
                        var go = new GameObject("GlobalBootstrapper (Auto-Created)");
                        _instance = go.AddComponent<GlobalBootstrapper>();
                    }
                }
                return _instance;
            }
        }

        public IEventBus EventBus { get; private set; }
        public SaveService SaveService { get; private set; }
        public GameStateOrchestrator GameStateOrchestrator { get; private set; }
        public ContentVersion ContentVersion { get; private set; } = new("content.0.1.0");

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeGlobalState();
        }

        private void InitializeGlobalState()
        {
            // AAA standard: Ensure localization is fully initialized
            LocalizationSettings.InitializationOperation.WaitForCompletion();

            // 1. Core Services
            EventBus = new SimpleEventBus();
            GameStateOrchestrator = new GameStateOrchestrator(EventBus);

            // 2. Infrastructure
            var savePath = Application.persistentDataPath;
            var validator = new SaveDataValidator(ContentVersion);
            var repository = new JsonSaveRepository(savePath, validator);
            SaveService = new SaveService(repository);

            // 3. Global Presenters
            var presenter = gameObject.AddComponent<GameStateOrchestratorPresenter>();
            presenter.Bind(EventBus);
        }
    }
}
