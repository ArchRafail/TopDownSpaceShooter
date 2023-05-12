using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class ObjectPoller : MonoBehaviour
    {
        public static ObjectPoller Current;
        public GameObject pooledObjectPrefab;
        public int pooledAmount;
        public bool willGrow = true;

        private List<GameObject> _pooledObjects;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            _pooledObjects = new List<GameObject>();
            for (int i = 0; i < pooledAmount; i++)
            {
                var objectToPool = Instantiate(pooledObjectPrefab);
                objectToPool.SetActive(false);
                _pooledObjects.Add(objectToPool);
            }
        }

        public GameObject GetPooledObject()
        {
            foreach (var objectInPool in _pooledObjects)
            {
                if (!objectInPool.activeSelf)
                {
                    return objectInPool;
                }
            }

            if (willGrow)
            {
                var objectToPool = Instantiate(pooledObjectPrefab);
                objectToPool.SetActive(false);
                _pooledObjects.Add(objectToPool);
                return objectToPool;
            }

            return null;
        }
    }
}
