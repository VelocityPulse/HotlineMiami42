using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Vector3 _mousePosition;
	private bool _alive;
	[SerializeField] private Checkpoint _checkpoint;
	[SerializeField] private float speed;
	// [SerializeField] private Weapon _weapon;

	void Start () {
		_mousePosition = transform.position;
		speed = 5;

	}
	
	void FixedUpdate () {
		if (Input.GetMouseButtonDown(0)) {
			_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_mousePosition.z = transform.position.z;
		}
		transform.position = Vector3.MoveTowards(transform.position, _mousePosition, speed * Time.deltaTime);
	}

	void OnCollisionEnter2D(Collision2D other) {
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			_mousePosition = other.gameObject.transform.position;
			print("PLAYER");
		}
	}
}
