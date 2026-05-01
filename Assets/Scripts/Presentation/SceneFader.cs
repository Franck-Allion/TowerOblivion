using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerOblivion.Presentation
{
    public sealed class SceneFader : MonoBehaviour
    {
        [Header("Timing & Easing")]
        [SerializeField] private float _fadeDuration = 0.65f;
        [SerializeField] private float _minimumLoadingDuration = 0.45f;
        [SerializeField] private float _postActivationSettleDuration = 0.15f;
        [SerializeField] private float _roomRevealOverlapDelay = 0.15f;
        [SerializeField] private AnimationCurve _fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("References")]
        [SerializeField] private CanvasGroup _mainCanvasGroup;
        [SerializeField] private CanvasGroup _loadingIconGroup;
        [SerializeField] private RectTransform _loadingSpinner;

        private static SceneFader _instance;
        private bool _isTransitioning;
        public bool IsTransitioning => _isTransitioning;

        public static SceneFader Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            transform.SetParent(null, false);
            transform.localScale = Vector3.one;
            DontDestroyOnLoad(gameObject);
            EnsureReferences();
            SetupInitialState();
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private void SetupInitialState()
        {
            EnsureReferences();

            _mainCanvasGroup.alpha = 0;
            _mainCanvasGroup.blocksRaycasts = false;
            _loadingIconGroup.alpha = 0;
            _loadingIconGroup.blocksRaycasts = false;
        }

        public Coroutine FadeOverlayIn() => StartCoroutine(FadeCanvas(_mainCanvasGroup, 1f, true));
        public Coroutine FadeOverlayOut() => StartCoroutine(FadeCanvas(_mainCanvasGroup, 0f, false));
        public Coroutine FadeLoadingIn() => StartCoroutine(FadeCanvas(_loadingIconGroup, 1f, false));
        public Coroutine FadeLoadingOut() => StartCoroutine(FadeCanvas(_loadingIconGroup, 0f, false));

        public void TransitionToScene(string sceneName)
        {
            if (_isTransitioning)
            {
                return;
            }

            StartCoroutine(TransitionToSceneSequence(sceneName));
        }

        private IEnumerator TransitionToSceneSequence(string sceneName)
        {
            _isTransitioning = true;

            yield return FadeCanvas(_mainCanvasGroup, 1f, true);
            yield return FadeCanvas(_loadingIconGroup, 1f, false);

            yield return new WaitForSecondsRealtime(_minimumLoadingDuration);
            SceneManager.LoadScene(sceneName);
            yield return null;
            yield return new WaitForSecondsRealtime(_postActivationSettleDuration);

            yield return RevealLoadedScene();

            _isTransitioning = false;
        }

        private IEnumerator RevealLoadedScene()
        {
            var loadingFade = StartCoroutine(FadeCanvas(_loadingIconGroup, 0f, false));
            var overlapDelay = Mathf.Clamp(_roomRevealOverlapDelay, 0f, _fadeDuration);

            if (overlapDelay > 0f)
            {
                yield return new WaitForSecondsRealtime(overlapDelay);
            }

            yield return FadeCanvas(_mainCanvasGroup, 0f, false);
            yield return loadingFade;
        }

        private IEnumerator FadeCanvas(CanvasGroup group, float targetAlpha, bool blockRaycasts)
        {
            if (group == null) yield break;

            group.blocksRaycasts = blockRaycasts;
            float startAlpha = group.alpha;
            float elapsed = 0f;

            while (elapsed < _fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = _fadeCurve.Evaluate(elapsed / _fadeDuration);
                group.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                yield return null;
            }

            group.alpha = targetAlpha;
            if (!blockRaycasts) group.blocksRaycasts = false;
        }

        private void Update()
        {
            if (_loadingSpinner != null && _loadingIconGroup != null && _loadingIconGroup.alpha > 0.01f)
            {
                _loadingSpinner.Rotate(Vector3.forward, -200f * Time.unscaledDeltaTime);
            }
        }

        private void EnsureReferences()
        {
            var canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = gameObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }

            canvas.overrideSorting = true;
            canvas.sortingOrder = short.MaxValue;

            if (GetComponent<CanvasScaler>() == null)
            {
                gameObject.AddComponent<CanvasScaler>();
            }

            if (GetComponent<GraphicRaycaster>() == null)
            {
                gameObject.AddComponent<GraphicRaycaster>();
            }

            _mainCanvasGroup ??= GetComponent<CanvasGroup>();
            if (_mainCanvasGroup == null)
            {
                _mainCanvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            EnsureRectTransformCoversScreen(transform as RectTransform);
            EnsureBlackOverlay();
            EnsureLoadingSpinner();
        }

        private void EnsureBlackOverlay()
        {
            var overlay = transform.Find("BlackOverlay") as RectTransform;
            if (overlay == null)
            {
                var overlayObject = new GameObject("BlackOverlay", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
                overlay = overlayObject.GetComponent<RectTransform>();
                overlay.SetParent(transform, false);
            }

            EnsureRectTransformCoversScreen(overlay);

            var image = overlay.GetComponent<Image>();
            if (image == null)
            {
                image = overlay.gameObject.AddComponent<Image>();
            }

            image.color = Color.black;
            image.raycastTarget = true;
        }

        private void EnsureLoadingSpinner()
        {
            if (_loadingIconGroup != null)
            {
                _loadingSpinner ??= _loadingIconGroup.transform as RectTransform;
                _loadingIconGroup.alpha = 0;
                _loadingIconGroup.blocksRaycasts = false;
                return;
            }

            var spinnerGroup = transform.Find("LoadingSpinnerGroup") as RectTransform;
            if (spinnerGroup == null)
            {
                var groupObject = new GameObject("LoadingSpinnerGroup", typeof(RectTransform), typeof(CanvasGroup));
                spinnerGroup = groupObject.GetComponent<RectTransform>();
                spinnerGroup.SetParent(transform, false);
            }

            spinnerGroup.anchorMin = new Vector2(0.5f, 0.5f);
            spinnerGroup.anchorMax = new Vector2(0.5f, 0.5f);
            spinnerGroup.pivot = new Vector2(0.5f, 0.5f);
            spinnerGroup.anchoredPosition = Vector2.zero;
            spinnerGroup.sizeDelta = new Vector2(128f, 128f);
            spinnerGroup.localScale = Vector3.one;

            _loadingIconGroup = spinnerGroup.GetComponent<CanvasGroup>();
            _loadingIconGroup.alpha = 0;
            _loadingIconGroup.blocksRaycasts = false;

            _loadingSpinner = spinnerGroup;
        }

        private static void EnsureRectTransformCoversScreen(RectTransform rectTransform)
        {
            if (rectTransform == null)
            {
                return;
            }

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.localScale = Vector3.one;
        }

        public void AssignReferences(CanvasGroup main, CanvasGroup loading, RectTransform spinner)
        {
            _mainCanvasGroup = main;
            _loadingIconGroup = loading;
            _loadingSpinner = spinner;
        }
    }
}
