using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Script Dependencies")]
    [SerializeField] private BossBehaviour _bossBehaviour;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Here is where the boss will check if they take damage when they're hit
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //when the boss is hit by a bat, if they're not in attack mode and are activated they will lose a life
        if (collision.gameObject.tag != "Missile" && _bossBehaviour.GetBossState() == "ATTACK")
        {
            _bossBehaviour.LoseHealth();
        }
    }
}
