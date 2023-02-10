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
        if(collision.gameObject.TryGetComponent<HyenaBehaviour>(out HyenaBehaviour h) || collision.gameObject.name.Equals("Cube"))
        {
            print(transform.rotation.eulerAngles);
            transform.Rotate(0, -20, 0);
            print(transform.rotation.eulerAngles);
        }
    }

    IEnumerator Animation()
    {
        transform.Rotate(0, -20, 0);
        yield return null;
    }
}
