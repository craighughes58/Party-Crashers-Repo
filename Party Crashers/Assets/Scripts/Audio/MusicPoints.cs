using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPoints : MonoBehaviour
{
    public int songNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MusicPoint"))
        {
            songNumber++;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
