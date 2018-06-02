using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Vector3 _target;
	private bool _alive;
	private bool _alerted;
	[SerializeField] private Checkpoint _checkpoint;
	[SerializeField] private float speed;
	// [SerializeField] private Weapon _weapon;

	public GameObject sprites;
	

	void Start () {
		_target = transform.position;
		_alerted = false;
	}
	
	void FixedUpdate () {
		HandleDirection();
		
		if (!_alerted && _checkpoint != null) {
			if (_target == transform.position) {
				_checkpoint = _checkpoint.nextCheckpoint;
			}
			_target = _checkpoint.transform.position;			
		} 
		// else if (_alerted && _target == transform.position) {
		// 	_target = new Vector3(Random.Range(transform.position.x -2, transform.position.x + 2), Random.Range(transform.position.y -2, transform.position.y + 2));
		// }

		transform.position = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);
	}

	void OnCollisionEnter2D(Collision2D other) {
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (_alerted) {
				print("ARRETE");
				_alerted = false;
				// StartCoroutine("StopFollowPlayer");
			}
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		transform.rotation = Quaternion.Euler(0, 0, 0);

		if (other.gameObject.tag == "Player") {
			// TODO passer les portes !
			Vector3 playerPos = other.gameObject.transform.position;
			if (_alerted) {
				_target = playerPos;
			} else {

				Vector3 dir = (playerPos - transform.position).normalized;
				RaycastHit2D hit = Physics2D.Raycast(transform.localPosition, dir, Mathf.Infinity, LayerMask.GetMask ("Player", "Wall"));
				if (hit) {
					if (hit.collider.gameObject.layer == 11) {
						_alerted = true;
						_target = playerPos;
					} 
				}
			}
		}
	}

	private void HandleDirection() {
		Vector3 difference = _target - transform.position;
		difference.Normalize ();
		float rotation_z = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		sprites.transform.rotation = Quaternion.Euler (0f, 0f, rotation_z + 90);
	}

	private IEnumerator StopFollowPlayer() {
		yield return new WaitForSeconds(2);
		_alerted = false;
	}
}
