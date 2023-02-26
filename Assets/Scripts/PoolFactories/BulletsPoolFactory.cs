using Bullets;
using UnityEngine;
using System.Collections.Generic;

namespace PoolFactories
{
    public class BulletsPoolFactory : AbstractPoolFactory<Bullet>
    {
        protected override void PushPulled()
        {
            foreach (var t in _pulledItemQueue)
            {    
                t.StopCreationRoutine();
                t.Push();
            }
            _pulledItemQueue = new Queue<Bullet>();
        }

        public Bullet Pull(Quaternion rotation, Vector3 position)
        {
            Bullet bull = base.Pull(position);
            bull.transform.rotation = rotation;
            bull.Pushed += Push;
            return bull;
        }
    }
}
