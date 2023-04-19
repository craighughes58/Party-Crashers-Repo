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
            FindObjectOfType<AudioManager>().Play("Bonk_" + Random.Range(0, 4).ToString());
            //Debug.Log("Object Shattered");


            if (name.Contains("Fork"))
            {
                Destroy(Instantiate(shatteredFork, transform.position, transform.rotation), 1f);
                Destroy(gameObject);
            }

            if (name.Contains("Cup"))
            {
                Destroy(Instantiate(shatteredCup, transform.position, transform.rotation), 1f);
                Destroy(gameObject);
            }

            if (name.Contains("Cake"))
            {
                Destroy(Instantiate(shatteredCake, transform.position, transform.rotation), 1f);
                Destroy(gameObject);
            }

            if (name.Contains("Plate"))
            {
                Destroy(Instantiate(shatteredPlate, transform.position, transform.rotation), 1f);
                Destroy(gameObject);
            }

            if (name.Contains("MedRock"))
            {
                Destroy(Instantiate(shatteredRockMed, transform.position, transform.rotation), 1f);
                Destroy(gameObject);
            }

            if (name.Contains("SmallRock"))
            {
                Destroy(Instantiate(shatteredRockSmall, transform.position, transform.rotation), 1f);
                Destroy(gameObject);
            }

            if (name.Contains("LargeRock"))
            {
                Destroy(Instantiate(shatteredRockLarge, transform.position, transform.rotation), 1f);
                Destroy(gameObject);
            }
        }
    }
}
