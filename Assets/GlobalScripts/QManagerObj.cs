using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Unity Object to house the QuestManager for easy access from other scripts
/// </summary>
public class QManagerObj : MonoBehaviour {

	public static QuestManager manager;

	void Start() {
		DontDestroyOnLoad (gameObject);
		manager = new QuestManager ();
	}		

}

/// <summary>
/// A class to manage the tracking and completion of quests
/// </summary>
[System.Serializable]
public class QuestManager {

	/// <summary>
	/// A list containing all the quests chosen
	/// </summary>
	private Quest[] chosenQuests;
	private int noOfQuests = 0;
	private Quest currentQuest = null;

	//The time the quest should be finished by to pass an inTimeLimit quest
	private int finishTime;

	/// <summary>
	/// Dictionary storing the current values of all possible quest conditions
	/// (e.g. one for each in <see cref="questTypes"/> 
	/// </summary>
	public IDictionary<QuestDef, bool> conditions = new Dictionary<QuestDef, bool>();

	/// <summary>
	/// Stores all questTypes where an event being triggered would fail the quest instead of completing it.
	/// Consequently, these quest conditions should also be initalised to true rather than false
	/// </summary>
	public List<questTypes> negativeQuestConds = new List<questTypes> () {questTypes.noFainting, questTypes.noSpecialMoves, questTypes.onlyOneCharacter, questTypes.noHealingStations};

	/// <summary>
	/// Initializes a new instance of the <see cref="QuestManager"/> class
	/// </summary>
	/// <param name="chosenQuests">Chosen quests.</param>
	public QuestManager ()
	{
		this.chosenQuests = new Quest[2];
	}

	public Quest CurrentQuest {
		get {
			return this.currentQuest;
		}
	}

	public Quest firstQuest {
		get {
			return this.chosenQuests [0];
		}
	}

	public Quest secondQuest {
		get {
			return this.chosenQuests [1];
		}
	}

	/// <summary>
	/// Set current quest, set all dictionary objects to their default value and start timer if an inTimeLimit quest
	/// </summary>
	/// <param name="quest">Quest.</param>
	private void startQuest(Quest quest) {
		currentQuest = quest;
		foreach (QuestDef def in new List<QuestDef>() {quest.Main, quest.Side, quest.Cond}) {
			if (conditions.ContainsKey (def)) {
				conditions [def] = questDefVal (def.Type);
			} else {
				conditions.Add (def, questDefVal (def.Type));
			}
			if (def.Type == questTypes.inTimeLimit) {
				finishTime = (int) Time.time + int.Parse (def.Data);
			}
			Debug.Log (conditions [def]);
		}

	}

	/// <summary>
	/// Determines whether a quest has a negative condition or not, meaning logging a variable means the quest has failed rather than passed
	/// </summary>
	/// <returns><c>true</c>, if negative condition., <c>false</c> otherwise.</returns>
	/// <param name="type">Type.</param>
	private bool isNegativeQuestCond(questTypes type) {
		return negativeQuestConds.Contains (type);
	}

	/// <summary>
	/// Returns the default value for all quest types
	/// </summary>
	/// <returns><c>true</c>, if default value is true, <c>false</c> otherwise.</returns>
	/// <param name="type">Type.</param>
	private bool questDefVal(questTypes type) {
		return isNegativeQuestCond (type);
	}

	/// <summary>
	/// Adds a new quest to chosen quests
	/// </summary>
	/// <param name="quest">The quest to add</param>
	public void addQuest (Quest quest) {
		chosenQuests [noOfQuests] = quest;
		noOfQuests++;
	}

	/// <summary>
	/// Updates which quest is currently being undertaken
	/// </summary>
	/// <param name="level">The level that the user is now on</param>
	public void updateCurrentQuest (string level) {
		foreach (Quest quest in chosenQuests) {
			if (quest.location == level) {
				startQuest (quest);
				return;
			}
		}
	}

	/// <summary>
	/// Logs a quest condition
	/// </summary>
	/// <param name="type">The type of quest condition that has been triggered</param>
	/// <param name="data">Some optional data describing the current occurence (e.g. name of enemy just defeated)</param>
	public void logQuestVariable(questTypes type, string data = "") {
		Debug.Log ("Logged type " + type);
		Debug.Log ("Data: " + data);
		var buffer = new List<QuestDef> (conditions.Keys);
		foreach (QuestDef def in buffer) {
			//Needs to invert option for the onlyOneCharacter quest type
			//If using the character specified, set string to "n/a" so that the quest condition is not changed.
			//If not using the character specified, then set to the empty string so that the quest condition is changed.
			if (def.Type == questTypes.onlyOneCharacter && type == questTypes.onlyOneCharacter) {
				if (def.Data == data) {
					data = "n/a";
				} else {
					data = "";
				}
			}
			if (def.Type == type && (def.Data == data || def.Data == "")) {
				if (isNegativeQuestCond(type)) {
					conditions [def] = false;
				} else {
					conditions [def] = true;
				}
				Debug.Log ("New Value: " + conditions [def]);
			}
		}
	}		

	/// <summary>
	/// Updates the time limit condition if necessary before applying money and exp rewards if quest was completed successfully
	/// </summary>
	private void finishQuest() {
		var buffer = new List<QuestDef> (conditions.Keys);
		foreach (QuestDef def in buffer) {
			if (def.Type == questTypes.inTimeLimit) {
				conditions [def] = Time.time <= finishTime;
			}
		}
		if (currentQuest.checkQuestCompleted () == questStatues.completed) {
			Debug.Log ("Quest completed!");
			PlayerData.instance.data.Money += currentQuest.money;
			foreach (Player player in PlayerData.instance.data.Players) {
				player.gainExp (currentQuest.exp);
			}
			currentQuest = null;
		}
	}

}
