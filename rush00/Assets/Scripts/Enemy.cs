using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Vector3 _target;
	private Weapon weapon;
	private bool _alive;
	private bool _alerted;
	private bool _search;
	private bool _see;
	

	[SerializeField] private Checkpoint _checkpoint = null;
	[SerializeField] private float speed;

	public List<GameObject> weaponPrefabs;
	public GameObject sprites;
	public GameObject head;
	public GameObject weaponAttach;
	public GameObject body;
	public GameObject leg;


	void Start () {
		_target = transform.position;
		_alerted = false;
		_search = false;
		_see = false;
		// _alive = true;
		if (_checkpoint != null) {
			leg.GetComponent<Animator>().Play("legMoving");
		}

		int randomValue = Random.Range (0, weaponPrefabs.Count);
		weaponPrefabs [randomValue].transform.localPosition = transform.localPosition;
		weapon = Instantiate (weaponPrefabs [randomValue]).GetComponent<Weapon>();
		weapon.playerWeapon = false;
		weaponAttach.SetActive (true);
		weaponAttach.GetComponent<SpriteRenderer> ().sprite = weapon.weaponAttach;
	}

	void FixedUpdate () {
		HandleDirection ();
		if (_alerted) {
			tryToFire ();
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
		// Je cherche je dois aller a la porte suivante
		if (_search && _target == transform.position) {
			print("je cherche");
			// donne un chemin au hasard
			// _target = doorManager.NextDoor(_target);
			roomManager.OtherDoor(_target);
			// FOUILLER ALEATOIREMENT
		}
		// else if (_alerted && _target == transform.position) {
		// 	_target = new Vector3(Random.Range(transform.position.x -2, transform.position.x + 2), Random.Range(transform.position.y -2, transform.position.y + 2));
		// }

		// if (_search) {
		// 	_target = 
		// }

		transform.position = Vector3.MoveTowards (transform.position, _target, speed * Time.deltaTime);
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (_alerted) {
				StartCoroutine("StopFollowPlayer");
			}
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		transform.rotation = Quaternion.Euler (0, 0, 0);

		if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
			// TODO passer les portes !
			Vector3 playerPos = other.gameObject.transform.position;
			Vector3 dir = (playerPos - transform.position).normalized;
			RaycastHit2D hit = Physics2D.Raycast (transform.localPosition, dir, Mathf.Infinity, LayerMask.GetMask ("Player", "Wall"));
			if (hit) {
				// Je vois le player
				if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
					_alerted = true;
					_search = false;
					_target = playerPos;
				}
				// Je l'ai entendu
				else if (_search && hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall")) {
					_search = true;
					// _target = doorManager.NextDoor(_target);
				}
			}
		// Si il y a un projectile (je l'entends) et si je le cherche pas deja
		} else if (!_search && other.gameObject.layer == LayerMask.NameToLayer("ProjectilePlayer")) {
			_alerted = true;
			_search = true;
			_target = other.gameObject.transform.position;
		}
	}

	private void HandleDirection () {
		Vector3 difference = _target - transform.position;
		difference.Normalize ();
		float rotation_z = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		sprites.transform.rotation = Quaternion.Euler (0f, 0f, rotation_z + 90);
	}

	private IEnumerator StopFollowPlayer () {
		yield return new WaitForSeconds(5);
		_alerted = false;
		_search = false;
	}

	void tryToFire() {
		
		fire ();
	}

	void fire () {
		weapon.fire (sprites.transform.rotation, transform);
	}

	// public void ChangeRoomManager(GameObject newRoomManager) {
	// 	roomManager = newRoomManager.GetComponent<RoomManager>();
	// }

}
