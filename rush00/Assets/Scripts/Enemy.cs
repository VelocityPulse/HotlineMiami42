using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Vector3 _target;
	private bool _alive;
	private bool _alerted;
	// private float _angle;
	[SerializeField] private Checkpoint _checkpoint;
	[SerializeField] private float speed;
	// [SerializeField] private Weapon _weapon;

	public GameObject sprites;
	

	void Start () {
		// TODO remettre a 0 la rotation du collider de l'ennemi aussi

		_target = transform.position;
		_alerted = false;
		// _angle = AngleBetweenVector2(transform.position, _target);		
		// speed = 5;
	}
	
	void FixedUpdate () {


		// _angle = AngleBetweenVector2(transform.position, _target);
		HandleDirection();
		
		// if (Input.GetMouseButtonDown(0)) {
		// 	_target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// 	_target.z = transform.position.z;
		// }
		if (!_alerted) {
			if (_target == transform.position) {
				_checkpoint = _checkpoint.nextCheckpoint;
			}
			_target = _checkpoint.transform.position;			
		} 
		// else if (_alerted && _target == transform.position) {
		// 	_target = new Vector3(Random.Range(transform.position.x -2, transform.position.x + 2), Random.Range(transform.position.y -2, transform.position.y + 2));
		// }



		transform.position = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);
		transform.rotation = Quaternion.Euler (0f, 0f, 0);

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

	// void OnTriggerEnter2D(Collider2D other) {
	// 	if (other.gameObject.tag == "Player") {
	// 		Vector2 playerPos = other.gameObject.transform.position;
	// 		RaycastHit2D hit = Physics2D.Raycast(transform.position, playerPos, Mathf.Infinity, LayerMask.GetMask ("Player", "Wall"));
	// 		Debug.DrawRay(transform.position, playerPos);
	// 		if (hit) {
	// 			if (hit.collider.gameObject.layer == 11) {
	// 				float directionTarget = AngleBetweenVector2(transform.position, playerPos);
	// 				if (_angle + 90 >= 180) {
	// 					_angle = 180 - _angle;
	// 				} else if (_angle - 90 <= -180) {
	// 					_angle = (180 - (180 - _angle));
	// 				}
	// 				if (directionTarget >= (_angle - 90) && directionTarget <= (_angle + 90)) {
	// 					_alerted = true;
	// 					_target = playerPos;
	// 				}
	// 			}
	// 		}
	// 	}
	// }

	// private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2) {
	// 	Vector2 difference = vec2 - vec1;
	// 	float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
	// 	return Vector2.Angle(Vector2.right, difference) * sign;
	// }

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
