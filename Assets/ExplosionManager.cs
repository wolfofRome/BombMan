using UnityEngine;
using System.Collections;

public class ExplosionManager : MonoBehaviour {

	ParticleSystem[] ps;
	float rtime;

	void Start () {
		ps = this.transform.GetComponentsInChildren<ParticleSystem>();
		ps[0].Play();
		ps[1].Play();
		ps[2].Play();
		ps[3].Play();

		rtime = ps[0].duration;
	}

	void Update () {
		rtime -= Time.deltaTime;
		if (rtime < 0) {
			ps[0].emissionRate = 0;
			ps[1].emissionRate = 0;
			ps[2].emissionRate = 0;
			ps[3].emissionRate = 0;
			Destroy(this.gameObject, 0.5f);
		}
	}

	public void Init(Vector3 _pos) {
		this.transform.position = _pos;
	}
}
