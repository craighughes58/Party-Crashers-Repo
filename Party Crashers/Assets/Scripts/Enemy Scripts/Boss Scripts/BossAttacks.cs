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

    [Header("Timers")]
    [Range(0f, 10f)]
    [SerializeField] private float _exhaustionTimer;

    [Range(0f, 10f)]
    [SerializeField] private float _timeBetweenAttacks;

    [Header("Attack Missiles")]
    [SerializeField] private GameObject _missileObject;
    [SerializeField] private Transform[] _missileSpawnPoint;
    private Vector3 _missileSpawnPos;
    [SerializeField] private GameObject _missileAnimObject;
    public bool isAttacking;

    [Header("Scores")]
    [Tooltip("Amount score decreases if player is hit, increases if deflected, and scored if eye attack")]
    [SerializeField] private int _scoreLost;
    [SerializeField] private int _scoreGainedDeflect;
    [SerializeField] private int _scoreGainedAttack;

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
        if (_bossBehaviour.currentFrame == 43 && isAttacking)
        {
            GameObject g = Instantiate(_missileObject, _missileSpawnPoint[attackPoint].position, Quaternion.identity);
            g.GetComponent<MissileBehaviour>()._bossBehaviour = _bossBehaviour;
            _missileAnimObject.SetActive(false);
        }
        if(_bossBehaviour.currentFrame >= 83 && isAttacking)
        {
            isAttacking = false;
            _bossBehaviour.ResetTriggers();
            _bossBehaviour.animator.SetTrigger("StopAnims");
        }
    }
    #endregion

    #region Attacks

    /// <summary>
    /// Controls the attach phase events
    /// </summary>
    /// <returns></returns>
    public IEnumerator AttackPhase()
    {
        _currentAttacks = _maxAttacks;
        while (_currentAttacks > 0)
        {
            AttackPlayer();
            yield return new WaitForSeconds(_timeBetweenAttacks);
        }
        StartCoroutine(_bossBehaviour.EnterExhaustion());
    }

    /// <summary>
    /// Decreases boss attacks by 1
    /// </summary>
    private void DecreaseAttacks()
    {
        _currentAttacks--;
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
            _bossBehaviour.animator.SetTrigger("Right");
            _missileAnimObject.SetActive(true);
            isAttacking = true;
        }
        else
        {
            _bossBehaviour.ResetTriggers();
            _bossBehaviour.animator.SetTrigger("Left");
            _missileAnimObject.SetActive(true);
            isAttacking = true;
        }

        //GameObject g = Instantiate(_missileObject, _missileSpawnPos, Quaternion.identity);
        //g.GetComponent<MissileBehaviour>()._bossBehaviour = _bossBehaviour;
        DecreaseAttacks();
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
    #endregion

    #endregion
}
