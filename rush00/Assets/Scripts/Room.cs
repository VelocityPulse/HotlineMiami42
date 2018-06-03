using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public List<Door> doors = new List<Door>();
	public List<Checkpoint> checkpoints = new List<Checkpoint> ();

	void Start () {
		
	}
	
	void Update () {
		
	}

	public Checkpoint getRandomCheckPoint() {
		if (checkpoints.Count == 0){
			return null;
		}
		return checkpoints [Random.Range (0, checkpoints.Count)];
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
	}
}
