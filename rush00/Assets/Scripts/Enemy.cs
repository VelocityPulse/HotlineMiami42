﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Vector3 _target;
	private GameObject _targetObject = null;

	private Weapon weapon;
	private bool _alerted;
	private bool _search;

	[Header ("leave empty for random checkpoint")]
	public Checkpoint _checkpoint = null;


	[SerializeField] private float speed;
	[SerializeField] private Room _room;
	public List<GameObject> weaponPrefabs;
	public GameObject sprites;
	public GameObject head;
	public GameObject weaponAttach;
	public GameObject body;
	public GameObject leg;

	public List<AudioClip> dieSong = new List<AudioClip> ();

	private Animator legAnimator;


	void Start () {
		_target = transform.position;
		_alerted = false;
		_search = false;


		if (_checkpoint != null) {
			leg.GetComponent<Animator> ().Play ("legMoving");
			_targetObject = _checkpoint.gameObject;
		} else {
			_targetObject = null;
		}

		if (!_checkpoint) {
			_checkpoint = _room.getRandomCheckPoint ();
		}

		int randomValue = Random.Range (0, weaponPrefabs.Count);

		weapon = Instantiate (weaponPrefabs [randomValue], transform).GetComponent<Weapon> ();
		weapon.GetComponent<Rigidbody2D> ().simulated = false;
		weapon.GetComponent<SpriteRenderer> ().enabled = false;
		weapon.playerWeapon = false;
		weapon.ammo = -1;
		weaponAttach.SetActive (true);

		legAnimator = leg.GetComponent<Animator> ();
	}

	void FixedUpdate () {
		HandleAnimation ();
		HandleDirection ();
		HandleTarget ();
		transform.position = Vector3.MoveTowards (transform.position, _target, speed * Time.deltaTime);
		// DebugPrint ();
	}

	void HandleAnimation () {
		if (transform.position == _target) {
			legAnimator.Play ("idle");
		} else {
			legAnimator.Play ("legMoving");
		}
	}

	void DebugPrint () {
		Debug.Log ("PRINT ------------------------------");
		Debug.Log ("_target  : " + _target);
		if (_targetObject) {
			Debug.Log ("_targetObject : " + _targetObject.name);
		} else {
			Debug.Log ("_targetObject : null");
		}
		Debug.Log ("_alerted : " + _alerted);
		Debug.Log ("_search  : " + _search);
	}

	void HandleTarget () {
		if (_targetObject != null) {
			_target = _targetObject.transform.position;
		} else if (!_alerted) {
			_target = transform.position;
		}
		if (_alerted && !_search) {
			legAnimator.Play ("legMoving");
			Fire ();
		}
		if (_alerted && _search && !_targetObject) {
			Door newDoor = _room.NextDoor (_target);
			_targetObject = newDoor.gameObject;
		}

		if (!_alerted && _checkpoint != null) {
			if (_target == transform.position) {
				_checkpoint = _checkpoint.nextCheckpoint;
			}
			_targetObject = _checkpoint.gameObject;
		} else if (_alerted && Vector3.Distance (_target, transform.position) < 0.05) {
			DoorGestion ();
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (_alerted) {
				StartCoroutine ("StopFollowPlayer");
				_search = true;
			}
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		transform.rotation = Quaternion.Euler (0, 0, 0);

		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			// TODO passer les portes !
			Vector3 playerPos = other.gameObject.transform.position;
			Vector3 dir = (playerPos - transform.position).normalized;
			RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, Mathf.Infinity, LayerMask.GetMask ("Player", "Wall"));
			if (hit) {
				if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Player")) {
					_alerted = true;
					_search = false;
					_targetObject = null;
					_target = playerPos;
				} else if (_search && hit.collider.gameObject.layer == LayerMask.NameToLayer ("Wall")) {
					_alerted = true;
					_search = true;
					Door newDoor = _room.NextDoor (_target);
					_targetObject = newDoor.gameObject;
				}
			}
		} else if (!_search && other.gameObject.layer == LayerMask.NameToLayer ("ProjectilePlayer")) {
			_alerted = true;
			_search = true;
			Door newDoor = _room.NextDoor (other.gameObject.transform.position);
			_targetObject = newDoor.gameObject;
		}
	}

	private void HandleDirection () {
		Vector3 difference = _target - transform.position;
		difference.Normalize ();
		float rotation_z = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		sprites.transform.rotation = Quaternion.Euler (0f, 0f, rotation_z + 90);
	}

	private IEnumerator StopFollowPlayer () {
		yield return new WaitForSeconds (10);
		_alerted = false;
		_search = false;
		_checkpoint = _room.getRandomCheckPoint ();
	}

	void Fire () {
		weapon.fire (sprites.transform.rotation, transform);
	}

	private void DoorGestion () {
		if (_targetObject != null) {
			if (_room.name == _targetObject.GetComponent<Door> ().room1.name) {
				_room = _targetObject.GetComponent<Door> ().room2;
			} else {
				_room = _targetObject.GetComponent<Door> ().room1;
			}
			_targetObject = _room.OtherDoor (_targetObject.GetComponent<Door> ()).gameObject;
		}

		// CAS CUL DE SAC A GERER SAC A MERDE
		// Door newDoor = _room.OtherDoor(_targetObject.GetComponent<Door>());
	}

	public void die () {
		if (Player.p) {
			Player.p.audioSource.PlayOneShot (dieSong[Random.Range(0, dieSong.Count)]);
		}
		Destroy (gameObject);
	}
}
