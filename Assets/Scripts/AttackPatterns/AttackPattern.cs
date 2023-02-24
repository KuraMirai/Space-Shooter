using System.Collections;
using UnityEngine;

namespace AttackPatterns
{
    public class AttackPattern : MonoBehaviour
    {
        protected float _attackInterval;
        protected float _attackPower;
        protected float _bulletSpeed;
        protected bool _isPlayer;
        protected Coroutine _attackRoutine;
        protected Transform _bulletsParent;
        protected Vector3 _attackDirection;

        public void Init(Transform bulletsParent, float attackInterval, float attackPower, float bulletSpeed, bool isPlayer = false)
        {
            _attackInterval = attackInterval;
            _attackPower = attackPower;
            _bulletSpeed = bulletSpeed;
            _isPlayer = isPlayer;
            _bulletsParent = bulletsParent;
        }

        public void SetAttackDirection(Vector3 direction)
        {
            _attackDirection = direction;
        }

        public virtual void Attack()
        {
            StartAttacking();
        }

        public virtual IEnumerator AttackRoutine()
        {
            yield return null;
        }

        public void StartAttacking()
        {
            if (_attackRoutine == null)
                _attackRoutine = StartCoroutine(AttackRoutine());
        }

        public void StopAttacking()
        {
            if (_attackRoutine != null)
            {
                StopCoroutine(_attackRoutine);
                _attackRoutine = null;
            }
        }
    }
}
