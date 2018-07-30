using UnityEngine;
using System.Collections;
using EZ_Pooling;

public class jetPack : MonoBehaviour {

	Player player;
    public bool isCombo;

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag == "Player" && !GameManager.gameOver) {
			player = col.gameObject.GetComponent<Player> ();
            if (isCombo && player.headstarted)
            {
                player.jetpackTotalFuel += 5;
                player.enableShield(player.shieldStateCounter + 5f);
            }
            else if (!player.headstarted)
            {
                player.jetpackTotalFuel = 5;
                player.enableShield(5.5f);
            }

			player.jetpackPickup ();
			EZ_PoolManager.Despawn (transform);
		}
	}
}
