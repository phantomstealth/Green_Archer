using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuCommand : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject formMenu;

    public void BtnFirstStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level_1");
    }
    public void BtnQuit()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void BtnResume()
    {
        Time.timeScale = 1;
        formMenu.SetActive(false);
    }

}
