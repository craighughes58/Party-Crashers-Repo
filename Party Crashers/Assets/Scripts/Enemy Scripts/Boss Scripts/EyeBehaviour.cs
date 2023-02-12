using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Script Dependencies")]
    [SerializeField] private BossAttacks _bossAttacks;
    #endregion

    /// <summary>
    /// Here is where the boss will check if they take damage when they're hit
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //when the boss is hit by a bat, if they're not in attack mode and are activated they will lose a life
        if (_bossAttacks.BH.GetBossState() == "EXHAUSTION" && collision.gameObject.tag.Equals("Bat"));
        {
            _bossAttacks.BH.LoseHealth();
            _bossAttacks.PB.AddScore(_bossAttacks.ScoreGainedAttack);
           
        }
    }
}
