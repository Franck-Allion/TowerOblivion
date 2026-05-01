using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Persistence;
using TowerOblivion.Gameplay.RunGeneration;
using TowerOblivion.Infrastructure.Persistence;
using TowerOblivion.Infrastructure.Content;
using TowerOblivion.Infrastructure.Content.Conversion;
using TowerOblivion.Infrastructure.Content.Validation;
using TowerOblivion.Presentation.SceneFlow;

namespace TowerOblivion.Presentation
{
    /// <summary>
    /// Global composition root for presentation services.
    /// Ensures core infrastructure (EventBus, Save, Orchestration, Content) is available across scenes.
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
        public ContentService ContentService { get; private set; }
        public ContentVersion ContentVersion { get; private set; } = new("content.0.1.0");
        public RunState ActiveRun { get; private set; }

        [Header("Content Authoring")]
        [SerializeField] private PlaceholderContentAuthoringSet _authoringSet;

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
            try
            {
                // AAA standard: Ensure localization is fully initialized
                LocalizationSettings.InitializationOperation.WaitForCompletion();

                // 1. Content Bootstrapping & Validation
                var authoringSet = _authoringSet != null ? _authoringSet : PlaceholderDataFixture.Create();
                var converter = new PlaceholderContentConverter();
                var catalogs = converter.Convert(authoringSet);

                var validator = new PlaceholderContentValidator();
                var validationResult = validator.Validate(catalogs);

                if (validationResult.IsFailure)
                {
                    Debug.LogError($"[Content Validation Failed] {validationResult.ErrorCode}: {validationResult.ErrorMessage}");
                    FailClosed();
                    return;
                }

                ContentService = new ContentService(catalogs);

                // 2. Core Services
                EventBus = new SimpleEventBus();
                GameStateOrchestrator = new GameStateOrchestrator(EventBus);
                EventBus.Subscribe<RunStarted>(evt => ActiveRun = evt.InitialState);

                // 3. Infrastructure
                var savePath = Application.persistentDataPath;
                var saveDataValidator = new SaveDataValidator(ContentVersion);
                var repository = new JsonSaveRepository(savePath, saveDataValidator);
                SaveService = new SaveService(repository);

                // 4. Global Presenters
                var presenter = gameObject.AddComponent<GameStateOrchestratorPresenter>();
                presenter.Bind(EventBus);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                FailClosed();
            }
        }

        private void FailClosed()
        {
            if (_instance == this) _instance = null;
            Destroy(gameObject);
        }
    }
}
