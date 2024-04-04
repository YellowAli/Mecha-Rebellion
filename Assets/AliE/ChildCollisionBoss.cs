using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollisionBoss : MonoBehaviour
{
    public EnemyBehaviourBoss en;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit me baby one more time");
        en.HandleColission(collision);
    }
}
