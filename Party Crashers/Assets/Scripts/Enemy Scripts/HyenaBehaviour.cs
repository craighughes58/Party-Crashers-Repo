using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyenaBehaviour : MonoBehaviour
{
    [Tooltip("Refrence to the PlayerBehavior script to use the necessary" +
             " functions from it")]
    PlayerBehaviour pb;

    [Tooltip("Refrence to the player's transform")]
    Transform player;

    [Tooltip("Refrence to the enemy's transform")]
    Transform enemy;

    [Tooltip("Speed at which the mutant moves")]
    [SerializeField] float moveSpeed = 1;

    [Tooltip("How much score the player loses when being hit by this enemy")]
    [SerializeField]
    private int scoreDamage;

    [Tooltip("How many hits the hyena can take before being destroyed")]
    [SerializeField] int enemyLives;

    [Tooltip("The player's score")]
    [SerializeField] int victoryScore;

    [SerializeField] bool isTutorial;

    [SerializeField] Material flash;

    Rigidbody rb;

    [SerializeField]
    GameObject entranceParticles;

    [Tooltip("The candy that appears after the hyena dies")]
    [SerializeField]
    private GameObject deathParticle1;
    /* [SerializeField] GameObject deathParticle2;
       [SerializeField] GameObject deathParticle3;
       [SerializeField] GameObject deathParticle4;
       [SerializeField] GameObject deathParticle5;
       [SerializeField] GameObject deathParticle6;
       [SerializeField] GameObject deathParticle7; */

    [SerializeField]
    GameObject shatteredHyena1;

    [SerializeField]
    GameObject shatteredHyena2;

    [Tooltip("Radius at which the enemy can attack")]
    [SerializeField] float AttackRange = 1;

    [SerializeField] float attackTimer;
    [SerializeField] float attackInterval = 2;

    [SerializeField] float flashTimer;
    [SerializeField] float flashInterval = 1;

    private UnityEngine.AI.NavMeshAgent meshAgent;

    bool gotHit;

    private bool attacking = false;

    private Animator anim;

    private GameController gc;

    [SerializeField] private int scoreAmount;

    [Tooltip("how far away the enemy will stop from the player")]
    [SerializeField]
    private float offset;

    public int currentFrame = 0;
    AnimatorClipInfo[] animationClip;
    int amountTimeLooped = 0;
    bool hitPlayer = false;

    [SerializeField]
    private SkinnedMeshRenderer hyenaRenderer1;
    [SerializeField]
    private SkinnedMeshRenderer hyenaRenderer2;
    [SerializeField]
    private SkinnedMeshRenderer hyenaRenderer3;
    [SerializeField]
    private SkinnedMeshRenderer hyenaRenderer4;
    [SerializeField]
    private SkinnedMeshRenderer hyenaRenderer5;
    [SerializeField]
    private SkinnedMeshRenderer hyenaRenderer6;

    [SerializeField]
    private Material originalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        // spawn sound 

        Destroy(Instantiate(entranceParticles, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), Quaternion.identity), 10f);
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        //gc.AddEnemy();
        pb = FindObjectOfType<PlayerBehaviour>();
        rb = GetComponent<Rigidbody>();
        meshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //reference to the player transform
        player = FindObjectOfType<PlayerBehaviour>().transform;
        // Radomize the move speed of the hyenas
        moveSpeed = Random.Range(7, 11);
        meshAgent.speed = moveSpeed;

        enemy = transform;

        attackTimer = attackInterval;

        FindObjectOfType<AudioManager>().Play("Hyena_Alert_" + Random.Range(0, 2).ToString());
        // add necessary audio to each hyena prefab
        FindObjectOfType<AudioManager>().AddSound("Hyena_Attack", gameObject);

        gotHit = false;
        anim = GetComponent<Animator>();

        hyenaRenderer1 = transform.GetChild(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        hyenaRenderer2 = transform.GetChild(0).transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        hyenaRenderer3 = transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>();
        hyenaRenderer4 = transform.GetChild(0).transform.GetChild(3).GetComponent<SkinnedMeshRenderer>();
        hyenaRenderer5 = transform.GetChild(0).transform.GetChild(4).GetComponent<SkinnedMeshRenderer>();
        hyenaRenderer6 = transform.GetChild(0).transform.GetChild(5).GetComponent<SkinnedMeshRenderer>();

        originalMaterial = hyenaRenderer1.material;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotation();
    }

    private void FixedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animationClip = anim.GetCurrentAnimatorClipInfo(0);
            print(animationClip);

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime - amountTimeLooped > 1)
            {
                amountTimeLooped++;
            }
            currentFrame = (int)((anim.GetCurrentAnimatorStateInfo(0).normalizedTime - amountTimeLooped) * (animationClip[0].clip.length * 24));
            print(anim.GetCurrentAnimatorStateInfo(0).normalizedTime + " " + animationClip[0].clip.length);
        }
        else
        {
            amountTimeLooped = 0;
        }
        
    }

    private void Movement()
    {
        // An offset distance to ensure the hyenas don't get right up in the
        // player's face
        //Vector3 offset = new Vector3(6, 0, 6);
        //enemy.position.x >= (player.position.x - offset.x) && (enemy.position.z >= (player.position.z - offset.z))
        // If the hyena is close enough to the player, it gets ready to attack
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Flinch"))
        {
            if (Vector3.Distance(player.position, transform.position) <= offset && !isTutorial)
            {
                if (!attacking)
                {
                    attacking = true;
                    anim.SetBool("attacking", attacking);
                    FindObjectOfType<AudioManager>().PlayAddedSound("Hyena_Attack", gameObject);
                }

                AttackWindUp();
                meshAgent.SetDestination(transform.position);
            }

            else
            {
                //StartCoroutine(RandomSound());
                if (attacking)
                {
                    attacking = false;
                    anim.SetBool("attacking", attacking);
                    //FindObjectOfType<AudioManager>().Stop("Hyena_Attack");
                }
                //ONLY MOVES WHEN WALKING?
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    meshAgent.isStopped = false;
                    meshAgent.SetDestination(player.position);
                }
            }
        }
        //Stops momentarily when hit
        else
        {
            attacking = false;
            anim.SetBool("attacking", attacking);
            meshAgent.isStopped = true;
            meshAgent.SetDestination(transform.position);
        }

    }

    /// <summary>
    /// Function that ensures that the 
    /// </summary>
    private void Rotation()
    {
        transform.LookAt(player.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    /// <summary>
    /// Function that handles the hyena's attack wind-up phase
    /// </summary>
    private void AttackWindUp()
    {
        // Freeze the hyena in place
        //meshAgent.isStopped = true;
        meshAgent.isStopped = true;
        //print("prepping attack");

        if (currentFrame == 40 && !hitPlayer)
        {
            Attack();
            hitPlayer = true;
        }
        else if (currentFrame == 41 && hitPlayer)
        {
            hitPlayer = false;
        }

        // If there's still time on the attack timer, continue the
        // countdown
        /*if (attackTimer >= 0)
        {
            attackTimer -= Time.deltaTime;
        }

        // When the interval's up, attack and reset the timer
        else
        {
            attackTimer = attackInterval;
            Attack();
        }*/


    }

    /// <summary>
    /// Function for the hyena's attacks
    /// </summary>
    private void Attack()
    {
        // Print the proof of it attacking to the console (we'll replace this
        // with the actual attack stuff as we get that developed)
        Debug.Log("Attack!");
        // The player then loses one life
        ScoreLoss();

        // Go back to the movement function to determine whether the hyena can
        // keep attacking, or if it needs to start moving again
        //Movement();
    }

    private void ScoreLoss()
    {
        pb.LoseScore(scoreDamage);
        Debug.Log("Score lost!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bat"))
        {
            FindObjectOfType<AudioManager>().Play("Hit_Enemy");
            Debug.Log("Life lost!");
            enemyLives--;
            anim.SetTrigger("Hit");
            FindObjectOfType<AudioManager>().Play("Hyena_Flinch_" + Random.Range(0, 4).ToString());
            FindObjectOfType<AudioManager>().StopAddedSound("Hyena_Attack", gameObject);
            HitReaction();
        }
    }

    /// <summary>
    /// Function for handling the hyena's reaction to getting hit by the 
    /// player
    /// </summary>
    private void HitReaction()
    {
        Flash();

        if (enemyLives <= 0)
        {
            FindObjectOfType<AudioManager>().Play("Enemy_Death" + Random.Range(0, 4).ToString());

            Destroy(Instantiate(deathParticle1, transform.position, Quaternion.identity), 15f);

            gc.LoseEnemy();

            if (name.Contains("Mat2"))
            {
                Destroy(Instantiate(shatteredHyena2, transform.position, transform.rotation), 5f);
                pb.AddScore(victoryScore);
                Destroy(gameObject);
            }

            if (name.Contains("Mat1"))
            {
                Destroy(Instantiate(shatteredHyena1, transform.position, transform.rotation), 5f);
                pb.AddScore(victoryScore);
                Destroy(gameObject);
            }

            currentFrame = 0;
        }
    }

    private IEnumerator RandomSound()
    {
        yield return new WaitForSeconds(5f);
        FindObjectOfType<AudioManager>().Play("Hyena_Alert_" + Random.Range(0, 2).ToString());
        StartCoroutine(RandomSound());
    }

    private void Flash()
    {
        hyenaRenderer1.material = flash;
        hyenaRenderer2.material = flash;
        hyenaRenderer3.material = flash;
        hyenaRenderer4.material = flash;
        hyenaRenderer5.material = flash;
        hyenaRenderer6.material = flash;

        StartCoroutine(EndFlash());
    }

    private IEnumerator EndFlash()
    {
        yield return new WaitForSeconds(.25f);

        hyenaRenderer1.material = originalMaterial;
        hyenaRenderer2.material = originalMaterial;
        hyenaRenderer3.material = originalMaterial;
        hyenaRenderer4.material = originalMaterial;
        hyenaRenderer5.material = originalMaterial;
        hyenaRenderer6.material = originalMaterial;

        yield return null;
    }
}
