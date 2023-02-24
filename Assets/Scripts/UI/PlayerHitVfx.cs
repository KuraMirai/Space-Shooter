using System.Collections;
using Extensions;
using Managers;
using UnityEngine;

namespace UI
{
    public class PlayerHitVfx : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
    
        private Coroutine _playerHitRoutine;

        private void Awake()
        {
            EventManager.StartListening(Constants.PlayerHit, Hit);
        }

        private void Hit()
        {
            if (_playerHitRoutine == null)
            {
                _playerHitRoutine = StartCoroutine(HitVfx());
            }
        }

        private IEnumerator HitVfx()                                   
        {
            canvasGroup.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            canvasGroup.SetActive(false);
            StopCoroutine(_playerHitRoutine);
            _playerHitRoutine = null;
        }
    }
}
