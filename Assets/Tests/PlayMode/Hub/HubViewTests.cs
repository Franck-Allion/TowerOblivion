using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.Persistence;
using TowerOblivion.Gameplay.Progression;
using TowerOblivion.Presentation.Hub;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace TowerOblivion.Tests.PlayMode.Hub
{
    public sealed class HubViewTests
    {
        [UnityTest]
        public IEnumerator HubPresenter_UpdatesViewOnInitialize()
        {
            // Setup
            var go = new GameObject("HubViewTest");
            var view = go.AddComponent<HubView>();
            var presenter = go.AddComponent<HubPresenter>();

            var buttonGo = new GameObject("StartButton");
            buttonGo.transform.SetParent(go.transform);
            var startButton = buttonGo.AddComponent<Button>();

            var progressionSummaryLabel = new GameObject("ProgressionLabel").AddComponent<TextMeshProUGUI>();     
            progressionSummaryLabel.transform.SetParent(go.transform);
            progressionSummaryLabel.text = "Initial";

            var saveStatusLabel = new GameObject("SaveLabel").AddComponent<TextMeshProUGUI>();
            saveStatusLabel.transform.SetParent(go.transform);
            saveStatusLabel.text = "Initial";

            view.Initialize(startButton, progressionSummaryLabel, saveStatusLabel);
            presenter.InitializeView(view);

            var eventBus = new FakeEventBus();
            var profile = new PlayerProfileState();
            profile.UnlockedSouvenirIds.Add(new SouvenirId("souvenir.1"));

            var metadata = new SaveMetadata
            {
                SavedAtUtc = new DateTime(2026, 4, 26, 12, 0, 0, DateTimeKind.Utc)
            };

            // Act
            presenter.Initialize(profile, metadata, eventBus);

            // Wait a few frames for potential async updates
            for (int i = 0; i < 5; i++) yield return null;

            // Assert
            Assert.IsNotNull(progressionSummaryLabel.text, "Progression label text should not be null");
            Assert.IsNotNull(saveStatusLabel.text, "Save status label text should not be null");

            // Cleanup
            UnityEngine.Object.DestroyImmediate(go);
        }

        [UnityTest]
        public IEnumerator StartRunButton_PublishesStartRunRequested()
        {
            // Setup
            var go = new GameObject("HubViewTest");
            var view = go.AddComponent<HubView>();
            var presenter = go.AddComponent<HubPresenter>();

            var buttonGo = new GameObject("StartButton");
            buttonGo.transform.SetParent(go.transform);
            var startButton = buttonGo.AddComponent<Button>();

            var progressionSummaryLabel = new GameObject("ProgressionLabel").AddComponent<TextMeshProUGUI>();     
            progressionSummaryLabel.transform.SetParent(go.transform);

            var saveStatusLabel = new GameObject("SaveLabel").AddComponent<TextMeshProUGUI>();
            saveStatusLabel.transform.SetParent(go.transform);

            view.Initialize(startButton, progressionSummaryLabel, saveStatusLabel);
            presenter.InitializeView(view);

            var eventBus = new FakeEventBus();
            presenter.Initialize(new PlayerProfileState(), new SaveMetadata(), eventBus);

            // Act
            startButton.onClick.Invoke();

            yield return null;

            // Assert
            Assert.That(eventBus.PublishedEvents, Has.Count.EqualTo(1));
            Assert.That(eventBus.PublishedEvents[0], Is.InstanceOf<StartRunRequested>());

            // Cleanup
            UnityEngine.Object.DestroyImmediate(go);
        }

        private sealed class FakeEventBus : IEventBus
        {
            public List<IGameEvent> PublishedEvents { get; } = new List<IGameEvent>();

            public void Publish<TEvent>(TEvent evt) where TEvent : IGameEvent
            {
                PublishedEvents.Add(evt);
            }

            public IDisposable Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent
            {
                return null;
            }
        }
    }
}
