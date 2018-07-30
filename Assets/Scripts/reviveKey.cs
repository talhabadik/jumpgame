using UnityEngine;
using System.Collections;
using EZ_Pooling;

public class reviveKey : MonoBehaviour {
	AudioSource audi;

	void Start(){
		audi = GetComponent<AudioSource> ();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.tag == "Player") {
			PlayerPrefs.SetInt ("reviveKey", PlayerPrefs.GetInt ("reviveKey") + 1);
			audi.Play ();
			EZ_PoolManager.Despawn (transform);
		}
	}
}