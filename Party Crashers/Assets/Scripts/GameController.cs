using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //The current point in the map the player has reached
    //This int iterates across the different scense
    private int SectionNum = 0;

    [SerializeField]
    private int currentEnemyNum;

    private StopAtPoints SAP;

    [Tooltip("Reference to the hyena prefab")]
    [SerializeField]
    private GameObject Hyena;
    [SerializeField]
    private GameObject HyenaVariation;

    [Tooltip("Reference to the bird prefab")]
    [SerializeField]
    private GameObject Bird;
    [SerializeField]
    private GameObject BirdVariation;

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
            //Tutorial Spawns
            case 1:
                Vector3 spawnLocation = new Vector3(10, 0, -115);
                Instantiate(Hyena, spawnLocation, Quaternion.identity);
                AddEnemy();

                spawnLocation.x = 31;
                spawnLocation.y = 7.85f;
                spawnLocation.z = -95;
                Instantiate(Bird, spawnLocation, Quaternion.identity);
                AddEnemy();
                return;
            //First Area Spawns
            case 2:
                Vector3 spawnLocationStop1 = new Vector3(-30,0,-95);
                Instantiate(Hyena, spawnLocationStop1, Quaternion.identity);
                AddEnemy();

                spawnLocationStop1.x = -20;
                spawnLocationStop1.z = -80;
                Instantiate(HyenaVariation, spawnLocationStop1, Quaternion.identity);
                AddEnemy();

                spawnLocationStop1.x = -5;
                spawnLocationStop1.z = -75;
                Instantiate(Hyena, spawnLocationStop1, Quaternion.identity);
                AddEnemy();
                return;
            case 3:
                Vector3 spawnLocationStop2 = new Vector3(7, 23, 6);
                Instantiate(Bird, spawnLocationStop2, Quaternion.identity);
                AddEnemy();
                spawnLocationStop2.x = -2;
                spawnLocationStop2.y = 19;
                spawnLocationStop2.z = 0;
                Instantiate(Bird, spawnLocationStop2, Quaternion.identity);
                AddEnemy();
                spawnLocationStop2.x = 40;
                spawnLocationStop2.y = 25;
                spawnLocationStop2.z = -17;
                Instantiate(Bird, spawnLocationStop2, Quaternion.identity);
                AddEnemy();
                print("spawn birds");
                return;
            case 4:
                Vector3 spawnLocationStop3 = new Vector3(38, 0,140);
                Instantiate(HyenaVariation, spawnLocationStop3, Quaternion.identity);
                AddEnemy();
                spawnLocationStop3.x = 30;
                Instantiate(Hyena, spawnLocationStop3, Quaternion.identity);
                AddEnemy();
                spawnLocationStop3.x = 22;
                Instantiate(HyenaVariation, spawnLocationStop3, Quaternion.identity);
                AddEnemy();
                spawnLocationStop3.x = 5;
                spawnLocationStop3.z = 70;
                Instantiate(HyenaVariation, spawnLocationStop3, Quaternion.identity);
                AddEnemy();
                spawnLocationStop3.x = 1;
                spawnLocationStop3.z = 85;
                Instantiate(Hyena, spawnLocationStop3, Quaternion.identity);
                AddEnemy();

                spawnLocationStop3.x = -41;
                spawnLocationStop3.y = 48.17f;
                spawnLocationStop3.z = 83;
                Instantiate(Bird, spawnLocationStop3, Quaternion.identity);
                AddEnemy();
                spawnLocationStop3.x = -22;
                spawnLocationStop3.y = 48.17f;
                spawnLocationStop3.z = 117;
                Instantiate(Bird, spawnLocationStop3, Quaternion.identity);
                AddEnemy();
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
        print(currentEnemyNum);
        currentEnemyNum--;
        if(currentEnemyNum <= 0)
        {
            print("OSJGSD");
            SAP.StartPlayer();
        }
    }

    //called when an enemy is instantiated
    public void AddEnemy()
    {
        print("ADD");
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

/*    private void Update()
    {
        if (currentEnemyNum <= 0)
        {
            SAP.StartPlayer();
        }
    }*/
}
