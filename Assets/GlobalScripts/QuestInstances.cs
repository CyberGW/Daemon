using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class storing all the pre-defined quests that the user can choose from
/// </summary>
public class QuestInstances : MonoBehaviour{

	public static Quest CS = new Quest ("BSc Computer Science", "CS", 
		new QuestDef ("Beat Colin's Moustache", questTypes.defeatEnemy, "Haskell Simulation"),
		new QuestDef ("Find the Shield", questTypes.gainItem, "Shield"),
		new QuestDef ("Don't let any character's faint", questTypes.noFainting),
		10, 200);
	
	public static Quest TFTV = new Quest ("BSc Film and Television Production", "TFTV",
		new QuestDef ("Beat the Boss", questTypes.defeatEnemy, "Boss"),
		new QuestDef ("Find the Hammer", questTypes.gainItem, "Hammer"),
		new QuestDef ("Don't use any healing stations", questTypes.noHealingStations),
		10, 200);
	
	public static Quest PHY = new Quest ("BSc Physics", "PHY",
		new QuestDef ("Beat the boss", questTypes.defeatEnemy, "BattleCharacter"),
		new QuestDef ("Defy Physics", questTypes.talkToNPC, "RoomGuy"),
		new QuestDef ("Complete in under 200 seconds", questTypes.inTimeLimit, "200"),
		10, 200);
	
	public static Quest LAW = new Quest ("LLB Law", "LAW",
		new QuestDef ("Beat the boss", questTypes.defeatEnemy, "Boss"),
		new QuestDef ("Talk to Bartholomew", questTypes.talkToNPC, "Bartholomew"),
		new QuestDef ("Don't use any special moves", questTypes.noSpecialMoves),
		10, 200);
	
	public static Quest MUS = new Quest ("BA Music", "MUS",
		new QuestDef ("Beat the boss", questTypes.defeatEnemy, "Boss"),
		new QuestDef ("Take down a Goose", questTypes.defeatEnemy, "Goose"),
		new QuestDef ("Only use Jamie", questTypes.onlyOneCharacter, "Jamie"),
		10, 200);
	
	public static Quest BIO = new Quest ("BSc Biology", "BIO",
		new QuestDef ("Beat the boss", questTypes.defeatEnemy, "Boss"),
		new QuestDef ("Get a Rabbit Carcass", questTypes.gainItem, "Rabbit Carcass"),
		new QuestDef ("Don't let the Gorilla faint", questTypes.noFainting, "Gorilla"),
		10, 20);

}
