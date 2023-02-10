using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAtPoints : MonoBehaviour
{
    private bool isStopped = false;
    private GameController GC;

    private void Start()
    {
        GC = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stop"))
        {
            GC.MoveToNextPoint();
            isStopped = true;
            //StartCoroutine(PauseMoving());//THIS IS TEMPORARY
        }
    }

    IEnumerator PauseMoving()//THIS IS TEMPORARY
    {
        print("WAITING");
        yield return new WaitForSeconds(13f);
        isStopped = false;
    }

    public bool GetStopped()
    {
        return isStopped;
    }

    public void StopPlayer()
    {
        isStopped = true;
    }
    public void StartPlayer()
    {
        isStopped = false;
    }
}
