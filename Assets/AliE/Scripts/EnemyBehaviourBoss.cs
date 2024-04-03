using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourBoss : MonoBehaviour
{
    public float health;
    private bool alive = true;
    public float attackIncrement2;
    public float attackIncrement3;
    float randomNumber = 1;
    bool wait;
    bool doEnemyAttck = true;
    float miliseconds = 0;
    public Transform attackPoint;
    public Transform attackPoint2;
    public Transform attackPoint3;
    public NavMeshAgent agent;
    public Transform playerTransform;
    public GameObject bullet;
    public GameObject rocketBullet;
    public GameObject laserBullet;

    public LayerMask groundMask, playerMask;
    public Vector3 WalkingPoint;
    bool walkPointOn;
    public float Range;
    public float spread;
    public float shootForce, upwardForce;

    public float attackIncrements;
    bool attackOccured;

    public float sightRange, attackRange;
    public bool withinSightRange, withinAttackRange;


    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
       
       // anim.SetBool("dead", false);

        anim = GetComponent<Animator>();
        playerTransform = GameObject.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        
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
            miliseconds = miliseconds + Time.deltaTime;

            if (!wait && miliseconds > 5)
            {
                doEnemyAttck = false;
                wait = true;
                miliseconds = 0;
            }

            if (wait && miliseconds > 2)
            {
                doEnemyAttck = true;
                wait = false;
                miliseconds = 0;
                randomNumber = Random.Range(0, 3);
            }

            if (doEnemyAttck)
            {
                EnemyAttack(randomNumber);
            }

           



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


        anim.SetBool("Walk", true);
        //anim.SetBool("Idle", false);

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
        anim.SetBool("Walk", true);
        agent.SetDestination(playerTransform.position);

    }

    private void EnemyAttack(float random)
    {
        agent.SetDestination(transform.position);
        transform.LookAt(playerTransform);

        anim.SetBool("Walk", false);
        //anim.SetBool("Idle", true);

        if (!attackOccured)
        {
            Shoot(random);
            attackOccured = true;
            if (random == 0)
            {
                Invoke(nameof(ResetAttack), attackIncrements);
                Debug.Log("1");
            }
            else if (random == 1)
            {
                Invoke(nameof(ResetAttack), attackIncrement2);
                Debug.Log("2");
            }
            else if (random == 2)
            {
                Invoke(nameof(ResetAttack), attackIncrement3);
                Debug.Log("3");
            }
        }

    }

    private void ResetAttack()
    {
        attackOccured = false;

    }

    private void Shoot(float random)
    {
        if (random == 0)
        {
            transform.LookAt(playerTransform);


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

        }
        else if (random == 1)
        {
            transform.LookAt(playerTransform);

            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = (playerTransform.position - rayOrigin).normalized;

            Ray ray = new Ray(rayOrigin, rayDirection);
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(75); //Just a point far away from the player


            Vector3 directionWithoutSpread = targetPoint - attackPoint2.position;

            //Calculate spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //Calculate new direction with spread
            Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

            //Instantiate bullet/projectile
            GameObject currentBullet = Instantiate(laserBullet, attackPoint2.position, Quaternion.identity); //store instantiated bullet in currentBullet
                                                                                                             //Rotate bullet to shoot direction
            currentBullet.transform.forward = directionWithSpread.normalized;

            //Add forces to bullet
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
            currentBullet.GetComponent<Rigidbody>().AddForce(transform.up * upwardForce, ForceMode.Impulse);

            //Instantiate muzzle flash, if you have one
        }

        else if (random == 2)
        {
            transform.LookAt(playerTransform);

            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = (playerTransform.position - rayOrigin).normalized;

            Ray ray = new Ray(rayOrigin, rayDirection);
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(75); //Just a point far away from the player


            Vector3 directionWithoutSpread = targetPoint - attackPoint3.position;

            //Calculate spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //Calculate new direction with spread
            Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

            //Instantiate bullet/projectile
            GameObject currentBullet = Instantiate(rocketBullet, attackPoint3.position, Quaternion.identity); //store instantiated bullet in currentBullet
                                                                                                              //Rotate bullet to shoot direction
            currentBullet.transform.forward = directionWithSpread.normalized;

            //Add forces to bullet
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
            currentBullet.GetComponent<Rigidbody>().AddForce(transform.up * upwardForce, ForceMode.Impulse);

            //Instantiate muzzle flash, if you have one
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("this is working");
        if (collision.gameObject.tag == "mainBullet")
        {
            Debug.Log("the health is decreasing" + health);
            health = health - 2;
        }

        if (health <= 0)
        {
            Debug.Log(health);
            alive = false;
            
        }
    }



}