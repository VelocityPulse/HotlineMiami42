using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public List<Door> doors = new List<Door>();


	void Start () {
		
	}
	
	void Update () {
		
	}

	public Door NextDoor(Vector3 target) {
		float closest = Vector3.Distance(target, doors[0].transform.localPosition);
		Door response = doors[0];
		foreach (Door door in doors) {
			Vector3 localPosition = door.transform.localPosition;
			float distance = Vector3.Distance(target, localPosition);
			if (distance > 1 && distance <  closest) {
				print("DISTANCE ' " + distance);
				response = door;
			}
		}
		return response;
	}

	// public Vector3 OtherDoor(Vector3 target) {
	// 	float closest = Vector3.Distance(target, doors[0].GetComponent<Renderer>().bounds.center);
	// 	Door response = doors[0];
	// 	foreach (Door door in doors) {
	// 		Vector3 localPosition = door.GetComponent<Renderer>().bounds.center;
	// 		float distance = Vector3.Distance(target, localPosition);
	// 		if (distance <  closest) {
	// 			response = door;
	// 		}
	// 	}
	// 	return response;
	// }
}
