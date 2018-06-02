using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public bool coldWeapon = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!coldWeapon) {
			transform.Translate (Vector3.right * 30 * Time.deltaTime * -1);
		}
	}
}
