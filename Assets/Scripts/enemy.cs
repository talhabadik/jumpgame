using UnityEngine;
using System.Collections;
using EZ_Pooling;

public class enemy : MonoBehaviour {
	public float maxX;
	public float hiz = 0.01f;
	float minX;
	public bool HareketliDusman;
	AudioSource audi;

	Transform cam;
	bool died = false;
	public bool UsttenOlum;
	public bool AlttanOlum;
	public bool YerdekiDusman;
	public bool HavadakiDusman;
	public bool notChild = false;

	SpriteRenderer enemySprite;

	Player player;
	groundCheck check;

	int yon = 1;
	void Start () {
		cam = Camera.main.transform;
		minX = transform.localPosition.x;
		if (HavadakiDusman)
			minX = -maxX;
		enemySprite = GetComponent<SpriteRenderer> ();
		audi = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (cam.position.y - 10 > transform.position.y) {
			if (notChild)
				EZ_PoolManager.Despawn (transform);
			else {
				died = false;
				enemySprite.enabled = true;
			}
		}


		if (HareketliDusman && !died) {
			if (transform.localPosition.x > maxX) {
				yon = yon * -1;
				transform.localScale = new Vector3 (1f, 1f, 1f);
			}
			if (transform.localPosition.x < minX) {
				yon = yon * -1;
				transform.localScale = new Vector3 (-1f, 1f, 1f);
			}
				transform.Translate (new Vector2 (hiz * yon, 0));
		}
	}





		void OnTriggerEnter2D(Collider2D col){
		if (!died) {
			if (col.gameObject.tag == "GroundCheck")
				check = col.gameObject.GetComponent<groundCheck> ();
			if (col.gameObject.tag == "Player")
				player = col.gameObject.GetComponent<Player> ();


			if (col.gameObject.name == "Player" && player && player.rb.velocity.y > 0 && AlttanOlum) {//karekter aşağıdan geliyor 
				player.die ();
			} else if (col.gameObject.tag == "GroundCheck" && check && check.character.rb.velocity.y < 0 && !GameManager.gameOver) { // karekter yukardan geliyor
				if (UsttenOlum) {
					check.character.die (); // yukardan geliyorsa karakter ölür
				} else {
				
					check.character.jump (check.character.jumpSpeed);
					// eğer yukardan gelenden ölüyorsa ölür

					if (AlttanOlum) {
						deleteEnemy ();
						audi.Play ();
					}
				}
			}
		}
	}

	void deleteEnemy(){
		if (notChild)
			EZ_PoolManager.Despawn (transform);
		else {
			enemySprite.enabled = false;
			died = true;
		}
	}
}

