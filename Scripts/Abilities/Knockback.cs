using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : AbilityBase, IDamage
{
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private float impactDelay = 0.25f;
    [SerializeField]
    private float forceAmount = 10f;

    private LayerMask layerMask;
    private Animator animator;
    private Collider[] attackResults;
    
    public int Damage { get { return damage; } }

    private void Awake()
    {
        string currentLayerName = LayerMask.LayerToName(gameObject.layer);
        layerMask = ~LayerMask.GetMask(currentLayerName);
        animator = GetComponentInChildren<Animator>();
        attackResults = new Collider[10];
    }

    private void Attack()
    {
        animator.SetTrigger(animationTrigger);
        StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        yield return new WaitForSeconds(impactDelay);

        Vector3 position = transform.position + transform.forward;
        int hitCount = Physics.OverlapSphereNonAlloc(position, attackRadius, attackResults,layerMask);
        for (int i = 0; i < hitCount; i++)
        {
            //that way we can get all the things that have ITakeHit interface - not only the boxes
            var takeHit = attackResults[i].GetComponent<ITakeHit>();

            if (takeHit != null)
            {
                takeHit.TakeHit(this);
            }
            var hitRigidbody = attackResults[i].GetComponent<Rigidbody>();
            if(hitRigidbody != null)
            {
                var direction = Vector3.Normalize(hitRigidbody.transform.position - transform.position);

                hitRigidbody.AddForce(direction * forceAmount, ForceMode.Impulse);
            }
        }
    }
    protected override void OnUse()
    {
        Attack();
    }
}
