using UnityEngine;
using System.Collections;
using EZ_Pooling;

public class despawnOnBack : MonoBehaviour {

	Transform cam;
	public float limit = 6;
	public GameObject sprites;
	public SpriteRenderer spriteRend;
	public coin coinScript;

	// Use this for initialization
	void Start () {
		cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.y < cam.position.y - limit) {
			if (sprites)
				sprites.SetActive (true);
			if (spriteRend)
				spriteRend.enabled = true;
			if (coinScript)
				coinScript.taked = false;
			EZ_PoolManager.Despawn (transform);
		}
	}
}
