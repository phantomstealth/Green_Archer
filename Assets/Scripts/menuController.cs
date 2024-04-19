using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuController : MonoBehaviour
{
    public GameObject frmMenu;
    // Update is called once per frame
    private void Update()
    {
        if ((Input.backButtonLeavesApp) || (Input.GetKeyDown(KeyCode.Escape)))
        {
            frmMenu.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("back");
        }

    }
}
