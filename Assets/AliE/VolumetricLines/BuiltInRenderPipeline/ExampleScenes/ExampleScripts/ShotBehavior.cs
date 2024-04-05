using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		transform.position += transform.forward * Time.deltaTime * 500f;
	
	}

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            
            GameObject player = GameObject.FindWithTag("Player");
            PlayerMovementTutorial heals = player.GetComponent<PlayerMovementTutorial>();
            heals.health = heals.health - 5;
        }
        Destroy(gameObject);
    }

}
