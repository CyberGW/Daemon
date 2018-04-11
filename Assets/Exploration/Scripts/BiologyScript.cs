using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [EXTENSION] - A script to handle taking a player and turning them into a Gorilla for the Biology level
/// </summary>
public class BiologyScript : MonoBehaviour {

	public static BiologyScript instance = null;
	DataManager data;

	/// <summary>
	/// Taken the first player in the party and swap them for a Gorilla
	/// </summary>
	void Start () {
		
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else { 
			Destroy (gameObject);
		}

		data = PlayerData.instance.data;
		if (GlobalFunctions.instance.takenPlayer == null) { //if a player hasn't already been taken
			GlobalFunctions.instance.takenPlayer = data.Players [0]; //take the first party member
			//replace with Gorilla
			data.Players [0] = new Player ("Gorilla", 15, 100, 40, 20, 10, 10, 40, 30, 0, null,
				new MagicAttack ("dabbed at", "dab like DK", 2, 20),
				new RaiseAttack ("buff up", "increase attack", 3, 0.15f),
				(Texture2D)Resources.Load ("Gorilla", typeof(Texture2D)));
		}
		Debug.Log (GlobalFunctions.instance.takenPlayer.Name);
	}
	
	/// <summary>
	/// Swap the Gorilla back with the player that was originally taken
	/// </summary>
	public void restorePlayer() {
		int index = data.getPlayerIndex ("Gorilla"); //find index of Gorilla
		data.Players [index] = GlobalFunctions.instance.takenPlayer; //swap with taken player
	}

}
