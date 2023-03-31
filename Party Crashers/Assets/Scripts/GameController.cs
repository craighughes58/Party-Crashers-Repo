using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

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

    private GameObject leftHand, rightHand, wrappingPapper, shield;

    [Tooltip("Is the game paused?")]
    [SerializeField]
    private bool isPaused = false;

    //UI game object references so they can be turned on and off
    [SerializeField]
    private GameObject pauseMenu, settingsMenu;

    public BirdBehaviour bb;
    private GameObject tempBird;

    [SerializeField] BossBehaviour _bossBehaviour;

    [SerializeField] bool devBossTesting;

    [Header("Audio")]
    public AudioManager am;
    public int musicTrack = 0;
    private int currentMusic;

    public List<BirdBehaviour> birds = new List<BirdBehaviour>();
    // Start is called before the first frame update
    void Start()
    {
        
        DeactivateUIMenu(settingsMenu);
        DeactivateUIMenu(pauseMenu);
        SAP = GameObject.Find("XR Origin").GetComponent<StopAtPoints>();
        leftHand = GameObject.Find("LeftHand (Smooth locomotion)");
        rightHand = GameObject.Find("RightHand (Teleport Locomotion)");
        wrappingPapper = GameObject.FindGameObjectWithTag("Bat");
        shield = GameObject.FindGameObjectWithTag("Shield");
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        isPaused = false;
        //pauseMenu.SetActive(false);
        //settingsMenu.SetActive(false);
        SwapVisibiltyHands(false);
        SwapHands(true);
        if(devBossTesting)
            DevBossTest();

    }

    //This script will spawn the enemies after each trigger
    public void MoveToNextPoint()
    {
        am.Stop("Path_Footsteps");
        am.Play("Enemy_Spawn");
        musicTrack++;
        print("Playing music track number " + musicTrack);

        SectionNum++;
        switch (SectionNum)
        {
            //Tutorial Spawns
            case 1:
                Vector3 spawnLocation = new Vector3(-20, 0, -115);
                AddEnemy();//this is for the tutorial bird
                am.SwitchMusic(musicTrack);
                return;
            //First Area Spawns
            case 2:
                Vector3 spawnLocationStop1 = new Vector3(-30, 0, -95);
                Instantiate(Hyena, spawnLocationStop1, Quaternion.identity);
                AddEnemy();

                spawnLocationStop1.x = -20;
                spawnLocationStop1.z = -80;
                Instantiate(HyenaVariation, spawnLocationStop1, Quaternion.identity);
                AddEnemy();

                spawnLocationStop1.x = -15f;
                spawnLocationStop1.z = -70f;
                Instantiate(Hyena, spawnLocationStop1, Quaternion.identity);
                AddEnemy();
                am.SwitchMusic(musicTrack);
                return;
            case 3:
                Vector3 spawnLocationStop2 = new Vector3(71.57f, 18.86f, 20.84f);
                birds.Add(Instantiate(Bird, spawnLocationStop2, Quaternion.identity).GetComponent<BirdBehaviour>());
                AddEnemy();

                spawnLocationStop2.x = 14.24f;
                spawnLocationStop2.y = 12.13f;
                spawnLocationStop2.z = 5.77f;
                birds.Add(Instantiate(Bird, spawnLocationStop2, Quaternion.identity).GetComponent<BirdBehaviour>());
                AddEnemy();

                spawnLocationStop2.x = 42.84f;
                spawnLocationStop2.y = 12.09f;
                spawnLocationStop2.z = -19.19f;
                birds.Add(Instantiate(Bird, spawnLocationStop2, Quaternion.identity).GetComponent<BirdBehaviour>());
                AddEnemy();

                for ( int i=1; i < birds.Count; i++)
                {
                    birds[i].bb = birds[i - 1];
                }
                birds[0].bb = birds[birds.Count-1];

                birds[0].StartCoroutine(birds[0].Attack());
                am.SwitchMusic(musicTrack);
                return;
            case 4:
                birds.Clear();
                Vector3 spawnLocationStop3 = new Vector3(38, 0, 140);
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

                spawnLocationStop3.x = -43.24f;
                spawnLocationStop3.y = 39.42f;
                spawnLocationStop3.z = 93.62f;
                birds.Add(Instantiate(Bird, spawnLocationStop3, Quaternion.identity).GetComponent<BirdBehaviour>());
                AddEnemy();

                spawnLocationStop3.x = -34.39f;
                spawnLocationStop3.y = 39.23f;
                spawnLocationStop3.z = 111.7f;
                birds.Add(Instantiate(Bird, spawnLocationStop3, Quaternion.identity).GetComponent<BirdBehaviour>());
                AddEnemy();

                for (int i = 1; i < birds.Count; i++)
                {
                    birds[i].bb = birds[i - 1];
                }
                birds[0].bb = birds[birds.Count - 1];

                birds[0].StartCoroutine(birds[0].Attack());
                am.SwitchMusic(musicTrack);
                return;
            case 5:
                StartCoroutine(_bossBehaviour.ActivateBoss());
                
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
        if (currentEnemyNum <= 0)
        {
            print("OSJGSD");
            SAP.StartPlayer();
        }
    }

    public void LoseBird(BirdBehaviour removeMe)
    {
        birds.Remove(removeMe);
        print(birds.Count);
        if (birds.Count == 2)
        {
            birds[0].bb = birds[1];
            birds[1].bb = birds[0];
        }
        else if (birds.Count==1)
        {
            birds[0].bb = birds[0];
        }
    }

    //called when an enemy is instantiated
    public void AddEnemy()
    {
        print("ADD");
        currentEnemyNum++;
    }

    public void SwapHands(bool leftHanded)
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
            wrappingPapper.transform.localRotation = Quaternion.Euler(110f, 0f, 0f);
            shield.transform.localRotation = Quaternion.Euler(0f,90f,90f);


        }
    }

    /// <summary>
    /// Sets the visibility of the ray hands when called based on input of param
    /// </summary>
    /// <param name="visible">Visibility of the ray. True when in menu false when in game</param>
    public void SwapVisibiltyHands(bool visible)
    {
        leftHand.gameObject.transform.GetChild(2).gameObject.GetComponent<XRInteractorLineVisual>().enabled = visible;
        rightHand.gameObject.transform.GetChild(2).gameObject.GetComponent<XRInteractorLineVisual>().enabled = visible;
        wrappingPapper.gameObject.SetActive(!visible);
        shield.gameObject.SetActive(!visible);
    }

    /// <summary>
    /// Changes the scene to the named scene
    /// </summary>
    /// <param name="sceneName"> Name of scene you want to change to </param>
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DeactivateUIMenu(GameObject uiMenu)
    {
        uiMenu.gameObject.SetActive(false);
        am.Play("Menu_Back");
    }

    public void ActivateUIMenu(GameObject uiMenu)
    {
        uiMenu.gameObject.SetActive(true);
        am.Play("Menu_Confirm");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        DeactivateUIMenu(pauseMenu);
        ActivateUIMenu(settingsMenu);
    }
    public void CloseSettings()
    {
        ActivateUIMenu(pauseMenu);
        DeactivateUIMenu(settingsMenu);
    }

    private void OnPause(InputValue value)
    {
        if (isPaused == false)
        {
            am.Stop("Path_Footsteps");
            currentMusic = musicTrack;
            am.SwitchMusic(10);
            am.Play("Pause_Game");

            isPaused = true;
            Time.timeScale = 0;
            SwapVisibiltyHands(true);
            Debug.Log("Menu hands, activate");
            ActivateUIMenu(pauseMenu);
        }
        else if (isPaused == true)
        {
            isPaused = false;
            SwapVisibiltyHands(false);
            Debug.Log("Menu hands, deactivate");
            ResumeScene();
        }
    }

    public void ResumeScene()
    {
        am.Play("Menu_Back");
        am.Stop("Pause_Music");
        am.SwitchMusic(currentMusic);
        am.Play("Path_Footsteps");

        Time.timeScale = 1;
        isPaused = false;
        SwapVisibiltyHands(false);
        Debug.Log("Menu hands, deactivate");
        DeactivateUIMenu(pauseMenu);
        DeactivateUIMenu(settingsMenu);
    }

    /*    private void Update()
        {
            if (currentEnemyNum <= 0)
            {
                SAP.StartPlayer();
            }
        }*/

    /// <summary>
    /// bypassing gameplay to test boss 
    /// </summary>
    private void DevBossTest()
    {
        GameObject.Find("XR Origin").GetComponent<BezierFollow>().enabled = false;
        SAP.gameObject.transform.position = new Vector3(-33.42f, 13.55f, 97.933f);
        StartCoroutine(_bossBehaviour.ActivateBoss());
    }
}
