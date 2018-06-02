﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public Sprite weapon;
	public Sprite weaponAttach;
	public GameObject shoot;

	public float speedFire;
	public int ammo;
	[HideInInspector] public bool playerWeapon;

	[HideInInspector] public SpriteRenderer spriteRenderer;

	private bool coroutineDropping = false;
	private bool coroutineFire = false;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {

		if (coroutineDropping) {
			transform.Translate (Vector3.up * 15 * Time.deltaTime * -1);
			//transform.Rotate (Vector3.forward * 20);
			//transform.rotation = Quaternion.Euler (0f, 0f, 2000 * Time.deltaTime);
		}
	}

	public void fire (Quaternion rotation, Transform parent) {
		if (ammo != 0 && !coroutineFire) {
			ammo--;
			GameObject newShoot = Instantiate (shoot,
						parent.localPosition + shoot.transform.localPosition,
						 rotation);
			if (!playerWeapon) {
				newShoot.layer = LayerMask.NameToLayer ("ProjectileEnemy");
			} else {
				newShoot.layer = LayerMask.NameToLayer ("ProjectilePlayer");
			}
			coroutineFire = true;
			StartCoroutine (waitForFire ());
		}
	}

	IEnumerator waitForFire () {
		yield return new WaitForSeconds (speedFire);
		coroutineFire = false;
	}

	public void drop (Quaternion rotation, Vector3 localPosition) {
		transform.localScale = new Vector3 (1, -1, 1);
		transform.rotation = rotation;
		transform.localPosition = localPosition;
		spriteRenderer.enabled = true;
		coroutineDropping = true;
		StartCoroutine (stopDropping (0.15f));
	}

	IEnumerator stopDropping (float toWait) {
		yield return new WaitForSeconds (toWait);
		coroutineDropping = false;
	}
}