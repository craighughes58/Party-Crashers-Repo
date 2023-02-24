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
    private Transform SpawnBird;

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
        if(!isDeflected)
        {
            transform.LookAt(PlayerLocation);
            transform.position = Vector3.MoveTowards(transform.position, PlayerLocation.position, maxDistance);
        }
        else
        {
            transform.LookAt(SpawnBird);
            transform.position = Vector3.MoveTowards(transform.position, SpawnBird.position, maxDistance);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && !isDeflected)
        {
            //LOSE SCORE
            other.gameObject.GetComponent<PlayerBehaviour>().LoseScore(scoreDamage);
            Destroy(gameObject);
            // player hit by bird
        }
        else if (other.gameObject.tag.Equals("Bird") && isDeflected)
        {
            other.gameObject.GetComponent<BirdBehaviour>().BirdHit();//add score
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag.Equals("Shield"))
        {
            isDeflected = true;
            // deflected bird sfx
        }
    }

    public void ConnectToBird(Transform BirdLocation)
    {
        SpawnBird = BirdLocation;
    }
}
