using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public Rigidbody rigid;
    public GameObject explosion;

    public LayerMask TBD;

    public float bounciness;
    public bool useGravity;

    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial material;

    private void Initial()
    {
        material = new PhysicMaterial();
        material.bounciness = bounciness;
        material.frictionCombine = PhysicMaterialCombine.Minimum;
        material.bounceCombine = PhysicMaterialCombine.Maximum;
        GetComponent<SphereCollider>().material = material;

        rigid.useGravity = useGravity;
    }

    private void Explode()
    {



        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);

        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, TBD);

        for (int i = 0; i < enemies.Length; i++)
        {
            Rigidbody enemyRigidbody = enemies[i].GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Initial();
    }

    private void OnCollisionEnter(Collision collision)
    {

        collisions++;
        if (collision.collider.CompareTag("PlayerArmature") && explodeOnTouch)
        {
            Explode();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (collisions > maxCollisions)
        {
            Explode();
        }

        maxLifetime = maxLifetime - Time.deltaTime;
        if (maxLifetime >= 0)
        {
            Explode();
        }


    }
}
