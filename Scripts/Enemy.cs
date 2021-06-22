using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Attacker))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : PooledMonoBehaviour, ITakeHit, IDie
{

    [SerializeField]
    private int maxHealth = 3;

    private Attacker attacker;
    private int currentHealth;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float dieDelay = 6f;
    private Character target;

    public event Action<IDie> OnDied = delegate { };
    public event Action<int, int> OnHealthChanged = delegate { };
    public event Action OnHit = delegate { };

    private bool isDead { get { return currentHealth <= 0; } }

    public int Damage { get { return 1; } }

    public bool Alive { get; private set; }

    private void Awake()
    {
        attacker = GetComponent<Attacker>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        Alive = true;
    }

    private void Update()
    {if (isDead)
            return;
        if (target == null || target.Alive == false)
        {
            AquireTarget();
        }
        else
        {
            if(attacker.InAttackRange(target) == false)
            {
                FollowTarget();
            }
            else
            {
                TryAttack();
            }
        }
    }

    private void AquireTarget()
    {
        target = Character.All
                  .OrderBy(t => Vector3.Distance(transform.position, t.transform.position))
                  .FirstOrDefault();
        animator.SetFloat("Speed", 0f);
    }

    private void FollowTarget()
    {
        navMeshAgent.SetDestination(target.transform.position);
        navMeshAgent.isStopped = false;
        animator.SetFloat("Speed", 1f);
    }

    private void TryAttack()
    {
        animator.SetFloat("Speed", 0f);
        navMeshAgent.isStopped = true;
        if (attacker.CanAttack)
        {
            attacker.Attack(target);
        }
    }

    public void TakeHit(IDamage hitBy)
    {
        OnHit();
        currentHealth-= hitBy.Damage;
        OnHealthChanged(currentHealth, maxHealth);
        if (currentHealth > 0)
        {
            animator.SetTrigger("Hit");
        }

        else
        {
            Die();
        }
    }

    private void Die()
    {
        Alive = false;
        OnDied(this);
        animator.SetTrigger("Die");
        navMeshAgent.isStopped = true;
        ReturnToPool(dieDelay);
    }
}
