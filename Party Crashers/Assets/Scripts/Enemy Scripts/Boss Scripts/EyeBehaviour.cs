using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Script Dependencies")]
    [SerializeField] private BossAttacks _bossAttacks;
    public bool beenHit;
    #endregion

    /*
    /// <summary>
    /// Here is where the boss will check if they take damage when they're hit
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        print(_bossAttacks.BH.GetBossState() + " " + collision.gameObject.name + " " + collision.gameObject.tag);
        //when the boss is hit by a bat, if they're not in attack mode and are activated they will lose a life
        if (_bossAttacks.BH.GetBossState() == "EXHAUSTION" && collision.gameObject.tag.Equals("Bat"))
        {
            print("BOSS HIT");
            _bossAttacks.BH.LoseHealth();
            _bossAttacks.PB.AddScore(_bossAttacks.ScoreGainedAttack);
        }
}*/


    private void OnCollisionStay(Collision collision)
    {
        if (_bossAttacks.BH.GetBossState() == "EXHAUSTION" && collision.gameObject.tag.Equals("Bat") && !beenHit)
        {
            print("BOSS HIT");
            _bossAttacks.BH.LoseHealth();
            _bossAttacks.PB.AddScore(_bossAttacks.ScoreGainedAttack);
            _bossAttacks.RemoveAllMissiles();
            beenHit = true;
        }
        else if(_bossAttacks.BH.GetBossState() == "ATTACK" && collision.gameObject.tag.Equals("Missile"))
        {
            _bossAttacks.RemoveAllMissiles();
            //change state to exhausted
            StartCoroutine(_bossAttacks.BH.MoveToExhPos());
            //stop firing missiles
            _bossAttacks.ActivateHitSignal();
        }
    }
}
