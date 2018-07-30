using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

	public Player player;
	public float playerDiePos;
	public float camPos = 0; // kameranın başlanıç posizyonu başta yeri görşün diye böyle verdim
	public bool move = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (player.transform.position.y > camPos) //eğer player kameradan yukarıdaysa kamera onu takip eder
			camPos = player.transform.position.y;
		else if (player.transform.position.y < camPos - playerDiePos && GameManager.gameOver == false) { // eğer player kameranın sınırından aşağıdaysa (yani düştüyse) oyun biter.
			Player.shieldState = false;
			player.shield.SetActive (false);
			player.die ();
			GameManager.gameOver = true;
		}
		if(GameManager.gameStarted && !GameManager.gameOver && move)
			camPos += 0.01f;
		transform.position = new Vector3 (transform.position.x, camPos, transform.position.z);
	}
}
