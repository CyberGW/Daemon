using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script describing a healing station, where all player's health and magic are restored
/// </summary>
public class HealingStation : MonoBehaviour {

	private DialogueScript dManager;
	private PlayerMovement movementScript;
	/// <summary>
	/// The text to display when interacted with
	/// </summary>
	private string[] text;
	/// <summary>
	/// Holds whether the user is in range of the station, so that it is not continually triggered when the user
	/// is within range
	/// [CHANGE] - Make the variable static, so if a menu item is selected and scene is loaded, this value does not change.
	/// This stops the healing station being retriggered when returning from a menu screen.
	/// </summary>
	private static bool inRange = false;

	// Use this for initialization
	void Start () {
		dManager = FindObjectOfType<DialogueScript> ();
		movementScript = FindObjectOfType<PlayerMovement> ();
		text = new string[1];
		text [0] = "All teammates have been healed and magic restored";
	}

	/// <summary>
	/// When collides with the player and <see cref="inRange"/> is <c>false</c> , trigger functionality.
	/// This involves setting <see cref="inRange"/> to <c>true</c>,
	/// playing sound effect, displaying text and calling <see cref="healPlayers"/>  
	/// [EXTENSION] - Log that healing station has been used, don't activate when menu open
	/// </summary>
	/// <param name="other">The object this has collided with, triggering functionality when it's the player</param>
	void OnTriggerStay2D(Collider2D other) {
		//if collided with player, player wasn't already on space, no dialogue screen is active and the menu isn't open
		if (other.gameObject.name == "Player" && !inRange && !dManager.dialogueActive && !SceneChanger.instance.menuOpen) {
			inRange = true; //set inRange to true so won't be triggered again without first stepping off space
			SoundManager.instance.playSFX ("interact");
			if (dManager == null) {
				dManager = FindObjectOfType<DialogueScript> (); //locate dialoguescript if needed
			}
			dManager.showDialogue (text); //display text
			movementScript.setCanMove (false); //stop player moving when text is shown
			healPlayers (); //heal the players
			QManagerObj.manager.logQuestVariable (questTypes.noHealingStations); //log healing station has been used
		}
	}

	/// <summary>
	/// When the player leaves the collider, set <see cref="inRange"/> to false so that the functionality
	/// can be triggered again when the player re-enters</summary>
	/// <param name="other">The object this has collided with, triggering functionality when it's the player</param>
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.name == "Player") {
			inRange = false;
		}
	}

	/// <summary>
	/// Loops through <see cref="DataManager.Players"/> setting all instantiated player's health back to 100 and
	/// all magic stats back to the specified <see cref="Players.MaximumMagic"/> value  
	/// </summary>
	public void healPlayers() {
		Player[] playerArray = PlayerData.instance.data.Players;
		foreach (Player player in playerArray) {
			if (player != null) {
				player.Health = 100;
				player.Magic = player.MaximumMagic;
			}
		}
	}
		
}
