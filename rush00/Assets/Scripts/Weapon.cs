using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public Sprite weapon;
	public Sprite weaponAttach;
	public Sprite shoot;

	public float speedFire;
	public int ammo;


	private bool coroutineRunning = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (coroutineRunning) {
			transform.Translate (Vector3.up * 15 * Time.deltaTime * -1);
			//transform.Rotate (Vector3.forward * 20);
			//transform.rotation = Quaternion.Euler (0f, 0f, 2000 * Time.deltaTime);
		}
	}

	void fire () {

	}

	public void drop (Quaternion rotation, Vector3 localPosition) {
		transform.localScale = new Vector3 (1, -1, 1);
		transform.rotation = rotation;
		transform.localPosition = localPosition;
		gameObject.SetActive (true);
		coroutineRunning = true;
		StartCoroutine (stopDropping (0.15f));
	}

	IEnumerator stopDropping (float toWait) {

		yield return new WaitForSeconds (toWait);
		coroutineRunning = false;
	}
}
