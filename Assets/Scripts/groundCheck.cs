using UnityEngine;
using System.Collections;

public class groundCheck : MonoBehaviour {

	public Player character; // Player
	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D col) {
		if (GameManager.gameStarted && character.rb.velocity.y < 0 && col.gameObject.tag == "platform") { // eğer player düşüyorsa ve bir platforma bastıysa zıplar
			character.jump (character.jumpSpeed);
		}
	}
}
