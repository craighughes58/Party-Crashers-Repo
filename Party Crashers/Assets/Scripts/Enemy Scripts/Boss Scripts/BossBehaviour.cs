using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Script Dependencies")]
    [SerializeField] private BossAttacks _bossAttacks;
    [SerializeField] private Transition _transition;

    [Header("Health")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    //the boss has two modes, attack and exhaustion. When the boss is attacking they can't take damage but when they're exhausted they can't attack and they're open to taking damage
    private enum BossState { ATTACK, EXHAUSTION }
    [Header("States")]
    [SerializeField] private BossState _currentBossState;

    [Header("Boss Activation")]
    [Range(0f, 10f)]
    [SerializeField] private float _bossActivationTime;
    private bool _isActivated; //the boss will stay stationary until he is activated by an external source in the ActivateBoss method

    #endregion

    #region Functions

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //_isActivated = false;
        StartCoroutine(ActivateBoss());
    }

    private void Update()
    {
    }

    #region Getters
    /// <returns> _currentBossState as a string </returns>
    public string GetBossState()
    {
        return _currentBossState.ToString();
    }

    #endregion

    #region Boss fight
    /// <summary>
    /// Sets the boss state
    /// </summary>
    /// <param name="state"> string name of the state being set </param>
    private void SetBossState(BossState state)
    {
        Debug.Log("Entering " + state);
        _currentBossState = state;
        switch (state)
        {
            case BossState.ATTACK:
                break;
            case BossState.EXHAUSTION:
                break;
        }
    }

    /// <summary>
    /// This method will be called by the game controller to activate the attacks of the boss
    /// </summary>
    private IEnumerator ActivateBoss()
    {
        print("Boss is paused");
        yield return new WaitForSeconds(_bossActivationTime);
        print("Boss is active");
        EnterAttack();
        // play starting animation
        // set initial attack state

    }

    /// <summary>
    /// Sets the boss state to exhaustion
    /// </summary>
    public void EnterExhaustion()
    {
        SetBossState(BossState.EXHAUSTION);
        StartCoroutine(_bossAttacks.ExhaustionPhase());
    }

    /// <summary>
    /// Sets the boss state to attack
    /// </summary>
    public void EnterAttack()
    {
        SetBossState(BossState.ATTACK);
        StartCoroutine(_bossAttacks.AttackPhase());
    }

    /// <summary>
    /// Decreases boss health by 1
    /// </summary>
    public void LoseHealth()
    {
        _currentHealth--;

        if (_currentHealth == 0)
        {
            BossDeath();
        }
        else
        {
            EnterAttack();
        }
    }  

    /// <summary>
    /// Controls the boss's death events
    /// </summary>
    private void BossDeath()
    {
        Debug.Log("boss dies");
        //do stuff when the boss dies

        _transition.LoadLevel();
    }
    #endregion
    #endregion
}
