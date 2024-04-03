
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurretEnemyBehaviour : MonoBehaviour
{
    private bool alive = true;
    public float health;
    //ublic NavMeshAgent agent;
    public Transform attackPoint;
   Transform playerTransform;
    public GameObject bullet;
    bool ingame = true;

    public LayerMask groundMask, playerMask;
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
    private Vector3 ground;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameObject.FindWithTag("Plane"));
        playerTransform = GameObject.FindWithTag("Plane").transform;
        //agent = GetComponent<NavMeshAgent>();
        ground = new Vector3(555, -193, 555);

    }

    // Update is called once per frame
    void Update()
    {
        if(ingame)
        {
            Debug.Log(GameObject.FindWithTag("Plane"));
            playerTransform = GameObject.FindWithTag("Plane").transform;
            ingame = false;
        }
        withinSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        withinAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);


        if (withinSightRange && withinAttackRange && alive )
        {
            
            EnemyAttack();
        }

        Debug.Log(health);
       

    }


    private void EnemyPatrol()
    {
        if (!walkPointOn)
        {
            SearchWalkPoint();
        }

        if (walkPointOn)
        {
            // agent.SetDestination(WalkingPoint);
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
        //agent.SetDestination(playerTransform.position);

    }

    private void EnemyAttack()
    {
        //agent.SetDestination(transform.position);
        transform.LookAt(playerTransform);

        if (!attackOccured )
        {
            Shoot();
            attackOccured = true;
            Invoke(nameof(ResetAttack), attackIncrements);
        }
    }

    private void ResetAttack()
    {
        attackOccured = false;

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
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(transform.up * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("here");
        if (collision.gameObject.tag == "mainBullet")
        {
            Debug.Log("the health is decreasing"+health);
            health = health - 50;
        }

        if (health <= 0)
        {
            Debug.Log(health);
            alive = false;
            transform.LookAt(ground);
        }
    }

}
