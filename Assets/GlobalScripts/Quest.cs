using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum questTypes {gainItem, noFainting, defeatEnemy, talkToNPC, inTimeLimit };

public class QuestDef {

	private KeyValuePair<questTypes, string> pair;

	public QuestDef(questTypes type, string data) {
		this.pair = new KeyValuePair<questTypes, string> (type, data);
	}

	public questTypes Type {
		get {
			return this.pair.Key;
		}
	}

	public string Data {
		get {
			return this.pair.Value;
		}
	}

}

public class Quest {

	public string title;
	public string location;
	/// The dictionary key representing the varaible condition for the main part of the quest
	private QuestDef main;
	/// The dictionary key representing the varaible condition for side quest part
	private QuestDef side;
	/// The dictionary key representing the varaible condition for extra quest condition
	private QuestDef cond;
	//Reward Parameters
	public int money;
	public int exp;

	public Quest (string title, string location, QuestDef main, QuestDef side, QuestDef cond,
		int moneyReward, int expReward)
	{
		this.title = title;
		this.location = location;
		this.main = main;
		this.side = side;
		this.cond = cond;
		this.money = moneyReward;
		this.exp = expReward;
	}

	public QuestDef Main {
		get {
			return this.main;
		}
	}

	public QuestDef Side {
		get {
			return this.side;
		}
	}

	public QuestDef Cond {
		get {
			return this.cond;
		}
	}

	public bool questCompleted() {
		return QManagerObj.manager.conditions[main] && QManagerObj.manager.conditions[side] && QManagerObj.manager.conditions[cond];
	}
	

//	public delegate bool questElement();
//	public delegate void questInitialisor();
//	questElement mainQuest;
//	questElement sideQuest;
//	questElement conditional;
//	questInitialisor startQuest;
//
//	public Quest (string title, string location, questElement mainQuest, questElement sideQuest, questElement conditional, questInitialisor startQuest)
//	{
//		this.title = title;
//		this.location = location;
//		this.mainQuest = mainQuest;
//		this.sideQuest = sideQuest;
//		this.conditional = conditional;
//		this.startQuest = startQuest;
//	}
	

}
