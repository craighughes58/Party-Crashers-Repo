using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHyena : MonoBehaviour
{
    [Tooltip("How many hits the enemy can take before dying")]
    [SerializeField]
    private int lives;

    [Tooltip("How many points the player gets for beating this enemy")]
    [SerializeField]
    private int points;

    private GameController gc;
    private PlayerBehaviour pb;

    [Tooltip("The particle appears after the hyena is destroyed")]
    [SerializeField]
    private GameObject deathParticle1;
    [Tooltip("The object that appears after the hyena is destroyed")]
    [SerializeField]
    private GameObject shatteredHyena1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bat"))
        {
            FindObjectOfType<AudioManager>().Play("Hit_Enemy");
            lives--;
            HitReaction();
        }
    }
    private void HitReaction()
    {
        if (lives <= 0)
        {
            FindObjectOfType<AudioManager>().Play("Hit_Enemy");
            Destroy(Instantiate(deathParticle1, transform.position, Quaternion.identity), 15f);
            gc.LoseEnemy();
            Destroy(Instantiate(shatteredHyena1, transform.position, transform.rotation), 5f);
            pb.AddScore(points);
            Destroy(gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        gc.AddEnemy();
        pb = FindObjectOfType<PlayerBehaviour>();
    }
}
