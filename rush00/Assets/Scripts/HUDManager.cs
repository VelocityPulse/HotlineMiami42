using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour {

	[SerializeField] private Text _weaponText;
	[SerializeField] private Text _ammoText;
	[SerializeField] private Canvas winCanvas;
	[SerializeField] private Canvas looseCanvas;


	void Start () {
	}
	
	void Update () {
	}

	public void PlayButton() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Time.timeScale = 1;
	}

	public void QuitButton() {
		Application.Quit();
	}

	public void Win() {
		winCanvas.gameObject.SetActive(true);
	}


	public void Loose() {
		looseCanvas.gameObject.SetActive(true);
	}

	public void ChangeWeapon(string weaponName) {
		_weaponText.text = weaponName;
	}

	public void ChangeAmmo(string ammo) {
		_ammoText.text = ammo;
	}
}
