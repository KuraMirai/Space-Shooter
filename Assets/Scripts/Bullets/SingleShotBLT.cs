using Enemy;
using Player;
using UnityEngine;

namespace Bullets
{
    public class SingleShotBLT : Bullet
    {
        private void Start()
        {
            Destroy(gameObject, 5f);
        }

        private void ActivateHitVfx()
        {
            Destroy(gameObject, 0.4f);
        }

        private void OnTriggerEnter(Collider collision)
        {
            GetCollidedObject(collision);
        }

        private void GetCollidedObject(Collider collidedObj)
        {
            if (collidedObj.CompareTag(Constants.EnemyTag) && _isPlayerBullet)
            {
                collidedObj.GetComponentInParent<EnemyAI>().TakeDamage(_damage);
                ActivateHitVfx();
                Destroy(gameObject);
            }

            else if (collidedObj.CompareTag(Constants.PlayerTag) && !_isPlayerBullet)
            {
                collidedObj.GetComponentInParent<PlayerShip>().TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
