using System.Collections;
using Bullets;
using Managers;
using UnityEngine;

namespace AttackPatterns
{
    public class SingleShotAPT : AttackPattern
    {
        [SerializeField] private Bullet bullet;

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
                Bullet Flybullet = Instantiate(bullet, transform.position, Quaternion.LookRotation(_attackDirection, Vector3.up), _bulletsParent.transform);
                Flybullet.Init(_attackDirection, _bulletSpeed, _attackPower, _isPlayer);
            }
        }
    }
}
