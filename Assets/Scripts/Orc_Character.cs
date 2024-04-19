using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_Character : MonoBehaviour
{
    Animator anim;
    public int MaxSpeed = 2;
    public int Speed;
    private Transform PlayerTransform;
    public float AttackDistance = 3.5f;
    public float moveDistance = 10f;
    public float CurrentDistanceWithPlayer;
    public bool animAttack = false;
    public float TimeBetweenAttack = 1;
    public float timeAttackCounter;
    public int Health = 3;
    public GameObject Prize;



    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Awake()
    {
        PlayerTransform = FindObjectOfType<Bowman_Character>().transform;
    }

    private void FixedUpdate()
    {
        VerifyDistance();
        Move();
    }

    private void VerifyDistance()
    {
        CurrentDistanceWithPlayer = PlayerTransform.position.x - transform.position.x;
        if (CurrentDistanceWithPlayer < 0 && transform.localScale.x > 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x = theScale.x * -1;
            transform.localScale = theScale;
        }
        else if (CurrentDistanceWithPlayer > 0 && transform.localScale.x < 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x = theScale.x * -1;
            transform.localScale = theScale;
        }
        if (Mathf.Abs(CurrentDistanceWithPlayer) < AttackDistance)
        {
            Attack();
        }
        else if ((Mathf.Abs(CurrentDistanceWithPlayer) < moveDistance))
        {
            Speed = MaxSpeed;
        }
        else
        {
            //Debug.Log("Anim.HIT - " + anim.GetBool("hit"));
            Speed = 0;
        }
    }

    private void Move()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            Speed = 0;
            Debug.Log("HIT!!!");
        }
        if (Speed > 0)
        {
            var p = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, PlayerTransform.position, Speed * Time.deltaTime);
            anim.SetFloat("speed", (transform.position - p).magnitude / Time.deltaTime);
        }

    }


    private void Attack()
    {
        Speed = 0;
        timeAttackCounter = timeAttackCounter - Time.deltaTime;
        if (timeAttackCounter <= 0)
        {
            timeAttackCounter = TimeBetweenAttack;
            if (!animAttack)
            {
                anim.SetTrigger("attack");
            }
        }
    }

    public bool AnimationAttack()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("attack");
    }


    public void StartAttack()
    {
        animAttack = true;
        Debug.Log("StartAttack");
    }
    public void EndAttack()
    {
        animAttack = false;
        Debug.Log("EndAttack");
    }

    public void EndHit()
    {
       Speed = MaxSpeed;
    }
    public void Ok()
    {
        Debug.Log("Ok");
    }

    public void Hit(int damage)
    {
        if (Health > 0) Health = Health - damage; else return;
        if (Health < 1)
        {
            Death();
            return;
        }
        anim.SetTrigger("hit");
        //Debug.Log("Anim Hit is " + anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"));
        Debug.Log("Орку больно HP - "+Health);
        //MonsterSound.Stop();
        SoundHandler.PlaySound(SoundHandler.Орку_больно);
        //MonsterSound.Play();
    }

    void Death()
    {
        Debug.Log("Орк умирает HP - " + Health);
        anim.SetBool("hit", true);
        Speed = 0;
        //MonsterSound.Stop();
        SoundHandler.PlaySound(SoundHandler.Орк_умирает);
        Instantiate(Prize, transform.position + transform.up * 0.37f, transform.rotation);
        Destroy(gameObject);
    }

}
