
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Script Dependencies")]
    [SerializeField] private BossAttacks _bossAttacks;
    public bool beenHit;
    [SerializeField] private Material flash;
    Material startMat;
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
        //print("Stay");
/*        if (_bossAttacks.BH.GetBossState() == "EXHAUSTION" && collision.gameObject.tag.Equals("Bat") && !beenHit)
        {
            FindObjectOfType<AudioManager>().Play("Hit_Enemy");
            //print("BOSS HIT");
            _bossAttacks.BH.LoseHealth();
            _bossAttacks.PB.AddScore(_bossAttacks.ScoreGainedAttack);
            _bossAttacks.RemoveAllMissiles();

            if (!beenHit)
            {
                HitReaction();
            }
            beenHit = true;
        }*/
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_bossAttacks.BH.GetBossState() == "ATTACK" && collision.gameObject.tag.Equals("Missile"))
        {
            FindObjectOfType<AudioManager>().Play("Hit_By_Projectile");
            _bossAttacks.RemoveAllMissiles();
            //change state to exhausted
            StartCoroutine(_bossAttacks.BH.MoveToExhPos());
            //stop firing missiles
            _bossAttacks.ActivateHitSignal();
        }
        else if (_bossAttacks.BH.GetBossState() == "EXHAUSTION" && collision.gameObject.tag.Equals("Bat") && !beenHit)
        {
            FindObjectOfType<AudioManager>().Play("Hit_Enemy");
            //print("BOSS HIT");
            _bossAttacks.BH.LoseHealth();
            _bossAttacks.PB.AddScore(_bossAttacks.ScoreGainedAttack);
            _bossAttacks.RemoveAllMissiles();

            if (!beenHit)
            {
                HitReaction();
            }
            beenHit = true;
        }
    }

    private void HitReaction()
    {
        startMat = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        GetComponent<BossBehaviour>().animator.SetTrigger("Flinch");
        Flash();
    }

    private void Flash()
    {
        gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = flash;
        gameObject.transform.GetChild(0).transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = flash;

        StartCoroutine(EndFlash());
    }

    private IEnumerator EndFlash()
    {
        yield return new WaitForSeconds(.1f);
        gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = startMat;
        gameObject.transform.GetChild(0).transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = startMat;
        beenHit = false;
        yield return null;
    }
}