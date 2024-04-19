using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesBar : MonoBehaviour
{
    private Transform[] hearths = new Transform[5];
    private Bowman_Character bowman_character;

    private void Awake()
    {
        bowman_character = FindObjectOfType<Bowman_Character>();
        for (int i = 0; i < hearths.Length; i++)
        {
            hearths[i] = transform.GetChild(i);
        }
        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < hearths.Length; i++)
        {
            if (i < bowman_character.Health)
            {
                hearths[i].gameObject.SetActive(true);
            }
            else
                hearths[i].gameObject.SetActive(false);
        }
    }
}
