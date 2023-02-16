using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceBehavior : MonoBehaviour
{
    IEnumerator Animation()
    {
        while (transform.rotation.eulerAngles.z != 270)
        {
            transform.Rotate(0, 0, - 1);
            print(transform.rotation.eulerAngles.z);
            yield return null;
        }
    }
}
