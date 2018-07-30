using UnityEngine;
using System.Collections;
using EZ_Pooling;
using UnityEngine.Audio;

public class coin : MonoBehaviour {
	
	AudioSource audi;
	public AudioMixer audioMix;
	public SpriteRenderer spriteRend;
	public bool taked = false;
    Transform playerTrasnform;

	void Start(){
		audi = GetComponent<AudioSource> ();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.tag == "Player" && !GameManager.gameOver && GameManager.gameStarted && !taked)
        {
			taked = true;
			audioMix.SetFloat ("coinPitch",getCoinPitch() + 0.02f);
            GameManager.coinEarned += 1*GameManager.coinMultiplier;
			audi.Play ();
			spriteRend.enabled = false;
			Invoke ("destroyCoin", 1f);
		}
        if (col.gameObject.tag == "magnet")
        {
            playerTrasnform = col.gameObject.GetComponentInParent<Player>().transform;
            magneting();
        }
    }

	void destroyCoin(){
		spriteRend.enabled = true;
		EZ_PoolManager.Despawn (transform);
		taked = false;
	}

    public void magneting()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTrasnform.position, 0.2f);
        if(taked == false)
            Invoke("magneting", 0.01f);
    }

	public float getCoinPitch(){
		float value;
		bool result =  audioMix.GetFloat("coinPitch", out value);
		if(result){
			return value;
		}else{
			return 0f;
		}
	}
}
