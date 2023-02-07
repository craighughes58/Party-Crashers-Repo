using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class BossBehaviour : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transition _transition;

    #region Scipt Dependencies
    [Header("Script Dependencies")]
    [SerializeField] private BossAttacks _bossAttacks;

    #endregion

    #region Health 
    [Header("Health")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    #endregion

    #region Phases & Activation
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

    #endregion

    #region Movement Variables
    private Vector3 _moveVelocity;

    [Header("Movement Variables")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    private Rigidbody _rb;
    #endregion

    #endregion

    #region Functions

    #region Awake, Start, Update
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
    #endregion

    #region Getters
    /// <returns> _currentBossState as a string </returns>
    public string GetBossState()
    {
        return _currentBossState.ToString();
    }

    #endregion

    #region Boss fight

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

    #region State Control Functions
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
    /// Sets the boss state to exhaustion
    /// </summary>
    public void EnterExhaustion()
    {
        StartCoroutine(MoveToExhPos());
    }

    /// <summary>
    /// Sets the boss state to attack
    /// </summary>
    public void EnterAttack()
    {
        StartCoroutine(MoveToAtkPos());
    }

    /// <summary>
    /// Initiates vulnerability and sets boss state
    /// </summary>
    private void BeginExhaustion()
    {
        SetBossState(BossState.EXHAUSTION);
        StartCoroutine(_bossAttacks.ExhaustionPhase());
    }

    /// <summary>
    /// Initiates missile spawning & sets boss state
    /// </summary>
    private void BeginAttack()
    {
        SetBossState(BossState.ATTACK);
        StartCoroutine(_bossAttacks.AttackPhase());
    }
    #endregion

    #region Movement & State begin Functions
    /// <summary>
    /// Moves the boss to the attack position
    /// </summary>
    private IEnumerator MoveToAtkPos()
    {
        while (Vector3.Distance(transform.position, _attackPos.position) > 0.001f)
        {
            RotateToVal(transform.rotation, _attackPos.rotation, _rotateSpeed);
            MoveToLocation(transform.position, _attackPos.position, _moveSpeed);

            yield return new WaitForSeconds(0.01f);
        }
        BeginAttack();
    }

    /// <summary>
    /// Moves the boss to the exh pos where the player can hit the eye
    /// </summary>
    private IEnumerator MoveToExhPos()
    {
        while (Vector3.Distance(transform.position, _exhaustPos.position) > 0.001f)
        {
            RotateToVal(transform.rotation, _exhaustPos.rotation, _rotateSpeed);
            MoveToLocation(transform.position, _exhaustPos.position, _moveSpeed);

            yield return new WaitForSeconds(0.01f);
        }
        BeginExhaustion();
    }

    #endregion

    #region Helper Functions
    /// <summary>
    /// Moves the object from one point to another at a speed
    /// </summary>
    /// <param name="from"> original location </param>
    /// <param name="to"> ending location </param>
    /// <param name="moveSpeed"> speed of movement </param>
    private void MoveToLocation(Vector3 from, Vector3 to, float moveSpeed)
    {
        transform.position = Vector3.Lerp(from, to, moveSpeed);
    }

    /// <summary>
    /// Rotates the object at a specific speed
    /// </summary>
    /// <param name="from"> origal rotation </param>
    /// <param name="to"> end rotation </param>
    /// <param name="rotateSpeed"> speed of rotation </param>
    private void RotateToVal(Quaternion from, Quaternion to, float rotateSpeed)
    {
        _rb.MoveRotation(Quaternion.RotateTowards(from, to, rotateSpeed));
    }

    #endregion

    #region Boss life functions
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
            _bossAttacks.StopAllCoroutines();
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
    #endregion
}
