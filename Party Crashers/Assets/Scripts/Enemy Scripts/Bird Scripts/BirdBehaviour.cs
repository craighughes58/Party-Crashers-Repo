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
    private GameObject CurrentProjectile;

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
    private MeshRenderer BeakRenderer;

    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        //gc.AddEnemy();
        Destroy(Instantiate(IntroParticles,transform.position,Quaternion.identity),10f);
        player = FindObjectOfType<PlayerBehaviour>().transform;
        pb = FindObjectOfType<PlayerBehaviour>();
        RandomizeColor();

    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        CheckProjectile();
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
        if(CurrentProjectile == null)
        {
            CurrentProjectile = Instantiate(Projectile, transform.position + (transform.forward * ProjectileOffset), transform.rotation);
            CurrentProjectile.GetComponent<BirdProjectileScript>().ConnectToBird(transform);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void BirdHit()
    {
        gc.LoseEnemy();
        Destroy(gameObject);
        Destroy(Instantiate(OutroParticles, transform.position, Quaternion.identity), 10f);
        Destroy(Instantiate(DestroyedBird, transform.position, transform.rotation),5f);
        pb.AddScore(25);

    }

    private void RandomizeColor()
    {
        GetComponent<MeshRenderer>().material = Colors[Random.Range(0, Colors.Count)];
        BeakRenderer.material = Colors[Random.Range(0, Colors.Count)];
    }

}
