using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceBehavior : MonoBehaviour
{
    bool triggered;
    IEnumerator Animation()
    {
        while (transform.parent.rotation.eulerAngles.x != 90)
        {
            transform.parent.Rotate(1, 0, 0);
            print(transform.rotation.eulerAngles.z);
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Hyena") && !triggered)
        {
            triggered = true;
            StartCoroutine("Animation");
        }
    }
}
