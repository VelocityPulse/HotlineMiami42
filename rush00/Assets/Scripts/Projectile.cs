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

	private void OnCollisionEnter2D (Collision2D collision) {
		Debug.Log ("COLLISION");
		if (collision.gameObject.layer == LayerMask.GetMask ("Wall")) {
			Destroy (gameObject);
		}

		Debug.Log (tag);
		Debug.Log (collision.collider.gameObject.layer);
		Debug.Log (LayerMask.GetMask ("Ennemy"));

		if (tag == "Player" && collision.gameObject.layer == LayerMask.NameToLayer ("Ennemy")) {
			Destroy (collision.gameObject);
		} else if (tag == "Ennemy" && collision.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			collision.gameObject.GetComponent<Player> ().die ();
		}
	}
}
