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
		manager = new QuestManager (new Quest[3] {QuestInstances.CS, null, null});
	}
		

}

/// <summary>
/// A class to manage the tracking and completion of quests
/// </summary>
public class QuestManager {

	/// <summary>
	/// A list containing all the quests chosen
	/// </summary>
	private Quest[] chosenQuests;

	//Start variables
	private int startTime;

	/// <summary>
	/// Dictionary storing the current values of all possible quest conditions
	/// (e.g. one for each in <see cref="questTypes"/> 
	/// </summary>
	public IDictionary<QuestDef, bool> conditions = new Dictionary<QuestDef, bool>();

	public QuestManager (Quest[] chosenQuests)
	{
		this.chosenQuests = chosenQuests;
		//[TEMP]
		startQuest(QuestInstances.CS);
	}

	/// <summary>
	/// Set all dictionary objects to their default value
	/// </summary>
	/// <param name="quest">Quest.</param>
	private void startQuest(Quest quest) {
		foreach (QuestDef def in new List<QuestDef>() {quest.Main, quest.Side, quest.Cond}) {
			conditions.Add (def, questDefVal(def.Type));
		}
	}

	/// <summary>
	/// Returns the default value for all quest types
	/// </summary>
	/// <returns><c>true</c>, if default value is true, <c>false</c> otherwise.</returns>
	/// <param name="type">Type.</param>
	private bool questDefVal(questTypes type) {
		return type == questTypes.noFainting || type == questTypes.noSpecialAttacks || type == questTypes.noHealingStations || type == questTypes.onlyOneCharacter;
	}

//	public bool doneInTimeLimit() {
//		return Time.time - startTime <= currentQuest.timeLimit;
//	}

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
			if (def.Type == type && (def.Data == data || def.Data == "")) {
				if (def.Type == questTypes.noFainting) {
					conditions [def] = false;
				} else {
					conditions [def] = true;
				}
				Debug.Log ("New Value: " + conditions [def]);
			}
		}
	}

//	private void resetVariables() {
//		conditions[questTypes.gainItem] = false;
//		conditions[questTypes.noFainting] = true;
//		conditions[questTypes.defeatEnemy] = false;
//		conditions[questTypes.talkToNPC] = false;
//	}

	private void startNewQuest() {
		//resetVariables ();
	}

//	private void checkQuestCompletion() {
//		if (currentQuest.questCompleted()) {
//			Debug.Log ("Quest completed!");
//			//Give exp
//			//Give money
//		} else {
//			//fail
//		}
//	}
}
