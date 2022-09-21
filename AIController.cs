using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enum for the different states of the AI
/// </summary>
public enum zombieBehaviour { Patrol = 0, Chase, Attack }
public class AIController : MonoBehaviour
{
    public zombieBehaviour behaviour;
    public ZombieTypes zombieTypes;
    public NavMeshAgent agent;
    [Tooltip("Assign Player to make enemy chase")]
    public Transform player;

    //Patrolling
    public Vector3 patrolPoint;
    //Checks if the patrolPoint is set
    private bool patrolPointSet;
    //Determines the range of the patrol
    public float patrolPointRange;
    //Layers to help the AI to navigate
    public LayerMask IsGround, IsPlayer;

    [Tooltip("Determines Attack Speed.")]
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    [Tooltip("Determines Attack Range of AI.")]
    public float attackRange;
    [Tooltip("Determines Sight Range of AI.")]
    public float sightRange;
    [Tooltip("Sets a Y Offset for the patrol points. Prevents Zombies from trying to walk under the floor.")]
    public float offsetPatrolPointY = 0.7f;
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        zombieTypes = GetComponent<ZombieTypes>();
    }

    private void Update()
    {
        SetBehaviour();
        KillZombie();
    }

    /// <summary>
    /// Checks if the AI is in range to attack the player
    /// </summary>
    /// <param name="attackRadius"></param>
    /// <returns></returns>
    private bool CheckAttackRangeSmallerOrBiggerThan(float attackRadius)
    {
        float distance = (transform.position - player.position).magnitude;
        return distance < attackRadius;
    }

    /// <summary>
    /// Checks if the distance from the AI to the Player is smaller than the sight range of the AI
    /// </summary>
    /// <param name="sightRange"></param>
    /// <returns></returns>
    private bool CheckSightRangeSmallerOrBiggerThan(float sightRange)
    {
        float distance = (transform.position - player.position).magnitude;
        return distance < sightRange;
    }

    /// <summary>
    /// Checks if the player can be seen via raycast
    /// </summary>
    /// <returns></returns>
    private bool PlayerCanBeSeen()
    {
        PlayerTag tag = null;

        if (Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hit, sightRange))
        {
            if (hit.transform.gameObject.GetComponent<PlayerTag>() != null)
            {
                tag = hit.transform.gameObject.GetComponent<PlayerTag>();
            }
        }
        return tag != null;
    }

    /// <summary>
    /// Sets the state for the AI. Patrolling, Chasing or Attacking
    /// </summary>
    private void SetBehaviour()
    {
        switch (behaviour)
        {
            case zombieBehaviour.Patrol:
                SetPatrol();
                SetChase();
                break;
            case zombieBehaviour.Chase:
                SetPatrol();
                SetChase();
                SetAttack();
                break;
            case zombieBehaviour.Attack:
                SetChase();
                SetAttack();
                break;
        }
    }

    /// <summary>
    /// Sets the state for the AI. Patrolling, Chasing or Attacking
    /// </summary>
    /// <param name="Behaviour"></param>
    private void SetBehaviourState(zombieBehaviour Behaviour)
    {
        behaviour = Behaviour;
    }

    /// <summary>
    /// Patrol state of the AI
    /// </summary>
    private void SetPatrol()
    {
        if (behaviour == zombieBehaviour.Patrol)
        {
            Patrolling();
            return;
        }
        if (!CheckSightRangeSmallerOrBiggerThan(sightRange) || !PlayerCanBeSeen())
        {
            SetBehaviourState(zombieBehaviour.Patrol); 
        }
    }

    /// <summary>
    /// Method for chasing after the player. Needs to be able to see the player, and be in range of it
    /// </summary>
    private void SetChase()
    {
        if (behaviour == zombieBehaviour.Chase)
        {
            agent.SetDestination(player.position);
            return;
        }
        if (CheckSightRangeSmallerOrBiggerThan(sightRange) && !CheckAttackRangeSmallerOrBiggerThan(attackRange) && PlayerCanBeSeen())
        {
            SetBehaviourState(zombieBehaviour.Chase);
        }
    }

    /// <summary>
    /// The attackstate of the AI, attacks when it's in range
    /// </summary>
    private void SetAttack()
    {
        if (CheckSightRangeSmallerOrBiggerThan(sightRange) && CheckAttackRangeSmallerOrBiggerThan(attackRange))
        {
            SetBehaviourState(zombieBehaviour.Attack);
            AttackPlayer();
        }
    }

    /// <summary>
    /// Makes the AI patrol between patrolpoints
    /// </summary>
    private void Patrolling()
    {
        if (!patrolPointSet) SearchPatrolPoint();

        if (patrolPointSet)
        {
            agent.SetDestination(patrolPoint);
        }

        //Calculates distance to the patrolPoint
        Vector3 distanceToPatrolPoint = patrolPoint - transform.position;

        //patrolPoint reached
        if (distanceToPatrolPoint.magnitude < 0.7f)
        {
            patrolPointSet = false;
        }
    }

    /// <summary>
    /// Calculates a random point in range and sets it as a patrolpoint for Patrolling
    /// </summary>
    private void SearchPatrolPoint()
    {
        float randomZ = UnityEngine.Random.Range(-patrolPointRange, patrolPointRange);
        float randomX = UnityEngine.Random.Range(-patrolPointRange, patrolPointRange);

        //Creates a random temporary patrolpoint to check if the AI can walk there, before setting it as next patrol point
        Vector3 tempPatrolPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Checks that the patrolPoint is within the map, if it is, patrol there
        NavMeshHit hit;
        if ((NavMesh.SamplePosition(tempPatrolPoint, out hit, 1f, NavMesh.AllAreas)))
        {
            patrolPoint = hit.position;
            patrolPointSet = true;
        }
    }

    /// <summary>
    /// Makes the AI attack the player.
    /// </summary>
    private void AttackPlayer()
    {
        //Makes enemy stop moving while attacking
        agent.SetDestination(transform.position);
        //Makes enemy look at player while attacking
        transform.LookAt(player);

        //Swing timer for the enemy, basically attackspeed
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    /// <summary>
    /// Resets the attack
    /// </summary>
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    /// <summary>
    /// Disables the zombie so that it can be put back in the Object Pool.
    /// </summary>
    private void KillZombie()
    {
        if(zombieTypes.ZombieStats[1] <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Checks if the AI is hit with a gameObject with the BulletTag. If so, it takes damage.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BulletTag>() !=null)
        {
            zombieTypes.ZombieStats[1] -= collision.gameObject.GetComponent<Bullet>().Damage;
        }
    }

    /// <summary>
    /// Draws out spheres in the scene for attackRange and sightRange and the patrolPoint.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(patrolPoint, 1);
    }
}
