using UnityEngine;
using System.Collections;
using EZ_Pooling;

public class platformEffect : MonoBehaviour {

	AudioSource audi;//ses dosyasını eklemek için kullanılır
	public AudioSource blockBroke;
	Animator anim;
	public bool isfake = false;
	public bool roundMotion = false;
	public bool isBlock = false;
	public bool despawn = true;
	public SpriteRenderer mySprite;
	public Sprite normal;
	public Sprite damage1;
	public Sprite damage2;
	int damage;
	Transform cam;
	BoxCollider2D box;
	bool isCracked = false;
	groundCheck check;
	Player player;
	public float despawnPoint = 5;

	public void Start(){
		cam = Camera.main.transform;
		audi = GetComponent<AudioSource> ();//Ses dosyasının audi değişkenine atadık
		box = GetComponent<BoxCollider2D> ();
		isCracked = false;
		anim = GetComponent<Animator> ();

		if (isfake) {
			box.enabled = true;
			anim.SetBool ("restarted", true);
			anim.SetBool ("broke",false);
		}
	}

	void FixedUpdate(){
		if (roundMotion)
			anim.SetBool ("roundPlatform", true);
		
		if (cam.position.y - despawnPoint > transform.position.y && despawn) {
			if (isfake) {
				box.enabled = true;
				anim.SetBool ("restarted", true);
				anim.SetBool ("broke",false);
			}

			if (isBlock) {
				mySprite.sprite = normal;
				damage = 0;
			}
			EZ_PoolManager.Despawn (transform);
		}
	}

	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D col) {
		if (!GameManager.gameOver && GameManager.gameStarted) {

			if (col.gameObject.tag == "GroundCheck")
				check = col.gameObject.GetComponent<groundCheck> ();
			else if (col.gameObject.tag == "Player")
				player = col.gameObject.GetComponent<Player> ();

			if (col.gameObject.tag == "GroundCheck" && (check.character.rb.velocity.y < 0 || check.character.rb.velocity.y == check.character.jumpSpeed) && !roundMotion && !isfake && !GameManager.gameOver) {
				anim.SetTrigger ("shake");
			} else if (!isCracked && col.gameObject.tag == "GroundCheck" && (check.character.rb.velocity.y < 0 || check.character.rb.velocity.y == check.character.jumpSpeed) && !roundMotion && isfake) {
				anim.SetBool ("broke", true);
				audi.Play ();//Ses dosyamızı ne zaman çalışacağını gösterdik
				box.enabled = false;
				anim.SetBool ("restarted", false);
			} else if (col.gameObject.tag == "Player" && (player.rb.velocity.y > 1 || player.rb.velocity.y != player.jumpSpeed && !Player.jetpack) && !roundMotion && !isfake && isBlock) {
				player.rb.velocity = new Vector2 (player.rb.velocity.x, -1f);
				damage++;

				if (damage == 1)
					mySprite.sprite = damage1;
				else if (damage == 2)
					mySprite.sprite = damage2;
				else if (damage == 3) {
					mySprite.sprite = null;
				}
				if (damage == 3) {
					anim.SetTrigger ("broke");
					Invoke ("despawnThis", 2f);
					blockBroke.Play ();
				}
				else {
					audi.Play ();
					anim.SetTrigger ("blockShake");//block
				}
					

			}
		}
	}

	void despawnThis(){
		mySprite.sprite = normal;
		EZ_PoolManager.Despawn (transform);
	}
}
