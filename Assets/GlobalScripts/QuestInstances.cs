using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInstances : MonoBehaviour{

	public static bool fun() {
		return true;
	}

	public static Quest CS = new Quest ("BSc Computer Science", "CS", new QuestDef (questTypes.defeatEnemy, "Haskell Simulation"),
		                         new QuestDef (questTypes.gainItem, "Shield"), new QuestDef (questTypes.inTimeLimit, "200"), 10, 200);

}
