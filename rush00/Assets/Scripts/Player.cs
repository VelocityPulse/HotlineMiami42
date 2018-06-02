using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public GameObject sprites;
	public GameObject head;
	public GameObject weaponAttach;
	public GameObject body;
	public GameObject leg;

	private Weapon weapon = null;

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

		if (Input.GetKeyDown (KeyCode.E)) {
			tryPickUpWeapon ();
		}

		if (Input.GetMouseButtonDown (0) && weapon) {
			weapon.fire ();
		}

		if (Input.GetMouseButtonDown(1) && weapon) {
			dropWeapon ();
		}

	}

	void makeTranslateAndAnimation () {
		transform.Translate (new Vector3 (direction.x, direction.y, 0)
							 * Time.deltaTime
							 * speed);

		if (direction != Vector2.zero) {
			legAnimator.Play ("legMoving");
		} else {
			legAnimator.Play ("idle");
		}
	}

	void handleDirection () {
		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		difference.Normalize ();
		float rotation_z = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		sprites.transform.rotation = Quaternion.Euler (0f, 0f, rotation_z + 90);
	}

	// Update is called once per frame
	void Update () {
		handleControls ();
		handleDirection ();
		makeTranslateAndAnimation ();
	}

	private void tryPickUpWeapon () {
		RaycastHit2D hit;
		GameObject target;
		hit = Physics2D.Raycast (transform.localPosition,
								  Vector2.up,
								  0,
								  LayerMask.GetMask ("Weapon"));

		if (hit) {
			target = hit.collider.gameObject;
			if (target.tag == "Weapon" &&
				Vector3.Distance (target.transform.localPosition, transform.localPosition) < 0.4) {
				pickUpWeapon (target);
			}
		}
	}

	void pickUpWeapon (GameObject w) {
		weapon = w.GetComponent<Weapon> ();
		weaponAttach.SetActive (true);
		weaponAttach.GetComponent<SpriteRenderer> ().sprite = weapon.weaponAttach;
		weapon.gameObject.SetActive(false);
	}

	void dropWeapon() {
		weapon.drop (sprites.transform.rotation, transform.localPosition);
		weapon = null;
		weaponAttach.SetActive (false);
	}
}