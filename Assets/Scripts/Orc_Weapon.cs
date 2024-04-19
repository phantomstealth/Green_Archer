using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    //public Orc_Character orc_Character;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player" && orc_Character.animAttack) collision.gameObject.SendMessage("Hit");
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.tag == "Player") collision.gameObject.SendMessage("Hit");
    }


    // Update is called once per frame

}
