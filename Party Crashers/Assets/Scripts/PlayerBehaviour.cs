using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private int Score; 


    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// This method is called by enemies when they attack the player 
    /// the player will lose score based on the amount passed through by the enemy scripts
    /// </summary>
    public void LoseScore(int amt)
    {
        if(Score - amt <= 0)
        {
            Score = 0;
        }
        else
        {
            Score -= amt;
        }
        UpdateUI();
    }

    /// <summary>
    /// When an ememy dies, they will call this function to add their score value to the player's
    /// total score
    /// </summary>
    public void AddScore(int amt)
    {
        Score += amt;
        UpdateUI();
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateUI()
    {

    }
}
