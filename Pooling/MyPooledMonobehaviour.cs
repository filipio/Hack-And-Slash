using System;
using UnityEngine;

public class MyPooledMonobehaviour : MonoBehaviour
{
    [SerializeField]
    private int poolInitialSize = 50;

    public int PoolInitialSize{get {return poolInitialSize;}}

    public event Action<MyPooledMonobehaviour> OnReturnToPool;

    private T Get<T>(bool enabled = true) where T : MyPooledMonobehaviour
    {
       var pool = MyPool.GetMyPool(this);
        var pooledObject = pool.Get<T>();
        if (enabled)
             pooledObject.gameObject.SetActive(true);
        return pooledObject;
        
    }

    private T Get<T>(Vector3 position, Quaternion rotation) where T : MyPooledMonobehaviour
    {
        var pooledObject = Get<T>();
        pooledObject.transform.position = position;
        pooledObject.transform.rotation = rotation;
        return pooledObject as T;


    }
    private void OnDisable()
    {
        if(OnReturnToPool != null)
        {
            OnReturnToPool(this);
        }
    }
}