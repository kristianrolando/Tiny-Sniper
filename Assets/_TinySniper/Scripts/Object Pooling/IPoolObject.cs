using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aldo.ObjectPooling
{
    public interface IPoolObject
    {
        public Transform transform { get; }
        public GameObject gameObject { get; }
        PoolingSystem poolingSystem { get; }

        void Initial(PoolingSystem poolSystem);
        void OnCreate(); //this function is executed right after the object is active
        void StoreToPool(); //This function is used to return the object to the pool and inActive it
    }
}