using System.Collections;
using Bullets;
using Managers;
using UnityEngine;

namespace AttackPatterns
{
    public class SingleShotAPT : AttackPattern
    {
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
                Bullet Flybullet = _bulletsPoolFactory.Pull(Quaternion.LookRotation(_attackDirection, Vector3.up), transform.position);
                Flybullet.Init(_attackDirection, _bulletSpeed, _attackPower, _isPlayer);
            }
        }
    }
}
