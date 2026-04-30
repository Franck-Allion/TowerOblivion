using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace TowerOblivion.Presentation.UI
{
    /// <summary>
    /// Provides visual feedback (scale and optional color tint) when interacting with a UI button.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class UIButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Scale Settings")]
        [SerializeField] private float _hoverScale = 1.05f;
        [SerializeField] private float _pressedScale = 0.95f;
        [SerializeField] private float _animationDuration = 0.1f;

        [Header("Color Settings")]
        [SerializeField] private bool _useColorTint = false;
        [SerializeField] private Color _hoverColor = new Color(1.1f, 1.1f, 1.1f, 1f);
        
        private Vector3 _originalScale;
        private Image _image;
        private Color _normalColor;

        private void Awake()
        {
            _originalScale = transform.localScale;
            _image = GetComponent<Image>();
            if (_image != null) _normalColor = _image.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ApplyScale(_originalScale * _hoverScale);
            if (_useColorTint && _image != null) _image.color = _hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ApplyScale(_originalScale);
            if (_useColorTint && _image != null) _image.color = _normalColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ApplyScale(_originalScale * _pressedScale);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera))
            {
                ApplyScale(_originalScale * _hoverScale);
            }
            else
            {
                ApplyScale(_originalScale);
            }
        }

        private void ApplyScale(Vector3 targetScale)
        {
            StopAllCoroutines();
            StartCoroutine(ScaleRoutine(targetScale));
        }

        private IEnumerator ScaleRoutine(Vector3 targetScale)
        {
            float elapsed = 0;
            Vector3 startScale = transform.localScale;
            while (elapsed < _animationDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / _animationDuration);
                yield return null;
            }
            transform.localScale = targetScale;
        }

        private void OnDisable()
        {
            transform.localScale = _originalScale;
            if (_useColorTint && _image != null) _image.color = _normalColor;
        }
    }
}
