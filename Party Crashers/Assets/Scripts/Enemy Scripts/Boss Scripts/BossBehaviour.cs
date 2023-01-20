using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField]
    private int health;


    //the boss has two modes, attack and exhaustion. When the boss is attacking they can't take damage but when they're exhausted they can't attack and they're open to taking damage
    private enum BossState { ATTACK, EXHAUSTION }
    [SerializeField] private BossState _currentBossState;

    [SerializeField]
    private int maxAttacks; //the maximum amount of attacks the boss can make before entering exhaustion

    private int currentAttacks; //this is the variable that keeps track of how many attacks the boss has

    private bool isActivated; //the boss will stay stationary until he is activated by an external source in the ActivateBoss method


    // Start is called before the first frame update
    void Start()
    {
        isActivated = false;
        SetBossState("attack");
        currentAttacks = maxAttacks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets the boss state
    /// </summary>
    /// <param name="state"> string name of the state being set </param>
    private void SetBossState(string state)
    {
        switch (state)
        {
            case "attack":
                _currentBossState = BossState.ATTACK;
                break;
            case "exhaustion":
                _currentBossState = BossState.EXHAUSTION;
                break;
        } 
    }


    /// <summary>
    /// This method will be called by the game controller to activate the attacks of the boss
    /// </summary>
    public void ActivateBoss()
    {
        //play starting animation

        // set initial attack state
    }
}
