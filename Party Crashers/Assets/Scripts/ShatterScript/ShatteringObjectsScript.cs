using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteringObjectsScript : MonoBehaviour
{

    [SerializeField]
    GameObject shatteredCup;

    [SerializeField]
    GameObject shatteredPlate;

    [SerializeField]
    GameObject shatteredCake;

    [SerializeField]
    GameObject shatteredFork;

    [SerializeField]
    GameObject shatteredRockSmall;
    [SerializeField]
    GameObject shatteredRockMed;
    [SerializeField]
    GameObject shatteredRockLarge;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bat"))
        {
            Debug.Log("Object Shattered");


            if (name.Contains("Fork"))
            {
                Destroy(Instantiate(shatteredFork, transform.position, transform.rotation), 5f);
                Destroy(gameObject);
            }

            if (name.Contains("Cup"))
            {
                Destroy(Instantiate(shatteredCup, transform.position, transform.rotation), 5f);
                Destroy(gameObject);
            }

            if (name.Contains("Cake"))
            {
                Destroy(Instantiate(shatteredCake, transform.position, transform.rotation), 5f);
                Destroy(gameObject);
            }

            if (name.Contains("Plate"))
            {
                Destroy(Instantiate(shatteredPlate, transform.position, transform.rotation), 5f);
                Destroy(gameObject);
            }
        }
    }
}
