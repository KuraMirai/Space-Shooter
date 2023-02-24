using Managers;
using UnityEngine;

namespace Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] protected Rigidbody rb;

        protected float _moveSpeed;
        protected float _damage;
        protected bool _isPlayerBullet;
        protected Vector3 _direction;


        public void Init(Vector3 direction, float moveSpeed, float damage, bool isPlayerBullet = false)
        {
            _isPlayerBullet = isPlayerBullet;
            _moveSpeed = moveSpeed;
            _damage = damage;
            _direction = direction;
        }

        private void Update()
        {
            if (!PauseManager.IsPause)
                rb.velocity = _direction * _moveSpeed;
            else
                rb.velocity = Vector3.zero;
        }
    }
}
