using UnityEngine;
using System.Collections;

public class Shots : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //transform.position += transform.forward * Time.deltaTime * 1000f;

    }

    private void OnCollisionEnter(Collision collision)
    {
       
        Destroy(gameObject);

        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("its colliding with player");
            GameObject player = GameObject.FindWithTag("Player");
            PlayerMovementTutorial heals = player.GetComponent<PlayerMovementTutorial>();
            heals.health = heals.health - 100; 
        }
    }
}
