using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
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
            rb.velocity = transform.forward * speed;//perpetually move the rocket forward

            Quaternion TargetRotation = Quaternion.LookRotation(PlayerPos.position - transform.position);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, TargetRotation, rotateSpeed));
            //WAY 2
            //transform.position += (PlayerPos.position - transform.position).normalized * speed * Time.deltaTime;
            /*            Vector3 direction = PlayerPos.position - transform.position;
                        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
                        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.time);*/
            //transform.LookAt(PlayerPos);//THIS MAY NEED TO BE CHANGED WITH SOMETHING THAT IS LESS PINPOINT
            yield return new WaitForSeconds(.01f);

        }
    }

    public IEnumerator Launch()
    {
        rb.AddForce(launchSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(2f);
        rb.velocity = Vector3.zero;
        StartCoroutine(SeekPlayer());
    }
}
