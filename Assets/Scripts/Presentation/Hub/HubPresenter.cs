using UnityEngine;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Progression;
using TowerOblivion.Gameplay.Persistence;
using UnityEngine.Localization;
using System;

namespace TowerOblivion.Presentation.Hub
{
    public sealed class HubPresenter : MonoBehaviour
    {
        [SerializeField] private HubView _view;
        
        [SerializeField] private LocalizedString _progressionString = new LocalizedString("UI", "Hub.Label.Progression");
        [SerializeField] private LocalizedString _saveStatusString = new LocalizedString("UI", "Hub.Label.SaveStatus");
        [SerializeField] private LocalizedString _startButtonString = new LocalizedString("UI", "Hub.StartButton");

        private IEventBus _eventBus;
        private PlayerProfileState _profile;
        private SaveMetadata _metadata;
        private bool _isInitialized;

        public void Initialize(PlayerProfileState profile, SaveMetadata metadata, IEventBus eventBus)
        {
            if (_isInitialized)
            {
                RefreshView();
                return;
            }

            _profile = profile;
            _metadata = metadata;
            _eventBus = eventBus;

            if (_view != null)
            {
                PrepareLocalizationArguments();

                _view.StartRunClicked += OnStartRunClicked;
                
                // Bind localized strings to view for reactive updates (e.g. locale change)
                _progressionString.StringChanged += _view.SetProgressionSummary;
                _saveStatusString.StringChanged += _view.SetSaveStatus;
                _startButtonString.StringChanged += _view.SetStartButtonText;

                // Initial synchronous update
                RefreshView();
            }

            _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (_view != null)
            {
                _view.StartRunClicked -= OnStartRunClicked;
            }

            _progressionString.StringChanged -= _view.SetProgressionSummary;
            _saveStatusString.StringChanged -= _view.SetSaveStatus;
            _startButtonString.StringChanged -= _view.SetStartButtonText;
        }

        private void OnStartRunClicked()
        {
            // Intent: Player wants to start
            _eventBus.Publish(new StartRunRequested());
        }

        private void RefreshView()
        {
            PrepareLocalizationArguments();

            // Synchronous update for the first display to meet AAA standards (no flicker)
            if (_view != null)
            {
                _view.SetProgressionSummary(_progressionString.GetLocalizedStringAsync().WaitForCompletion());
                _view.SetSaveStatus(_saveStatusString.GetLocalizedStringAsync().WaitForCompletion());
                _view.SetStartButtonText(_startButtonString.GetLocalizedStringAsync().WaitForCompletion());
            }
        }

        private void PrepareLocalizationArguments()
        {
            if (_profile != null)
            {
                var souvenirsCount = _profile.UnlockedSouvenirIds.Count;
                var level = _profile.Level;
                
                // Update Smart String arguments: {0} = Souvenirs, {1} = Level
                _progressionString.Arguments = new object[] { souvenirsCount, level };
            }
            else
            {
                _progressionString.Arguments = new object[] { 0, 0 };
            }

            if (_metadata != null)
            {
                // Update Smart String arguments: {0} = Date
                _saveStatusString.Arguments = new object[] { _metadata.SavedAtUtc.ToLocalTime() };
            }
            else
            {
                _saveStatusString.Arguments = new object[] { DateTime.MinValue };
            }
        }
        
        // For testing
        public void InitializeView(HubView view)
        {
            _view = view;
        }
    }
}
