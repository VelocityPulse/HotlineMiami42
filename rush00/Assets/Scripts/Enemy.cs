using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Vector3 _target;
	private GameObject _targetObject = null;
	private Weapon weapon;
	private bool _alive;
	private bool _alerted;
	private bool _search;
	private bool _see;
	

	[SerializeField] private Checkpoint _checkpoint = null;
	[SerializeField] private float speed;
	[SerializeField] private Room _room;

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
			_targetObject = _checkpoint.gameObject;
		} else {
			_targetObject = null;
		}

		int randomValue = Random.Range (0, weaponPrefabs.Count);
		weaponPrefabs [randomValue].transform.localPosition = transform.localPosition;
		weapon = Instantiate (weaponPrefabs [randomValue]).GetComponent<Weapon>();
		weapon.playerWeapon = false;
		weaponAttach.SetActive (true);
		// weaponAttach.GetComponent<SpriteRenderer> ().sprite = weapon.weaponAttach;
	}

	void FixedUpdate () {
		HandleDirection ();
		if (_targetObject != null) {
			if (_targetObject.layer == LayerMask.NameToLayer("Wall")) {
				_target = _targetObject.GetComponent<Renderer>().bounds.center;
			} else {
				_target = _targetObject.transform.position;
			}
		} else {
			_target = transform.position;
		}
		if (_alerted) {
			tryToFire ();
		}

		if (!_alerted && _checkpoint != null) {
			if (_target == transform.position) {
				_checkpoint = _checkpoint.nextCheckpoint;
			}
			_targetObject = _checkpoint.gameObject;
		} else if (Vector3.Distance(_target, transform.position) < 0.4) {
			// print(_targetObject.transform.position);
		print(Vector3.Distance(_target, transform.position));
			Door newDoor = _room.NextDoor(_target);
			if (_room.name == newDoor.room1.name) {
				_room = newDoor.room2;
			} else {
				_room = newDoor.room1;
			}
			newDoor = _room.NextDoor(_target);
			_targetObject = newDoor.gameObject;
		}

		// if (_search && _target == transform.position) {
		// 	// _target = _room.OtherDoor(_target);
		// 	Door newDoor = _room.NextDoor(_target);
		// 	if (_room.name == newDoor.room1.name) {
		// 		_room = newDoor.room2;
		// 	} else {
		// 		_room = newDoor.room1;
		// 	}
		// 	print("je cherche");
		// 	// donne un chemin au hasard
		// 	// _target = doorManager.NextDoor(_target);
		// 	// roomManager.OtherDoor(_target);
		// 	// FOUILLER ALEATOIREMENT
		// }
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
				if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
					_alerted = true;
					_search = false;
					// _targetObject = other.gameObject;
					_targetObject = null;
					_target = playerPos;
				}
				else if (_search && hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall")) {
					_search = true;
					print(Vector3.Distance(_target, transform.position));
					// print("NEXT DOOR");
					Door newDoor = _room.NextDoor(_target);
					_targetObject = newDoor.gameObject;
					// _target = newDoor.gameObject.GetComponent<Renderer>().bounds.center;
				}
			}
		} else if (!_search && other.gameObject.layer == LayerMask.NameToLayer("ProjectilePlayer")) {
			_alerted = true;
			_search = true;
			print(_targetObject.transform.position);
					// print("FIRST NEXT DOOR");
			Door newDoor = _room.NextDoor(other.gameObject.transform.position);
			_targetObject = newDoor.gameObject;
			// _target = other.gameObject.transform.position;
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
}
