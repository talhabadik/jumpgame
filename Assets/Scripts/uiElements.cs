using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class uiElements : MonoBehaviour {

	public GameManager gameMan;
	public Text resumeCounterText; //pause dan çıkıncaki 3 saniyelik sayaç
	public GameObject pauseScreen;
	public Animator mainMenuAnim;
	public Animator revieveMenuAnim;
	public Animator dieMenuAnim;
	public Animator gameMenuAnim;
	float resumeCounter = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		mainMenuAnim.SetBool ("gameStarted", GameManager.gameStarted);
		gameMenuAnim.SetBool ("gameStarted", GameManager.gameStarted);
		revieveMenuAnim.SetBool ("revieveMenu", gameMan.revieveScreen);
		dieMenuAnim.SetBool ("revieveMenu", gameMan.revieveScreen);
		dieMenuAnim.SetBool ("gameOver", GameManager.gameOver);
		gameMenuAnim.SetBool ("gameOver", GameManager.gameOver);

		if (resumeCounter != 0) {
			resumeCounter -= Time.fixedDeltaTime;
			resumeCounterText.text = Mathf.Ceil (resumeCounter).ToString ();
		}
		if (resumeCounter < 0) {
			resumeCounter = 0;
			resumeCounterText.gameObject.SetActive (false);
			Time.timeScale = 1f;
			pauseScreen.SetActive (false);
		}
	}

	public void pause(){
		Debug.Log ("paused...");
		Time.timeScale = 0f;
	}
	public void resume(){
		Debug.Log ("resumed...");
		resumeCounter = 3;
	}
}