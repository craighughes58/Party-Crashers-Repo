using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkOnShatter : MonoBehaviour
{
    Vector3 breakForce = new Vector3(0, 200, 0);
    
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rbd = GetComponent<Rigidbody>();
        rbd.AddForce(breakForce);
    }
}
