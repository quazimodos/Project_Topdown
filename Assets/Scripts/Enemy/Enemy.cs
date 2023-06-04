using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State { Idle, Chasing, Attacking };
    State currentState;

    [Header("Options")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float sightRange, attackRange;
    [Header("Ranged Enemy Options")]
    [SerializeField] private bool isEnemyRange;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject projectile;
    [Header("Visuals")]
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] int score;

    [Header("Layers")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsCore;

    [Header("Components")]
    [SerializeField] LivingEntity targetEntity;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] int randomAttackCount;
    private NavMeshAgent pathfinder;

    //Booleans
    private bool alreadyAttacked;
    private bool playerInSightRange, playerInAttackRange, baseInAttackRange;

    //Target transform
    private Transform player;
    private Transform core;

    bool isGameOver;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        core = GameObject.FindGameObjectWithTag("Core").transform;

        pathfinder = GetComponent<NavMeshAgent>();
        targetEntity = player.GetComponent<LivingEntity>();
    }

    protected override void Start()
    {
        base.Start();
        OnDeath += OnEnemyDeath;
        currentState = State.Chasing;
        GameManager.Instance.OnGameOverEvent += GameManager_OnGameOverEvent;
        GameManager.Instance.OnGamePauseEvent += Instance_OnGamePauseEvent;
    }

    private void Instance_OnGamePauseEvent(object sender, System.EventArgs e)
    {
        targetEntity = null;
    }

    private void GameManager_OnGameOverEvent(object sender, System.EventArgs e)
    {
        isGameOver = true;
        targetEntity = null;
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (damage >= currentHealth)
        {
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection))
                as GameObject, 2f);
        }
        base.TakeHit(damage, hitPoint, hitDirection);
    }

    private void Update()
    {

        if (isGameOver || GameManager.Instance.IsGamePaused()) return;
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        baseInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsCore);

        if (!playerInSightRange && !playerInAttackRange) ChaseTarget(core);
        if (!playerInSightRange && baseInAttackRange) AttackTarget(core);
        if (playerInSightRange && !playerInAttackRange) ChaseTarget(player);
        if (playerInAttackRange && playerInSightRange) AttackTarget(player);
    }

    public void ChaseTarget(Transform target)
    {
        animator.SetBool("Run", true);
        if (target != null && !dead)
        {
            targetEntity = target.GetComponent<LivingEntity>();
            pathfinder.SetDestination(target.position);
        }
    }

    public void AttackTarget(Transform targetTransform)
    {
        animator.SetBool("Run", false);
        if (targetTransform != null && !dead)
        {
            pathfinder.SetDestination(transform.position);
            Vector3 target = new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z);
            transform.LookAt(target);
            Attack();
        }
    }

    private void Attack()
    {
        currentState = State.Attacking;
        if (!alreadyAttacked)
        {
            if (isEnemyRange)
            {
                //Range enemy attack.
                RangedAttackAnimation();
            }
            else
            {
                //Melee enemy attack.
                MeleeAttackAnimation();
            }
            //End of attack code
            alreadyAttacked = true;
            currentState = State.Chasing;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnEnemyDeath()
    {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
        ScoreManager.Instance.AddScore(Random.Range(score, score + 10));
    }

    private void MeleeAttackAnimation()
    {
        animator.SetInteger("AttackIndex", Random.Range(0, randomAttackCount + 1));
        animator.SetTrigger("Attack");
    }

    private void RangedAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void HitMeleeAttack()
    {
        if (targetEntity != null)
        {
            targetEntity.TakeDamage(damage);
        }
    }

    public void HitRangedAttack()
    {
        Projectile newProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(transform.eulerAngles)).GetComponent<Projectile>();
        newProjectile.SetSpeedAndDamageOnly(projectileSpeed, damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGamePauseEvent -= Instance_OnGamePauseEvent;
        GameManager.Instance.OnGameOverEvent -= GameManager_OnGameOverEvent;
    }
}
