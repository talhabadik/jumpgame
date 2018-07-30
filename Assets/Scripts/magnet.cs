using UnityEngine;
using System.Collections;
using EZ_Pooling;

public class magnet : MonoBehaviour {

    

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player.magnet = true;
            EZ_PoolManager.Despawn(transform);
        }
    }

}
