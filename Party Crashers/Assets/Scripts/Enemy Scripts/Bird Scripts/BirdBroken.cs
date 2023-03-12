using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBroken : MonoBehaviour
{
    //public static Material beakMat;
    //public static Material bodyMat;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void setColor(Material beakMat, Material bodyMat)
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = beakMat;

        for (int a = 1; a < 11; a++)
        {
            transform.GetChild(a).GetComponent<MeshRenderer>().materials = new Material[] { bodyMat, bodyMat };
        }

        Destroy(gameObject, 5f);
    }
}
