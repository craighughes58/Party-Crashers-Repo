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
    private Vector3 ProjectileOffset;

    [Tooltip("The shattered version that spawns once this enemy dies")]
    [SerializeField]
    private GameObject DestroyedBird;
    //the current projectile
    private GameObject CurrentProjectile;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().transform;
        pb = FindObjectOfType<PlayerBehaviour>();

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
            CurrentProjectile = Instantiate(Projectile, transform.position + ProjectileOffset, transform.rotation);
            CurrentProjectile.GetComponent<BirdProjectileScript>().ConnectToBird(transform);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void BirdHit()
    {
        Destroy(gameObject);
        Destroy(Instantiate(DestroyedBird, transform.position, transform.rotation),5f);
        pb.AddScore(25);

    }

}
