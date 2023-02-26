using System;
using Managers;
using PoolFactories.Interfaces;
using UnityEngine;
using System.Collections;

namespace Bullets
{
    public class Bullet : MonoBehaviour, IPullable
    {
        [SerializeField] protected Rigidbody rb;

        protected float _moveSpeed;
        protected float _damage;
        protected bool _isPlayerBullet;
        protected Vector3 _direction;
        protected Coroutine _creationRoutine;

        public event Action<Bullet> Pushed; 

        public void Init(Vector3 direction, float moveSpeed, float damage, bool isPlayerBullet = false)
        {
            _isPlayerBullet = isPlayerBullet;
            _moveSpeed = moveSpeed;
            _damage = damage;
            _direction = direction;
            SetActive(true);
            StartCreationRoutine();
        }

        public void Push()
        {
            Pushed?.Invoke(this);
            Pushed = null;
            SetActive(false);
        }

        private void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private void Update()
        {
            if (!PauseManager.IsPause)
                rb.velocity = _direction * _moveSpeed;
            else
                rb.velocity = Vector3.zero;
        }
        
        private void StartCreationRoutine()
        {
            if (_creationRoutine == null)
            {
                _creationRoutine = StartCoroutine(CreationRoutine());
            }
        }

        public void StopCreationRoutine()
        {
            if (_creationRoutine != null)
            {
                StopCoroutine(_creationRoutine);
                _creationRoutine = null;
            }
        }
        
        private IEnumerator CreationRoutine()
        {
            yield return new WaitForSeconds(5f);
            _creationRoutine = null;
            Push();
        }
    }
}
