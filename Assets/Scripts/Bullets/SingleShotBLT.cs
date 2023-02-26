using Enemy;
using Player;
using UnityEngine;

namespace Bullets
{
    public class SingleShotBLT : Bullet
    {
        private void OnTriggerEnter(Collider collision)
        {
            GetCollidedObject(collision);
        }

        private void GetCollidedObject(Collider collidedObj)
        {
            if (collidedObj.CompareTag(Constants.EnemyTag) && _isPlayerBullet)
            {
                collidedObj.GetComponentInParent<EnemyAI>().TakeDamage(_damage);
                StopCreationRoutine();
                Push();
            }

            else if (collidedObj.CompareTag(Constants.PlayerTag) && !_isPlayerBullet)
            {
                collidedObj.GetComponentInParent<PlayerShip>().TakeDamage(_damage);
                StopCreationRoutine();
                Push();
            }
        }
    }
}
