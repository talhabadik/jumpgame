using UnityEngine;
using System.Collections;
using EZ_Pooling;

public class jumpShoes : MonoBehaviour {

	Transform cam;


	// Use this for initialization
	void Start () {
		cam = Camera.main.transform;
	}

	void FixedUpdate(){
		if (cam.position.y - 10 > transform.position.y)
			EZ_PoolManager.Despawn (transform);
	}

	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag == "Player" && !GameManager.gameOver && !Player.jetpack) {
			col.gameObject.GetComponent<Player>().jumpShoesEnabled ();
			EZ_PoolManager.Despawn (transform);
		}
	}
}
