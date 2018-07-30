using UnityEngine;
using System.Collections;

public class Trambolin : MonoBehaviour {

	public Animator anim;
	groundCheck check;
	AudioSource audi;

	void Start(){
		audi = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!check && col.gameObject.tag == "GroundCheck")
			check = col.gameObject.GetComponent<groundCheck> ();
		
		
		if (col.gameObject.tag == "GroundCheck" && check.character.rb.velocity.y < 0 && !GameManager.gameOver )
		{	
			check.character.enableTrambolin ();
			check.character.jump (check.character.jumpSpeed * 2);
			anim.SetTrigger ("trambolin");
			audi.Play ();

		}
}
}