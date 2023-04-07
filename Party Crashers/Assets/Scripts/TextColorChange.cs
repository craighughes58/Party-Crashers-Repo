using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextColorChange : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro scoreText;
    bool hitGreen = false;
    Color aimColor;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = gameObject.transform.GetChild(0).GetComponent<TextMeshPro>();
        aimColor = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {
        if(scoreText.color.r < .1f && scoreText.color.b < .1f && !hitGreen)
        {
            hitGreen = true;
            aimColor = Color.white;
            scoreText.color = Color.magenta;
        }
        else if(scoreText.color.r > .9f && scoreText.color.b > .9f && hitGreen)
        {
            hitGreen = false;
            aimColor = Color.magenta;
            scoreText.color = Color.white;
        }
        scoreText.color = Color.LerpUnclamped(scoreText.color, aimColor, Time.deltaTime*4);
        transform.position = new Vector3(transform.position.x, transform.position.y + .01f, transform.position.z);
    }
}
