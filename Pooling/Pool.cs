using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private static Dictionary<PooledMonoBehaviour, Pool> pools = new Dictionary<PooledMonoBehaviour, Pool>();

    private Queue<PooledMonoBehaviour> objects = new Queue<PooledMonoBehaviour>();
    private PooledMonoBehaviour prefab;

    public static Pool GetPool(PooledMonoBehaviour prefab)
    {
        if (pools.ContainsKey(prefab))
            return pools[prefab];

        Pool poolGameObject = new GameObject("Pool - " + prefab.name).AddComponent<Pool>();
       //why we can call this in this way? line below
        poolGameObject.prefab = prefab;
        pools.Add(prefab, poolGameObject);
        return poolGameObject;

      
    }

    //whatever type is T, it is the type of PooledMonoBehaviour - right now a little bit confusing
    public T Get<T>() where T : PooledMonoBehaviour
    {
        if(objects.Count == 0)
        {
            GrowPool();
        }
        var pooledObject = objects.Dequeue();
        //return it as whatever type it was requested
        return pooledObject as T;
    }

    private void GrowPool()
    {
        for(int i=0; i<prefab.InitialPoolSize; i++)
        {
            var pooledObject = Instantiate(prefab) as PooledMonoBehaviour;
            pooledObject.name += " " + i;
            pooledObject.OnReturnToPool += AddObjectToAvailableQueue;

            pooledObject.transform.SetParent(this.transform);
            pooledObject.gameObject.SetActive(false);
        }
    }

    private void AddObjectToAvailableQueue(PooledMonoBehaviour pooledObject)
    {
        pooledObject.transform.SetParent(this.transform);
        objects.Enqueue(pooledObject);
    }
}
