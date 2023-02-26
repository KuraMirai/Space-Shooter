using System.Collections;
using PoolFactories;
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
        protected BulletsPoolFactory _bulletsPoolFactory;
        protected Vector3 _attackDirection;

        public void Init(BulletsPoolFactory factory, float attackInterval, float attackPower, float bulletSpeed, bool isPlayer = false)
        {
            _attackInterval = attackInterval;
            _attackPower = attackPower;
            _bulletSpeed = bulletSpeed;
            _isPlayer = isPlayer;
            _bulletsPoolFactory = factory;
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
