﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public Checkpoint nextCheckpoint = null;

	void Start () {
		if (!nextCheckpoint) {
			nextCheckpoint = this;
		}
	}
	
	void Update () {

	}
}
