using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayQuest : MonoBehaviour {

	public Text name;
	public Text main;
	public Text side;
	public Text cond;
	public Button selectButton;
	private string currentQuest = "";
	private int chosen = 0;

	// Use this for initialization
	void Start () {		
	}

	public void showDesc (string key) {
		if (currentQuest == "") {
			selectButton.interactable = true;
		}
		Quest quest = QuestInstances.defs [key];
		name.text = "Name: " + quest.title;
		main.text = "Main: " + quest.Main.Desc;
		side.text = "Side: " + quest.Side.Desc;
		cond.text = "Cond: " + quest.Cond.Desc;
		currentQuest = key;
	}

	public void selectQuest () {
		Debug.Log("Chosen");
		GameObject.Find(currentQuest).GetComponent<Button>().interactable = false;
		currentQuest = "";
		chosen++;
		selectButton.interactable = false;
		if (chosen >= 2) {
			Debug.Log ("Chosen Two");
		}
	}

	void Update () {
		
	}
	


}
