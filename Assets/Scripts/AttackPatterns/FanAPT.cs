using System.Collections;
using Bullets;
using Managers;
using UnityEngine;

namespace AttackPatterns
{
    public class FanAPT : AttackPattern
    {
        [SerializeField] private int bulletCount = 3;

        #region Constants

        private const float StartAngle = -255f;
        private const float SpreadAngle = -150;
        private const float Offset = 0.27f;

        #endregion

        private Transform _playerTransform;

        public override void Attack()
        {
            StartAttacking();
        }

        public override IEnumerator AttackRoutine()
        {
            Shoot();
            yield return new WaitForSeconds(_attackInterval);
            StopAttacking();
            StartAttacking();
        }

        private void Shoot()
        {
            if (!PauseManager.IsPause)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    float addedOffset = StartAngle + (i * ((Offset + SpreadAngle) / (bulletCount - 1)));
                    float x = _attackDirection.x * Mathf.Cos(addedOffset) - _attackDirection.y * Mathf.Sin(addedOffset);
                    float y = _attackDirection.x * Mathf.Sin(addedOffset) + _attackDirection.y * Mathf.Cos(addedOffset);
                    Vector3 newAttackDirection = new Vector3(-x, -y);
                    Bullet Flybullet = _bulletsPoolFactory.Pull(Quaternion.LookRotation(newAttackDirection, Vector3.up), transform.position);
                    Flybullet.Init(newAttackDirection, _bulletSpeed, _attackPower, _isPlayer);
                }
            }
        }
    }
}
