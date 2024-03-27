using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform playerTransform;
    public GameObject bullet;

    public LayerMask groundMask, playerMask;
    public Vector3 WalkingPoint;
    bool walkPointOn;
    public float Range;

    public float attackIncrements;
    bool attackOccured;

    public float sightRange, attackRange;
    public bool withinSightRange, withinAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        withinSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        withinAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!withinSightRange && !withinAttackRange)
        {
            EnemyPatrol();
        }

        if (withinSightRange && !withinAttackRange)
        {
            EnemyChase();
        }

        if (withinSightRange && withinAttackRange)
        {
            EnemyAttack();
        }


    }


    private void EnemyPatrol()
    {
        if (!walkPointOn)
        {
            SearchWalkPoint();
        }

        if (walkPointOn)
        {
            agent.SetDestination(WalkingPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - WalkingPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointOn = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomeZ = Random.Range(-Range, Range);
        float randomeX = Random.Range(-Range, Range);

        WalkingPoint = new Vector3(transform.position.x + randomeX, transform.position.y, transform.position.z + randomeZ);

        if (Physics.Raycast(WalkingPoint, -transform.up, 2f, groundMask))
        {
            walkPointOn = true;
        }

    }

    private void EnemyChase()
    {
        agent.SetDestination(playerTransform.position);

    }

    private void EnemyAttack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(playerTransform);

        if (!attackOccured)
        {
            attackOccured = true;
            Invoke(nameof(ResetAttack), attackIncrements);
        }
    }

    private void ResetAttack()
    {
        attackOccured = false;

    }

}