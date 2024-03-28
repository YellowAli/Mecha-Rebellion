using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBehaviour : MonoBehaviour
{
    public float speed = 50.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Move the beam upward (or forward from its own perspective) every frame
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy the beam when it collides with any object
        Destroy(gameObject);
    }
}
