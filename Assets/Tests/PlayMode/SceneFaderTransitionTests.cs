using System.Collections;
using NUnit.Framework;
using TowerOblivion.Presentation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TowerOblivion.Tests.PlayMode
{
    public sealed class SceneFaderTransitionTests
    {
        [UnityTest]
        public IEnumerator TransitionToScene_LoadsTargetSceneAndRevealsFromBlack()
        {
            var faderObject = new GameObject("SceneFaderTransitionTest", typeof(RectTransform));
            var fader = faderObject.AddComponent<SceneFader>();

            yield return null;

            fader.TransitionToScene("Rooms");

            const float timeoutSeconds = 8f;
            var deadline = Time.realtimeSinceStartup + timeoutSeconds;
            while (SceneManager.GetActiveScene().name != "Rooms" && Time.realtimeSinceStartup < deadline)
            {
                yield return null;
            }

            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Rooms"));

            var canvasGroup = fader.GetComponent<CanvasGroup>();
            deadline = Time.realtimeSinceStartup + timeoutSeconds;
            while (canvasGroup != null && canvasGroup.alpha > 0.01f && Time.realtimeSinceStartup < deadline)
            {
                yield return null;
            }

            Assert.That(canvasGroup, Is.Not.Null);
            Assert.That(canvasGroup.alpha, Is.LessThanOrEqualTo(0.01f));

            Object.Destroy(faderObject);
        }
    }
}
