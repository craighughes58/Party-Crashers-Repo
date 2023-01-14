using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyenaBehaviour : MonoBehaviour
{

    [Tooltip("Refrence to the player's transform")]
    Transform player;

    [Tooltip("Speed at which the mutant moves")]
    [SerializeField] float moveSpeed = 1;

    Rigidbody rb;

    [Tooltip("Radius at which the enemy can attack")]
    [SerializeField] float AttackRange = 1;

    private UnityEngine.AI.NavMeshAgent meshAgent;

    [SerializeField]
    private int scoreAmount;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        meshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //reference to the player transform
        player = FindObjectOfType<PlayerBehaviour>().transform;
        meshAgent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotation();
    }

    private void Movement()
    {
        meshAgent.SetDestination(player.position);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Rotation()
    {
        transform.LookAt(player.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
