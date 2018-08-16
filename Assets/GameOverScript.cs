using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {

	public GameObject GameOverScreen;

	AudioSource voiceSource;
	public AudioClip pain;

	public bool onGround;

	// Use this for initialization
	void Start () {
		GameOverScreen.SetActive (false);
		voiceSource = gameObject.AddComponent<AudioSource> ();
		voiceSource.clip = pain;

		onGround = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Ground") {
			GameOverScreen.SetActive (true);
			if (!onGround) {
				voiceSource.Play ();
			}


			onGround = true;
		}
	}

	public void RestartLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void MainMenu() {
		SceneManager.LoadScene ("mainMenu");
	}
}
