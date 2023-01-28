using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //The current point in the map the player has reached
    //This int iterates across the different scense
    private int SectionNum = 0;

    private int currentEnemyNum;

    private StopAtPoints SAP;

    // Start is called before the first frame update
    void Start()
    {
        SAP = GameObject.Find("XR Origin").GetComponent<StopAtPoints>();
    }

    //This script will spawn the enemies after each trigger
    public void MoveToNextPoint()
    {
        SectionNum++;
        switch (SectionNum)
        {
            case 1:
                return;
            case 2:
                return;
            case 3:
                return;
            default:
                print("SOMETHING IS TERRIBLY WRONG");
                return;
        }

    }
    //calls when an enemy is destroyed
    //if none are less then you move to the next zone
    public void LoseEnemy()
    {
        currentEnemyNum--;
        if(currentEnemyNum <= 0)
        {
            SAP.StartPlayer();
        }
    }

    //called when an enemy is instantiated
    public void AddEnemy()
    {
        currentEnemyNum++;
    }
}
