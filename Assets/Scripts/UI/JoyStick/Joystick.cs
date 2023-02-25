using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace UI.JoyStick
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;

        public event Action<float> Horizontal;
        public event Action<float> Vertical;

        private Vector2 _input = Vector2.zero;
        private Vector2 _startPos;

        protected virtual void Start()
        {
            _startPos = background.position;
        }

        private void UpdateInputPosition(Vector2 pos)
        {
            var delta = pos - _startPos;
            Vector2 radius = background.sizeDelta / 2;
            _input = delta / radius;
            HandleInput(_input.magnitude, _input.normalized);
            handle.anchoredPosition = _input * radius;
        }

        private void HandleInput(float magnitude, Vector2 normalised)
        {
            if (magnitude > 1)
                _input = normalised;
            Horizontal?.Invoke(_input.x);
            Vertical?.Invoke(_input.y);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            UpdateInputPosition(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateInputPosition(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            Horizontal?.Invoke(_input.x);
            Vertical?.Invoke(_input.y);
        }
    }
}