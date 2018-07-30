using UnityEngine;
using System.Collections;
using EZ_Pooling;

public class doubleCoin : MonoBehaviour {
	
	AudioSource audi;
	public GameObject sprites;

	void Start(){
		audi = GetComponent<AudioSource> ();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.tag == "Player" && !Player.jetpack)
        {
            GameManager.coinMultiplier *= 2;
			audi.Play ();
			sprites.SetActive (false);
			Invoke ("despawnThis", 1f);
            Invoke("final", 10);
        }
    }
		
	void despawnThis(){
		sprites.SetActive (true);
		EZ_PoolManager.Despawn (transform);
	}

    void final()
    {
        GameManager.coinMultiplier = GameManager.coinMultiplier / 2;
    }
}
