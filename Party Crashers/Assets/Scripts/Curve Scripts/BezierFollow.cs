using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    [SerializeField]
    private Transform[] Routes;

    private int routeToGo;

    private float tParam;

    //THIS IS THE PLAYER POSITION
    private Vector3 catPosition;

    private float speedModifier;

    private bool coroutineAllowed;


    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.15f;
        coroutineAllowed = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        coroutineAllowed = false;

        Vector3 p0 = Routes[routeNumber].GetChild(0).position;
        Vector3 p1 = Routes[routeNumber].GetChild(1).position;
        Vector3 p2 = Routes[routeNumber].GetChild(2).position;
        Vector3 p3 = Routes[routeNumber].GetChild(3).position;

        

        while(tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            catPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.LookAt(catPosition);
            yield return new WaitForEndOfFrame();

            transform.position = catPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        routeToGo += 1;

        if(routeToGo > Routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineAllowed = true;
    }
}
