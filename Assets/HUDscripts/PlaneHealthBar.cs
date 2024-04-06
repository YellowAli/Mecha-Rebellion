using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaneHealthBar : MonoBehaviour
{
    public Slider healthBar;
    JetBehaviour jetHealth;
    public TextMeshProUGUI healthBarNumber;

    // Start is called before the first frame update
    void Start()
    {
        jetHealth = GameObject.FindGameObjectWithTag("Plane").GetComponent<JetBehaviour>();

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = jetHealth.health;
        healthBarNumber.SetText(healthBar.value.ToString());
    }
}
