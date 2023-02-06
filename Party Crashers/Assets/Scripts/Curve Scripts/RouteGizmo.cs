using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteGizmo : MonoBehaviour
{
    [SerializeField]
    private Transform[] ControlPoints;

    private Vector3 gizmosPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for(float t = 0; t<= 1; t += 0.05f)
        {
            //draws the lines and places them in the gizmo position var
            gizmosPosition = Mathf.Pow(1 - t, 3) * ControlPoints[0].position +
            3 * Mathf.Pow(1 - t, 2) * t * ControlPoints[1].position +
            3 * (1 - t) * Mathf.Pow(t, 2) * ControlPoints[2].position +
            Mathf.Pow(t, 3) * ControlPoints[3].position;

            Gizmos.DrawSphere(gizmosPosition, 0.3f);
        }

        Gizmos.DrawLine(new Vector3(ControlPoints[0].position.x, ControlPoints[0].position.y, ControlPoints[0].position.z), new Vector3(ControlPoints[1].position.x, ControlPoints[1].position.y, ControlPoints[1].position.z));
        Gizmos.DrawLine(new Vector3(ControlPoints[2].position.x, ControlPoints[2].position.y, ControlPoints[2].position.z), new Vector3(ControlPoints[3].position.x, ControlPoints[3].position.y, ControlPoints[3].position.z));

    }
}
