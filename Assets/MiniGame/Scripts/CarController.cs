using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ALL ASSESSMENT 3 ADDITIONS

/// <summary>
/// A controller to controll the sataus of the minigame and most of its components
/// [EXTENSIONS] - New function added to manage the change of a level
/// [CHANGES] - Level handling code removed from <see cref="ChangeSpeed"/> and placed in <see cref="levelUp"/>  
/// </summary>
public class CarController : MonoBehaviour
{
    /// <summary> An array of the different spawners being controlled to summon cars </summary>
    public GameObject[] Spawners;
    /// <summary> A tick that keeps counting ,used to manage the timings of how cars spawn </summary>
    private float tick;
    /// <summary> The increment of tick </summary>
    private float spd = 0.5f;
    /// <summary> the current level  </summary>
    private int level = 0;
    /// <summary>The scenechanger to get a nice transition </summary>
    private SceneChanger sceneChanger;
    /// <summary>Sound effect to play when transitioning</summary>
    private AudioClip SFX;
	/// <summary>
	/// [EXTENSION] - Array to track if each car spawner has spawned during the current cycle, to stop duplicates
	/// </summary>
	private bool[] spawnedThisCycle;

    // Use this for initialization
	/// <summary>
	/// [EXTENSION] - Start minigame music
	/// </summary>
    void Start()
    {
        sceneChanger = GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
		spawnedThisCycle = new bool[6];
		//play minigame music
		SoundManager.instance.playBGM(Resources.Load("Audio/minigame", typeof(AudioClip)) as AudioClip);
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        //cheat if you find it hard press "p" a few times and then finish the level once
        if(Input.GetKeyDown(KeyCode.P))
        {
            level += 1;
        }
        //summon cars in a "nice" patten can increase spd to make cars summon faster
		if ((10 - tick) <= 1)
        {
			if (!spawnedThisCycle [0]) {
				Spawners [0].GetComponent<CarSpawner> ().Trigger (0);
				spawnedThisCycle [0] = true;
			}
        }
		if ((30 - tick) <= 1)
        {
			if (!spawnedThisCycle [1]) {
				Spawners [1].GetComponent<CarSpawner> ().Trigger (1);
				spawnedThisCycle [1] = true;
			}
        }
		if ((50 - tick) <= 1)
        {
			if (!spawnedThisCycle [2]) {
				Spawners [2].GetComponent<CarSpawner> ().Trigger (1);
				spawnedThisCycle [2] = true;
			}
			if (!spawnedThisCycle [5]) {
				Spawners [5].GetComponent<CarSpawner> ().Trigger (0);
				spawnedThisCycle [5] = true;
			}
        }
		if ((70 - tick) <= 1)
        {
			if (!spawnedThisCycle [3]) {
				Spawners [3].GetComponent<CarSpawner> ().Trigger (2);
				spawnedThisCycle [3] = true;
			}
        }
		if ((90 - tick) <= 1)
        {
			if (!spawnedThisCycle [4]) {
				Spawners [4].GetComponent<CarSpawner> ().Trigger (2);
				spawnedThisCycle [4] = true;
			}
        }
        //reset tick when a cycle is done
        tick += spd;
        if (tick > 100)
        {
            tick = 0;
			resetSpawnedThisCycle ();
        }

    }

	private void resetSpawnedThisCycle() {
		for (int i = 0; i < spawnedThisCycle.Length; i++) {
			spawnedThisCycle[i] = false;
		}
	}

    /// <summary>
    /// Draw the current level on the screen
    /// </summary>
    public void OnGUI()
    {
        Rect bounds = new Rect(40,45,140,140);
        GUI.Label(bounds, "Level: " + (level+1) + " /3");
    }

	/// <summary>
	/// [EXTENSION] - New function taking code from <see cref="ChangeSpeed"/> to separate functionality when a level is beat and to increase the car's speed
	/// 			- Change music back to main
	/// </summary>
	public void levelUp() {
		if (level == 2) { //if the final level has just been beat
			//load world map
			SoundManager.instance.playSFX ("transition");
			GameObject.FindObjectOfType<MiniMove> ().setCanMove(false);
			SoundManager.instance.playBGM(Resources.Load("Audio/bgm", typeof(AudioClip)) as AudioClip);
			if (SceneManager.GetActiveScene ().name == "MiniGame") { //if currently on "MiniGame"
				sceneChanger.loadLevel ("WorldMap", new Vector2 (-4.16f, -41f)); //load on Hes West side
			} else { //if currently on "MinigameR"
				sceneChanger.loadLevel ("WorldMap", new Vector2 (29.91f, -45f)); //load on Hes East side
			}
		} else {
			//otherwise increase level, car speed and call ChangeSpeed to change car spawn rate
			level += 1;
			spd += 0.5f;
			ChangeSpeed (1.1f);
		}
	}

    /// <summary>
    /// [CHANGE] - Originally this function also handelled changing the level and potentially completion of the minigame.
	/// To increase modularity, this code has been separated into <see cref="levelUp"/> 
	/// Now, this only changes the car spawn rate
    /// </summary>
    /// <param name="amount">the amount of which to increase the car speeds</param>
    public void ChangeSpeed(float amount) {      
		//For all spawners
        for (int i = 0; i < Spawners.Length; i++) {
            Spawners[i].GetComponent<CarSpawner>().Harder(amount);
			//Make them spawn more cars
        }  
    }

    /// <summary>
    /// When the player runs out of lives this is called
    /// It resets level back to 0 and sets all the car speeds back to what they were origionally
    /// </summary>
    public void Restart()
    {
        level = 0;
        spd = 0.5f;
        for (int i = 0; i < Spawners.Length; i++)
        {
            Spawners[i].GetComponent<CarSpawner>().Restart();
        }
    }
}
