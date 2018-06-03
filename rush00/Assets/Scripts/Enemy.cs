using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Vector3 _target;
	private Weapon weapon;
	private bool _alive;
	private bool _alerted;

	[SerializeField] private Checkpoint _checkpoint = null;
	[SerializeField] private float speed;

	public List<GameObject> weaponPrefabs;
	public GameObject sprites;
	public GameObject head;
	public GameObject weaponAttach;
	public GameObject body;
	public GameObject leg;
	public bool moving;

	void Start () {
		_target = transform.position;
		_alerted = false;
		if (moving) {
			leg.GetComponent<Animator> ().Play ("legMoving");
		}

		int randomValue = Random.Range (0, weaponPrefabs.Count);
		weaponPrefabs [randomValue].transform.localPosition = transform.localPosition;
		weapon = Instantiate (weaponPrefabs [randomValue]).GetComponent<Weapon>();
		weapon.GetComponent<SpriteRenderer> ().enabled = false;
		weapon.playerWeapon = false;
		weaponAttach.SetActive (true);
		weaponAttach.GetComponent<SpriteRenderer> ().sprite = weapon.weaponAttach;
	}

	void FixedUpdate () {
		HandleDirection ();
		if (_alerted) {
			// tryToFire ();
		}
// if (Input.GetMouseButtonDown(0)) {
// 				_target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
// 				_target.z = transform.position.z;
// }

		if (!_alerted && _checkpoint != null) {
			if (_target == transform.position) {
				_checkpoint = _checkpoint.nextCheckpoint;
			}
			_target = _checkpoint.transform.position;
		}
		// else if (_alerted && _target == transform.position) {
		// 	_target = new Vector3(Random.Range(transform.position.x -2, transform.position.x + 2), Random.Range(transform.position.y -2, transform.position.y + 2));
		// }

		transform.position = Vector3.MoveTowards (transform.position, _target, speed * Time.deltaTime);
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (_alerted) {
				_alerted = false;
				// StartCoroutine("StopFollowPlayer");
			}
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		transform.rotation = Quaternion.Euler (0, 0, 0);

		if (other.gameObject.tag == "Player") {
			// TODO passer les portes !
			Vector3 playerPos = other.gameObject.transform.position;
			if (_alerted) {
				_target = playerPos;
			} else {

				Vector3 dir = (playerPos - transform.position).normalized;
				RaycastHit2D hit = Physics2D.Raycast (transform.localPosition, dir, Mathf.Infinity, LayerMask.GetMask ("Player", "Wall"));
				if (hit) {
					if (hit.collider.gameObject.layer == 11) {
						_alerted = true;
						_target = playerPos;
					}
				}
			}
		}
	}

	private void HandleDirection () {
		Vector3 difference = _target - transform.position;
		difference.Normalize ();
		float rotation_z = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		sprites.transform.rotation = Quaternion.Euler (0f, 0f, rotation_z + 90);
	}

	private IEnumerator StopFollowPlayer () {
		yield return new WaitForSeconds (2);
		_alerted = false;
	}

	void tryToFire() {
		
		fire ();
	}

	void fire () {
		weapon.fire (sprites.transform.rotation, transform);
	}

}
