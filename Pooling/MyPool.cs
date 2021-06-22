using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPool : MonoBehaviour
{
    // to keep different kinds of pools for different prefabs - pool is a value, prefab is type -> create a pool of that kind of prefab
    private static Dictionary<MyPooledMonobehaviour, MyPool> pools = new Dictionary<MyPooledMonobehaviour, MyPool>();
    // to keep objects that we can take
    private Queue<MyPooledMonobehaviour> objects = new Queue<MyPooledMonobehaviour>();
    private MyPooledMonobehaviour prefab;

    public static MyPool GetMyPool(MyPooledMonobehaviour prefab)
    {
        if(pools.ContainsKey(prefab))
        {
            return pools[prefab];
        }

        MyPool pooledObject = new GameObject("Pool" + prefab.name).AddComponent<MyPool>();
        pooledObject.prefab = prefab;
        pools.Add(prefab, pooledObject);
        return pooledObject;

    }

    public T Get<T>() where T : MyPooledMonobehaviour
    {
        if(objects.Count == 0)
        {
            GrowPool();
        }

        var pooledObject = objects.Dequeue();
        return pooledObject as T;
    }

    private void GrowPool()
    {
        for(int i = 0; i<prefab.PoolInitialSize; i++)
        {
           var pooledObject = Instantiate(prefab) as MyPooledMonobehaviour;
            pooledObject.name += " " + i;
            pooledObject.OnReturnToPool += AddObjectToAvaibleQueue;
            //who is "this"? 
            pooledObject.transform.SetParent(this.transform);
            pooledObject.gameObject.SetActive(false);
        }
    }

    private void AddObjectToAvaibleQueue(MyPooledMonobehaviour pooledObject)
    {
        pooledObject.transform.SetParent(this.transform);
        objects.Enqueue(pooledObject);
    }
}
