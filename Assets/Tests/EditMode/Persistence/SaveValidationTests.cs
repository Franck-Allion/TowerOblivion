using System;
using NUnit.Framework;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Persistence;
using TowerOblivion.Gameplay.RunGeneration;
using TowerOblivion.Infrastructure.Persistence;

using TowerOblivion.Gameplay.Content;
namespace TowerOblivion.Tests.EditMode.Persistence
{
    public sealed class SaveValidationTests
    {
        private SaveDataValidator _validator;
        private ContentVersion _currentVersion;

        [SetUp]
        public void SetUp()
        {
            _currentVersion = new ContentVersion("content.1.2.3");
            _validator = new SaveDataValidator(_currentVersion);
        }

        [Test]
        public void Validate_ValidSnapshot_ReturnsSuccess()
        {
            var snapshot = CreateValidSnapshot();
            var result = _validator.Validate(snapshot);
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void Validate_SoftVersionMatch_ReturnsSuccess()
        {
            var snapshot = CreateValidSnapshot();
            // Different minor/patch but same major (1)
            snapshot.Metadata.ContentVersion = new ContentVersion("content.1.0.0");
            
            var result = _validator.Validate(snapshot);
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void Validate_MismatchedMajorVersion_ReturnsFailure()
        {
            var snapshot = CreateValidSnapshot();
            snapshot.Metadata.ContentVersion = new ContentVersion("content.2.0.0");

            var result = _validator.Validate(snapshot);
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.ErrorCode, Is.EqualTo("save.incompatible_version"));
        }

        [Test]
        public void Validate_MissingProfile_ReturnsFailure()
        {
            var snapshot = CreateValidSnapshot();
            snapshot.PlayerProfile = null;

            var result = _validator.Validate(snapshot);
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.ErrorCode, Is.EqualTo("save.missing_profile"));
        }

        [Test]
        public void Validate_MissingNarrative_ReturnsFailure()
        {
            var snapshot = CreateValidSnapshot();
            snapshot.Narrative = null;

            var result = _validator.Validate(snapshot);
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.ErrorCode, Is.EqualTo("save.missing_narrative"));
        }

        [Test]
        public void Validate_InvalidRunIdFormat_ReturnsFailure()
        {
            var snapshot = CreateValidSnapshot();
            snapshot.ActiveRun = new RunState { RunId = new RunId("INVALID ID") };

            var result = _validator.Validate(snapshot);
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.ErrorCode, Is.EqualTo("save.invalid_id"));
        }

        [Test]
        public void Validate_ValidSnakeCaseId_ReturnsSuccess()
        {
            var snapshot = CreateValidSnapshot();
            snapshot.ActiveRun = new RunState { RunId = new RunId("run.my_id_123") };

            var result = _validator.Validate(snapshot);
            Assert.That(result.IsSuccess, Is.True);
        }

        private SaveSnapshotV1 CreateValidSnapshot()
        {
            return SaveSnapshotFactory.CreateDefault(_currentVersion, new SaveVersion(1), DateTime.UtcNow);
        }
    }
}
