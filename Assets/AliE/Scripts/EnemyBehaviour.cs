using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
   float time = 5;
    public bool alive = true;
    public float health;
    public NavMeshAgent agent;
    public Transform playerTransform;
    public GameObject bullet;
    public GameObject fire;

    public Transform attackPoint;

    public LayerMask groundMask, playerMask,bulletMask;
    public Vector3 WalkingPoint;
    bool walkPointOn;
    public float Range;

    public float attackIncrements;
    bool attackOccured;

    public float sightRange, attackRange;
    public bool withinSightRange, withinAttackRange;

    public float spread;
    public float shootForce, upwardForce;
    public GameObject muzzleFlash;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        withinSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        withinAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!withinSightRange && !withinAttackRange && alive)
        {
            EnemyPatrol();
        }

        if (withinSightRange && !withinAttackRange && alive)
        {
            EnemyChase();
        }

        if (withinSightRange && withinAttackRange && alive)
        {
            EnemyAttack();
        }

        if(time < 2 && alive == false)
        {
            Destroy(fire);
            Destroy(gameObject);
            
        }
        if(health <= 0 && time > 0)
        {
            time = 0;
            Instantiate(fire, transform.position, Quaternion.identity);
            alive = false;
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
            Shoot();
            attackOccured = true;
            Invoke(nameof(ResetAttack), attackIncrements);
        }
    }

    private void Shoot()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = (playerTransform.position - rayOrigin).normalized;

        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player


        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.LookRotation(directionWithSpread)); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(transform.up * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
    }

    private void ResetAttack()
    {
        attackOccured = false;

    }

    public void HandleColission(Collision collision)
    {
        //Debug.Log("here");
        if (collision.gameObject.tag == "mainBullet")
        {
            //Debug.Log("Here");
            health = health - 50;
        }

    }

}