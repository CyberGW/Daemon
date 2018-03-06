using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class storing all the pre-defined quests that the user can choose from
/// </summary>
public class QuestInstances : MonoBehaviour{

	public static Quest CS = new Quest ("BSc Computer Science", "CS", new QuestDef (questTypes.defeatEnemy, "Haskell Simulation"),
		                         new QuestDef (questTypes.gainItem, "Shield"), new QuestDef (questTypes.inTimeLimit, "200"), 10, 200);

}
