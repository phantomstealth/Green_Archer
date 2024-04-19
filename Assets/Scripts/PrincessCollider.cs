using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PrincessCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D collider)
    {
        verify_collision_player(collider.gameObject);
    }

    void verify_collision_player(GameObject collObject)
    {
        if (collObject.tag == "Player")
        {
            //collObject.SendMessage("ColliderPrincess");
            //SoundHandler.PlaySound(SoundHandler.Принцесса_Неубит_Дракон);
            SceneManager.LoadScene("Win");
        }
    }



}
