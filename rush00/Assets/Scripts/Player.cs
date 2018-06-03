using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public GameObject sprites;
	public GameObject head;
	public GameObject weaponAttach;
	public GameObject body;
	public GameObject leg;
	public HUDManager hudManager;

	public AudioClip winSong = null;
	public AudioClip dieSong = null;

	public AudioSource audioSource = null;

	public static Player p = null;

	private Weapon weapon = null;

	public float speed;
	public bool invincible = false;

	private Animator legAnimator;
	private Vector2 direction;

	// Use this for initialization
	void Start () {
		if (p == null) {
			p = this;
		}
		legAnimator = leg.GetComponent<Animator> ();
		direction = Vector2.zero;
		//Object.DontDestroyOnLoad (gameObject);
	}

	void handleControls () {
		direction.x = Input.GetAxis ("Horizontal");
		direction.y = Input.GetAxis ("Vertical");

		if (Input.GetKeyDown (KeyCode.E)) {
			tryPickUpWeapon ();
		}

		if (Input.GetMouseButton (0) && weapon) {
			weapon.fire (sprites.transform.rotation, transform);
		}

		if (Input.GetMouseButtonDown (1) && weapon) {
			dropWeapon ();
		}
	}

	void makeTranslateAndAnimation () {
		gameObject.GetComponent<Rigidbody2D> ().velocity = direction * speed;
		gameObject.transform.rotation = new Quaternion (0, 0, 0, 0);

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
		changeAmmo();
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
				if (weapon) {
					dropWeapon ();
				}
				pickUpWeapon (target);
			}
		}
	}

	void pickUpWeapon (GameObject w) {
		weapon = w.GetComponent<Weapon> ();
		weapon.playerWeapon = true;
		weaponAttach.SetActive (true);
		weaponAttach.GetComponent<SpriteRenderer> ().sprite = weapon.weaponAttach;
		weapon.spriteRenderer.enabled = false;
		hudManager.ChangeWeapon(weapon.weaponName.ToString());
	}

	void dropWeapon () {
		weapon.drop (sprites.transform.rotation, transform.localPosition);
		weapon = null;
		weaponAttach.SetActive (false);
		hudManager.ChangeWeapon("No Weapon");
	}

	void changeAmmo() {
		if (weapon != null) {
			if (weapon.ammo <= 0) {
				hudManager.ChangeAmmo("No Minution");
			} else {
				hudManager.ChangeAmmo(weapon.ammo.ToString());				
			}
		} else {
			hudManager.ChangeAmmo("-");
		}
	}

	private void OnTriggerStay2D (Collider2D other) {
		transform.localRotation = new Quaternion (0, 0, 0, 0);
	}

	public void die () {
		// Debug.Log ("DIE");
		if (invincible) {
			return;
		}
		// UnityEngine.SceneManagement.SceneManager.LoadScene (
		// 	UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name);
		// Debug.Log ("reload");
		audioSource.PlayOneShot (dieSong);
		hudManager.Loose();
	}

	public void win () {
		audioSource.PlayOneShot (winSong);
		hudManager.Win();
		// Debug.Log ("WIN");
	}

}