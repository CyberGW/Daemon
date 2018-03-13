using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologyScript : MonoBehaviour {

	public static BiologyScript instance = null;
	DataManager data;

	// Use this for initialization
	void Start () {
		
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else { 
			Destroy (gameObject);
		}

		data = PlayerData.instance.data;
		if (GlobalFunctions.instance.takenPlayer == null) {
			GlobalFunctions.instance.takenPlayer = data.Players [0];
			data.Players [0] = new Player ("Gorilla", 15, 100, 40, 20, 10, 10, 40, 30, 0, null,
				new MagicAttack ("dabbed at", "dab like DK", 2, 20),
				new RaiseAttack ("buff up", "increase attack", 3, 0.15f),
				(Texture2D)Resources.Load ("Gorilla", typeof(Texture2D)));
		}
		Debug.Log (GlobalFunctions.instance.takenPlayer.Name);
	}
	
	public void restorePlayer() {
		int index = data.getPlayerIndex ("Gorilla");
		data.Players [index] = GlobalFunctions.instance.takenPlayer;
	}

}
