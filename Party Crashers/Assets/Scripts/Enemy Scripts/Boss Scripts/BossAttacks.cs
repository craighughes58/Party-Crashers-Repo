using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    #region Variables
    [Header("Script/Object Dependencies")]
    [SerializeField] private BossBehaviour _bossBehaviour;

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
    [SerializeField] Vector3 _missileSpawnPoint;
    #endregion

    private void Awake()
    {
        _currentAttacks = _maxAttacks;
    }

    /// <summary>
    /// Controls the exhaustion phase duration & events
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExhaustionPhase()
    {
        yield return new WaitForSeconds(_exhaustionTimer);

        _bossBehaviour.EnterAttack();
    }

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
        Instantiate(_missileObject, _missileSpawnPoint, Quaternion.identity);
        DecreaseAttacks();
    }
}
