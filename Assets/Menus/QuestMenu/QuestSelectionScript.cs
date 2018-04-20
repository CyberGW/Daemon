using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to handle the quest selection menu
/// </summary>
public class QuestSelectionScript : MonoBehaviour {

	public Text title;
	public Text main;
	public Text side;
	public Text cond;
	public Button selectButton;
	// The quest that is currently selected
	private string currentQuest = "";
	// The number of quests that have been selected so far
	private int chosen = 0;

	private GameObject player;

	// Use this for initialization
	void Start () {		
		player = GameObject.Find ("Player");
		player.SetActive (false);
		showDesc ("CS"); //initally have CS selected
	}

	/// <summary>
	/// Shows the description of a particular quest
	/// </summary>
	/// <param name="key">The key for the particular quest, as stored in <see cref="QuestInstances.defs"/> </param>
	public void showDesc (string key) {
		if (currentQuest == "") {
			selectButton.interactable = true;
		}
		Quest quest = QuestInstances.defs [key];
		title.text = "Name: " + quest.title;
		main.text = "Main: " + quest.Main.Desc;
		side.text = "Side: " + quest.Side.Desc;
		cond.text = "Cond: " + quest.Cond.Desc;
		currentQuest = key;
	}

	/// <summary>
	/// When select button is pressed, add quest to <see cref="QManagerObj"/>, stop user from selecting the same quest again,
	/// and if second quest selected, start the game 
	/// </summary>
	public void selectQuest () {
		QManagerObj.manager.addQuest (QuestInstances.defs [currentQuest]);
		GameObject.Find(currentQuest).GetComponent<Button>().interactable = false;
		currentQuest = "";
		chosen++;
		selectButton.interactable = false;
		if (chosen >= 2) {
			StartLevel ();
		}
	}

	/// <summary>
	/// Starts the game, by loading in all default values before loading the CS-Jail scene
	/// </summary>
	public void StartLevel(){

		//Setup global data to initial values
		PlayerData.instance.data = new DataManager (
			new Player ("George", 1, 100, 5, 5, 5, 5, 5, 5, 0, null, 
				new MagicAttack ("hi-jump kicked", "Kick with power 15", 3, 15),
				new RaiseDefence ("buffed up against", "Increase your defence by 10%", 2, 0.1f),
				(Texture2D)Resources.Load ("Character1", typeof(Texture2D))));
		PlayerData.instance.data.addPlayer (new Player ("Hannah", 1, 100, 5, 3, 5, 5, 15, 5, 0, null, 
			new IncreaseMoney ("stole money from", "Increase money returns by 50%", 2, 0.5f),
			new MagicAttack ("threw wine bottles at", "Throw wine bottles with damage 15", 2, 15),
			(Texture2D) Resources.Load("Character2", typeof(Texture2D))));
		GlobalFunctions.instance.currentLevel = 0;
		GlobalFunctions.instance.objectsActive = new Dictionary<string, bool> ();

		QManagerObj.manager.updateCurrentQuest ("CS");

		SoundManager.instance.playSFX ("interact");
		player.SetActive (true);
		SceneChanger.instance.loadLevel ("CS-Jail", new Vector2 (0, 0));
	}
	


}
