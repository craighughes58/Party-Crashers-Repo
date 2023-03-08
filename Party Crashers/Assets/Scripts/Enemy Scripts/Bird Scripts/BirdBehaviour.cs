using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{

    //Refrence to the PlayerBehavior script to use the necessary functions from it
    private PlayerBehaviour pb;

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

    private Animator playerAnimator;
    public int currentFrame = 0;
    AnimatorClipInfo[] animationClip;
    int amountTimeLooped=0;
    bool spawnedOne;

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

        StartCoroutine(RandomSound());
        animationClip = playerAnimator.GetCurrentAnimatorClipInfo(0);

    }

    private void FixedUpdate()
    {
        if(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime - amountTimeLooped > 1)
        {
            amountTimeLooped++;
        }
        currentFrame = (int)((playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime - amountTimeLooped) * (animationClip[0].clip.length * 24));
    }
    // Update is called once per frame
    void Update()
    {   
        Rotate();
        if (currentFrame == 123 && !spawnedOne && CheckTutorialStatus())
        {
            CheckProjectile();
        }
        else if (currentFrame == 124)
            spawnedOne = false;
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

            // do sound
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    public void BirdHit()
    {
        gc.LoseEnemy();
        Destroy(gameObject);
        // bird destroyed sound

        Destroy(Instantiate(OutroParticles, transform.position, Quaternion.identity), 10f);
        Destroy(Instantiate(DestroyedBird, transform.position, transform.rotation),5f);
        pb.AddScore(25);
    }

    private void RandomizeColor()
    {
        BodyRenderer.material = Colors[Random.Range(0, Colors.Count)];
        Beak.GetComponent<MeshRenderer>().material = Colors[Random.Range(0, Colors.Count)];
    }

    private IEnumerator RandomSound()
    {
        yield return new WaitForSeconds(2.5f);
        // do sound
        StartCoroutine(RandomSound());
    }

    private void OnDestroy()
    {
        foreach(GameObject g in CurrentProjectiles)
        {
            Destroy(g);
        }
    }

    private bool CheckTutorialStatus()
    {
        if(TutorialWaiter == null)
        {
            return true;
        }
        return false;
    }
}
