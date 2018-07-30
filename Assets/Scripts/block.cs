using UnityEngine;
using System.Collections;

public class block : MonoBehaviour {

	AudioSource audi;
	void Start () {
		audi = GetComponent<AudioSource> ();
	}
	

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && !Player.jetpack)
		{
			audi.Play ();
		}
	}
}
