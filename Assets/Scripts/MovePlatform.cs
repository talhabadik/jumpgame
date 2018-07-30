using UnityEngine;
using System.Collections;

public class MovePlatform : MonoBehaviour {

		public float maxX;
		public float hiz = 0.03f;
		public float maxY;
	 	float minY;
		public bool unlimitedTurn = false;
		public int yon = 1;
		public bool isUp;

	void Start(){
		minY = transform.position.y;
		
		
		
	}
	void FixedUpdate () {
		if (!isUp && !unlimitedTurn) {
			if (transform.position.x > maxX)
				yon = yon * -1;
			if (transform.position.x < maxX * -1)
				yon = yon * -1;
		
			transform.Translate (new Vector2 (hiz * yon, 0));
		} else if (!unlimitedTurn) {
			if (minY + maxY < transform.position.y)
				yon = yon * -1;
			if (minY > transform.position.y)
				yon = yon * -1;
		
			transform.Translate (new Vector2 (0, yon * hiz));
		} else {
			transform.Translate (new Vector2 (hiz * yon, 0));

			if(transform.position.x > maxX + 2f)//ekranın sağından çıktıysa
				transform.position = new Vector3((maxX + 2f) * -1,transform.position.y,transform.position.z);
			else if (transform.position.x < (maxX + 2f) * -1)
				transform.position = new Vector3((maxX + 2f),transform.position.y,transform.position.z);
		}
	}
}