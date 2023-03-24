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
    public Animator animator;

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
    [SerializeField] private float _transitionTime;
    #endregion

    #region Animation

    public int currentFrame = 0;
    AnimatorClipInfo[] animationClip;
    int amountTimeLooped = 0;

    #endregion

    #endregion

    #region Functions

    #region Awake, Start, Update
    private void Awake()
    {
        _currentHealth = _maxHealth;
        _moveVelocity = new Vector3(0f, _transitionTime, 0f);
        GameObject g = GameObject.Find("Boss Movement Points");
        _attackPos = g.transform.GetChild(0).transform;
        _exhaustPos = g.transform.GetChild(1).transform;
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        //StartCoroutine(ActivateBoss());
    }

    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("RightAttack"))
        {
            animationClip = animator.GetCurrentAnimatorClipInfo(0);

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime - amountTimeLooped > 1)
            {
                amountTimeLooped++;
            }
            currentFrame = (int)((animator.GetCurrentAnimatorStateInfo(0).normalizedTime - amountTimeLooped) * (animationClip[0].clip.length * 24));
        }
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
    public IEnumerator ActivateBoss()
    {
        yield return new WaitForSeconds(_bossActivationTime);
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
    public IEnumerator EnterExhaustion()
    {
        yield return new WaitForSeconds(4f);
        StartCoroutine(MoveToExhPos());
    }

    /// <summary>
    /// Sets the boss state to attack
    /// </summary>
    public void EnterAttack()
    {
        Debug.Log("EnterAttack");
        StartCoroutine(MoveToAtkPos());
    }

    /// <summary>
    /// Initiates vulnerability and sets boss state
    /// </summary>
    private void BeginExhaustion()
    {
        animator.SetTrigger("StopAnims");
        SetBossState(BossState.EXHAUSTION);
        gameObject.GetComponent<EyeBehaviour>().beenHit = false;
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

    public void ResetTriggers()
    {
        animator.ResetTrigger("Left");
        animator.ResetTrigger("Right");
        animator.ResetTrigger("StopAnims");
    }
    #endregion

    #region Movement & State begin Functions
    /// <summary>
    /// Moves the boss to the attack position
    /// </summary>
    private IEnumerator MoveToAtkPos()
    {
        float pathPercentage = 0;
        Vector3 startPos = transform.position;
        Quaternion startQ = transform.rotation;

        while (pathPercentage < _transitionTime)
        {
            RotateToVal(startQ, _attackPos.rotation, pathPercentage / _transitionTime);
            MoveToLocation(startPos, _attackPos.position, pathPercentage / _transitionTime);

            pathPercentage += Time.deltaTime;
            yield return null;
        }
        BeginAttack();
    }

    /// <summary>
    /// Moves the boss to the exh pos where the player can hit the eye
    /// </summary>
    private IEnumerator MoveToExhPos()
    {
        float pathPercentage = 0;
        Vector3 startPos = transform.position;
        Quaternion startQ = transform.rotation;

        while(pathPercentage < _transitionTime)
        {
            RotateToVal(startQ, _exhaustPos.rotation, pathPercentage / _transitionTime);
            MoveToLocation(startPos, _exhaustPos.position, pathPercentage / _transitionTime);

            pathPercentage += Time.deltaTime;
            yield return null;
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
    private void MoveToLocation(Vector3 from, Vector3 to, float transitionSpeed)
    {
        transform.position = Vector3.Lerp(from, to, transitionSpeed);
    }

    /// <summary>
    /// Rotates the object at a specific speed
    /// </summary>
    /// <param name="from"> origal rotation </param>
    /// <param name="to"> end rotation </param>
    /// <param name="rotateSpeed"> speed of rotation </param>
    private void RotateToVal(Quaternion from, Quaternion to, float transitionSpeed)
    {
        transform.rotation = Quaternion.Lerp(from, to, transitionSpeed);
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
