using System;
using System.Linq;
using NUnit.Framework;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Combat;

namespace TowerOblivion.Tests.EditMode
{
    public class AssemblyBoundaryTests
    {
        [Test]
        public void CoreAssembly_DoesNotReferenceProjectSpecificOrUnityAssemblies()
        {
            var references = typeof(Result).Assembly.GetReferencedAssemblies()
                .Select(reference => reference.Name)
                .ToArray();

            Assert.That(references.Any(IsUnityAssembly), Is.False);
            Assert.That(references.Any(name => name.StartsWith("TowerOblivion.", StringComparison.Ordinal)), Is.False);
        }

        [Test]
        public void GameplayAssembly_ReferencesCoreOnlyAmongProjectAssemblies()
        {
            var references = typeof(CombatState).Assembly.GetReferencedAssemblies()
                .Select(reference => reference.Name)
                .ToArray();

            Assert.That(references, Does.Contain("TowerOblivion.Core"));
            Assert.That(references.Where(name => name.StartsWith("TowerOblivion.", StringComparison.Ordinal)), Is.EquivalentTo(new[] { "TowerOblivion.Core" }));
            Assert.That(references.Any(IsUnityAssembly), Is.False);
        }

        private static bool IsUnityAssembly(string assemblyName)
        {
            return assemblyName.StartsWith("UnityEngine", StringComparison.Ordinal)
                || assemblyName.StartsWith("Unity.", StringComparison.Ordinal)
                || assemblyName.StartsWith("UnityEditor", StringComparison.Ordinal);
        }
    }
}
