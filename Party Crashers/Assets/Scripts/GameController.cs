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

    [Tooltip("Reference to the hyena prefab")]
    [SerializeField]
    private GameObject Hyena;

    [Tooltip("Reference to the bird prefab")]
    [SerializeField]
    private GameObject Bird;

    //reference to the squid
    private GameObject Squid;

    public bool leftHanded = false;
    private GameObject leftHand;
    private GameObject rightHand;
    public GameObject wrappingPapper;
    public GameObject shield;
    // Start is called before the first frame update
    void Start()
    {
        SAP = GameObject.Find("XR Origin").GetComponent<StopAtPoints>();
        leftHand = GameObject.Find("LeftHand (Smooth locomotion)");
        rightHand = GameObject.Find("RightHand (Teleport Locomotion)");

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

    public void SwapHands()
    {
        if (leftHand != null && rightHand != null && wrappingPapper != null && shield != null)
        {
            Vector3 localPos = wrappingPapper.transform.localPosition;
            Vector3 localPos2 = shield.transform.localPosition;
            if (!leftHanded)
            {
                wrappingPapper.transform.parent = rightHand.transform;
                shield.transform.parent = leftHand.transform;
            }
            else
            {
                wrappingPapper.transform.parent = leftHand.transform;
                shield.transform.parent = rightHand.transform;
            }
            wrappingPapper.transform.localPosition = localPos;
            shield.transform.localPosition = localPos2;

        }
    }    
}
