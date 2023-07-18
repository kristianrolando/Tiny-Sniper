using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aldo.ObjectPooling
{
    public abstract class PoolObject : MonoBehaviour, IPoolObject
    {
        public PoolingSystem poolingSystem { private set; get; }
        void IPoolObject.Initial(PoolingSystem poolSystem)
        {
            poolingSystem = poolSystem;
        }
        public virtual void OnCreate() { }
        public virtual void StoreToPool()
        {
            poolingSystem.Store(this);
            gameObject.SetActive(false);
        }
    }
}