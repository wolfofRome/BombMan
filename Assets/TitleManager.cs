using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

	public Text BlinkText;

	public Canvas tc;
	public Canvas sgc;
	public Canvas tac;
	public Canvas sac;
	public Canvas settingc;

	float time;

	void Start () {
		time = 0.0f;
		sgc.enabled = false;
		tac.enabled = false;
		sac.enabled = false;
		settingc.enabled = false;

		Time.timeScale = 1;
	}

	void Update () {
		time += Time.deltaTime;

		if (time >= 0.8f) {
			BlinkText.enabled = !BlinkText.enabled;
			time = 0.0f;
		}

		if (tc.enabled && Input.GetMouseButtonDown (0)) {
			if (Input.touchCount > 0 &&
				Input.touches[0].position.x < Screen.width * 0.85 &&
				Input.touches[0].position.y > Screen.height * 0.12) {
				tc.enabled = false;
				tc.gameObject.SetActive(false);
				sgc.enabled = true;
			}
		}
	}

	public void SelectTA() {
		sgc.enabled = false;
		tac.enabled = true;
	}

	public void SelectSA() {
		sgc.enabled = false;
		sac.enabled = true;
	}

	public void StartTA(int num) {
		PlayerPrefs.SetInt("optionkey", num);
		PlayerPrefs.Save();
		Application.LoadLevel("TAGamePlay");
	}

	public void StartSA(int num) {
		PlayerPrefs.SetInt("optionkey", num);
		PlayerPrefs.Save();
		// Application.LoadLevel("SAGamePlay");
		Application.LoadLevel("TAGamePlay");
	}

	public void showSetting() {
		settingc.enabled = true;
		tc.enabled = false;
		tc.gameObject.SetActive(false);
	}

	public void ResetScore() {

		// bool b = false;

		// DialogManager.Instance.SetLabel("Yes", "No", "Close");
		// DialogManager.Instance.ShowSelectDialog(
		// 	"注意",
		// 	"データを全て初期化してもよろしいですか？",
		// 	(bool result) => {
		// 		b = result;
		// 		// Debug.Log("result:" + result.ToString());
		// 	}
		// );

		// if (b) {
		// 	PlayerPrefs.DeleteAll();
		// 	DialogManager.Instance.ShowSubmitDialog(
		// 		"初期化完了",
		// 		"データを初期化しました。",
		// 		(bool result) => {
		// 			// Debug.Log ("submited!");
		// 		}
		// 	);
		// }

		// bool b = UnityEditor.EditorUtility.DisplayDialog("注意", "データを全て初期化してもよろしいですか？", "Yes", "No");
		// if (b) {
			PlayerPrefs.DeleteAll();
		// 	UnityEditor.EditorUtility.DisplayDialog("初期化完了", "データを初期化しました。", "OK");
		// }

		BackToTitle();
	}

	public void BackToTitle() {
		settingc.enabled = false;
		tc.enabled = true;
		tc.gameObject.SetActive(true);
	}
}
