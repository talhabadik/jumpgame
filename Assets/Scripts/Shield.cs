using UnityEngine;
using System.Collections;
using EZ_Pooling;

public class Shield : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag == "Player" && !GameManager.gameOver && !Player.jetpack) {
			col.gameObject.GetComponent<Player>().shieldEnabled ();
			 EZ_PoolManager.Despawn (transform);
		}
	}
}
