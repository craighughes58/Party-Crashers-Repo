using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectScript : MonoBehaviour
{
    Vector3 floatForce;
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (collision.gameObject.tag.Equals("Bat"))
        {
            FindObjectOfType<AudioManager>().Play("Bonk_" + Random.Range(0, 4).ToString());
            Debug.Log("Balloon Hit");

            if (name.Contains("group"))
            {
                floatForce.y += 100;
                rb.AddForce(floatForce);
            }
            else
            {
                if (name.Contains("baloon"))
                {
                    //Balloon popping sound
                    Destroy(gameObject);
                }
                if (name.Contains("tire"))
                {
                    rb.mass = .5f;
                }
            }
        }

    }

}