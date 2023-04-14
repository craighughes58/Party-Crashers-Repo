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
                floatForce.y += 10;
                rb.AddForce(floatForce);
            }
            else
            {
                if (name.Contains("baloon")||name.Contains("balloon"))
                {
                    //Balloon popping sound
                    Destroy(gameObject);
                }
                if (name.Contains("tire"))
                {
                    rb.useGravity = true;
                    //rb.mass = .5f;
                }
            }
        }

    }

}