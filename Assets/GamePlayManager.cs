using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour {

	public Canvas playCanvas;
	public Canvas pauseCanvas;
	public Canvas gameoverCanvas;
	public Canvas gameclearCanvas;
	public SpriteRenderer gameoverSprite;
	public SpriteRenderer gameclearSprite;
	public SpriteRenderer pauseBG;
	public SpriteRenderer gameoverBG;

	public Text scoreArea;
	public Text hiArea;
	public Text hiScoreArea1;
	public Text hiScoreArea2;
	public Text hiScoreArea3;

	public GameObject EnemyPrefab;
	GameObject[] EnemyBombman;
	int numOfEnemy;

	bool playStates;
	bool gameFlag;
	float timeToClear;

	float score;

	void Start () {
		Time.timeScale = 1;
		pauseCanvas.enabled = false;
		gameoverCanvas.enabled = false;
		gameoverSprite.enabled = false;
		gameclearCanvas.enabled = false;
		gameclearSprite.enabled = false;
		gameoverBG.enabled = false;
		pauseBG.enabled = false;
		playStates = true;
		gameFlag = true;
		timeToClear = 0;
		score = 0.0f;

		numOfEnemy = PlayerPrefs.GetInt("optionkey", 10);
		LocateEnemys();
	}

	void Update () {
		// if (Input.touchCount > 0) {
		// 	Debug.Log("x座標: " + Input.touches[0].position.x);
		// 	Debug.Log("y座標: " + Input.touches[0].position.y);
		// }
		// else {
		// 	Debug.Log("width: " + Screen.width);
		// 	Debug.Log("height: " + Screen.height);
		// }

		if (numOfEnemy == 0 && gameFlag) {
			GameClear();
		}
		else if ((numOfEnemy > 0 || timeToClear != 0) && !gameFlag) {
			timeToClear += Time.deltaTime;

			if (timeToClear > 3) {
				GameOver();
			}
		}
		else {
			// Debug.Log(numOfEnemy);
		}

		if (gameFlag) {
			score += Time.deltaTime;
			if (score >= 1000000) {
				score = 999999.99f;
			}
			scoreArea.text = string.Format("Score: {0:f2}", score);
		}
	}

	public void ChangePauseState() {
		if (gameFlag) {
			if(playCanvas.enabled){
				Time.timeScale = 0;
				pauseCanvas.enabled = true;
				pauseBG.enabled = true;
				playCanvas.enabled = false;
				playStates = false;
			}
			else {
				Time.timeScale = 1;
				pauseCanvas.enabled = false;
				pauseBG.enabled = false;
				playCanvas.enabled = true;
				playStates = true;
			}
		}
	}

	public bool GetPlayStates() {
		return playStates;
	}

	public void GameDone() {
		gameFlag = false;
		timeToClear += 0.01f;
	}

	void GameClear() {
		playStates = false;
		gameFlag = false;
		if (Time.timeScale > 0) {
			Time.timeScale = 0;
			gameclearCanvas.enabled = true;
			gameclearSprite.enabled = true;
			ShowScore();
			Debug.Log("Game Clear!");
		}
	}

	void ShowScore() {
		int i;
		float hiscore;
		float tmp;
		string key;

		for (i = 0; i < 3; i++) {
			key = "ta_" + PlayerPrefs.GetInt("optionkey", 10) + "_" + i;
			hiscore = PlayerPrefs.GetFloat(key, 1000000);
			if (hiscore > score) {
				hiArea.text = "Hi Score !!!";
				WriteScore(i, score);
				PlayerPrefs.SetFloat(key, score);
				tmp = hiscore;
				for (i = i+1; i < 3; i++) {
					key = "ta_" + PlayerPrefs.GetInt("optionkey", 10) + "_" + i;
					hiscore = tmp;
					tmp = PlayerPrefs.GetFloat(key, 1000000);
					PlayerPrefs.SetFloat(key, hiscore);
					WriteScore(i, hiscore);
				}
				break;
			}
			else {
				WriteScore(i, hiscore);
			}
		}
		PlayerPrefs.Save();
	}

	void WriteScore(int num, float val) {
		switch (num) {
			case 0:
				if (val < 1000000){
					hiScoreArea1.text = string.Format("Score: {0:f2}", val);
				}
				else {
					hiScoreArea1.text = string.Format("Score: null");
				}
				break;
			case 1:
				if (val < 1000000){
					hiScoreArea2.text = string.Format("Score: {0:f2}", val);
				}
				else {
					hiScoreArea2.text = string.Format("Score: null");
				}
				break;
			case 2:
				if (val < 1000000){
					hiScoreArea3.text = string.Format("Score: {0:f2}", val);
				}
				else {
					hiScoreArea3.text = string.Format("Score: null");
				}
				break;
		}
	}

	void GameOver() {
		playStates = false;
		if (Time.timeScale > 0) {
			Time.timeScale = 0;
			gameoverCanvas.enabled = true;
			gameoverSprite.enabled = true;
			gameoverBG.enabled = true;
			Debug.Log("Game Over !!!");
		}
	}

	public void DiedEnemy() {
		numOfEnemy--;
	}


	void LocateEnemys() {
		int i;
		EnemyBombman = new GameObject[numOfEnemy];

		for (i = 0; i < numOfEnemy; i++) {
			EnemyBombman[i] = (GameObject)Instantiate(EnemyPrefab);
			EnemyBombman[i].transform.position = new Vector3(Random.Range(-2.3f, 2.3f), Random.Range(-3.5f, 3.5f), 0);
		}
	}

	public void RestartGame() {
		Application.LoadLevel("TAGamePlay");
	}
	public void GotoTitle() {
		Application.LoadLevel("Title");
	}
}
