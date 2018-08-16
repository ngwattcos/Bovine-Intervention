using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngineInternal;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CowBehavior : MonoBehaviour {

	GameOverScript gos;

	AudioSource musicPlayer;
	public AudioClip song;

	AudioSource feedbackPlayer;
	public AudioClip good;
	public AudioClip bad;

	float chooseTargetTimer;

    bool running = true;
    bool tricking = false;
	bool trickTrigger = false;

	//bool beginTrigger = false;
	public bool begunYet = false;

    public int level = 1;
    // Animator anim;
    Animation anim;

    public string[][] levelAnimations;

	public string[][] animationNames;
	public string[][] keysToPress;

	public bool[][] animationKeys;
	public bool[][] nonAnimationKeys;

	float speed = 40f;
	float rotateSpeed = 2f;

    public Vector3 target;
    LineRenderer lineRenderer;
	string newAnimation;


	Vector3 curRot;

	float timeToReact = 0f;
	float timeLeft = 0f;
	public bool completedCombo;
	public bool onCow;
	int animationNum;


	public GameObject comboScreen;
	public Slider timeSlider;
	public Text moveName;
	public GameObject WPanel;
	public GameObject APanel;
	public GameObject SPanel;
	public GameObject DPanel;

	public Text levelScreenText;

	public Text remainingTime;

	public float totalTime = 45f;
	float currentTimeRemaining = 0;

	public FixedJoint saddlePoint;

	public GameObject roundOverScreen;

	public bool pausedTrigger = false;
	public bool paused = false;
	public bool wonTrigger = false;
	public bool won = false;

	public GameObject pauseScreen;

	public GameObject beginScreen;


	public AudioSource mooSound;
	public AudioClip[] mooSoundClips;

	public AudioSource gallopSound;
	public AudioClip [] gallopSoundClips;

	public void Moo() {
		Debug.Log ("cow says moo");
	}

	public void Gallop ()
	{
		Debug.Log ("cow runs");
	}


	public void beginRide() {
		begunYet = true;
		beginScreen.SetActive (false);
	}

	// Use this for initialization
	void Start() {

		gos = GameObject.Find("Cowboy").GetComponent<GameOverScript> ();
		chooseTargetTimer = 0.0f;

		musicPlayer = gameObject.AddComponent<AudioSource> ();
		musicPlayer.clip = song;
		musicPlayer.loop = true;
		musicPlayer.volume = 0.2f;
		musicPlayer.Play ();

		feedbackPlayer = gameObject.AddComponent<AudioSource> ();

		beginScreen.SetActive (true);

		//beginTrigger = false;
		begunYet = false;

		levelScreenText.text = "Level " + level;

		currentTimeRemaining = totalTime;

		completedCombo = false;
		onCow = true;
		won = false;
		wonTrigger = false;
		paused = false;
		pausedTrigger = false;

		timeToReact = 1.2f - level * 0.1f;
		timeLeft = timeToReact;

		curRot = this.transform.eulerAngles;

		levelAnimations = new string [] [] {
			new string[] {
				"Bucking_Forward", "Buck_Backward", "Buck_Backward", "Shake"
			},
			new string[] {
				"Bucking_Forward", "Buck_Backward", "Shake", "Shake"
			},
			new string[] {
				"Bucking_Forward", "Buck_Backward", "Head_Explode", "Head_Explode"
			},
			new string[] {
				"Bucking_Forward", "Buck_Backward", "Grow_Tall", "Shrink"
			},
			new string[] {
				"Bucking_Forward", "Buck_Backward", "Grow_Tall", "Spin"
			}
		};

		/*levelAnimations = new string[][] {
            new string[] {
				"Armature|Bucking_Forward", "Armature|Buck_Backward", "Armature|Buck_Backward", "Armature|Shake"
            },
            new string[] {
				"Armature|Bucking_Forward", "Armature|Buck_Backward", "Armature|Shake", "Armature|Shake"
            },
            new string[] {
				"Armature|Bucking_Forward", "Armature|Buck_Backward", "Armature|Head_Explode", "Armature|Head_Explode"
            },
            new string[] {
				"Armature|Bucking_Forward", "Armature|Buck_Backward", "Armature|Grow_Tall", "Armature|Shrink"
            },
            new string[] {
				"Armature|Bucking_Forward", "Armature|Buck_Backward", "Armature|Grow_Tall", "Armature|Spin", "Armature|Vibrate"
            }
        };*/

		animationNames = new string[][] {
			new string[] {
				"Buck Forwards", "Buck Backwards", "Buck Backwards", "Shake"
			},
			new string[] {
				"Buck Forwards", "Buck Backwards", "Shake", "Shake"
			},
			new string[] {
				"Buck Forwards", "Buck Backwards", "Explosion Head", "Explosion Head"
			},
			new string[] {
				"Buck Forwards", "Buck Backwards", "Growth Hormones", "Shrink"
			},
			new string[] {
				"Buck Forwards", "Buck Backwards", "Growth Hormones", "Spin"
			}
		};


		UpdateAnimationKeys();



		keysToPress = new string[][] {
			new string[] {"w", "s", "s", "ad"},
			new string[] {"w", "s", "ad", "ad"},
			new string[] {"w", "s", "ws", "ws"},
			new string[] {"w", "s", "awd", "asd"},
			new string[] {"w", "s", "awd", "aw"}
		};


        anim = GetComponentInChildren<Animation>();

        anim.Play("Idle");

		newAnimation = "Idle";

		float theta = UnityEngine.Random.Range(0, 2 * 3.1415926535f);
		float d = UnityEngine.Random.Range(35f, 90f);
		target = new Vector3(Mathf.Cos (theta) * d, -0.3f, Mathf.Sin (theta) * d);
		//target = new Vector3(UnityEngine.Random.Range(-190f, 190f), 0, UnityEngine.Random.Range(-190f, 190f));

		comboScreen.SetActive (false);

		roundOverScreen.SetActive (false);

		pauseScreen.SetActive (false);
	}

	bool k(string s) {
		return Input.GetKey(s);
	}

	void UpdateAnimationKeys() {
		

		animationKeys = new bool [] [] {
			new bool[] {
				k("w"), k("s"), k("s"), k("a") && k("d")
			},
			new bool[] {
				k("w"), k("s"), k("a") && k("d"), k("a") && k("d")
			},
			new bool[] {
				k("w"), k("s"), k("w") && k("s"), k("w") && k("s")
			},
			new bool[] {
				k("w"), k("s"), k("a") && k("w") && k("d"), k("a") && k("s") && k("d")
			},
			new bool[] {
				k("w"), k("s"), k("a") && k("w") && k("d"), k("a") && k("w"), k("s") && k("d")
			}
		};

		nonAnimationKeys = new bool [] [] {
			new bool[] {
				k("a") || k("s") || k("d"), k("a") || k("w") || k("d"), k("a") || k("w") || k("d"), k("w") || k("s")
			},
			new bool[] {
				k("a") || k("s") || k("d"), k("a") || k("w") || k("d"), k("w") || k("s"), k("w") || k("s")
			},
			new bool[] {
				k("a") || k("s") || k("d"), k("a") || k("w") || k("d"), k("a") && k("d"), k("a") && k("d")
			},
			new bool[] {
				k("a") || k("s") || k("d"), k("a") || k("w") || k("d"), k("s"), k("w")
			},
			new bool[] {
				k("a") || k("s") || k("d"), k("a") || k("w") || k("d"), k("s"), k("s") || k("d")
			}
		};



	}

	void DeactivateKeys() {
		WPanel.SetActive (false);
		APanel.SetActive (false);
		SPanel.SetActive (false);
		DPanel.SetActive (false);
	}

	void updateHintKeys() {
		string thisMove = keysToPress[level - 1][animationNum];
		DeactivateKeys ();
		//Debug.Log (thisMove + ", " + thisMove.Length);
		for (int i = 0; i < thisMove.Length; i++) {
			
			if (thisMove.Substring(i, 1).Equals("w")) {
				WPanel.SetActive(true);
			}
			if (thisMove.Substring (i, 1).Equals ("a")) {
				APanel.SetActive (true);
			}
			if (thisMove.Substring (i, 1).Equals ("s")) {
				SPanel.SetActive (true);
			}
			if (thisMove.Substring (i, 1).Equals ("d")) {
				DPanel.SetActive (true);
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		UpdateAnimationKeys();

		//Debug.Log ("W: " + Input.GetKey ("w") + ", " + k ("w"));
		//Debug.Log ("A: " + Input.GetKey ("a") + ", " + k ("a"));
		//Debug.Log ("S: " + Input.GetKey ("s") + ", " + k ("d"));
		//Debug.Log ("D: " + Input.GetKey ("d") + ", " + k ("w"));
		//Debug.Log (k ("w") + ", " + k ("a") + ", " + k ("s") + ", " + k ("d"));

		if (begunYet) {
			if (!paused && Input.GetKeyDown (KeyCode.Escape) && !won && !gos.onGround) {
				paused = true;
				pauseScreen.SetActive (true);
				musicPlayer.Pause ();
			}

			if (paused) {
				/*if (Input.GetMouseButtonDown (0)) {
					paused = false;
					pauseScreen.SetActive (false);
				}*/
			}

			if (!paused) {
				currentTimeRemaining -= Time.deltaTime;
				if (currentTimeRemaining < 0) {
					currentTimeRemaining = 0f;
				}
			}

			if (onCow && currentTimeRemaining <= 0) {
				wonTrigger = true;
			}


			remainingTime.text = "Time Remaining: " + ((int)currentTimeRemaining) + "s";

			if (wonTrigger) {
				won = true;
				wonTrigger = false;

				roundOverScreen.SetActive (true);
			}

			if (!won && !paused) {
				// clamp rotation
				curRot = new Vector3 (0, transform.eulerAngles.y, 0);
				transform.rotation = Quaternion.Euler (curRot);



				if (running) {
					Vector3 targetDir = (target - transform.position);
					//Quaternion lookRotation = Quaternion.LookRotation (targetDir);
					float step = rotateSpeed * Time.deltaTime;
					Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);
					transform.rotation = Quaternion.LookRotation (newDir);
					transform.Translate (Vector3.forward * Time.deltaTime * speed);
					transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
				}

				bool inRange = Vector3.Distance (transform.position, target) < 15;
				if (running && !inRange) {
					chooseTargetTimer += Time.deltaTime;

					if (chooseTargetTimer > 5f) {
						ChooseNewTarget ();
					}
				}
				// if the bull has arrived at destination
				if (running && inRange) {
					running = false;

					if (onCow) {
						ChooseNewAnimation ();
						trickTrigger = true;
					} else {
						anim.Play ("Idle");
					}

				}

				// trigger the bucking
				if (trickTrigger) {
					comboScreen.SetActive (true);
					updateHintKeys ();

					moveName.text = animationNames [level - 1] [animationNum];
					tricking = true;
					anim.Play (newAnimation);
					trickTrigger = false;

					timeLeft = timeToReact;
				}

				// while the bull is still bucking, and you did not win yet
				if (tricking && !won) {
					timeLeft -= Time.deltaTime;
					//Debug.Log (timeLeft + ", " + completedCombo);

					//Debug.Log (animationKeys [level - 1] [animationNum]);


					if (!completedCombo && (nonAnimationKeys[level - 1][animationNum] == true)) {
						FallOff ();
					}

					if (!completedCombo && (animationKeys[level - 1][animationNum] == true) && onCow) {
						//Debug.Log ("set to true");
						completedCombo = true;
						DeactivateKeys ();

						feedbackPlayer.clip = good;
						feedbackPlayer.Play ();
					}



					timeSlider.value = timeLeft * 1.0f / timeToReact;

					if (!completedCombo && timeLeft <= 0f) {
						// death, game over
						FallOff ();
					}
				}

				// the bull just finished bucking
				if (tricking && !anim.isPlaying) {
					completedCombo = false;
					running = true;
					tricking = false;
					ChooseNewTarget ();

					timeLeft = timeToReact;
					comboScreen.SetActive (false);
				}

				if (running) {
					anim.Play ("Running");
				} else {
					//anim.Play();
				}
			}
		}




	}

    void ChooseNewTarget() {
		while (Vector3.Distance(transform.position, target) < 20f) {
			float theta = UnityEngine.Random.Range (0, 2 * 3.1415926535f);
			float d = UnityEngine.Random.Range (5f, 80f);
			target = new Vector3 (Mathf.Cos (theta) * d, -0.3f, Mathf.Sin (theta) * d);
			chooseTargetTimer = 0f;
		}


    }

	void ChooseNewAnimation() {
		animationNum = UnityEngine.Random.Range (0, levelAnimations [level - 1].Length);
		//animationNum = 4;
		newAnimation = levelAnimations[level - 1][animationNum];
		//anim.Play(newAnimation);
	}

	void FallOff() {
		if (onCow) {
			feedbackPlayer.clip = bad;
			feedbackPlayer.Play ();
		}

		onCow = false;
		Destroy (saddlePoint);


	}

	public void LoadNextRound() {
		if (level == 1) {
			SceneManager.LoadScene ("level02");
		}
		if (level == 2) {
			SceneManager.LoadScene ("level03");
		}
		if (level == 3) {
			SceneManager.LoadScene ("level04");
		}
		if (level == 4) {
			SceneManager.LoadScene ("level05");
		}
		if (level == 5) {
			SceneManager.LoadScene ("ending");
		}
	}

	public void Unpause() {
		paused = false;
		pauseScreen.SetActive (false);
		musicPlayer.UnPause ();
	}

	public void RestartLevel ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void MainMenu ()
	{
		SceneManager.LoadScene ("mainMenu");
	}
}
