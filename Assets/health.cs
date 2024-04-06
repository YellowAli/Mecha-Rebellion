using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class health : MonoBehaviour
{
    public float healths;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (healths < 0)
        {
            SceneManager.LoadScene("ReplayScreen");
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemyBullet")
        {
            Debug.Log(healths);
            healths = healths -10;
        }

    }
}
