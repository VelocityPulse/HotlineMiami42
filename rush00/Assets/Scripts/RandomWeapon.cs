using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeapon : MonoBehaviour {

	public List<GameObject> weaponPrefabs;

	// Use this for initialization
	void Start () {
		int randomValue = Random.Range (0, weaponPrefabs.Count);

		// weaponPrefabs [randomValue].transform.localPosition = transform.localPosition;
		GameObject newWeapon = Instantiate (weaponPrefabs [randomValue], transform.position, Quaternion.identity);
		newWeapon.GetComponent<Weapon> ().playerWeapon = true;
	}

	// Update is called once per frame
	void Update () {

	}
}
