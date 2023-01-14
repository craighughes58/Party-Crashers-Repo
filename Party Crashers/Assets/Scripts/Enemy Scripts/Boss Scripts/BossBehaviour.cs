using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField]
    private int health;

    [SerializeField]
    private bool attackMode;//the boss has two modes, attack and exhaustion. When the boss is attacking they can't take damage but when they're exhausted they can't attack and they're open to taking damage

    [SerializeField]
    private int maxAttacks; //the maximum amount of attacks the boss can make before entering exhaustion

    private int currentAttacks; //this is the variable that keeps track of how many attacks the boss has

    private bool isActivated; //the boss will stay stationary until he is activated by an external source in the ActivateBoss method


    // Start is called before the first frame update
    void Start()
    {
        isActivated = false;
        currentAttacks = maxAttacks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Here is where the boss will check if they take damage when they're hit
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //when the boss is hit by a bat, if they're not in attack mode and are activated they will lose a life
    }


    /// <summary>
    /// This method will be called by the game controller to activate the attacks of the boss
    /// </summary>
    public void ActivateBoss()
    {

    }
}
