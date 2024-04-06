using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lvl3HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public TextMeshProUGUI healthBarNumber;
    health playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.Find("PlayerArmature").GetComponent<health>();

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerHealth.healths;
        healthBarNumber.SetText(healthBar.value.ToString());
    }
}
