using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    //public AudioSource audioGameOver;
    public float restartGameDelay = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //audioGameOver.Play();
        RestartGame(restartGameDelay);
    }
    public void RestartGame(float delay)
    {
        StartCoroutine(RestartGameCoroutine(delay));
    }

    private IEnumerator RestartGameCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Level_1");
    }
}
