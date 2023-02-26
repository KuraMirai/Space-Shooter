using Enemy;
using UnityEngine;
using System.Collections.Generic;

namespace PoolFactories
{
    public class SingleShotEnemyShipPoolFactory : AbstractPoolFactory<EnemyAI>
    {
        public override EnemyAI Pull(Vector3 position)
        {
            EnemyAI enemy = base.Pull(position);
            enemy.transform.position = position;
            enemy.Pushed += Push;
            return enemy;
        }

        protected override void PushPulled()
        {
            foreach (var t in _pulledItemQueue)
            {
                t.Push();
            }
            _pulledItemQueue = new Queue<EnemyAI>();
        }
    }
}
