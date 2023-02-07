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

    [Header("Timers")]
    [Range(0f, 10f)]
    [SerializeField] private float _exhaustionTimer;

    [Range(0f, 10f)]
    [SerializeField] private float _timeBetweenAttacks;

    [Header("Attack Missiles")]
    [SerializeField] private GameObject _missileObject;
    [SerializeField] private Transform _missileSpawnPoint;
    private Vector3 _missileSpawnPos;

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
        _missileSpawnPos = _missileSpawnPoint.position;
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
        _bossBehaviour.EnterExhaustion();
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
        Instantiate(_missileObject, _missileSpawnPos, Quaternion.identity);
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
        yield return new WaitForSeconds(_exhaustionTimer);

        _bossBehaviour.EnterAttack();
    }
    #endregion

    #endregion
}
