using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBehaviourPlane : MonoBehaviour
{
    // Start is called before the first frame update
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.position += transform.forward * Time.deltaTime * 1000f;

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Plane")
        {

            GameObject player = GameObject.FindWithTag("Plane");
            JetBehaviour heals = player.GetComponent<JetBehaviour>();
            heals.health = heals.health - 10;
        }
        Destroy(gameObject);
    }
}
