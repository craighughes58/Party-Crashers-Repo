using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    #region Variables
    [Header("Script/Object Dependencies")]
    [SerializeField] private BossBehaviour _bossBehaviour;
    [SerializeField] private PlayerBehaviour _playerBehaviour;
    [SerializeField] private AudioManager _audioManager;

    [Header("Attacks")]
    [SerializeField] private int _maxAttacks; //the maximum amount of attacks the boss can make before entering exhaustion
    [SerializeField] private int _currentAttacks; //this is the variable that keeps track of how many attacks the boss has
    public int attackPoint;
    private List<GameObject> RecentMissiles = new List<GameObject>();
    [SerializeField]
    private GameObject Bird;
    private int birdsNum;

    [Header("Timers")]
    [Range(0f, 10f)]
    [SerializeField] private float _exhaustionTimer;    
    [Range(0f, 2f)]
    [SerializeField] private float _attackAudioTimer;

    [Header("Attack Missiles")]
    [SerializeField] private GameObject _missileObject;
    [SerializeField] private Transform[] _missileSpawnPoint;
    private Vector3 _missileSpawnPos;
    [SerializeField] private GameObject _missileAnimObject;
    public bool isAttacking;
    [SerializeField]private bool hitSignal = false;

    [Header("Scores")]
    [Tooltip("Amount score decreases if player is hit, increases if deflected, and scored if eye attack")]
    [SerializeField] private int _scoreLost;
    [SerializeField] private int _scoreGainedDeflect;
    [SerializeField] private int _scoreGainedAttack;

    public bool spawnedOne = false;
    [SerializeField] private bool decreasedAttack;

    public GameObject Missile;

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
        /*if (_bossBehaviour.currentFrame == 83)
        {
            //DecreaseAttacks();
            _bossBehaviour.ResetTriggers();
        }*/
        int normalizedBossFrame = Mathf.Abs(_bossBehaviour.currentFrame);
        if (_bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack") || _bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("RightAttack"))
        {
            if (normalizedBossFrame == 43 && isAttacking && !spawnedOne)
            {
                Missile = Instantiate(_missileObject, _missileSpawnPoint[attackPoint].position, Quaternion.identity);
                Missile.GetComponent<MissileBehaviour>()._bossBehaviour = _bossBehaviour;
                _missileAnimObject.SetActive(false);
                spawnedOne = true;
                //decreasedAttack = false;
            }
            else if (normalizedBossFrame != 43 && spawnedOne)
            {
                spawnedOne = false;
            }
        }
    }
    #endregion

    #region Attacks

    /// <summary>
    /// Controls the attack phase events
    /// </summary>
    /// <returns></returns>
    public IEnumerator AttackPhase()
    {
        _currentAttacks = _maxAttacks;
        print("Enter AttackPhase");
        yield return new WaitWhile(() => !_bossBehaviour.GetBossState().Equals("ATTACK"));
        while (!hitSignal)//_currentAttacks > 0 WHILE THE BOSS ISNT'T HIT
        {
            yield return new WaitWhile(() => !_bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("Nothing") && !hitSignal || Missile != null);

            if(!hitSignal)
                AttackPlayer();

            yield return new WaitWhile(() => _bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("Nothing") && !hitSignal);
        }
        print("EXIT ATTACK PHASE");
        yield return null;
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
            StartCoroutine(AttackAudio(attackPoint));
            _missileAnimObject.SetActive(true);
            isAttacking = true;
        }
        else
        {
            _bossBehaviour.ResetTriggers();
            _bossBehaviour.animator.SetTrigger("Right");
            StartCoroutine(AttackAudio(attackPoint));
            _missileAnimObject.SetActive(true);
            isAttacking = true;
        }
    }

    IEnumerator AttackAudio(int side)
    {
        yield return new WaitForSeconds(_attackAudioTimer);
        if (side == 0)
        {
            _audioManager.Play("Octo_Atk_L");
            print("attack left");
        }
        else
        {
            _audioManager.Play("Octo_Atk_R");
            print("attack right");
        }
    }
    #endregion

    public IEnumerator Roar()
    {
        _bossBehaviour.animator.SetTrigger("Intro");
        yield return new WaitForSeconds(.3f);
        if(_bossBehaviour._currentHealth > 0)
        {
            GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
            if (gc.birds.Count == 0)
            {
                gc.birds.Add(Instantiate(Bird, new Vector3(-52, 15.5f, 148.5345f), Quaternion.identity).GetComponent<BirdBehaviour>());
                gc.birds.Add(Instantiate(Bird, new Vector3(-83.9f, 16.5f, 87.8672f), Quaternion.identity).GetComponent<BirdBehaviour>());
                for (int i = 1; i < gc.birds.Count; i++)
                {
                    gc.birds[i].bb = gc.birds[i - 1];
                }
                gc.birds[0].bb = gc.birds[gc.birds.Count - 1];

                gc.birds[0].StartCoroutine(gc.birds[0].Attack());
            }
        }
        yield return new WaitWhile(() => !_bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("Nothing"));
        _bossBehaviour.EnterAttack();
    }

    #region Exhaustion
    /// <summary>
    /// Controls the exhaustion phase duration & events
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExhaustionPhase()
    {
        //yield return new WaitForSeconds(_exhaustionTimer);
        yield return new WaitWhile(() => !_bossBehaviour.animator.GetCurrentAnimatorStateInfo(0).IsName("Nothing"));

        if (_bossBehaviour._currentHealth <= _bossBehaviour._maxHealth / 2)
        {
            _bossBehaviour.ResetTriggers();
            _bossBehaviour.StartCoroutine(_bossBehaviour.MoveToAtkPos(false));
        }
        else
        {
            _bossBehaviour.EnterAttack();
        }
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
        _audioManager.Stop("Octo_Atk_L");
        _audioManager.Stop("Octo_Atk_R");
    }
    public void DeactivateHitSignal()
    {
        hitSignal = false;
    }
    #endregion

    #endregion
}
