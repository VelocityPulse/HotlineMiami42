using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public bool coldWeapon = false;

	private bool coroutineKillProjectile;

	// Use this for initialization
	void Start () {
		if (!coldWeapon) {
			StartCoroutine (killProjectile(3));
		} else {
			StartCoroutine (killProjectile (0.04f));
		}
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.up * 30 * Time.deltaTime * -1);
	}

	IEnumerator killProjectile (float timer) {
		yield return new WaitForSeconds (timer);
		Destroy (gameObject);
	}
}
