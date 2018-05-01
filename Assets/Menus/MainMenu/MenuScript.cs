using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script to handle the main menu
/// </summary>
public class MenuScript : MonoBehaviour {

	/// <summary>
	/// Separate canvas asking if user is sure they want to quit
	/// </summary>
	public Canvas quitMenu;
	/// <summary>
	/// Exit button
	/// </summary>
	public Button exitText;
	/// <summary>
	/// Start Game button
	/// </summary>
	public Button startText;
	GameObject player;
	private Text audioText;
	/// <summary> [EXTENSION] - Reference to the autosave text button</summary>
	private Text autosaveText;
	/// <summary>
	/// Reference for current sound situation
	/// </summary>
	private bool soundOn;

	/// <summary>
	/// Sets us variable references
	/// [EXTENSION] - Set text for sound and autosave buttons to their current value
	/// </summary>
	void Start () {		
		quitMenu = quitMenu.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		audioText = GameObject.Find ("Audio").GetComponent<Text> ();
		soundOn = SoundManager.instance.isSoundOn (); //get initial sound state
		audioText.text = "Sound: " + ((soundOn) ? "On" : "Off"); //set text appropriately for curret sound state
		autosaveText = GameObject.Find ("Autosave").GetComponent<Text> (); //get current autosave option
		autosaveText.text = "Autosave: " + ((GlobalFunctions.instance.autoSave) ? "On" : "Off"); //set text according to current option
		quitMenu.enabled = false;
		player = GameObject.Find ("Player");
		player.SetActive (false);
	}

	/// <summary>
	/// Disables "start" and "exit" buttons on mainMenu and activates the quitMenu when "Exit" is selected
	/// </summary>
	public void ExitPress(){		
		quitMenu.enabled = true;
		exitText.enabled = false;
		startText.enabled = false;

	}
	/// <summary>
	/// When "no" is selected on quitMenu disable quitMenu and reenable start and exit buttons on mainMenu
	/// </summary>
	public void NoPress(){
		quitMenu.enabled = false;
		exitText.enabled = true;
		startText.enabled = true;
	}

	/// <summary>
	/// [CHANGE] - Load the quest selection screen instead of going straight into the game
	/// </summary>
	public void StartLevel() {
		//make the player object active
		player.SetActive (true);
		//go to quest selection before starting the game
		SceneChanger.instance.loadLevel ("QuestSelection");
	}

    /// <summary>
    /// Calls Load from DataManager. Similar behaviour to StartLevel, but will read iniitialisation data from save file.
    /// </summary>
    public void LoadPress()
    {
        PlayerData.instance.data = new DataManager();
        PlayerData.instance.data.Load();
        player.SetActive(true);
    }

    /// <summary>
    /// Closes application
    /// </summary>
    public void ExitGame() {
		Application.Quit ();
	}

	/// <summary>
	/// If <see cref="soundOn"/> is <c>true</c>, then turn sound off and update variables and text
	/// If <c>false</c>, then turn sound on and update variables and text similarly 
	/// </summary>
	public void sound() {
		if (soundOn) {
			SoundManager.instance.soundOn (false);
			soundOn = false;
			audioText.text = "Sound: Off";
		} else {
			SoundManager.instance.soundOn (true);
			soundOn = true;
			audioText.text = "Sound: On";
		}
	}

	/// <summary>
	/// [EXTENSION] - Function to handle turning autosave on and off
	/// </summary>
	public void autosave() {
		if (GlobalFunctions.instance.autoSave) { //if autosave currently on
			//set it off and update text
			GlobalFunctions.instance.autoSave = false;
			autosaveText.text = "Autosave: Off";
		} else { //otherwise if currently off
			//set it on and update text
			GlobalFunctions.instance.autoSave = true;
			autosaveText.text = "Autosave: On";
		}
		Debug.Log (GlobalFunctions.instance.autoSave);
	}
			
}
