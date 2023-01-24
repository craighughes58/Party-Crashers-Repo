using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Health")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    //the boss has two modes, attack and exhaustion. When the boss is attacking they can't take damage but when they're exhausted they can't attack and they're open to taking damage
    private enum BossState { ATTACK, EXHAUSTION }
    [Header("States")]
    [SerializeField] private BossState _currentBossState;

    [Header("Attacks")]
    [SerializeField] private int _maxAttacks; //the maximum amount of attacks the boss can make before entering exhaustion
    [SerializeField] private int _currentAttacks; //this is the variable that keeps track of how many attacks the boss has

    [Header("Timers")]
    [Range(0f, 10f)]
    [SerializeField] private float _exhaustionTimer;

    [Range(0f, 10f)]
    [SerializeField] private float _timeBetweenAttacks;

    private bool _isActivated; //the boss will stay stationary until he is activated by an external source in the ActivateBoss method
    #endregion

    #region Functions
    // Start is called before the first frame update
    private void Start()
    {
        //_isActivated = false;
        _currentAttacks = _maxAttacks;
        _currentHealth = _maxHealth;
        ActivateBoss();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Q"))
        {
            LoseHealth();
        }
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
    private void ActivateBoss()
    {
        // play starting animation
        // set initial attack state
        EnterAttack();
    }

    /// <summary>
    /// Sets the boss state to exhaustion
    /// </summary>
    public void EnterExhaustion()
    {
        SetBossState(BossState.EXHAUSTION);
    }

    /// <summary>
    /// Sets the boss state to attack
    /// </summary>
    public void EnterAttack()
    {
        SetBossState(BossState.ATTACK);
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
            StartCoroutine(AttackPhase());
        }
    }

    /// <summary>
    /// Decreases boss attacks by 1
    /// </summary>
    private void DecreaseAttacks()
    {
        _currentAttacks--;
    }

    /// <summary>
    /// Controls the exhaustion phase duration & events
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExhaustionPhase()
    {
        EnterExhaustion();

        yield return new WaitForSeconds(_exhaustionTimer);

        EnterAttack();
    }

    /// <summary>
    /// Controls the attach phase events
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackPhase()
    {
        EnterAttack();
        while (_currentAttacks > 0)
        {
            AttackPlayer();
        }
        yield return new WaitForSeconds(_timeBetweenAttacks);

        EnterExhaustion();
    }

    /// <summary>
    /// Attacks the player
    /// </summary>
    private void AttackPlayer()
    {
        print("Boss attacks player");
        DecreaseAttacks();
        if(_currentAttacks == 0)
        {
            StartCoroutine(ExhaustionPhase());
        }
    }

    /// <summary>
    /// Controls the boss's death events
    /// </summary>
    private void BossDeath()
    {
        //do stuff when the boss dies
    }
    #endregion
    #endregion
}
