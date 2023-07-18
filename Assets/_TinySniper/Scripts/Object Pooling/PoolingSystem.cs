using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aldo.ObjectPooling
{
    public class PoolingSystem
    {
        Queue<IPoolObject> storedList = new Queue<IPoolObject>();
        Queue<IPoolObject> spawnedList = new Queue<IPoolObject>();

        public IPoolObject CreateObject(IPoolObject objectPrefab, Vector3 spawnPos, Transform parent = null)
        {
            IPoolObject outObject;
            if (storedList.Count < 1 || storedList.Peek().gameObject == null)
            {
                outObject = MonoBehaviour.Instantiate(objectPrefab.gameObject).
                GetComponent<IPoolObject>();
                outObject.Initial(this);
            }
            else
            {
                outObject = storedList.Dequeue();
            }
            outObject.transform.position = spawnPos;
            outObject.transform.parent = parent;
            outObject.gameObject.SetActive(true);
            outObject.OnCreate();
            spawnedList.Enqueue(outObject);

            return outObject;

        }

        public IPoolObject CreateObject(IPoolObject objectPrefab, Vector3 spawnPos, Quaternion shooter, Transform parent = null)
        {
            IPoolObject outObject;
            if (storedList.Count < 1)
            {
                outObject = MonoBehaviour.Instantiate(objectPrefab.gameObject).
                GetComponent<IPoolObject>();
                outObject.Initial(this);
            }
            else
            {
                outObject = storedList.Dequeue();
            }
            outObject.transform.position = spawnPos;
            outObject.transform.rotation = shooter;
            outObject.transform.parent = parent;

            outObject.OnCreate();
            outObject.gameObject.SetActive(true);

            spawnedList.Enqueue(outObject);

            return outObject;
        }

        public void Store(IPoolObject poolObject)
        {
            storedList.Enqueue((IPoolObject)poolObject);
        }
    }
}