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

	void OnTriggerStay2D(Collider2D other) {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		// print(other.gameObject.name);
		if (other.gameObject.tag == "Player") {

			RaycastHit2D hit = Physics2D.Raycast(transform.position, other.gameObject.transform.position, Mathf.Infinity, 9);
			if (hit) {
				print(hit.collider.gameObject.name);
			}

			hit = Physics2D.Raycast(transform.position, other.gameObject.transform.position, Mathf.Infinity, 8);
			if (hit) {
				print(hit.collider.gameObject.name);
			}

			hit = Physics2D.Raycast(transform.position, other.gameObject.transform.position, Mathf.Infinity, 10);
			if (hit) {
				print(hit.collider.gameObject.name);
			}

			// print(transform.position + " " + other.gameObject.transform.position + " " + Vector2.right);
// Debug.DrawRay(other.gameObject.transform.position, transform.position);
			// print(other.gameObject.layer);
			// RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, other.gameObject.transform.position, Mathf.Infinity, 9);
			// RaycastHit2D[] hits = Physics2D.RaycastAll(other.gameObject.transform.position, transform.position);
			// foreach (RaycastHit2D hit in hits) {
			// 	print(hit.collider.gameObject.name);
			// }
			// if (hit) {
			// 	if (hit.collider.gameObject.tag == "Enemy") {
			// 		_mousePosition = other.gameObject.transform.position;
			// 	}
			// 	print(hit.collider.gameObject.name);
			// }
		}
		// Vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);


	}
}
