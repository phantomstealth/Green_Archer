using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour {
	[SerializeField]
	public float speed=1.0f;

	private Transform target;
	private Vector3 theScale;
	public GameObject prefabExplosion;
    public AudioClip AimArrow;
	//Animator animsword;//создается переменная для управления анимацие меча
	//Animator anim;
	bool rush;

	// Use this for initialization
	void Start () {
		if (!target) target = FindObjectOfType<Bowman_Character>().transform;
		theScale = target.localScale;
		if (tag == "sword") {
			//animsword = GetComponent<Animator> ();//
			//anim=FindObjectOfType<Character>().GetComponent<Animator>();
		}
	}

	void RushSword(){
		rush = true;
		Debug.Log (rush);
			//GetComponent<CircleCollider2D> ().enabled = true;
	}

	void RushSword_Off(){
		rush = false;
		Debug.Log (rush);
	}

	// Update is called once per frame
	void Update () {
		move_ball ();
		//verify_collider ();
		verify_outscreen ();
    }

	void move_ball() {
		//создаем переменную с текущим положение
		Vector3 Position = transform.position;
		//изменяем положение от текущей точки на 10 точек вправо ( чтобы двигалось по горизонтали постоянно);
		if (theScale.x > 0)
			Position.x = Position.x + 10;
		else
			Position.x = Position.x - 10;
				
		//двигаем объект на новую точку со скоростью speed (всегда вправо)
		transform.position = Vector3.Lerp(transform.position, Position, speed * Time.deltaTime);
	}

	void OnTriggerStay2D(Collider2D ObjectTrigger){
		OnTriggerEnter2D(ObjectTrigger);
	}

	void OnTriggerEnter2D(Collider2D ObjectTrigger) {
		float radius_collider=0;
		radius_collider = 0.2f;
		Vector3 Position=transform.position;
		if (transform.localScale.x > 0)
			Position.x = Position.x + 1.15f;
		else
			Position.x = Position.x - 1.15f;
		Collider2D[] colliders = Physics2D.OverlapCircleAll (Position, radius_collider);
		if (colliders.Length > 1) {
            SoundHandler.PlayOneShotPlayerSource(SoundHandler.PlayOneShotPlayer, AimArrow);
			for (int i = 0; i <= colliders.Length - 1; i++) {
				//Debug.Log (colliders [i].gameObject.name);
				if (colliders[i].gameObject.tag == "enemy_yellow_monster")
				{
					colliders[i].gameObject.SendMessage("Hit");
				}
				else if (colliders[i].gameObject.tag == "monster")
				{
					GetParrent(colliders[i].gameObject).SendMessage("Hit",3);					
				}
                else if (colliders[i].gameObject.tag == "Drago")
                {
                   colliders[i].gameObject.SendMessage("Hit", 3);
                }
                else if (colliders[i].gameObject.tag == "box")
				{
					colliders[i].gameObject.SendMessage("GiveMeSurprise");
					Destroy(colliders[i].gameObject);
					Instantiate(prefabExplosion, colliders[i].transform.position, transform.rotation);
				}
				Destroy (gameObject);
			}
		}
	}

    GameObject GetParrent(GameObject gameObject)
    {
        GameObject getParent = gameObject;
        //transform.root не подходит так обращается к группе monster, нужно к родителю Орк
        do
        {
            if (getParent.transform.parent != null)
            {
                if (getParent.tag == getParent.transform.parent.tag)
                {
                    getParent = getParent.transform.parent.gameObject;
                }
                else break;
            }
            else break;
        } while (true);
        Debug.Log("Player Damage by arrow - " + getParent.name);
        return getParent;
    }

    void verify_outscreen () {
		if (transform.position.x>target.position.x+40||transform.position.x<target.position.x-40) Destroy(gameObject);
	}
}
