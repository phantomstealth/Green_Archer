using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drago : MonoBehaviour
{
    private Vector3 direction;
    bool NotFlip = false;
    public float speed = 2f;
    Animator anim;
    private AudioSource MonsterSound;
    public AudioClip WalkYellowMonster;
    public int countPrize;
    public float RandomFloat;
    public float NeedFlyY;
    public float MaximumRangeFromPlayer = 20f;
    public float RangeFromPlayer;

    public GameObject Prize;
    public GameObject Enemy;
    private BoxCollider2D boxCollider;
    private float NumStepMove;
    public int Health = 5;
    private Transform Player;


    // Use this for initialization
    void Start()
    {
        direction.x = 1;
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        //anim.SetBool("hit", false);
        MonsterSound = GetComponent<AudioSource>();
        NumStepMove = transform.position.x;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        NeedFlyY = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();
        Verify_Range_Out_Player();
        //ToLanding();
    }

    void move()
    {
        Vector3 newDir = new Vector3(0, NeedFlyY-transform.position.y, 0);
        verify_colliders();
        if (Mathf.Abs(NumStepMove - transform.position.x) > 1) ToLanding();
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction+newDir, speed * Time.deltaTime);
    }

    void ToLanding()
    {
        GameObject NewPrize;
        RandomFloat = Random.Range(0f, 1.0f);
        if (countPrize > 0)
        {
            if (RandomFloat > 0.94f)
            {
                NewPrize = Instantiate(Prize, transform.position - transform.up * boxCollider.size.y, transform.rotation);
                NewPrize.GetComponent<box>().NumSurprise = 0;
                countPrize--;
            }
            else if (RandomFloat < 0.05f)
            {
                Instantiate(Enemy, transform.position - transform.up * boxCollider.size.y, transform.rotation);
                countPrize--;
            }
        }
        NumStepMove = transform.position.x;
    }

    void flip()
    {
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        direction.x *= -1;
        countPrize = countPrize + 3;
    }

    void verify_colliders()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * (boxCollider.size.y / 2) + transform.right * (boxCollider.size.x / 2) * direction.x, 0.1f);
        if (colliders.Length > 1)
        {
            for (int i = 0; i <= colliders.Length - 1; i++)
            {
                verify_collision_player(colliders[i].gameObject);
            }
            if (!NotFlip)
                flip();
            else NotFlip = false;
        }
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
        if (collObject.tag == "money")
            NotFlip = true;
    }


    void Verify_Range_Out_Player()
    {
        RangeFromPlayer = transform.position.x - Player.position.x;
        if (Mathf.Abs(RangeFromPlayer) > MaximumRangeFromPlayer)
        {
            if ((RangeFromPlayer>0)&(transform.localScale.x>0))
                flip();
            else if ((RangeFromPlayer<0)&(transform.localScale.x<0))
                flip();
        }
    }

    public void Hit(int damage)
    {
        Health=Health-damage;
        Instantiate(Prize, transform.position - transform.up * 0.37f, transform.rotation);
        if (Health < 1)
        {
            Death();
            return;
        }
        anim.SetBool("hit", true);
        flip();
        //MonsterSound.Stop();
        SoundHandler.PlaySound(SoundHandler.DeathYellowMonster);
        MonsterSound.Play();
    }


    void Death()
    {
        anim.SetBool("hit", true);
        speed = 0;
        MonsterSound.Stop();
        SoundHandler.PlaySound(SoundHandler.DeathYellowMonster);
    }

    void Destroy_me()
    {
        if (Health < 1)
        {
            Destroy(gameObject);
        }
        else
            anim.SetBool("hit", false);
    }
}
