using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{

    //Refrence to the PlayerBehavior script to use the necessary functions from it
    private PlayerBehaviour pb;

    public BirdBehaviour bb;
    //Refrence to the player's transform
    private Transform player;

    [Tooltip("Reference to the projectile that the bird shoots")]
    [SerializeField]
    private GameObject Projectile;

    [Tooltip("The difference between the bird origin and the projectile spawn point ")]
    [SerializeField]
    private float ProjectileOffset;

    [Tooltip("The shattered version that spawns once this enemy dies")]
    [SerializeField]
    private GameObject DestroyedBird;
    //the current projectile
    public List<GameObject> CurrentProjectiles = new List<GameObject>();

    [Header("PARTICLES")]
    [Tooltip("the particles that spawn when the bird script starts")]
    [SerializeField]
    private GameObject IntroParticles;
    [Tooltip("the particles that spawn when the bird script is destroyed")]
    [SerializeField]
    private GameObject OutroParticles;

    private GameController gc;

    [Tooltip("the different colors the pelican can be")]
    [SerializeField]
    private List<Material> Colors;

    [Tooltip("Reference to the beak")]
    [SerializeField]
    private GameObject Beak;

    [SerializeField]
    private SkinnedMeshRenderer BodyRenderer;

    [Tooltip("This will shut off some functions of the full bird")]
    [SerializeField]
    private bool isTutorial;

    [Tooltip("represents what the tutorial bird is trying to do ")]
    [SerializeField]
    private GameObject TutorialWaiter;

    //[Header("Sound")]
    //[Range(0f, 1f)]
    //[SerializeField] private float cawDelay;

    private Animator playerAnimator;
    public int currentFrame = 0;
    AnimatorClipInfo[] animationClip;
    int amountTimeLooped=0;
    [SerializeField] private bool spawnedOne;

    bool previousAttacked;
    public bool isBoss = false;

    private bool startingAttack;

    public AudioManager managerRef;
    public int attackNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        // spawn sound

        gc = GameObject.Find("GameController").GetComponent<GameController>();
        //gc.AddEnemy();
        if(!isTutorial)
        {
            Destroy(Instantiate(IntroParticles, transform.position, Quaternion.identity), 10f);
        }
        player = FindObjectOfType<PlayerBehaviour>().transform;
        pb = FindObjectOfType<PlayerBehaviour>();
        RandomizeColor();

        managerRef = FindObjectOfType<AudioManager>();
        managerRef.AddSound("Bird_Fire", gameObject);

        if (gameObject.name != "Tutorial_Bird")
        {
            for (int i = 0; i < 3; i++)
            {
                managerRef.AddSound("Bird_Caw_" + i.ToString(), gameObject);
            }
            StartCoroutine(RandomSound());
        }
        //animationClip = playerAnimator.GetCurrentAnimatorClipInfo(0);
        playerAnimator.logWarnings = false;
    }

    private void FixedUpdate()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animationClip = playerAnimator.GetCurrentAnimatorClipInfo(0);

            if (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime - amountTimeLooped > 1)
            {
                amountTimeLooped++;
            }
            currentFrame = (int)((playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime - amountTimeLooped) * (animationClip[0].clip.length * 24));
            //print(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        else if(amountTimeLooped != 0)
        {
            amountTimeLooped = 0;
        }

        
    }
    // Update is called once per frame
    void Update()
    {

        Rotate();
        if (bb != null && bb.playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            previousAttacked = true;
        }
        else if (bb != null && !bb.playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && previousAttacked && !startingAttack)
        {
            startingAttack = true;
            previousAttacked = false;
            StartCoroutine(Attack());
        }


        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && TutorialWaiter == null && isTutorial && !startingAttack)
        {
            startingAttack = true;
            StartCoroutine(Attack());
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && currentFrame == 123 && !spawnedOne)
        {
            CheckProjectile();
        }
        else if (currentFrame >= 124)
        {
            spawnedOne = false;
            startingAttack = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Rotate()
    {
        transform.LookAt(player.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckProjectile()
    {
        //if(CurrentProjectiles == null)
        //{
        if (!spawnedOne)
        {
            CurrentProjectiles.Add(Instantiate(Projectile, Beak.transform.position + (transform.forward * ProjectileOffset), transform.rotation));
            CurrentProjectiles[CurrentProjectiles.Count - 1].GetComponent<BirdProjectileScript>().ConnectToBird(this);
            spawnedOne = !spawnedOne;
        }
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    public void BirdHit()
    {
        BirdBehaviour currentBehavior = GetComponent<BirdBehaviour>();
        gc.LoseEnemy();
        gc.LoseBird(currentBehavior);
        Destroy(gameObject);

        FindObjectOfType<AudioManager>().Play("Enemy_Death" + Random.Range(0, 4).ToString());

        Destroy(Instantiate(OutroParticles, transform.position, Quaternion.identity), 10f);
        Instantiate(DestroyedBird, transform.position, transform.rotation).gameObject.GetComponent<BirdBroken>().setColor(Beak.GetComponent<MeshRenderer>().material, BodyRenderer.material);
        pb.AddScore(25);
    }

    private void RandomizeColor()
    {
        BodyRenderer.material = Colors[Random.Range(0, Colors.Count)];
        Beak.GetComponent<MeshRenderer>().material = Colors[Random.Range(0, Colors.Count)];
    }

    private IEnumerator RandomSound()
    {
        yield return new WaitForSeconds(Random.Range(3.5f, 6f));
        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            print("bird noizzz");
            FindObjectOfType<AudioManager>().Play("Bird_Caw_" + Random.Range(0, 3).ToString());
        }
        StartCoroutine(RandomSound());
    }

    //public void Attack()
    //{
        //StartCoroutine(attack());
    //}

    public IEnumerator Attack()
    {
        attackNum++;
        StartCoroutine(AttackSound(attackNum));
        yield return new WaitForSeconds(.4f);
        playerAnimator.SetTrigger("AttackTrigger");
    }

    /// <summary>
    /// Delays the bird firing audio to sync better
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackSound(int attackCount)
    {
        if (attackCount != 1)
        {
            yield return new WaitForSeconds(0.5f);
        }
        //managerRef.PlayAddedSound("Bird_Fire", gameObject);
    }

    private void OnDestroy()
    {
        foreach(GameObject g in CurrentProjectiles)
        {
            Destroy(g);
        }
    }
}
