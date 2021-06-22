using UnityEngine;
using System.Collections;
using System;

public class Attacker : AbilityBase,IAttack
{
    
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float attackOffset = 1;
    [SerializeField]
    private float attackRadius = 1f;
    [SerializeField]
    private float attackImpactDelay = 1f;
    [SerializeField]
    private float attackRange = 2f;
    
    public LayerMask layerMask;

    private Collider[] attackResults;
    private Animator animator;

    public int Damage { get { return damage; } }

    private void Awake()
    {
        string currentLayer = LayerMask.LayerToName(gameObject.layer);
        layerMask = ~LayerMask.GetMask(currentLayer);
        attackResults = new Collider[10];
        animator = GetComponentInChildren<Animator>();
        var animationImpactWatcher = GetComponentInChildren<AnimationImpactWatcher>();
        if(animationImpactWatcher != null)
            animationImpactWatcher.OnImpact += AnimationImpactWatcher_OnImpact;
    }
    public void Attack(ITakeHit target)
    {

        attackTimer = 0;
        StartCoroutine(DoAttack(target));
    }

    IEnumerator DoAttack(ITakeHit target)
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackImpactDelay);
        if (target.Alive && InAttackRange(target))
            target.TakeHit(this);
    }

    internal bool InAttackRange(ITakeHit target)
    {
        if (target.Alive == false)
            return false;
        var distance = Vector3.Distance(transform.position, target.transform.position);
        return distance < attackRange;
    }

    

    //called by animation event via AnimationImpactWatcher
    private void AnimationImpactWatcher_OnImpact()
    {
        Vector3 position = transform.position + transform.forward * attackOffset;
        int hitCount = Physics.OverlapSphereNonAlloc(position, attackRadius, attackResults,layerMask);
        for (int i = 0; i < hitCount; i++)
        {
            //that way we can get all the things that have ITakeHit interface - not only the boxes
            var takeHit = attackResults[i].GetComponent<ITakeHit>();

            if (takeHit != null)
            {
                takeHit.TakeHit(this);
            }
        }
    }

    public void Attack()
    {
        animator.SetTrigger(animationTrigger);
    }

    protected override void OnUse()
    {
        Attack();
    }
}
