using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(transform.rotation.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<HyenaBehaviour>(out HyenaBehaviour h))
        {
            transform.parent.transform.Rotate(0, 2, 0);
        }
    }
}
