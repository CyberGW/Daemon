using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInstances{

	public Quest CS = new Quest("BSc Computer Science", "CS", new Quest.questCond(QuestManager.getEnemyDefeated),
		new Quest.questCond(QuestManager.getGainedItem), new Quest.questCond(QuestManager.doneInTimeLimit),
		10, 200, "Shield", -1, "Haskell Simulation", "");		

}
