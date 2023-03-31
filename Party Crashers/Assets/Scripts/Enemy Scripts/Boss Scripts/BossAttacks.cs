using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    #region Variables
    [Header("Script/Object Dependencies")]
    [SerializeField] private BossBehaviour _bossBehaviour;
    [SerializeField] private PlayerBehaviour _playerBehaviour;

    [Header("Attacks")]
    [SerializeField] private int _maxAttacks; //the maximum amount of attacks the boss can make before entering exhaustion
    [SerializeField] private int _currentAttacks; //this is the variable that keeps track of how many attacks the boss has
    public int attackPoint;
    private List<GameObject> RecentMissiles = new List<GameObject>();

    [Header("Timers")]
    [Range(0f, 10f)]
    [SerializeField] private float _exhaustionTimer;

    [Header("Attack Missiles")]
    [SerializeField] private GameObject _missileObject;
    [SerializeField] private Transform[] _missileSpawnPoint;
    private Vector3 _missileSpawnPos;
    [SerializeField] private GameObject _missileAnimObject;
    public bool isAttacking;
    private bool hitSignal = false;

    [Header("Scores")]
    [Tooltip("Amount score decreases if player is hit, increases if deflected, and scored if eye attack")]
    [SerializeField] private int _scoreLost;
    [SerializeField] private int _scoreGainedDeflect;
    [SerializeField] private int _scoreGainedAttack;

    public bool spawnedOne = false;
    [SerializeField] private bool decreasedAttack;

    #region Getters
    [HideInInspector] public int ScoreLost => _scoreLost;
    [HideInInspector] public int ScoreGainedDeflect => _scoreGainedDeflect;
    [HideInInspector] public int ScoreGainedAttack => _scoreGainedAttack;
    [HideInInspector] public PlayerBehaviour PB => _playerBehaviour;
    [HideInInspector] public BossBehaviour BH => _bossBehaviour;

    #endregion

    #endregion

    #region Functions

    #region Awake, Start, Update
    private void Awake()
    {
        _currentAttacks = _maxAttacks;
        _playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    private void Update()
    {
        if (_bossBehaviour.currentFrame == 83 && !decreasedAttack)
        {
            DecreaseAttacks();
            _bossBehaviour.ResetTriggers();
        }
        else if (_bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack") || _bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("RightAttack"))
        {
            if (_bossBehaviour.currentFrame == 43 && isAttacking && !spawnedOne)
            {
                Instantiate(_missileObject, _missileSpawnPoint[attackPoint].position, Quaternion.identity).GetComponent<MissileBehaviour>()._bossBehaviour = _bossBehaviour;
                _missileAnimObject.SetActive(false);
                spawnedOne = true;
                decreasedAttack = false;
            }
            else if (_bossBehaviour.currentFrame != 43 && spawnedOne)
            {
                spawnedOne = false;
            }
        }
    }
    #endregion

    #region Attacks

    /// <summary>
    /// Controls the attach phase events
    /// </summary>
    /// <returns></returns>
    /// 

    public IEnumerator AttackPhase()
    {
        _currentAttacks = _maxAttacks;
        int xAttack = _currentAttacks;

        while (!hitSignal)//_currentAttacks > 0 WHILE THE BOSS ISNT'T HIT
        {
            yield return new WaitWhile(() => !_bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("Nothing"));
            AttackPlayer();
            yield return new WaitWhile(() => _bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("Nothing"));

            //yield return new WaitWhile(() => !_bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("Nothing"));
            //yield return new WaitWhile(() => _currentAttacks == xAttack);
            //print(xAttack + " " + _currentAttacks); 
            //xAttack--;

            //print(xAttack + " " + _currentAttacks);
        }
        
        yield return new WaitWhile(() => !_bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("Nothing"));

        //StartCoroutine(_bossBehaviour.EnterExhaustion());
        _bossBehaviour.BeginExhaustion(); //ONCE THE BOSS IS HIT ENTER EXHAUSTION
    }

    /// <summary>
    /// Decreases boss attacks by 1
    /// </summary>
    private void DecreaseAttacks()
    {
        _currentAttacks--;
        decreasedAttack = true;
    }

    /// <summary>
    /// Attacks the player
    /// </summary>
    private void AttackPlayer()
    {
        attackPoint = UnityEngine.Random.Range(0, 2);   // Chooses either right or left attack

        // Boss plays attack anim
        if (attackPoint == 0)
        {
            _bossBehaviour.ResetTriggers();
            _bossBehaviour.animator.SetTrigger("Left");
            // attack anim
            _missileAnimObject.SetActive(true);
            isAttacking = true;
        }
        else
        {
            _bossBehaviour.ResetTriggers();
            _bossBehaviour.animator.SetTrigger("Right");
            // attack anim
            _missileAnimObject.SetActive(true);
            isAttacking = true;
        }
    }
    #endregion

    #region Exhaustion
    /// <summary>
    /// Controls the exhaustion phase duration & events
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExhaustionPhase()
    {
        //yield return new WaitForSeconds(_exhaustionTimer);
        _bossBehaviour.Invoke("EnterAttack", _exhaustionTimer);
        yield return null;
        //_bossBehaviour.EnterAttack();
    }

    public void AddProjectile(GameObject RM)
    {
        RecentMissiles.Add(RM);
    }

    public void RemoveAllMissiles()
    {
        foreach(GameObject m in RecentMissiles)
        {
            Destroy(m);
        }
    }
    public void ActivateHitSignal()
    {
        hitSignal = true;
    }
    public void DeactivateHitSignal()
    {
        hitSignal = false;
    }
    #endregion

    #endregion
}
