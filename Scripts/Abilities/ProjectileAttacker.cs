using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttacker : AbilityBase,IAttack
{
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private Projectile projectilePrefab;
    [SerializeField]
    private float launchYoffset = 1f;
    [SerializeField]
    private float launchDelay = 1f;

    public int Damage { get { return damage; } }

    public void Attack()
    {
        StartCoroutine(LaunchAfterDelay());   
    }

    protected override void OnUse()
    {
        Attack();
    }
    private IEnumerator LaunchAfterDelay()
    {
        yield return new WaitForSeconds(launchDelay);
        projectilePrefab.Get<Projectile>(transform.position + Vector3.up * launchYoffset, transform.rotation);
    }
}
