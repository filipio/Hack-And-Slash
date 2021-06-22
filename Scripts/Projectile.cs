using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PooledMonoBehaviour,IDamage
{
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private PooledMonoBehaviour impactParticlePrefab;
    public int Damage { get { return damage; } }


    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;   
    }

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.collider.GetComponent<ITakeHit>();
        if(hit != null)
        {
            Impact(hit);
        }
        else
        {
            impactParticlePrefab.Get<PooledMonoBehaviour>(transform.position, Quaternion.identity);
            ReturnToPool();
        }
    }

    private void Impact(ITakeHit hit)
    {
        hit.TakeHit(this);
        ReturnToPool();
        impactParticlePrefab.Get<PooledMonoBehaviour>(transform.position, Quaternion.identity);
    }


}
