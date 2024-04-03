using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Challenges : MonoBehaviour
{
    // this is the final version
    public TextMeshProUGUI challengesList1, challengesList2, challengesList3;
    public Boolean equippedWeapon, eliminatedAll;
    PickUpController gun1, gun2, gun3;
    EnemyBehaviour enemy1, enemy2, enemy3, enemy4, enemy5, enemy6;
    private Boolean conditionMet1 = false, conditionMet2 = false;

    // Start is called before the first frame update
    void Start()
    {
        gun1 = GameObject.Find("SciFiGunLightRad").GetComponent<PickUpController>();
        gun2 = GameObject.Find("SciFiGunLightBlack").GetComponent<PickUpController>();
        gun3 = GameObject.Find("SciFiGunLightBlue").GetComponent<PickUpController>();
        enemy1 = GameObject.Find("CannonMachine Variant 2").GetComponent<EnemyBehaviour>();
        enemy2 = GameObject.Find("CannonMachine Variant 2 (1)").GetComponent<EnemyBehaviour>();
        enemy3 = GameObject.Find("CannonMachine Variant 2 (2)").GetComponent<EnemyBehaviour>();
        enemy4 = GameObject.Find("CannonMachine Variant 2 (3)").GetComponent<EnemyBehaviour>();
        enemy5 = GameObject.Find("CannonMachine Variant 2 (4)").GetComponent<EnemyBehaviour>();
        enemy6 = GameObject.Find("CannonMachine Variant 2 (5)").GetComponent<EnemyBehaviour>();

    }

    // Update is called once per frame
    void Update()
    {

        Boolean task1Completed = CheckTask1();
        Boolean task2Completed = CheckTask2();

        if (task1Completed && !conditionMet1)
        {
            UpdateCheckList1();
            conditionMet1 = true;
        }
        if (task2Completed && !conditionMet2)
        {
            UpdateCheckList2();
            conditionMet2 = true;
        }

    }

    Boolean CheckTask1()
    {
        if (gun1.equipped || gun2.equipped || gun3.equipped)
            return true;
        else
            return false;
    }

    Boolean CheckTask2()
    {

        if (!enemy1.alive && !enemy2.alive && !enemy3.alive && !enemy4.alive && !enemy5.alive && !enemy6.alive)
            return true;
        else
            return false;

    }

    void UpdateCheckList1()
    {
        challengesList1.color = Color.green;
    }

    void UpdateCheckList2()
    {
        challengesList2.color = Color.green;
    }

    void UpdateCheckList3()
    {
        challengesList3.color = Color.green;
    }

}
