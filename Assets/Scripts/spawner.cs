using UnityEngine;
using System.Collections;
using EZ_Pooling;

public class spawner : MonoBehaviour {

	float memCampos;
	float memSpawnMission;
	float campos;
	public camera cam;
	int platformID = 5;
	bool lastIsEnemy = false;

	[System.Serializable]
	public class platfrm {
		public Transform platformPrefab;
		public float[] probability; 
		public bool dontRepeat;
		public int showedLevel;
		public bool isItem = false;
		public bool friendlySpawn;
	}

	public Transform roundPlatform;
	public Transform trambolineCombo;
	public Transform boostComboRight;
	public Transform boostComboLeft;
	public Transform jetpackCoinCombo;
	public Transform boostPrefab;
	public Transform coinPrefab;
	public float[] levelLimits;
	public Transform[] comboPlatforms;
	public platfrm[] platform;
	//public Transform coin;
	public Player player;
	public float randomX;
	public float spawnHeight = 0.5f;
	public int level = 0;
	float friendyCounter;
	bool friendly;

	// Use this for initialization
	void Start () {

		EZ_PoolManager.Spawn (platform[0].platformPrefab, new Vector3 (Random.Range (-randomX, randomX), -2, 0), Quaternion.identity);

		platformID = -2;
		for (int i = 0; i < 7; i++) {
			spawnRandomly ();
		}
	}

	void comboState(){
		float rand = Random.Range (0, 100);
		if (rand < 20)
			combo1 ();
		else if (rand < 40)
			combo2 ();
		else if (rand < 44)
			combo3 ();
		else if (rand < 54)
			combo4 ();
		else if(rand < 55)
			combo5 ();
		else if(rand < 65)
			combo6 ();
		else if(rand < 75)
			combo7 ();
		else if(rand < 90)
			combo8 ();
		else if(rand < 100)
			combo9 ();
		
	}



	// Update is called once per frame
	void FixedUpdate () {
		if (cam.transform.position.y > levelLimits [level] && !GameManager.gameOver) {
			level++;
		}

		campos = Mathf.Round(cam.camPos);

		if (player.jetPack.activeSelf && player.jetpackTotalFuel < 2 && friendyCounter == 0) {
			friendyCounter = 4;
		}
		if (friendyCounter > 0) {
			friendyCounter -= Time.deltaTime;
			friendly = true;
		}else {
			friendyCounter = 0;
			friendly = false;
		}


		if (campos >= platformID - 4) {
			
			

			if (friendly) {
				spawnFriendly ();
				Debug.Log ("friend");
			}else {
				if (Random.Range (0, 100) < 95 || player.jetpackTotalFuel > 2)
					spawnRandomly ();
				else if (!player.jetPack.activeSelf)
					comboState ();
			}
		}
	}

	void spawnRandomly(){

		float allProbs = 0;
		int selectedID = -2;

		platformID++;
		float height = platformID / spawnHeight;
		float prob = Random.Range(0,100);

		for (int i = 0; i < platform.Length; i++) {
			allProbs = platform [i].probability[level] + allProbs;
			if (prob < allProbs && platform[i].showedLevel <= level) {
				selectedID = i;
				break;
			}
		}

		if (selectedID == 14 & player.jetPack.activeSelf)
			selectedID = 0;

		float randomXpos = Random.Range(-randomX,randomX);

		if (30 > Random.Range (0, 100) && !platform[selectedID].isItem) { // 30% şansla coin spawn edilir
			if (70 > Random.Range (0, 100))
				EZ_PoolManager.Spawn (coinPrefab, new Vector3 (randomXpos, height + (spawnHeight / 2), 0), Quaternion.identity);
			else {
				EZ_PoolManager.Spawn (coinPrefab, new Vector3 (randomXpos - 0.2f, height + (spawnHeight / 2), 0), Quaternion.identity);
				EZ_PoolManager.Spawn (coinPrefab, new Vector3 (randomXpos + 0.2f, height + (spawnHeight / 2), 0), Quaternion.identity);
			}
		}

		if(selectedID == -2)
			SpawnPlatform(0,new Vector3 (randomXpos,height,0),false);
		else
			SpawnPlatform(selectedID,new Vector3 (randomXpos,height,0),false);
	}

	public void spawnFriendly(){
		platformID++;
		float randX = Random.Range (-randomX, randomX);

		if (Random.Range (0, 100) < 30)
			SpawnPlatform (1,new Vector3(randX,platformID,0),false);
		else
			SpawnPlatform (0,new Vector3(randX,platformID,0),false);
		if (Random.Range (0, 100) < 20) {
			if (90 > Random.Range (0, 100))
				EZ_PoolManager.Spawn (coinPrefab, new Vector3 (randX, platformID + (spawnHeight / 2), 0), Quaternion.identity);
			else {
				EZ_PoolManager.Spawn (coinPrefab, new Vector3 (randX - 0.2f, platformID + (spawnHeight / 2), 0), Quaternion.identity);
				EZ_PoolManager.Spawn (coinPrefab, new Vector3 (randX + 0.2f, platformID + (spawnHeight / 2), 0), Quaternion.identity);
			}
		}
	}

	void SpawnPlatform (int spawnedID, Vector3 pos,bool isCombo){

		Transform pref = null;

		if(isCombo)
			 pref = comboPlatforms[spawnedID];
		else
			 pref = platform[spawnedID].platformPrefab;

		if (platform[spawnedID].isItem) {
			EZ_PoolManager.Spawn (platform [0].platformPrefab, pos, Quaternion.identity);
		}

		if (pref == null) {
			pref = platform [0].platformPrefab;
			Debug.LogError ("Bunu debug ile yaptım. Spawn kodunda seçilen platform listede yok!");
		}

		if (!isCombo && platform [spawnedID].dontRepeat && lastIsEnemy && !platform [spawnedID].isItem) {
			/*
			float prob = Random.Range(0,100);
			float allProbs = 0;

			for (int i = 0; i < platform.Length; i++) {
				allProbs = platform [i].probability[level] + allProbs;
				if (prob < allProbs && platform[i].showedLevel <= level) {
					pref = platform[i].platformPrefab;
					Debug.LogError (pref);
					SpawnPlatform (i,pos,isCombo);
					
					break;
				}
			}
			*/
			spawnedID = 0;
			pref = platform [0].platformPrefab;

			EZ_PoolManager.Spawn (pref, pos, Quaternion.identity);
			lastIsEnemy = false;

		} else {
			if (platform [spawnedID].isItem)
				pos = new Vector3 (pos.x, pos.y + 0.5f, pos.z);

			EZ_PoolManager.Spawn (pref, pos, Quaternion.identity);
			lastIsEnemy = platform [spawnedID].dontRepeat;
			//if (pos.y < cam.transform.position.y + 4) {
			//	spawnRandomly ();
			//}

		}
	}


	//COMBOLAR


	void combo1(){// sonsuz sonsuz sonsuz
		float height = platformID / spawnHeight;

		for (int i = 0; i < 10; i++) {
			EZ_PoolManager.Spawn (coinPrefab, new Vector3 (i - 2, height + 1.5f, 0), Quaternion.identity);
		}

		for (int i = 0; i < 10; i++) {
			EZ_PoolManager.Spawn (coinPrefab, new Vector3 (i - 2, height + 2.5f, 0), Quaternion.identity);
		}

		SpawnPlatform(0,new Vector3 (1,height + 3,0),true);
		SpawnPlatform(0,new Vector3 (-1,height + 3,0),true);

		SpawnPlatform(0,new Vector3 (-1,height + 1,0),true);
		SpawnPlatform(0,new Vector3 (2,height + 1,0),true);

		SpawnPlatform(0,new Vector3 (0,height + 2,0),true);
		SpawnPlatform(0,new Vector3 (-2,height + 2,0),true);

		platformID += 3;

	}


	void combo2(){//sonsuz hareketli sonsuz
		float height = platformID / spawnHeight;
		SpawnPlatform(0,new Vector3 (-1,height + 1,0),true);
		SpawnPlatform(0,new Vector3 (2,height + 1,0),true);

		for (int i = 0; i < 10; i++) {
			EZ_PoolManager.Spawn (coinPrefab, new Vector3 (i - 2, height + 1.5f, 0), Quaternion.identity);
		}

		SpawnPlatform(2,new Vector3 (0,height + 2,0),true);

		for (int i = 0; i < 10; i++) {
			EZ_PoolManager.Spawn (coinPrefab, new Vector3 (i - 2, height + 2.5f, 0), Quaternion.identity);
		}

		SpawnPlatform(0,new Vector3 (1,height + 3,0),true);
		SpawnPlatform(0,new Vector3 (-1,height + 3,0),true);

		platformID += 3;
	}

	void combo3(){ 
		platformID++;

		float height = platformID / spawnHeight;

		EZ_PoolManager.Spawn (jetpackCoinCombo, new Vector3 (0, height, 0), Quaternion.identity);

		platformID++;

		float xPlus = 0;

		for (int i = 0; i < 70; i++) {
			platformID++;

			xPlus = xPlus + (Random.Range (-1, 1) / 5f);
			if (xPlus > 2 || xPlus < -2)
				xPlus = 1;
			EZ_PoolManager.Spawn (coinPrefab, new Vector3 (xPlus,platformID / spawnHeight, 0), Quaternion.identity);
		}
	}

	void combo4 (){
		for (int j = 0; j < Random.Range(5,8); j++) {
			platformID += 2;

			float randomPos = Random.Range (-randomX, randomX);
			for (int i = 0; i < 3; i++) {
				EZ_PoolManager.Spawn (boostPrefab, new Vector3 (randomPos, platformID, 0), Quaternion.identity);
				platformID++;
			}
		}
		int memLevel = level;
		level = 1;
		spawnRandomly ();
		spawnRandomly ();
		spawnRandomly ();
		spawnRandomly ();
		spawnRandomly ();
		level = memLevel;
	}

	void combo5(){
		platformID+=2;
		float rand = Random.Range (-1, 1);
		EZ_PoolManager.Spawn (boostPrefab, new Vector3 (rand * 2, platformID + 1f, 0), Quaternion.identity);

		if (rand == 0) {
			EZ_PoolManager.Spawn (boostPrefab, new Vector3 (-2, platformID, 0), new Quaternion(0,0,180,1));
			EZ_PoolManager.Spawn (boostPrefab, new Vector3 (2, platformID, 0), new Quaternion(0,0,180,1));
		} else if (rand == 1) {
			EZ_PoolManager.Spawn (boostPrefab, new Vector3 (-2, platformID, 0), new Quaternion(0,0,180,1));
			EZ_PoolManager.Spawn (boostPrefab, new Vector3 (0, platformID, 0), new Quaternion(0,0,180,1));
		} else {
			EZ_PoolManager.Spawn (boostPrefab, new Vector3 (2, platformID, 0), new Quaternion(0,0,180,1));
			EZ_PoolManager.Spawn (boostPrefab, new Vector3 (0, platformID, 0), new Quaternion(0,0,180,1));
		}
		platformID+=2;
	}

	void combo6(){
		platformID+=2;
		EZ_PoolManager.Spawn (boostComboLeft,new Vector3( 2.5f,platformID, 0),Quaternion.identity);
		platformID+=17;
	}

	void combo7(){
		platformID+=2;
		EZ_PoolManager.Spawn (boostComboRight,new Vector3( 2.5f,platformID, 0),Quaternion.identity);
		platformID+=17;
	}

	void combo8(){
		platformID++;
		EZ_PoolManager.Spawn (roundPlatform, new Vector3 (0, platformID, 0), Quaternion.identity);
		platformID += 7;
	}
	void combo9(){
		platformID++;
		EZ_PoolManager.Spawn (trambolineCombo, new Vector3 (0, platformID, 0), Quaternion.identity);
		platformID += 38;
	}
}
