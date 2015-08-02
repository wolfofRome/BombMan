using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	float rtime;
	PlayerBombman ParentPObj;
	EnemyBombman ParentEObj;
	public GameObject ExplosionPrefab;
	GameObject Explosion;

	void Start() {
		rtime = 3.0f;
		Explosion = new GameObject();
	}

	void Update() {
		rtime -= Time.deltaTime;
		if (rtime < 0) {
			DestroyMySelf();
		}
	}

	public void Init(PlayerBombman p) {
		ParentPObj = p;
	}
	public void Init(EnemyBombman p) {
		ParentEObj = p;
	}

	void DestroyMySelf() {
		Explosion = (GameObject)Instantiate(ExplosionPrefab);

		ExplosionManager em = Explosion.GetComponent<ExplosionManager>();
		em.Init(this.GetComponent<Rigidbody>().position);

		if (ParentPObj) {
			ParentPObj.DestroyedBomb();
		}
		else if (ParentEObj) {
			ParentEObj.DestroyedBomb();
		}
		Destroy(this.gameObject);
	}

	void OnParticleCollision(GameObject obj) {
		rtime = -1;
	}
}
