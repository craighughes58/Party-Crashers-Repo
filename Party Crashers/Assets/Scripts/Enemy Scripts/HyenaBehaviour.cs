using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyenaBehaviour : MonoBehaviour
{
    [Tooltip("Refrence to the PlayerBehavior script to use the necessary" +
             " functions from it")]
    PlayerBehaviour pb;

    [Tooltip("Refrence to the player's transform")]
    Transform player;

    [Tooltip("Refrence to the enemy's transform")]
    Transform enemy;

    [Tooltip("Speed at which the mutant moves")]
    [SerializeField] float moveSpeed = 1;

    [Tooltip("How much score the player loses when being hit by this enemy")]
    [SerializeField]
    private int scoreDamage;

    Rigidbody rb;

    [Tooltip("Radius at which the enemy can attack")]
    [SerializeField] float AttackRange = 1;

    [SerializeField] float attackTimer;
    [SerializeField] float attackInterval = 2;

    private UnityEngine.AI.NavMeshAgent meshAgent;

    bool gotHit;

    [SerializeField] private int scoreAmount;
    // Start is called before the first frame update
    void Start()
    {

        pb = FindObjectOfType<PlayerBehaviour>();
        rb = GetComponent<Rigidbody>();
        meshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //reference to the player transform
        player = FindObjectOfType<PlayerBehaviour>().transform;

        // Radomize the move speed of the hyenas
        moveSpeed = Random.Range(3, 5);
        meshAgent.speed = moveSpeed;

        enemy = transform;

        attackTimer = attackInterval;

        gotHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotation();

        if (gotHit)
        {
            HitReaction();
        }
    }

    private void Movement()
    {
        // An offset distance to ensure the hyenas don't get right up in the
        // player's face
        Vector3 offset = new Vector3(6, 0, 0);

        // If the hyena is close enough to the player, it gets ready to attack
        if (enemy.position.x == (player.position.x - offset.x))
        {
            AttackWindUp();
        }
        // Otherwise, it keeps moving towards the player
        else
        {
            meshAgent.SetDestination(player.position - offset);
        }
    }

    /// <summary>
    /// Function that ensures that the 
    /// </summary>
    private void Rotation()
    {
        transform.LookAt(player.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    /// <summary>
    /// Function that handles the hyena's attack wind-up phase
    /// </summary>
    private void AttackWindUp()
    {
        // Freeze the hyena in place
        moveSpeed = 0;

        // If there's still time on the attack timer, continue the
        // countdown
        if (attackTimer >= 0)
        {
            attackTimer -= Time.deltaTime;
        }

        // When the interval's up, attack and reset the timer
        else
        {
            attackTimer = attackInterval;
            Attack();
        }

    }

    /// <summary>
    /// Function for the hyena's attacks
    /// </summary>
    private void Attack()
    {
        // Print the proof of it attacking to the console (we'll replace this
        // with the actual attack stuff as we get that developed)
        Debug.Log("Attack!");

        // The player then loses one life
        ScoreLoss();

        // Go back to the movement function to determine whether the hyena can
        // keep attacking, or if it needs to start moving again
        Movement();
    }

    private void ScoreLoss()
    {
        pb.LoseScore(scoreDamage);
        Debug.Log("Life lost!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        gotHit = true;
    }

    /// <summary>
    /// Function for handling the hyena's reaction to getting hit by the 
    /// player
    /// </summary>
    private void HitReaction()
    {
        // Have the hyena break apart and destroy it
    }

}
