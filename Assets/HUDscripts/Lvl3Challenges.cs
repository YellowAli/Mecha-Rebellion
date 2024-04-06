using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Lvl3Challenges : MonoBehaviour
{
    // this is the final version
    public TextMeshProUGUI challengesList1, challengesList2;
    public Boolean equippedWeapon, eliminatedAll;
    PickUpController gun1, gun2, gun3;
    EnemyBehaviourBoss boss;
    //     EnemyBehaviour enemy2, enemy3, enemy4, enemy6;
    private Boolean conditionMet1 = false, conditionMet2 = false;



    // Start is called before the first frame update
    void Start()
    {

        gun1 = GameObject.Find("SciFiGunLightRad Variant").GetComponent<PickUpController>();
        gun2 = GameObject.Find("SciFiGunLightBlack Variant").GetComponent<PickUpController>();
        gun3 = GameObject.Find("SciFiGunLightBlue Variant").GetComponent<PickUpController>();

        boss = GameObject.Find("Boss").GetComponent<EnemyBehaviourBoss>();
        

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
        if (boss.health == 0)
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


    public bool CanProceedToNextLevel()
    {
        // Your logic to determine if the challenges are completed
        // For example:
        return CheckTask1() && CheckTask2();
    }

}
