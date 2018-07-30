using UnityEngine;
using System.Collections;

public class boost : MonoBehaviour {

    public float mulitplier;
	AudioSource audi;
    Player playerScript;
	public int angle;
    /*  Angle
     * -1 Sol Çapraz
     *  0 Düz Yukarı
     *  1 Sağ Çapraz
     *  2 Düz Aşağı
     */

    void Start()
    {
        if ((int)transform.localEulerAngles.z < 43) // düz yukarı
            angle = 0;
        else if ((int)transform.localEulerAngles.z < 177) // sol
            angle = -1;
        else if ((int)transform.localEulerAngles.z < 312) // düz aşağı
            angle = 2;
         else// sağ
            angle = 1; 

		audi = GetComponent<AudioSource> ();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
		
        if (col.gameObject.tag == "Player")
        {
          
            audi.Play();
			playerScript = col.gameObject.GetComponent<Player> ();
            playerScript.enableShield(1f);
            if (angle == 0)
            {
				//yukarı
                playerScript.jump(playerScript.jumpSpeed * mulitplier);
            }
            else if (angle == -1)
            {
				//sol yukarı
				playerScript.addForceX(playerScript.jumpSpeed * -2);
                playerScript.jump(playerScript.jumpSpeed * mulitplier);
            }
            else if (angle == 1)
            {
				//sağ yukarı
				playerScript.addForceX(playerScript.jumpSpeed * 2);
                playerScript.jump(playerScript.jumpSpeed * mulitplier);
            }
            else if (angle == 2)
            {
				//aşağıya doğru
                playerScript.jump(playerScript.jumpSpeed * mulitplier * -0.5f);
            }

        }
    }
}
