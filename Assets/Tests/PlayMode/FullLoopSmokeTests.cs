using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TowerOblivion.Gameplay;
using TowerOblivion.Presentation;

namespace TowerOblivion.Tests.PlayMode
{
    public sealed class FullLoopSmokeTests
    {
        [UnityTest]
        public IEnumerator NavigateFullLoop_EndsAtHub()
        {
            Debug.Log("[Test] 1. Loading Hub");
            yield return SceneManager.LoadSceneAsync("Hub");
            yield return null; 
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Hub"));

            Debug.Log("[Test] 2. Clicking StartRunButton");
            yield return ClickButton("StartRunButton");
            yield return WaitForScene("Rooms");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Rooms"));

            Debug.Log("[Test] 3. Clicking GoToCombatButton");
            yield return ClickButton("GoToCombatButton");
            yield return WaitForScene("Combat");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Combat"));

            Debug.Log("[Test] 4. Clicking GoToRewardButton");
            yield return ClickButton("GoToRewardButton");
            yield return WaitForScene("Reward");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Reward"));

            Debug.Log("[Test] 5. Clicking ContinueButton");
            yield return ClickButton("ContinueButton");
            yield return WaitForScene("Resolution");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Resolution"));

            Debug.Log("[Test] 6. Clicking ReturnToHubButton");
            yield return ClickButton("ReturnToHubButton");
            yield return WaitForScene("Hub");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Hub"));
        }

        private IEnumerator ClickButton(string name)
        {
            GameObject go = null;
            float timeout = 5.0f;
            while (go == null && timeout > 0)
            {
                go = GameObject.Find(name);
                if (go == null)
                {
                    yield return null;
                    timeout -= Time.deltaTime;
                }
            }

            Assert.That(go, Is.Not.Null, "Button " + name + " not found");
            var button = go.GetComponent<Button>();
            Assert.That(button, Is.Not.Null, "GameObject " + name + " has no Button component");
            
            // Wait until SceneFader is not transitioning
            timeout = 5.0f;
            while (SceneFader.Instance != null && SceneFader.Instance.IsTransitioning && timeout > 0)
            {
                yield return null;
                timeout -= Time.deltaTime;
            }

            Debug.Log("[Test] Invoking onClick for " + name);
            button.onClick.Invoke();
            yield return null;
        }

        private IEnumerator WaitForScene(string sceneName)
        {
            Debug.Log("[Test] Waiting for scene: " + sceneName);
            float timeout = 15.0f; 
            while (SceneManager.GetActiveScene().name != sceneName && timeout > 0)
            {
                yield return null;
                timeout -= Time.deltaTime;
            }
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo(sceneName), "Timed out waiting for scene " + sceneName + ". Current scene: " + SceneManager.GetActiveScene().name);
            
            // Wait for transition to FINISH in the new scene
            yield return null;
            float transitionTimeout = 5.0f;
            while (SceneFader.Instance != null && SceneFader.Instance.IsTransitioning && transitionTimeout > 0)
            {
                yield return null;
                transitionTimeout -= Time.deltaTime;
            }
        }
    }
}
