using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace PoolFactories
{
    public abstract class AbstractPoolFactory<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T prefab;
        [SerializeField] private Transform parentTransform;
        [SerializeField] protected List<T> itemList;

        protected readonly Queue<T> _itemQueue = new Queue<T>();
        protected Queue<T> _pulledItemQueue = new Queue<T>();

        protected void Awake()
        {
            Init();
            EventManager.StartListening(Constants.GoHome, PushPulled);
            EventManager.StartListening(Constants.LevelCompleted, PushPulled);
        }

        protected void Init()
        {
            foreach (var t in itemList)
            {
                t.gameObject.SetActive(false);
                _itemQueue.Enqueue(t);
            }
            itemList = null;
        }
    
        public virtual void Push(T item)
        {
            _itemQueue.Enqueue(item);
        }
    
        public virtual T Pull(Vector3 position)
        {
            T t = _itemQueue.Count > 0 ? _itemQueue.Dequeue() : Instantiate(prefab);
            t.transform.position = position;
            t.transform.SetParent(parentTransform);
            _pulledItemQueue.Enqueue(t);
            return t;
        }

        protected abstract void PushPulled();
    }
}