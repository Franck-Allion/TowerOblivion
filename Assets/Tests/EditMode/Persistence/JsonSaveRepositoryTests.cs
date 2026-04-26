using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Narrative;
using TowerOblivion.Gameplay.Persistence;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.Progression;
using TowerOblivion.Infrastructure.Persistence;

namespace TowerOblivion.Tests.EditMode.Persistence
{
    public sealed class JsonSaveRepositoryTests
    {
        private string _temporaryDirectory;
        private SaveDataValidator _validator;
        private ContentVersion _contentVersion;

        [SetUp]
        public void SetUp()
        {
            _temporaryDirectory = Path.Combine(Path.GetTempPath(), "TowerOblivion_SaveTests", Guid.NewGuid().ToString("N"));
            _contentVersion = new ContentVersion("content.0.1.0");
            _validator = new SaveDataValidator(_contentVersion);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_temporaryDirectory))
            {
                Directory.Delete(_temporaryDirectory, true);
            }
        }

        [Test]
        public void SaveAndLoad_RoundTripsRepresentativeProfileAndNarrativeData()
        {
            var repository = new JsonSaveRepository(_temporaryDirectory, _validator);
            var service = new SaveService(repository);
            var saveVersion = new SaveVersion(1);

            var snapshot = service.CreateDefaultSnapshot(_contentVersion, saveVersion, new DateTime(2026, 4, 26, 8, 30, 0, DateTimeKind.Utc));
            snapshot.PlayerProfile.SettingsProfileId = new SettingsProfileId("settings.default");
            snapshot.PlayerProfile.UnlockedSouvenirIds.Add(new SouvenirId("souvenir.broken_laurel"));
            snapshot.PlayerProfile.Currencies.Add(new CurrencyAmountState
            {
                CurrencyId = new CurrencyId("currency.memory_embers"),
                Amount = 7
            });
            snapshot.Narrative.Flags.Add(new NarrativeFlagState
            {
                FlagId = new NarrativeFlagId("flag.prometheus_contacted"),
                Value = true
            });

            var saveResult = service.Save(SaveSlot.Primary, snapshot);
            var loadResult = service.Load(SaveSlot.Primary);

            Assert.That(saveResult.IsSuccess, Is.True);
            Assert.That(loadResult.Result.IsSuccess, Is.True);
            Assert.That(loadResult.HasSnapshot, Is.True);
            Assert.That(loadResult.Snapshot.PlayerProfile.UnlockedSouvenirIds.Count, Is.EqualTo(1));
            Assert.That(loadResult.Snapshot.PlayerProfile.Currencies[0].Amount, Is.EqualTo(7));
            Assert.That(loadResult.Snapshot.Narrative.Flags[0].FlagId.Value, Is.EqualTo("flag.prometheus_contacted"));
            Assert.That(loadResult.Snapshot.Metadata.ContentVersion, Is.EqualTo(_contentVersion));
            Assert.That(loadResult.Snapshot.Metadata.SaveVersion, Is.EqualTo(saveVersion));
        }

        [Test]
        public void Load_MissingFile_ReturnsExplicitNotFoundResult()
        {
            var repository = new JsonSaveRepository(_temporaryDirectory, _validator);
            var loadResult = repository.Load(new SaveSlot("profile_a"));

            Assert.That(loadResult.Result.IsFailure, Is.True);
            Assert.That(loadResult.Result.ErrorCode, Is.EqualTo("save.not_found"));
            Assert.That(loadResult.HasSnapshot, Is.False);
        }

        [Test]
        public void SaveAndLoad_InvalidSlot_ReturnsExplicitInvalidSlotResult()
        {
            var repository = new JsonSaveRepository(_temporaryDirectory, _validator);
            var snapshot = SaveSnapshotFactory.CreateDefault(
                _contentVersion,
                new SaveVersion(1),
                new DateTime(2026, 4, 26, 8, 30, 0, DateTimeKind.Utc));
            var invalidSlot = new SaveSlot("../outside");

            var saveResult = repository.Save(invalidSlot, snapshot);
            var loadResult = repository.Load(invalidSlot);

            Assert.That(saveResult.IsFailure, Is.True);
            Assert.That(saveResult.ErrorCode, Is.EqualTo("save.invalid_slot"));
            Assert.That(loadResult.Result.IsFailure, Is.True);
            Assert.That(loadResult.Result.ErrorCode, Is.EqualTo("save.invalid_slot"));
            Assert.That(loadResult.HasSnapshot, Is.False);
        }

        [Test]
        public void SaveAndLoad_SlotNameMatching_IsCaseInsensitive()
        {
            var repository = new JsonSaveRepository(_temporaryDirectory, _validator);
            var snapshot = SaveSnapshotFactory.CreateDefault(
                _contentVersion,
                new SaveVersion(1),
                new DateTime(2026, 4, 26, 8, 30, 0, DateTimeKind.Utc));
            var upperCaseSlot = new SaveSlot("Profile_A");
            var lowerCaseSlot = new SaveSlot("profile_a");

            var saveResult = repository.Save(upperCaseSlot, snapshot);
            var loadResult = repository.Load(lowerCaseSlot);

            Assert.That(saveResult.IsSuccess, Is.True);
            Assert.That(loadResult.Result.IsSuccess, Is.True);
            Assert.That(loadResult.HasSnapshot, Is.True);
        }

        [Test]
        public void Load_CorruptedData_ReturnsValidationError()
        {
            var repository = new JsonSaveRepository(_temporaryDirectory, _validator);
            var slot = new SaveSlot("corrupt");
            var path = Path.Combine(_temporaryDirectory, "corrupt.save.json");
            
            Directory.CreateDirectory(_temporaryDirectory);
            // Valid JSON but invalid content (wrong version)
            File.WriteAllText(path, "{\"Metadata\": {\"ContentVersion\": {\"Value\": \"invalid\"}, \"SaveVersion\": {\"Value\": 1}}}");

            var result = repository.Load(slot);

            Assert.That(result.Result.IsFailure, Is.True);
            Assert.That(result.Result.ErrorCode, Is.EqualTo("save.incompatible_version"));
        }

        [Test]
        public void Load_CorruptedPrimaryWithValidBackup_ReturnsBackupData()
        {
            var repository = new JsonSaveRepository(_temporaryDirectory, _validator);
            var slot = new SaveSlot("fallback");
            var primaryPath = Path.Combine(_temporaryDirectory, "fallback.save.json");
            var backupPath = Path.Combine(_temporaryDirectory, "fallback.save.json.bak");
            
            Directory.CreateDirectory(_temporaryDirectory);
            
            // 1. Create a valid backup
            var snapshot = SaveSnapshotFactory.CreateDefault(_contentVersion, new SaveVersion(1), DateTime.UtcNow);
            snapshot.PlayerProfile.SettingsProfileId = new SettingsProfileId("settings.backup");
            var json = JsonSerializationHelper.ToJson(snapshot);
            File.WriteAllText(backupPath, json);

            // 2. Create a corrupted primary
            File.WriteAllText(primaryPath, "THIS IS CORRUPT JSON");

            // 3. Load should fallback to backup
            var result = repository.Load(slot);

            Assert.That(result.Result.IsSuccess, Is.True);
            Assert.That(result.Snapshot.PlayerProfile.SettingsProfileId.Value, Is.EqualTo("settings.backup"));
        }

        [Test]
        public void SaveSlot_Equality_IsCaseInsensitive()
        {
            var upperCaseSlot = new SaveSlot("Profile_A");
            var lowerCaseSlot = new SaveSlot("profile_a");

            Assert.That(upperCaseSlot.Equals(lowerCaseSlot), Is.True);
            Assert.That(upperCaseSlot.GetHashCode(), Is.EqualTo(lowerCaseSlot.GetHashCode()));
        }
    }
}
