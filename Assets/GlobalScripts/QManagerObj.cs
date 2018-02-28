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

	//Start variables
	private int startTime;

	//Conditions dictionary - with initial conditions
	public IDictionary<QuestDef, bool> conditions = new Dictionary<QuestDef, bool>();

	public QuestManager (Quest[] chosenQuests)
	{
		this.chosenQuests = chosenQuests;
		//[TEMP]
		startQuest(QuestInstances.CS);
	}

	private void startQuest(Quest quest) {
		foreach (QuestDef def in new List<QuestDef>() {quest.Main, quest.Side, quest.Cond}) {
			conditions.Add (def, questDefVal(def.Type));
		}
	}

	private bool questDefVal(questTypes type) {
		return type == questTypes.noFainting;
	}

//	public bool doneInTimeLimit() {
//		return Time.time - startTime <= currentQuest.timeLimit;
//	}

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
