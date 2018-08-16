using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

	AudioSource musicPlayer;
	public AudioClip song;

	// Use this for initialization
	void Start () {
		musicPlayer = gameObject.AddComponent<AudioSource> ();
		musicPlayer.clip = song;
		musicPlayer.loop = true;
		musicPlayer.volume = 0.1f;
		musicPlayer.Play ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Begin() {
		SceneManager.LoadScene ("intro");
	}

	public void EnterRodeo() {
		SceneManager.LoadScene ("level01");
	}

	public void Quit() {
		Application.Quit ();
	}

	public void EndGame() {
		SceneManager.LoadScene ("credits");
	}

	public void MainMenu() {
		SceneManager.LoadScene ("mainMenu");
	}
}
