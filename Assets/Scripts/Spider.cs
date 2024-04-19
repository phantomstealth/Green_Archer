using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 direction;
    public float speed = 1f;
    public float Максимальная_высота = 0f;
    public float Минимальная_высота = 0f;
    public int Health = 3;

    
    public void Hit(int damage)
    {
        Health = Health - damage;
        if (Health < 1)
        {
            Death();
            return;
        }
        SoundHandler.PlaySound(SoundHandler.DeathYellowMonster);
    }

    void Death()
    {
        speed = 0;
        SoundHandler.PlaySound(SoundHandler.DeathYellowMonster);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        verify_collision_player(collider.gameObject);
    }

    void verify_collision_player(GameObject collObject)
    {
        if (collObject.tag == "Player")
        {
            collObject.SendMessage("Hit");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        direction.y = -1;
        Максимальная_высота = transform.position.y;
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        if (transform.position.y > Максимальная_высота) direction = direction * -1;
        if (transform.position.y < Минимальная_высота) direction = direction * -1;
    }
}
