using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public List<Hoop> hoops;
    public int hoopCounter = 0;
    public TextMeshProUGUI hoopText;

    void Start()
    {
        // Find and add all hoops to the list
        hoops = new List<Hoop>();
        for (int i = 0; i < 23; i++)
        {
            string hoopName = "2d ring target 20 Green" + (i > 0 ? " (" + i + ")" : "");
            Hoop hoop = GameObject.Find(hoopName).GetComponent<Hoop>();
            hoops.Add(hoop);
        }
    }

    void Update()
    {
        // Iterate through hoops and increment counter if passed
        for (int i = 0; i < hoops.Count; i++)
        {
            if (hoops[i].passed && hoopCounter == i)
            {
                hoopCounter++;
                break; // Stop the loop once the current hoop is found passed
            }
        }

        updateText();
    }

    void updateText()
    {
        hoopText.SetText("Passed through " + hoopCounter.ToString() + " hoops");
    }
}