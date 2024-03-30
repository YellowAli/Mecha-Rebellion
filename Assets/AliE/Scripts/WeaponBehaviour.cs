using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeaponBehaviour : MonoBehaviour
{
    public GameObject bullet;

    public NavMeshAgent agent;
    public Transform playerTransform;

    public LayerMask playerMask;

    public float attackIncrements;
    bool attackOccured;

    public float sightRange, attackRange;
    public bool withinSightRange, withinAttackRange;
    public Transform attackPoint;

    public float spread;
    public float shootForce, upwardForce;
    public GameObject muzzleFlash;


    // Update is called once per frame
    void Update()
    {
        withinSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        withinAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (withinSightRange && withinAttackRange)
        {
            EnemyAttack();
        }

    }

    private void EnemyAttack()
    {

        if (!attackOccured)
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
}