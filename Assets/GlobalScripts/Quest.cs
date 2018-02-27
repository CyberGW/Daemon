using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest {

	public string title;
	public string location;
	public delegate bool questCond();
	private questCond main;
	private questCond side;
	private questCond cond;
	//Conditional Parameters
	public string itemName;
	public int timeLimit;
	public string enemyTarget;
	public string NPCTarget;
	//Reward Parameters
	public int money;
	public int exp;

	public Quest (string title, string location, questCond main, questCond side, questCond cond, int moneyReward, int expReward,
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
		return main() && side() && cond();
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
