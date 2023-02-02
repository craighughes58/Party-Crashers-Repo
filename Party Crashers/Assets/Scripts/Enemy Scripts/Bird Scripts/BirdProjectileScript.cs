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
        PlayerLocation = GameObject.Find("XR Origin").GetComponent<Transform>();
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Player") && !isDeflected)
        {
            //LOSE SCORE
            collision.gameObject.GetComponent<PlayerBehaviour>().LoseScore(scoreDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag.Equals("Bird") && isDeflected)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag.Equals("Shield"))
        {
            isDeflected = true;
        }

    }

    public void ConnectToBird(Transform BirdLocation)
    {
        SpawnBird = BirdLocation;
    }
}
