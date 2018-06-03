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
		float closest = 100000;
		Door response = doors[0];

		foreach (Door door in doors) {

			Vector3 localPosition = door.transform.position;
			float distance = Vector3.Distance(target, localPosition);

			if (distance < closest) {
				closest = distance;
				response = door;
			}
		}
		return response;
	}

	public Door OtherDoor(Door currentDoor) {
		if (doors.Count == 1) {
			return doors[0];
		}
		int currentIndex = doors.IndexOf(currentDoor);
		int randomValue;
		while ((randomValue = Random.Range(0, doors.Count)) == currentIndex) {
			;
		}

		return doors[randomValue];
		


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
	}
}
