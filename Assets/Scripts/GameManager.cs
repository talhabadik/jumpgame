using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {
    public Text scoreText;
    public Text coinText;
    public Text coinMultiplierText;
    public Text dieCoinText;
    public Text dieScoreText;
    public Text totalCoinText;
    public Text highScoreText;
	public AudioSource fallaudio;
	public AudioMixer audioMix;
    public Image reviveHead;
    public Text keyCount;
    bool revived = false;
    public GameObject headstarts;
    public Button headstart100Button;
    public Button headstart250Button;
	public bool revieveScreen;
	public Player player;
	public static bool gameOver;
	public static bool gameStarted;
    

    public int score; //bu oyunun scoru
    int totalCoin;
	public float memCoinPitch;
	int dontChangedFrame;// coinlerin pitch'i nin kaç frame dir değişmediğidir.
	public static int coinEarned;
    public static int coinMultiplier;  //zamanla artıyor ve her coinin değerini arttırıyor

	// Use this for initialization
	void Start () {
        gameStarted = false;
        gameOver = false;
        coinEarned = 0;
        coinMultiplier = coinMultiplierCalculater(PlayerPrefs.GetInt("totalScore"));  //oyuncunun bütün scoreları toplamı "totalScore"
        headstart100Button.interactable = false;
        headstart250Button.interactable = false;
        PlayerPrefs.SetInt("headstart100", 100);
        PlayerPrefs.SetInt("headstart250", 250);
    }
	
	void Update () {
		if (getCoinPitch() != memCoinPitch) {
			dontChangedFrame = 0;
		} else {
			dontChangedFrame++;
		}

		if (dontChangedFrame > 20 && getCoinPitch () > 1f)
			audioMix.SetFloat ("coinPitch", 1);

		memCoinPitch = getCoinPitch ();

		if (gameStarted)
        {
            score = (int)Camera.main.transform.position.y;
            UIupdate();
        }

        if (Camera.main.transform.position.y > 5)
            headstarts.SetActive(false);

	}

    void UIupdate()
    // Oyun içi menüdeki UI'ı güncelliyor
    {
        scoreText.text = score.ToString()+"m";
        coinText.text = coinEarned.ToString();
        coinMultiplierText.text = coinMultiplier.ToString();
        if (PlayerPrefs.GetInt("headstart100") > 0)
            headstart100Button.interactable = true;
        if (PlayerPrefs.GetInt("headstart250") > 0)
            headstart250Button.interactable = true;
    }

    int coinMultiplierCalculater(int totalScore)
    // Oyuncunun bütün scoreları toplamını alarak coinMultiplier'ı bulur. 2nin üsleri * 100 score'da bir multiplier artar
    {
        int multiplier = 1;
        int count = 1;
        while (multiplier < totalScore / 100)
        {
            if (count < 100)
                count++;
            else
                break;
            multiplier *= 2;
        }
        PlayerPrefs.SetInt("coinMultiplier", count);
        return count;
    }

    public void restart()
    {
		SceneManager.LoadScene(0);
    }

    public void gameEnded()
    {
		revieveScreen = true;
        revived = false;
        gameOver = true;
        if (score > PlayerPrefs.GetInt("highScore"))
            PlayerPrefs.SetInt("highScore", score);
        PlayerPrefs.SetInt("totalScore", PlayerPrefs.GetInt("totalScore") + score);
        PlayerPrefs.SetInt("totalCoin", PlayerPrefs.GetInt("totalCoin") + coinEarned);

        dieCoinText.text = coinEarned.ToString();
        dieScoreText.text = score.ToString();
        totalCoinText.text = PlayerPrefs.GetInt("totalCoin").ToString();
        highScoreText.text = "Best: "+PlayerPrefs.GetInt("highScore").ToString();

        keyCount.text = PlayerPrefs.GetInt("reviveKey").ToString();
		fallaudio.Play ();
        reviveScreen();
    }

    public void reviveScreen()
    {
        
        if (!revived)
        {
            if (reviveHead.fillAmount != 0)
            {
                reviveHead.fillAmount -= 1 / 180f;
                Invoke("reviveScreen", 0.03f);
            }
            else
            {
				revieveScreen = false;
            }
        }
    }

    public void revive()
    {
        if (PlayerPrefs.GetInt("reviveKey") > 1)
        {
            PlayerPrefs.SetInt("reviveKey", PlayerPrefs.GetInt("reviveKey") - 2);
            revived = true;
			revieveScreen = false;
            reviveHead.fillAmount = 360;
            GameManager.gameOver = false;
            player.jetpackTotalFuel += 3;
            player.enableShield(3.5f);
            player.jetpackPickup();
            player.transform.position = new Vector3(0, Camera.main.transform.position.y - 3.9f, 0);
            player.characterSprite.transform.localScale = new Vector3(player.transform.localScale.x, 1, 1);
            fallaudio.Stop();
        }
        else {
            PlayerPrefs.SetInt("reviveKey", PlayerPrefs.GetInt("reviveKey") + 10);
            keyCount.text = PlayerPrefs.GetInt("reviveKey").ToString();
        }
    }

	public float getCoinPitch(){
		float value;
		bool result =  audioMix.GetFloat("coinPitch", out value);
		if(result){
			return value;
		}
        else{
			return 0f;
		}
	}

    public void headstart100()
    {
        player.jetpackTotalFuel += 1.8f;
        player.enableShield(2.5f);
        player.jetpackPickup();
        player.jumpSpeed = player.jumpSpeed * 2;
        PlayerPrefs.SetInt("headstart100", PlayerPrefs.GetInt("headstart100") - 1);
        player.headstarted = true;
    }

    public void headstart250()
    {
        player.jetpackTotalFuel += 5.8f;
        player.enableShield(6.5f);
        player.jetpackPickup();
        player.jumpSpeed = player.jumpSpeed * 2;
        PlayerPrefs.SetInt("headstart250", PlayerPrefs.GetInt("headstart250") - 1);
        player.headstarted = true;
    }

	public void tap(){
		if (!gameStarted) {
			gameStarted = true;
			player.onGameStart ();
		}
	}
}
