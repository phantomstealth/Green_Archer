using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveMonsters : MonoBehaviour {

	private Vector3 direction;
	bool NotFlip=false;
	float speed=1f;
	Animator anim;
    private AudioSource MonsterSound;
    public AudioClip WalkYellowMonster;
    public int Health=3;
    public GameObject Prize;

	// Use this for initialization
	void Start () {
		direction.x = -1;
		anim=GetComponent<Animator>();
		anim.SetBool ("hit", false);
        MonsterSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Move ();	
	}

	void Move(){
		verify_colliders ();
		verify_fall();
		transform.position = Vector3.MoveTowards (transform.position, transform.position + direction, speed * Time.deltaTime);
	}

	void flip(){
		Vector2 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		direction.x *= -1;
	}

	void verify_fall()
	{
		bool bl_platform = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.right * 0.5f * direction.x, 0.1f);
		if (colliders.Length > 1)
		{
			for (int i = 0; i <= colliders.Length - 1; i++)
			{
				if (colliders[i].gameObject.name.Contains("platform"))
					bl_platform=true;
			}
		}
		if (!bl_platform) flip();

	}

	void verify_colliders(){
		Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position + transform.up * 0.37f + transform.right * 0.5f * direction.x, 0.1f);
		if (colliders.Length > 1) {
			for (int i = 0; i <= colliders.Length-1; i++) {
				verify_collision_player (colliders [i].gameObject);
			}
			if (!NotFlip)
				flip ();
			else NotFlip=false;
		}
	}

	void OnCollisionEnter2D(Collision2D collider){
		verify_collision_player (collider.gameObject);
	}

	void verify_collision_player(GameObject collObject){
		if (collObject.tag == "Player") {
			collObject.SendMessage ("Hit");
		} 
		if (collObject.tag == "money")
			NotFlip = true;
	}

    public void Hit(int damage)
    {
        Health=Health-damage;
        if (Health < 1)
        {
            Death();
            return;
        }
        anim.SetBool("hit", true);
        flip();
        MonsterSound.Stop();
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



	void Destroy_me(){
        if (Health < 1)
        {
            Instantiate(Prize, transform.position + transform.up * 0.37f, transform.rotation);
            Destroy(gameObject);
        }
        else
            anim.SetBool("hit", false);
	}
}
