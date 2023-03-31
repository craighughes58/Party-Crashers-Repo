using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdProjectileScript : MonoBehaviour
{
    [Tooltip("This is how fast the party hat can move towards the player")]
    [SerializeField]
    private float maxDistance;
    private Transform PlayerLocation;
    private bool isDeflected = false;

    [Tooltip("How much score loss the player takes from the hit")]
    [SerializeField]
    private int scoreDamage;

    //HOW DO I FIND THE BIRD THAT SHOT THIS 
    private BirdBehaviour SpawnBird;

    // Start is called before the first frame update
    void Start()
    {
        PlayerLocation = GameObject.Find("Missile Aim Position").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate()
    {
        if (!isDeflected)
        {
            transform.LookAt(PlayerLocation);
            transform.position = Vector3.MoveTowards(transform.position, PlayerLocation.position, maxDistance);
        }
        else
        {
            transform.LookAt(SpawnBird.gameObject.transform);
            transform.position = Vector3.MoveTowards(transform.position, SpawnBird.gameObject.transform.position, maxDistance);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && !isDeflected)
        {
            //LOSE SCORE
            other.gameObject.GetComponent<PlayerBehaviour>().LoseScore(scoreDamage);
            SpawnBird.CurrentProjectiles.Remove(gameObject);
            Destroy(gameObject);
            FindObjectOfType<AudioManager>().Play("Hit_By_Projectile");
        }
        else if (other.gameObject.tag.Equals("Bird") && isDeflected)
        {
            other.gameObject.GetComponent<BirdBehaviour>().BirdHit();//add score
            other.gameObject.GetComponent<BirdBehaviour>().CurrentProjectiles.Remove(this.gameObject);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag.Equals("Shield"))
        {

            isDeflected = true;
            FindObjectOfType<AudioManager>().Play("Projectile_Deflect");
        }
    }

    public void ConnectToBird(BirdBehaviour BirdScript)
    {
        SpawnBird = BirdScript;
    }
}
