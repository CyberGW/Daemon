using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest {

	public string title;
	public string location;
	/// The dictionary key representing the varaible condition for the main part of the quest
	private string main;
	/// The dictionary key representing the varaible condition for side quest part
	private string side;
	/// The dictionary key representing the varaible condition for extra quest condition
	private string cond;
	//Conditional Parameters
	public string itemName;
	public int timeLimit;
	public string enemyTarget;
	public string NPCTarget;
	//Reward Parameters
	public int money;
	public int exp;

	public Quest (string title, string location, string main, string side, string cond, int moneyReward, int expReward,
		string itemName = "", int timeLimit = -1, string enemyTarget = "", string NPCTarget = "")
	{
		this.title = title;
		this.location = location;
		this.main = main;
		this.side = side;
		this.cond = cond;
		this.money = moneyReward;
		this.exp = expReward;
		this.itemName = itemName;
		this.timeLimit = timeLimit;
		this.enemyTarget = enemyTarget;
		this.NPCTarget = NPCTarget;
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
