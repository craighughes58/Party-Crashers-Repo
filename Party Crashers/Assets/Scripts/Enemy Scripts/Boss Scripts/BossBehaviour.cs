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

    [Header("Phase Positions")]
    [SerializeField] private Transform _attackPos;
    [SerializeField] private Transform _exhaustPos;

    #region Movement Variables
    private Vector3 _moveVelocity;

    [Header("Movement Variables")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    private Rigidbody _rb;
    #endregion

    #endregion

    #region Functions

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _moveVelocity = new Vector3(0f, _moveSpeed, 0f);
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(ActivateBoss());
        _rb = GetComponent<Rigidbody>();
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
        MoveToExhPos();
        SetBossState(BossState.EXHAUSTION);
        StartCoroutine(_bossAttacks.ExhaustionPhase());
    }

    /// <summary>
    /// Sets the boss state to attack
    /// </summary>
    public void EnterAttack()
    {
        StartCoroutine(MoveToAtkPos());
        SetBossState(BossState.ATTACK); 
    }

    /// <summary>
    /// Moves the boss to the attack position
    /// </summary>
    private IEnumerator MoveToAtkPos()
    {
        //gameObject.transform.position = _attackPos.position;
        //gameObject.transform.rotation = _attackPos.rotation;
        //_rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, _attackPos.rotation, _rotateSpeed));

        while (Vector3.Distance(transform.position, _attackPos.position) > .1)
        {
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, _attackPos.rotation, _rotateSpeed));
            transform.position = Vector3.MoveTowards(transform.position, _attackPos.position, _moveSpeed);

            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(_bossAttacks.AttackPhase());
    }

    /// <summary>
    /// Moves the boss to the exh pos where the player can hit the eye
    /// </summary>
    private void MoveToExhPos()
    {
        gameObject.transform.position = _exhaustPos.position;
        gameObject.transform.rotation = _exhaustPos.rotation;
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
        else if(_currentHealth > 0)
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
