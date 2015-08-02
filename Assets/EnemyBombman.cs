using UnityEngine;
using System.Collections;

public class EnemyBombman : MonoBehaviour {
	GamePlayManager gpm;
	Animator animator;
	Rigidbody enemy_3d;
	int animationStates;
	bool playStates;
	Vector2 v_tmp;
	bool deadFlag;

	public GameObject BombPrefab;
	const int BOMB_NUM = 3;
	GameObject[] Bomb = new GameObject[BOMB_NUM];
	int numOfBomb;

	// 自動化のための変数
	int BombFreq;
	Vector3 velo;
	bool veloH;
	bool veloV;


	void Start() {
		gpm = FindObjectOfType<GamePlayManager>();
		animator = GetComponent<Animator>();
		enemy_3d = GetComponent<Rigidbody>();
		animationStates = 3;
		playStates = true;
		deadFlag = false;
		numOfBomb = 0;

		BombFreq = Random.Range(250, 300);
		// veloH = (Random.Range(0, 2) == 0) ? true : false;
		// veloV = (Random.Range(0, 2) == 0) ? true : false;

		velo = new Vector3();
		velo.x = (Random.Range(0, 2) == 0) ? 0.5f : -0.5f;
		velo.y = (Random.Range(0, 2) == 0) ? 0.5f : -0.5f;
	}

	void Update() {
		if (gpm.GetPlayStates() && !deadFlag) {
			if (!playStates) {
				playStates = true;
				// ポーズ前の速度に戻す
				enemy_3d.velocity = v_tmp;
			}


			// 速度の変更
			// Vector3 velo = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
			if (Random.value < 0.01) {
				velo.x = velo.x * -1;
			}
			if (Random.value < 0.01) {
				velo.y = velo.y * -1;
			}

			velo.x += Random.Range(-0.1f, 0.1f);
			velo.y += Random.Range(-0.1f, 0.1f);
			if (velo.x > 1) {
				velo.x = 1;
			}
			else if (velo.x < -1) {
				velo.x = -1;
			}
			else if (velo.y > 1) {
				velo.y = 1;
			}
			else if (velo.y < -1) {
				velo.y = -1;
			}
			enemy_3d.AddForce(new Vector3(velo.x / 5.0f, velo.y / 5.0f, 0.0f), ForceMode.Impulse);

			// もっとも力のかかっている方向に応じてアニメーション変更
			if (Mathf.Abs(velo.x) >= Mathf.Abs(velo.y)) {
				if (velo.x > 0) {
					animator.Play ("Enemy_Right_Move");
					animationStates = 2;
				}
				else {
					animator.Play ("Enemy_Left_Move");
					animationStates = 4;
				}
			}
			else {
				if (velo.y > 0) {
					animator.Play ("Enemy_Back_Move");
					animationStates = 1;
				}
				else {
					animator.Play ("Enemy_Front_Move");
					animationStates = 3;
				}
			}

			// ランダム
			if (numOfBomb < BOMB_NUM && Random.Range(1, BombFreq) == 1) {
					Bomb[numOfBomb] = (GameObject)Instantiate(BombPrefab);
					Vector2 pos = enemy_3d.position;
					pos.y -= 0.2f;
					Bomb[numOfBomb].transform.position = pos;
					Bomb b = Bomb[numOfBomb].GetComponent<Bomb>();
					b.Init(this);
					numOfBomb++;
			}
		}
		else {
			if (playStates) {
				playStates = false;
				// ポーズ前の速度を保存
				v_tmp = enemy_3d.velocity;
				// 速度を0にする
				enemy_3d.velocity = new Vector3(0, 0, 0);
				StopAnimation();
			}
		}
	}

	public void DestroyedBomb() {
		numOfBomb--;
	}
	void DieBombman() {
		if (!deadFlag) {
			gpm.DiedEnemy();
		}

		deadFlag = true;
		this.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f);
		enemy_3d.velocity = new Vector3(0, 0, 0);
		enemy_3d.constraints = RigidbodyConstraints.FreezeAll;
		Destroy(this.gameObject, 2.0f);
	}

	void StopAnimation() {
		switch (animationStates) {
			case 1:
				animator.Play("Enemy_Back_Idle");
				break;
			case 2:
				animator.Play("Enemy_Right_Idle");
				break;
			case 3:
				animator.Play("Enemy_Front_Idle");
				break;
			case 4:
				animator.Play("Enemy_Left_Idle");
				break;
		}
	}

	void OnParticleCollision(GameObject obj) {
		DieBombman();
	}

	void OnCollisionEnter(Collision collision) {
		int c;
		string opName;
		bool collision_x = false;
		bool collision_y = false;

		for (c=0; c < collision.contacts.Length; c++) {
			opName = collision.gameObject.name;

			if (opName == "WallTop" || opName == "WallBottom") {
				collision_y = true;
			}
			else if (opName == "WallRight" || opName == "WallLeft") {
				collision_x = true;
			}
			else {
				collision_x = true;
				collision_y = true;
			}
		}

		if (collision_x) {
			velo.x = velo.x * -1;
		}
		if (collision_y) {
			velo.y = velo.y * -1;
		}
	}
}
