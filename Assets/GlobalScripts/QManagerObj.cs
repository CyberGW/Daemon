using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QManagerObj : MonoBehaviour {

	public static QuestManager manager;

	void Start() {
		DontDestroyOnLoad (gameObject);
		manager = new QuestManager (new Quest[3] {QuestInstances.CS, null, null});
	}
		

}

public class QuestManager {

	public delegate bool questCond ();
	private Quest[] chosenQuests;
	private Quest currentQuest;

	//Start variables
	private int startTime;

	//Conditions dictionary
	public IDictionary<string, bool> conditions = new Dictionary<string, bool>() {
		{"gainedItem", false}, {"playerDied", false}, {"enemyDefeated", false}, {"talkedToNPC", false}
	};

	public QuestManager (Quest[] chosenQuests)
	{
		this.chosenQuests = chosenQuests;
		//[TEMP]
		currentQuest = chosenQuests[0];
	}

	public bool doneInTimeLimit() {
		return Time.time - startTime <= currentQuest.timeLimit;
	}

	public void logGainedItem(string itemName) {
		Debug.Log ("Logged item");
		if (currentQuest.itemName == itemName) {
			conditions["gainedItem"] = true;
		}
		Debug.Log ("New value: " + conditions["gainedItem"]);
	}

	public void logPlayerDied() {
		conditions["playerDied"] = true;
	}

	public void logEnemyDefeated(string enemyName) {
		if (currentQuest.enemyTarget == enemyName) {
			conditions["enemyDefeated"] = true;
		}
	}

	public void logTalkedToNPC(string NPCName) {
		if (currentQuest.NPCTarget == NPCName) {
			conditions["talkedToNPC"] = true;
		}
	}

	private void resetVariables() {
		conditions["gainedItem"] = false;
		conditions["playerDied"] = false;
		conditions["enemyDefeated"] = false;
		conditions["talkedToNPC"] = false;
	}

	private void startNewQuest() {
		resetVariables ();
	}

	private void checkQuestCompletion() {
		if (currentQuest.questCompleted()) {
			Debug.Log ("Quest completed!");
			//Give exp
			//Give money
		} else {
			//fail
		}
	}
}
