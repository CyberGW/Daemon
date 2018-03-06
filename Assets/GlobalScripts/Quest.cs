using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of all quest types, to be used as QManagerObj dictionary key
/// </summary>
public enum questTypes {gainItem, noFainting, defeatEnemy, talkToNPC, inTimeLimit, noSpecialMoves, onlyOneCharacter, noHealingStations};

/// <summary>
/// Class defining one quest condition (part of a sequence of 3 in Quest)
/// </summary>
public class QuestDef {

	/// <summary>
	/// A pair relating the type of quest to a string storing specific data (e.g. the name of the specific enemy
	/// you must defeat for the defeatEnemy type)
	/// </summary>
	private KeyValuePair<questTypes, string> pair;
	private string desc;

	public QuestDef(string desc, questTypes type, string data = "" ) {
		this.pair = new KeyValuePair<questTypes, string> (type, data);
		this.desc = desc;
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

	public string Desc {
		get {
			return this.desc;
		}
	}

}

/// <summary>
/// A class to store a whole quest, being a sequence of 3 QuestDefs
/// Also stores a name of the quest, the location at which it's active along with the rewards for completing
/// the quest (being an amount of money and amount of exp)
/// </summary>
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

	/// <summary>
	/// Determines if the quest has been completed or not
	/// </summary>
	/// <returns><c>true</c>, if the dictionary values for all 3 parts of the quest return true, <c>false</c> otherwise.</returns>
	public bool questCompleted() {
		return QManagerObj.manager.conditions[main] && QManagerObj.manager.conditions[side] && QManagerObj.manager.conditions[cond];
	}	

}
