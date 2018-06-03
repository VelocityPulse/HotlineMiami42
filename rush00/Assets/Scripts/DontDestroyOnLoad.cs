using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

	static bool firstTime = true;

	private void Awake () {
		if (firstTime) {
			DontDestroyOnLoad (gameObject);
			firstTime = false;
		} else {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
