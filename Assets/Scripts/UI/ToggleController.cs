using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum ToggleState
    {
        NotActive = 0,
        Active = 1
    }

    public class ToggleController : MonoBehaviour
    {
        [SerializeField] private Image sliderBg;
        [SerializeField] private List<Sprite> sprites;

        private ToggleState _toggleState = ToggleState.Active;

        public void Awake()
        {
            _toggleState = ToggleState.Active;
            sliderBg.sprite = sprites[(int) _toggleState];
        }

        public void Switch()
        {
            _toggleState = _toggleState == ToggleState.Active ? ToggleState.NotActive : ToggleState.Active;
            AudioListener.volume = _toggleState == ToggleState.Active ? 1 : 0;
            sliderBg.sprite = sprites[(int) _toggleState];
        }
    }
}