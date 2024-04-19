using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Нужна для управления сценой, перезагрузки уровня
using UnityEngine.UI;


public class Bowman_Character : MonoBehaviour {
    private float buttonFireArrowY;
    private float buttonFireY;
    private float buttonShieldY;

    public Text countArrowText;
    public Text countMoneyText;
    public float hspeed;//скорость джойстика по оси горизонтальной
	public float vspeed;//скорость джойстика по оси вертикальной
	public float speed = 6f;//скорость движения персонажа влево, вправо при беге
    public float highSpeed = 6f;
	public float height = 2f;//высота прыжка при однократном зажатии вверх (умножается на speed в формуле)
	Rigidbody2D rb;//создается переменная для хранения информации о физ.компонентах
	Animator anim;//соз                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 даетcя переменная для управления анимацией
	bool grounded;//переменная для проверки находится ли персонаж в воздухе
	bool bJump = false;
	public GameObject PrefabArrow;
	bool m_facing_right=true;
	int countarrow=0;
    int countmoney = 0;
    public GameObject ButtonArrow;
    public Transform IsGrounded;
    bool swordhave=false;
    bool shieldhave = false;


    bool ShieldHave
    {
        get { return shieldhave; }
        set {
            shieldhave = value;
            if (!shieldhave)
                buttonShield.transform.position = new Vector3(buttonShield.transform.position.x, -200);

            else
                buttonShield.transform.position = new Vector3(buttonShield.transform.position.x, buttonShieldY);
        }
    }
    bool SwordHave
    {
        get { return swordhave; }
        set {
            swordhave = value;
            if (!swordhave)
                    buttonFire.transform.position = new Vector3(buttonFire.transform.position.x, -200);
            
            else
                    buttonFire.transform.position = new Vector3(buttonFire.transform.position.x, buttonFireY);
            }
        }
    int countArrow
    {
        get { return countarrow; }
        set
        {
            countarrow = value;
            countArrowText.text = ""+ countarrow;
            if (countarrow < 1)
            {
                ButtonArrow.transform.position = new Vector3(ButtonArrow.transform.position.x, -200);
            }
            else
                ButtonArrow.transform.position = new Vector3(ButtonArrow.transform.position.x, buttonFireArrowY);
        }
    }
    int countMoney
    {
        get { return countmoney; }
        set
        {
            countmoney = value;
            countMoneyText.text = "" + countmoney;
        }
    }
    public GameObject prefabExplosion;
	public GameObject VerifyZone;
	private int health=3;
    public int Health
    {
        get { return health;}
        set
        {
            if (value <= 5) health = value;
            livesbar.Refresh();
        }
    }

    private LivesBar livesbar;
    bool SoundStep = false;
    bool SoundJump = false;

    private MobileControllerButton buttonLeft;
    private MobileControllerButton buttonRight;
    private MobileControllerButton buttonJump;
    private MobileControllerButton buttonFire;
    private MobileControllerButton buttonFireArrow;
    private MobileControllerButton buttonShield;

    private AudioSource MusicSource;
    public AudioClip TakeMoney;
    public AudioClip CrushBox;
    public AudioClip SmashSword;
    public AudioClip ShotBow;
    public AudioClip Jump;
    public AudioClip Landing;
    public AudioClip HearthSound;


    // Use this for initialization
    void Start() {
        ButtonArrow = GameObject.FindGameObjectWithTag("buttonFireArrow");
        livesbar = FindObjectOfType<LivesBar>();
		rb = GetComponent<Rigidbody2D>();//rb присваивается физ.компонента персонажа (Rigidbody2D)
		anim = GetComponent<Animator>();//anim присваевается аниматор персонажа для управления анимацией(Red_hair_boy);
        MusicSource = Camera.main.GetComponent<AudioSource>();
        buttonLeft = GameObject.FindGameObjectWithTag("buttonLeft").GetComponent<MobileControllerButton>();
        buttonRight = GameObject.FindGameObjectWithTag("buttonRight").GetComponent<MobileControllerButton>();
        buttonJump = GameObject.FindGameObjectWithTag("buttonJump").GetComponent<MobileControllerButton>();
        buttonFire = GameObject.FindGameObjectWithTag("buttonFire").GetComponent<MobileControllerButton>();
        buttonFireArrow= GameObject.FindGameObjectWithTag("buttonFireArrow").GetComponent<MobileControllerButton>();
        buttonShield = GameObject.FindGameObjectWithTag("buttonShield").GetComponent<MobileControllerButton>();

        buttonFireY = buttonFire.transform.position.y;
        buttonFireArrowY = buttonFireArrow.transform.position.y;
        buttonShieldY = buttonShield.transform.position.y;

        IsGrounded = GameObject.FindGameObjectWithTag("isgrounded").GetComponent<Transform>();
        SwordHave = SwordHave;
        ShieldHave = ShieldHave;

        countArrow = 0;
        countMoney = 0;

    }

    void WriteData()    {
        //проверяем статус нажатых клавиш (силу нажатия крестовин джойстика (продолжительность)
        //hspeed = Input.GetAxis("Horizontal");//по горизонтали (влево, вправо)
        if (Input.GetAxis("Horizontal") != 0)
            hspeed = Input.GetAxis("Horizontal");
        //else if ((CrossPlatformInputManager.GetButton("Jump"))&& vspeed<1)
        else if (buttonLeft.GetKeyPress())
            if (hspeed > -1)
                hspeed = hspeed - 2 * Time.deltaTime;
            else
                hspeed = -1;
        else if (buttonRight.GetKeyPress())
            if (hspeed < 1)
                hspeed = hspeed + 2 * Time.deltaTime;
            else
                hspeed = 1;
        else
        {
            hspeed = 0;
        }


        //vspeed = Input.GetAxis("Vertical");//по вертикали (вверх, вниз)
        if (Input.GetAxis("Vertical") != 0)
            vspeed = Input.GetAxis("Vertical");
        //else if ((CrossPlatformInputManager.GetButton("Jump"))&& vspeed<1)
        else if ((buttonJump.GetKeyPress() && vspeed < 1))
            vspeed = vspeed + 2 * Time.deltaTime;
        else
        {
            vspeed = 0;
        }

        if ((!SoundStep & hspeed != 0 & grounded & !anim.GetBool("hit") & !anim.GetBool("rush")&!anim.GetBool("block")))
        {
            SoundStep = !SoundStep;
            SoundHandler.PlaySound(SoundHandler.WalkArcher);
        }

        if (hspeed == 0||!grounded)
        {
            SoundStep = false;
            SoundHandler.StopSound(SoundHandler.WalkArcher);
        }

        if (bJump & !SoundJump)
        {
            SoundJump = !SoundJump;
            SoundHandler.PlayOneShotPlayerSource(SoundHandler.PlayOneShotPlayer, Jump);
        }
        if (grounded&SoundJump&!bJump)
        {
            SoundJump = false;
            SoundHandler.PlayOneShotPlayerSource(SoundHandler.PlayOneShotPlayer, Landing);
        }

        anim.SetBool("run", hspeed != 0 & grounded & !anim.GetBool("hit") & !anim.GetBool("rush") & !anim.GetBool("block"));//анимируем персонажа бегом, если персонаж на земле и зажата влево или вправо
		anim.SetBool("jump", !grounded & rb.velocity.y>=0 & !anim.GetBool("rush") & !anim.GetBool("fire") & !anim.GetBool("block"));//анимируем песонажа прыжком, если персонаж в воздухе и ускорение вверх больше или равно 0
		anim.SetBool("jump_fall", !grounded & rb.velocity.y<0 & !anim.GetBool("rush") & !anim.GetBool("fire") & !anim.GetBool("block"));//анимируем персонажа падением, если персонаж в воздухе и усорение минусовое (падение)
		VerifyFall();
	}

	// Update is called once per frame
	void Update() {
		CheckGround();
		WriteData();
		if (speed == 0)
			return;
		if ((Input.GetKeyDown(KeyCode.UpArrow) || buttonJump.GetKeyDown()) & grounded) bJump=true;//если зажата клавиша прыжка поднимаем персонажа в воздух
		if (Input.GetKeyUp(KeyCode.UpArrow)||buttonJump.GetKeyUp()) bJump =false;
		if (Input.GetKeyDown (KeyCode.LeftControl)||buttonFire.GetKeyDown()) 
			Fire (false);
		else if(Input.GetKeyDown(KeyCode.RightControl) || buttonFireArrow.GetKeyDown())
            Fire(true);//если зажата клавиша огонь запускается подпрограмма fire
        if ((Input.GetKeyDown(KeyCode.DownArrow)||buttonShield.GetKeyDown())&ShieldHave) anim.SetBool("block", true)       ;//Если зажата клавиша пробел ставим блок
        if (Input.GetKeyUp(KeyCode.DownArrow)||buttonShield.GetKeyUp()) anim.SetBool("block", false);
        VerifyJump();
	}

    private void FixedUpdate()
    {
        Move();
    }

    void Move() {
		if (anim.GetBool("fire")||anim.GetBool("rush")||anim.GetBool("block")) return;
		if (hspeed>0||buttonLeft.GetKeyPress()||buttonRight.GetKeyPress()) {
            //если движение джойстика (курсора) больше 0 (нажата клавиша) двигаем характера со скоростью speed
			flip();
			transform.Translate(Vector2.right * hspeed * Time.deltaTime * speed);
            //if (TriggerStay) On
        }
		if (hspeed<0) {
            flip();
            //если движение джойстика (курсора) меньше 0 (нажата клавиша) двигаем характера со скоростью speed в обратную сторону (vspeed будет отрицательной)
            transform.Translate(Vector2.right * hspeed * Time.deltaTime * speed);
		}
    }

	void flip(){
		if (hspeed > 0 && !m_facing_right || hspeed < 0 && m_facing_right) {
			m_facing_right = !m_facing_right;
			Vector3 theScale = transform.localScale;
			theScale.x = theScale.x * -1;
			transform.localScale = theScale;
		}
	}

	void VerifyJump()
	{
		//используется Rigidbody2D, rb привязывается через Getcomponent<Rigidbody2d>
		if (bJump)
		{
			rb.velocity = new Vector2(rb.velocity.x, speed* 2.2f * height*vspeed);
			if (rb.velocity.y >=speed*height-0.1f) bJump = false;// елси больше максимальной величины остановить прыжок
		}

	}

	void VerifyDeath(){
		if (Health < 1)
			Death ();
		else {
			anim.SetBool ("hit", false);
			speed = highSpeed;
		}
	}

	void Hit(){
        if (anim.GetBool("block"))
        {
            SoundHandler.PlaySound(SoundHandler.ShieldStop);
            return;
        }
        if (anim.GetBool("hit")) return;
		Health = Health - 1;
		Debug.Log ("Player take damage - " + Health);
        VerifyDeath();
		anim.SetBool ("hit", true);
		speed = 0;
        SoundHandler.PlaySound(SoundHandler.HitArcher);
	}

	void Death(){
        MusicSource.Stop();
        SoundHandler.PlaySound(SoundHandler.DeathArcher);
        anim.SetBool ("fall", true);
		speed = 0;
	}

	void Reload_Scene()
    {
        SceneManager.LoadScene("GameOver");
        //SceneManager.LoadScene(SceneManager.GetSceneAt(0).name); 
	}

	void VerifyFall()
	{
        //если персонаж ниже определенного уровня перезагрузить сцену (смерть)
        if (transform.position.y < -20f)
        {
            Reload_Scene();
        }
	}

	public void CheckGround()
	{
		//проверяются столкновения в точке pivot спрайта с радиусом 0.2f, если столковений с ногами больше одного значит стоим на земле. (Не подходит в данном случае!!!)
		//Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);
		//проверяются столкновения в квадрате pivot спрайта (шириной квадрата 1 и высотой 0.1), если столковений больше одной значит стоим на земле.
		//Collider2D[] colliders=Physics2D.OverlapBoxAll(new Vector2(transform.position.x-0.2f,transform.position.y-0.8f), new Vector2(1.0f,0.1f),0);
		//grounded = colliders.Length > 1;
        Debug.DrawLine(IsGrounded.position, new Vector2(IsGrounded.position.x,IsGrounded.position.y-0.1f));
        Collider2D[] colliders2=Physics2D.OverlapCircleAll(IsGrounded.position,0.1f);
        grounded = colliders2.Length > 1;
        //if (colliders2.Length > 1) Debug.Log("Grounded"); else Debug.Log("not Grounded");


	}

	void OnTriggerStay2D(Collider2D ObjectTrigger){
		if (hspeed>0) OnTriggerEnter2D (ObjectTrigger);
	}
		
	void OnTriggerEnter2D(Collider2D ObjectTrigger){
		bool SwordCollider=false;

        //Выясняем столкнулся ли меч с объектом триггером
        Collider2D[] colliders = Physics2D.OverlapBoxAll(ObjectTrigger.gameObject.transform.position, ObjectTrigger.gameObject.transform.localScale, 0);
        //Проверяем по списку все коллайдеры(colliders.Length) учавствующие в столкновении
        for (int i = 0; i <= colliders.Length - 1; i++)
        {
            //если один из них меч отмечаем это в переменной SwordCollider
           // if (colliders[i].gameObject.tag == "sword") SwordCollider = true;
        }
        //если с тригером столкнулся меч выходим из процедуры если нет то продолжаем
        //if (SwordCollider) return;

        for (int i = 0; i <= colliders.Length - 1; i++)
        {
            if (colliders[i].gameObject.tag == "Player") SwordCollider = true;

            //если один из них меч отмечаем это в переменной SwordCollider
        }
        if (!SwordCollider) return;

        if (ObjectTrigger.gameObject.tag == "money") {
            SoundHandler.PlayOneShotSource(SoundHandler.PlayOneShot, TakeMoney);
			Destroy (ObjectTrigger.gameObject);
			countMoney++;
		}
        if (ObjectTrigger.gameObject.tag == "hearth")
        {
            SoundHandler.PlayOneShotPlayerSource(SoundHandler.PlayOneShot, HearthSound);
            Destroy(ObjectTrigger.gameObject);
            Health++;
        }
        if (ObjectTrigger.gameObject.tag == "Prize_Arrow")
        {
            SoundHandler.PlayOneShotSource(SoundHandler.PlayOneShot, TakeMoney);
            Destroy(ObjectTrigger.gameObject);
            countArrow++;
        }
        if (ObjectTrigger.gameObject.tag == "Prize_Sword")
        {
            SoundHandler.PlayOneShotSource(SoundHandler.PlayOneShot, TakeMoney);
            Destroy(ObjectTrigger.gameObject);
            SwordHave=true;
        }
        if (ObjectTrigger.gameObject.tag == "Prize_Shield")
        {
            SoundHandler.PlayOneShotSource(SoundHandler.PlayOneShot, TakeMoney);
            Destroy(ObjectTrigger.gameObject);
            ShieldHave = true;
        }

    }

    void Final_Anim()
	{
		anim.SetBool ("rush", false);//заканчиваем удар мечом
		anim.SetBool("fire",false);//заканчиваем стрелять луком
        anim.SetBool("block", false);//заканчиваем блок
	}

	void Instantiate_Arrow(){
        SoundHandler.PlayOneShotPlayerSource(SoundHandler.PlayOneShotPlayer, ShotBow);
        countArrow = countArrow - 1;
        Vector2 Position = transform.position;
		if (m_facing_right)
			Position.x = Position.x + 0.08f;
		else
			Position.x = Position.x - 0.08f;

		Position.y = Position.y - 0.049f;
		GameObject ArrowTransform;
		ArrowTransform=Instantiate (PrefabArrow, Position, transform.rotation);
		Vector3 theScale = transform.localScale;
		ArrowTransform.transform.localScale = theScale;

	}

	void Fire(bool RightCtrl)
	{
        //print(countArrow);
		if ((countArrow < 1||!RightCtrl)&(SwordHave)) {
			anim.SetBool("rush", true);//анимируем удар мечом
		} else if (SwordHave) {
            anim.SetBool ("fire", true);//анимируем стрельбу
		}

	}

    void SoundRushSword()
    {
        SoundHandler.PlayOneShotPlayerSource(SoundHandler.PlayOneShotPlayer, SmashSword);
    }

    void Verify_Weapon_Collider() {
        Vector2 Position; 
		Vector2 ScalePos;
		Position.x = transform.position.x + 0.757f*transform.localScale.x;
		Position.y = transform.position.y - 0.023f;
		ScalePos.x = 0.96f;
		ScalePos.y = 1.17f;
		Collider2D[] colliders = Physics2D.OverlapBoxAll (Position, ScalePos,0);
		VerifyZone.transform.position = Position;
		VerifyZone.transform.localScale = ScalePos;

		if (colliders.Length > 1) {

			for (int i = 0; i <= colliders.Length - 1; i++) {
                if (colliders[i].gameObject.tag == "monster")
                {
                    GetParrent(colliders[i].gameObject).SendMessage("Hit", 1);
                }
                else if (colliders[i].gameObject.tag == "box")
                {
                    SoundHandler.PlayOneShotSource(SoundHandler.PlayOneShot, CrushBox);
                    colliders[i].gameObject.GetComponent<box>().GiveMeSurprise();
                    Destroy(colliders[i].gameObject);
                    Instantiate(prefabExplosion, colliders[i].transform.position, transform.rotation);
                }
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
        Debug.Log("Player Damage by sword - " + getParent.name);
        return getParent;
    }


}
