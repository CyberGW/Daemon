using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

[TestFixture]
public class QuestTest {

	Quest questObject;
	QuestManager qManager;

	[SetUp]
	public void Setup() {
		this.questObject = new Quest ("quest title", "CS", 
			new QuestDef ("beat boss", questTypes.defeatEnemy, "Boss"),
			new QuestDef ("find item", questTypes.gainItem, "Shield"),
			new QuestDef ("time limit", questTypes.inTimeLimit, "200"),
			5, 5);

		this.qManager = new QuestManager ();
		qManager.addQuest (questObject); //QManagerObj.manager...
	}

	[Test]
	public void QuestStarts() {
		//Check quest starts
		Assert.AreEqual (questObject.Completed, questStatues.notStarted); //Assert.AreNotEqual (questObject.Completed, questStatues.completed);
		Assert.AreNotEqual (questObject, qManager.CurrentQuest);
		Assert.AreEqual (questObject, qManager.firstQuest);
		//Check quest updates to current
		qManager.updateCurrentQuest ("CS");
		Assert.AreEqual (questObject, qManager.CurrentQuest);
		Assert.AreNotEqual (qManager.checkQuestCompleted(qManager.CurrentQuest), questStatues.completed);
	}

	[Test]
	public void QuestCompletable() {
		qManager.updateCurrentQuest ("CS");

		//Check quest objectives can be completed
		Assert.False (qManager.conditions [questObject.Main]);
		qManager.logQuestVariable (questTypes.defeatEnemy, questObject.Main.Data);
		Assert.True (qManager.conditions [questObject.Main]);

		Assert.False (qManager.conditions [questObject.Side]);
		qManager.logQuestVariable (questTypes.gainItem, questObject.Side.Data);
		Assert.True (qManager.conditions [questObject.Side]);

		Assert.False (qManager.conditions [questObject.Cond]);
		qManager.conditions [questObject.Cond] = 100 <= int.Parse(questObject.Cond.Data);
		Assert.True (qManager.conditions [questObject.Cond]);
		Assert.AreEqual (qManager.checkQuestCompleted(qManager.CurrentQuest), questStatues.completed);
		qManager.conditions [questObject.Cond] = 300 <= int.Parse(questObject.Cond.Data);
		Assert.False (qManager.conditions [questObject.Cond]);
		Assert.AreEqual (qManager.checkQuestCompleted(qManager.CurrentQuest), questStatues.failed);
		qManager.conditions [questObject.Cond] = 200 <= int.Parse(questObject.Cond.Data);
		Assert.True (qManager.conditions [questObject.Cond]);
		Assert.AreEqual (qManager.checkQuestCompleted(qManager.CurrentQuest), questStatues.completed);
	}



	DataManager data;
	Player playerObject;

	[Test]
	public void QuestAwards() {
		//Initialize player to add exp & money
		this.playerObject = new Player ("Player", 10, 100, 10, 10, 10, 10, 10, 10, 2000, null, new MagicAttack("fireballed", "Fireball", 5, 30), null);
		data = new DataManager(playerObject);
		data.addPlayer (playerObject);
		//Complete quest objectives and check quest awarded
		qManager.updateCurrentQuest ("CS");
		qManager.conditions [questObject.Main] = true;
		qManager.conditions [questObject.Side] = true;
		qManager.conditions [questObject.Cond] = true;
		Debug.Log (qManager.finishQuest ());
	}
}
