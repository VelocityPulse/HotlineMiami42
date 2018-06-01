﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public GameObject head;
	public GameObject body;
	public GameObject leg;

	public float speed;

	private Animator legAnimator;
	private Vector2 direction;


	// Use this for initialization
	void Start () {
		legAnimator = leg.GetComponent<Animator> ();
		direction = Vector2.zero;
	}

	void handleControls () {
		if (Input.GetKeyDown (KeyCode.A)) {
			direction += Vector2.left;
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			direction += Vector2.right;
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			direction += Vector2.down;
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			direction += Vector2.up;
		}

		if (Input.GetKeyUp (KeyCode.A)) {
			direction -= Vector2.left;
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			direction -= Vector2.right;
		}
		if (Input.GetKeyUp (KeyCode.S)) {
			direction -= Vector2.down;
		}
		if (Input.GetKeyUp (KeyCode.W)) {
			direction -= Vector2.up;
		}
	}


	// Update is called once per frame
	void Update () {

		handleControls ();

		transform.Translate (new Vector3 (direction.x, direction.y, 0)
							 * Time.deltaTime
							 * speed);

		if (direction != Vector2.zero) {
			legAnimator.Play("legMoving");
		} else {
			Debug.Log ("idle");
			legAnimator.Play ("idle");
		}


	}
}
