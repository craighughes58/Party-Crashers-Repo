using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MissileBehaviour : MonoBehaviour
{
    //reference to the gameobjects rigidbody
    private Rigidbody rb;
    //THE TRANSFORM OF THE TARGET
    private Transform PlayerPos;
    //private CharacterController CharCon;

    [Header("SPEEDS")]
    [Tooltip("how fast the missile can go")]
    [SerializeField]
    private float speed;

    [Tooltip("The initial push that the missile gets before locking onto the player")]
    [SerializeField]
    private Vector3 launchSpeed;

    [Tooltip("How quick the missile can rotate to face its target")]
    [SerializeField]
    private float rotateSpeed;

    [Header("Spawning")]
    private GameObject[] _spawnPoints = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _spawnPoints.Length; i++)
        {
            _spawnPoints[i] = GameObject.Find("Point " + i);
        }

        //EXPLAIN
        rb = GetComponent<Rigidbody>();
        PlayerPos = GameObject.Find("XR Origin").GetComponent<Transform>();
        //CharCon = GameObject.Find("Player").GetComponent<CharacterController>();
        StartCoroutine(Launch());
    }

    /// <summary>
    /// when the missile hits an object, if it's the player they'll lose life. The rocket will always be destroyed from a collision
    /// </summary>
    /// <param name="collision">the object the missile is colliding with</param>
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// while the missile isn't at the player
    /// it will move towards the player and rotate itself to face the player
    /// once it's at the player it will destroy itself (THIS MAY BE A TEMPORARY LINE)
    /// </summary>
    /// <returns>null</returns>
    public IEnumerator SeekPlayer()
    {
        while (Vector3.Distance(PlayerPos.position, transform.position) > .3)
        {
            MoveToPos(PlayerPos.position, 1f);
            //WAY 2
            //transform.position += (PlayerPos.position - transform.position).normalized * speed * Time.deltaTime;
            /*            Vector3 direction = PlayerPos.position - transform.position;
                        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
                        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.time);*/
            //transform.LookAt(PlayerPos);//THIS MAY NEED TO BE CHANGED WITH SOMETHING THAT IS LESS PINPOINT
            yield return new WaitForSeconds(.01f);
        }
    }

    /// <summary>
    /// Launches the missile towards a rotation point. If the point is 1 (middle)
    /// It is launched straight upwards until it hits the point.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Launch()
    {
        int pointNum = UnityEngine.Random.Range(0, 3);
        Vector3 endPos = _spawnPoints[pointNum].transform.position;

        if (pointNum == 1)
        {
            rb.AddForce(launchSpeed, ForceMode.Impulse);
            yield return new WaitWhile(() => transform.position.y <= endPos.y);
        }
        else
        {
            while (Vector3.Distance(endPos, transform.position) > .3)
            {
                MoveToPos(endPos, 1.5f);

                yield return new WaitForSeconds(.01f);
            }
        }
        rb.velocity = Vector3.zero;
        StartCoroutine(SeekPlayer());
    }
    
    /// <summary>
    /// Moves the game obj towards endPos
    /// </summary>
    /// <param name="endPos"> destination of game obj </param>
    private void MoveToPos(Vector3 endPos, float addedSpeed)
    {
        rb.velocity = transform.forward * speed * addedSpeed;//perpetually move the obj forward

        Quaternion TargetRotation = Quaternion.LookRotation(endPos - transform.position);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, TargetRotation, rotateSpeed));
    }
}
