using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	public Animator anim;
	public Transform characterSprite;
	public Rigidbody2D rb;
    public GameManager gameman;
	public spawner spawnMan;
    public Scrollbar backFlipSlider;
	AudioSource audi;
	public AudioSource jetpackAudio;
	public AudioSource dieAudio;
	public Animator shieldAnim;

	public GameObject jumpShoes;
	public GameObject shield;

	public static bool shieldState;

	public static bool antiBlock = false;//blokların içinden geçebilme özelliği
	public static bool jetpack = false;
	public GameObject jetPack;
	public Animator jetPackAnimator;
	public float moveSpeed; //hareket hızı
	public float jumpSpeed; //zıplama hızı
	public float jetpackSpeed;
	public float screenRightLimit; // sagdan gidice soldan çıkmanın limiti
	public float horizontalInput = 0; // sag sol kontrolünden aldığımız değer
    float xVelocity = 0;  // sag yada sol boost'unun hızı
	float flipSpeed;
	bool canBackFlip = true;
    public bool headstarted = false;

	//counterlar
	public float jetpackFuel = 10;
	public float shieldStateCounter = 0;
	float shieldSpriteCounter = 0;
    public GameObject magnetCircle;
    public static bool magnet = false;
    float magnetSec;
    public float jetpackTotalFuel;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		audi = GetComponent<AudioSource> ();
        magnet = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// eğer editördeyse ok tuşu kullanılır
		if (Input.acceleration.x == 0)
			horizontalInput = Input.GetAxis ("Horizontal") * 2;
		else // eğer telefonda oynanıyorsa tilt kontrolu kullanır
			horizontalInput = Input.acceleration.x * 8;

		if (xVelocity != 0) { // eğer boost varsa
			xVelocity = Mathf.Lerp (xVelocity, 0, 0.1f); // boost'u azaltır
			if (xVelocity < 1 && xVelocity > -1) // eğer -1 ile 1 arasında ise
				xVelocity = 0; // boost biter
		}

		rb.velocity = new Vector2(horizontalInput * moveSpeed + xVelocity, rb.velocity.y);
		if(GameManager.gameOver)
			rb.velocity = new Vector2(0, rb.velocity.y);
		// saga sola donme
		if (horizontalInput < -0.2)
			characterSprite.localScale = new Vector3 (-1, characterSprite.localScale.y, 1);
		else if(horizontalInput > 0.2)
			characterSprite.localScale = new Vector3 (1, characterSprite.localScale.y, 1);

		if(transform.position.x > screenRightLimit)//ekranın sağından çıktıysa
			transform.position = new Vector3(screenRightLimit * -1,transform.position.y,transform.position.z);
		else if (transform.position.x < screenRightLimit * -1)
			transform.position = new Vector3(screenRightLimit,transform.position.y,transform.position.z);

		//ANIMASYON
		anim.SetBool("gameStarted",GameManager.gameStarted);
		anim.SetFloat ("walkVelocity", horizontalInput);
		anim.SetFloat ("yVelocity", rb.velocity.y);

		flipSpeed = Mathf.Lerp (flipSpeed, 0,0.05f);
		if (flipSpeed > 1 && flipSpeed < 90)
			flipSpeed -= 2;

		if (Input.GetKeyDown (KeyCode.UpArrow) && GameManager.gameStarted)
			backFlip ();


		

		if (GameManager.gameStarted) {
			if (xVelocity != 0) {
				flipSpeed = 0;
				characterSprite.localEulerAngles = new Vector3 (0, 0, xVelocity * -4);
			}
            else if(jetPack){
				characterSprite.localEulerAngles = new Vector3 (0, 0, (rb.velocity.x * -5f) + flipSpeed);
			}
            else {
				characterSprite.localEulerAngles = new Vector3 (0, 0, (rb.velocity.x * -1.5f) + flipSpeed);
			}
		}

		if (shieldStateCounter > 0) {
			shieldState = true;
			shieldStateCounter -= Time.deltaTime;
			if (shieldStateCounter < 0) {
				shieldStateCounter = 0;
				shieldState = false;
			}
		}
		if (shieldSpriteCounter > 0) {
			shield.SetActive(true);
			shieldSpriteCounter -= Time.deltaTime;
			if (shieldSpriteCounter < 2)
				shieldAnim.SetTrigger ("end");

			if (shieldSpriteCounter < 0) {
				shieldSpriteCounter = 0;
				shield.SetActive(false);
			}
		}

		if (jetpack && jetpackTotalFuel >= 0) {
			jetpackTotalFuel -= Time.deltaTime;
		}
        else if (jetpackTotalFuel < 0) {
            if (headstarted)
            { 
                headstarted = false;
                jumpSpeed = jumpSpeed / 2;
            }
            
			emptyJetpack ();
			jetpackTotalFuel = 0;
		}

        if (magnet)
        {
            magnetCircle.SetActive(true);
            magnetSec += 1 * Time.deltaTime;
        }
        else
            magnetCircle.SetActive(false);

        if (magnetSec > 10)
        {
            magnet = false;
            magnetSec = 0;
        }

        if (jetpack)
        {
			
            jetPack.transform.parent = characterSprite;
            jetPack.transform.localPosition = new Vector3(-0.2f, -0.17f, 0);
            jetPack.transform.localEulerAngles = new Vector3(0, 0, 180f);
            jetPack.SetActive(true);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed * 2);
        }


	}

	public void jump(float speed){
		if (GameManager.gameOver == false) { // oyun bitmediyse zıplar
			rb.velocity = new Vector2 (rb.velocity.x, speed);
			audi.Play ();
		}
	}

	public void die(){
		if (!shieldState) {
			characterSprite.localScale = new Vector3 (characterSprite.localScale.x, -1, 1);
			jump (jumpSpeed / 3);
			gameman.gameEnded ();
			audi.Play ();

		}
        
	}

	public void onGameStart(){ // oyun başladığında
		rb.constraints = RigidbodyConstraints2D.FreezeRotation; //playerın düşmesini aktif hale getirir
		jump(jumpSpeed);// ilk zıplamayı yapar
	}

    public void addForceX(float speed) {
		xVelocity = speed;
    }

	public void backFlip(){
		if (canBackFlip && !GameManager.gameOver)
        {
			
            flipSpeed = 360f;
            jump(jumpSpeed);
            canBackFlip = false;
            backFlipSlider.size = 0;
            counter();
			enableShield (1f);

        }
    }
    public void counter()
    {
        if (!canBackFlip)
        {
            if (backFlipSlider.size != 1)
            {
                backFlipSlider.size += 1 / 180f;
                Invoke("counter", 0.02f);
            }
            else
            {
                canBackFlip = true;
            }

        }
    }

	public void emptyJetpack(){
		jetPack.transform.parent = transform;
		jetPack.transform.localEulerAngles = new Vector3 (0,0,180);
		jetPackAnimator.SetTrigger ("empty");
		Invoke ("disableJetpack",1.5f);
		jetpack = false;
	}

	void disableJetpack(){
		jetPack.transform.GetChild (0).localEulerAngles = Vector3.zero;
		jetPack.SetActive (false);
        Debug.Log("dasdasd");
	}

	public void jumpShoesEnabled(){
		jumpShoes.SetActive (true);
		jumpSpeed = 10f;
		Invoke ("disableJumpShoes", 4f);
	}

	void disableJumpShoes(){
		jumpShoes.SetActive (false);
		jumpSpeed = 8f;
	}

	public void shieldEnabled(){
		enableShield (10f);
		shieldSpriteCounter = 10f;
		shieldState = true;
		shield.SetActive (true);
		Invoke ("disabledShield",10f);
	}

	public void disabledShield(){
		shield.SetActive (false);
		shieldState = false;

	}

	public void enableShield(float second){
		if(second > shieldStateCounter)
			shieldStateCounter = second;
	}


	public void enableTrambolin(){
        if (jumpShoes.activeSelf)
            enableShield(2f);
        else
            enableShield(1.2f);
    }

	public void jetpackPickup(){
        jetpack = true;
		jetpackAudio.Play ();
	}
	public void tap(){
		if (GameManager.gameStarted && !GameManager.gameOver) {
			backFlip ();
		}
	}
}