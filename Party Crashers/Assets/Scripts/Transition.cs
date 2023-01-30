using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private GameObject _transitionCanvas;
    private float _transitionDelayTime = 1.0f;

    /// <summary>
    /// Begins the delayed load coroutine
    /// </summary>
    public void LoadLevel()
    {
        StartCoroutine(DelayLoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    /// <summary>
    /// Begins the scene transition animation then loads the new scene upon
    /// completion
    /// </summary>
    /// <param name="index"> scene after the current scene </param>
    private IEnumerator DelayLoadLevel(int index)
    {
        _transitionCanvas.SetActive(true);
        animator.SetTrigger("TriggerTransition");
        yield return new WaitForSeconds(_transitionDelayTime);
        _transitionCanvas.SetActive(false);
        SceneManager.LoadScene(index);
    }
}