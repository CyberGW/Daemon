using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	private Quest[] chosenQuests;
	private static Quest currentQuest;

	//Start variables
	private static int startTime;

	//Conditions
	public static bool gainedItem;
	public static bool playerDied;
	public static bool enemyDefeated;
	public static bool talkedToNPC;

	//Quest Functions
	public static bool getGainedItem() {
		return gainedItem;
	}

	public static bool getPlayerDied() {
		return playerDied;
	}

	public static bool getEnemyDefeated() {
		return enemyDefeated;
	}

	public static bool getTalkedToNPC() {
		return talkedToNPC;
	}

	public static bool doneInTimeLimit() {
		return Time.time - startTime <= currentQuest.timeLimit;
	}

	public void logGainedItem(string itemName) {
		if (currentQuest.itemName == itemName) {
			gainedItem = true;
		}
	}

	public void logEnemyDefeated(string enemyName) {
		if (currentQuest.enemyTarget == enemyName) {
			enemyDefeated = true;
		}
	}

	public void logTalkedToNPC(string NPCName) {
		if (currentQuest.NPCTarget == NPCName) {
			talkedToNPC = true;
		}
	}

	private void resetVariables() {
		gainedItem = false;
		playerDied = false;
		enemyDefeated = false;
		talkedToNPC = false;
	}

	private void startNewQuest() {
		resetVariables ();
	}

	private void checkQuestCompletion() {
		if (currentQuest.questCompleted()) {
			//Give exp
			//Give money
		} else {
			//fail
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
