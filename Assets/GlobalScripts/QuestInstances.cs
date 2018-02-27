using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInstances : MonoBehaviour{

	public static bool fun() {
		return true;
	}

	public static Quest CS = new Quest("BSc Computer Science", "CS", "enemyDefeated",
		"getGainedItem", "doneInTimeLimit",
		10, 200, "Shield", -1, "Haskell Simulation", "");


}
