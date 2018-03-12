using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologyScript : MonoBehaviour {

	DataManager data = PlayerData.instance.data;
	Player takenPlayer;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		takenPlayer = data.Players [0];
		data.Players [0] = new Player ("Gorilla", 15, 100, 40, 20, 10, 10, 40, 30, 0, null,
			new MagicAttack("dabbed at", "dab like DK", 2, 20),
			new RaiseAttack("buff up", "increase attack", 3, 0.15f),
			(Texture2D)Resources.Load ("Gorilla", typeof(Texture2D)));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
