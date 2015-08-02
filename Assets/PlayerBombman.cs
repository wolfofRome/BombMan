using UnityEngine;
using System.Collections;

public class PlayerBombman : MonoBehaviour {

	public GamePlayManager gpm;
	Animator animator;
	float PlayerSpeed= 0.03f;
	Rigidbody bman_3d;
	int animationStates;
	bool playStates;
	Vector2 v_tmp;
	Vector3 acce;

	public GameObject BombPrefab;
	const int BOMB_NUM = 3;
	GameObject[] Bomb = new GameObject[BOMB_NUM];
	int numOfBomb;

	public GameObject DiePulsePrefab;
	GameObject DiePulse;

	void Start() {
		animator = GetComponent<Animator>();
		bman_3d = GetComponent<Rigidbody>();
		animationStates = 3;
		playStates = true;
		numOfBomb = 0;
	}

	void Update() {

		if (gpm.GetPlayStates()) {
			if (!playStates) {
				playStates = true;
				// ポーズ前の速度に戻す
				bman_3d.velocity = v_tmp;
			}

			// 加速度センサーの値に応じて力をかける
			acce = Input.acceleration;

			bman_3d.AddForce(new Vector3(acce.x / 5.0f, acce.y / 5.0f, 0.0f), ForceMode.Impulse);

			// もっとも力のかかっている方向に応じてアニメーション変更
			if (Mathf.Abs(acce.x) >= Mathf.Abs(acce.y)) {
				if (acce.x > 0) {
					animator.Play ("Bombman_Right_Move");
					animationStates = 2;
				}
				else {
					animator.Play ("Bombman_Left_Move");
					animationStates = 4;
				}
			}
			else {
				if (acce.y > 0) {
					animator.Play ("Bombman_Back_Move");
					animationStates = 1;
				}
				else {
					animator.Play ("Bombman_Front_Move");
					animationStates = 3;
				}
			}

			// 上下左右キーで移動
			if (Input.GetKey (KeyCode.RightArrow)) {
				transform.Translate (transform.right *PlayerSpeed);
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				transform.Translate (transform.right *-PlayerSpeed);
			}
			if (Input.GetKey (KeyCode.UpArrow)) {
				transform.Translate (transform.up*PlayerSpeed);
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				transform.Translate (transform.up *-PlayerSpeed);
			}

			//上下左右キー入力でアニメーション変更
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				animator.Play ("Bombman_Back_Move");
			}
			else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				animator.Play ("Bombman_Right_Move");
			}
			else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				animator.Play ("Bombman_Front_Move");
			}
			else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				animator.Play ("Bombman_Left_Move");
			}

			// // 画面がタッチされたら
			if (Input.touchCount > 0 && Input.touches[0].position.y > 100 && numOfBomb < BOMB_NUM) {
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began) {
					Bomb[numOfBomb] = (GameObject)Instantiate(BombPrefab);
					Vector2 pos = bman_3d.position;
					pos.y -= 0.2f;
					Bomb[numOfBomb].transform.position = pos;
					Bomb b = Bomb[numOfBomb].GetComponent<Bomb>();
					b.Init(this);
					numOfBomb++;
				}
			}

		}
		else {
			if (playStates) {
				playStates = false;
				// ポーズ前の速度を保存
				v_tmp = bman_3d.velocity;
				// 速度を0にする
				bman_3d.velocity = new Vector3(0, 0, 0);
				switch (animationStates) {
					case 1:
						animator.Play("Bombman_Back_Idle");
						break;
					case 2:
						animator.Play("Bombman_Right_Idle");
						break;
					case 3:
						animator.Play("Bombman_Front_Idle");
						break;
					case 4:
						animator.Play("Bombman_Left_Idle");
						break;
				}
			}
		}
	}

	public void DestroyedBomb() {
		numOfBomb--;
	}
	void DieBombman() {
		Vector3 pos = this.transform.position;
		DiePulse = (GameObject)Instantiate(DiePulsePrefab);
		DiePulse.transform.position = pos;
		Destroy(this.gameObject);
		gpm.GameDone();
	}

	void OnParticleCollision(GameObject obj) {
		DieBombman();
	}
}
