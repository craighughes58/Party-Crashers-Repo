using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAtPoints : MonoBehaviour
{
    private bool isStopped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stop"))
        {
            StartCoroutine(PauseMoving());
        }
    }

   

    IEnumerator PauseMoving()//THIS IS TEMPORARY
    {
        isStopped = true;
        yield return new WaitForSeconds(3f);
        isStopped = false;
    }

    public bool GetStopped()
    {
        return isStopped;
    }
}
