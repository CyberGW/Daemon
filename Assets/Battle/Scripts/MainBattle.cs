﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for handing a battle scene. Uses <see cref="BattleManager"/>, but handles turn order and execution as well
/// as updating the display appropriately </summary>
public class MainBattle : MonoBehaviour {

	//Objects
	private GameObject globalData;
	public BattleManager manager;
	private Player[] playerArray;
	private Button attackButton;
	private Button playerButton;
	private Button runButton;
	private Text textBox;
	//Battle Manager References
	/// <summary>
	/// Set to public
	/// </summary>
	public Player player;
	private Enemy enemy;
	private int moneyReward;
	private Item itemReward;
	private Player[] originalCopy;
	//Local Variables
	private string text;
	public bool playerDied;
	/// <summary>
	/// Variable to determine if a gorilla is doing a team-damaging move, to play sound effect and update bars appropriately
	/// </summary>
	private bool gorillaMove;
	//Test Enemy
	private Enemy enemyObject;
	//Moves
	private CharacterMove playerMove;
	private CharacterMove enemyMove;
	private bool moveChosen;
	//UI
	private GameObject attacksPanel;
	private GameObject playerStats;
	private GameObject enemyStats;
	private StatsScript playerHealthBar;
	private StatsScript playerMagicBar;
	private StatsScript enemyHealthBar;
	private StatsScript enemyMagicBar;
	private StatsScript expBar;
	private Image playerSprite;
	private Image enemySprite;
	//Scene Management
	private GameObject playerCamera;
	//Music
	private AudioClip BGM;
	private AudioClip victory;


	/// <summary>
	/// Includes finding game objects, setting references and changing background music
	/// [EXTENSION] - Log the player initally being sent into battle
	/// 			- Create an original copy of all the player stats to be restored to revert stat changes
	/// </summary>
	void Start () {
		
		//Find Objects
		attacksPanel = GameObject.Find("BattleCanvas").transform.Find("AttacksPanel").gameObject;
		playerStats = GameObject.Find("PlayerStats");
		enemyStats = GameObject.Find ("EnemyStats");
		attackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();
		playerButton = GameObject.Find ("PlayersButton").GetComponent<Button> ();
		runButton = GameObject.Find ("RunButton").GetComponent<Button> ();
		setButtonsInteractable (true);
		textBox = GameObject.Find ("TextBox").transform.Find ("Text").GetComponent<Text> ();
		playerSprite = GameObject.Find ("PlayerImage").GetComponent<Image> ();
		enemySprite = GameObject.Find ("EnemyImage").GetComponent<Image> ();


		//Setup Object references
		playerArray = PlayerData.instance.data.Players;
		enemyObject = GlobalFunctions.instance.getEnemy ();
		moneyReward = GlobalFunctions.instance.getMoney ();
		itemReward = GlobalFunctions.instance.getItem ();

		//create a copy of player stats before battle
		originalCopy = createOriginalCopy (playerArray);
		manager = new BattleManager (playerArray[0], enemyObject, moneyReward);
		player = manager.Player;
		enemy = manager.Enemy;
		Texture2D image;
		image = enemy.Image;
		enemySprite.sprite = Sprite.Create (image, new Rect (0.0f, 0.0f, image.width, image.height), new Vector2 (0.5f, 0.5f));
		image = player.Image;
		playerSprite.sprite = Sprite.Create (image, new Rect (0.0f, 0.0f, image.width, image.height), new Vector2 (0.5f, 0.5f));

		//Log player being used
		QManagerObj.manager.logQuestVariable(questTypes.onlyOneCharacter, player.Name);

		//setup references and inital displays of all bars
		expBar = playerStats.transform.Find ("Exp").GetComponent<StatsScript> ();
		playerHealthBar = playerStats.transform.Find("Health").GetComponent<StatsScript> ();
		playerMagicBar = playerStats.transform.Find ("Magic").GetComponent<StatsScript> ();
		enemyHealthBar = enemyStats.transform.Find("Health").GetComponent<StatsScript> ();
		enemyMagicBar = enemyStats.transform.Find ("Magic").GetComponent<StatsScript> ();
		expBar.setUpDisplay (player.Exp, player.ExpToNextLevel);
		playerHealthBar.setUpDisplay (player.Health, 100);
		enemyHealthBar.setUpDisplay (enemy.Health, 100);
		playerMagicBar.setUpDisplay (player.Magic, player.MaximumMagic);
		enemyMagicBar.setUpDisplay (enemy.Magic, enemy.MaximumMagic);
		//Setup local variables
		moveChosen = false;
		playerDied = false;

		//Change Music
		BGM = Resources.Load("Audio/battle", typeof(AudioClip)) as AudioClip;
		SoundManager.instance.playBGM(BGM);
		victory = Resources.Load ("Audio/victory", typeof(AudioClip)) as AudioClip;

	}
	
	/// <summary>
	/// Update this instance.
	/// If a move is chosen, then call <see cref="playerThenEnemy"/> or <see cref="enemyThenPlayer"/> dependent upon
	/// <see cref="BattleManager.playerFirst()"/> 
	/// </summary>
	void Update () {
		if (moveChosen) {
			if (manager.playerFirst()) {
				StartCoroutine (playerThenEnemy ());
			} else {
				StartCoroutine (enemyThenPlayer ());
			}
			moveChosen = false;
		}
	}		

	/// <summary>
	/// Performs the player's turn, then the enemy's\n
	/// Re-enables attack button afterwards
	/// </summary>
	/// <returns>Coroutine functions to perform the turns</returns>
	private IEnumerator playerThenEnemy () {
		yield return StartCoroutine (performTurn(playerMove));
		if (!manager.battleWon()) {
			Debug.Log (player.Name);
			enemyMove = manager.enemyMove (enemy, player);
			yield return StartCoroutine (performTurn(enemyMove));
			setButtonsInteractable (true);
		}
	}

	/// <summary>
	/// Performs the enemy's turn, then the player's\n
	/// Re-enables attack button afterwards
	/// </summary>
	/// <returns>Coroutine functions to perform the turns</returns>
	private IEnumerator enemyThenPlayer() {
		enemyMove = manager.enemyMove (enemy, player);
		yield return StartCoroutine (performTurn(enemyMove));
		if (!manager.playerFainted()) {
			yield return StartCoroutine (performTurn(playerMove));
			setButtonsInteractable (true);
		}
	}

	/// <summary>
	/// Performs the turn, updating text display and health and magic bars.
	/// Afterwards, checks to see whether the player has won or lost
	/// </summary>
	/// <returns>Coroutine functions</returns>
	/// <param name="move">The move to  be performed</param>
	private IEnumerator performTurn(CharacterMove move) {
		int previousHealth = move.Target.Health;
		int previousMagic = move.User.Magic;
		move.performMove ();
		textBox.text = move.User.Name + " " + move.Text + " " + move.Target.Name;
		if (manager.WasCriticalHit && (move is StandardAttack || move is MagicAttack)) {
			textBox.text += "\nCritical Hit!";
		}

		//Update bars and data
		if (move is SwitchPlayers) {
			updateToNewPlayer ();
		}
		if (!gorillaMove) {
			yield return updateBars (move, previousHealth, previousMagic);
		} else {
			textBox.text = "The Gorilla went mad and attacked " + move.Target.Name;
			SoundManager.instance.playSFX ("gorilla");
			yield return new WaitForSeconds (1.5f);
			gorillaMove = false;
		}
		if (move.Target is Enemy) {
			StartCoroutine( checkIfPlayerWon ());
		} else {
			StartCoroutine( checkIfPlayerLost ());
		}
	}

	private IEnumerator updateBars(CharacterMove move, int previousHealth, int previousMagic) {
		if (move.User is Player) {
			StartCoroutine(playerMagicBar.updateDisplay (previousMagic, move.User.Magic));
		} else { //if user is Enemy
			StartCoroutine(enemyMagicBar.updateDisplay (previousMagic, move.User.Magic));
		}
		if (move.Target is Player) {
			yield return playerHealthBar.updateDisplay (previousHealth, move.Target.Health);
		} else { //if target is Enemy
			yield return enemyHealthBar.updateDisplay( previousHealth, move.Target.Health);
		}
	}

	/// <summary>
	/// Checks if the player has won\n
	/// If they have, exp is given and shown on screen, before saving player data, adding money, adding the item and ending the battle
	/// [EXTENSION] - Log the enemy defeated, call <see cref="PlayerData.expShare"/> 
	/// 			- Give half exp to all other players
	/// 			- Restore the original stats of the players, before levelling up
	/// </summary>
	/// <returns>Coroutine function to update exp bar</returns>
	private IEnumerator checkIfPlayerWon() {
		if (manager.battleWon()) {
			SoundManager.instance.playBGM(victory);
			enemySprite.gameObject.SetActive (false);
			//restore stats from before battle
			playerArray = restoreOriginalStats(playerArray);
			//Reassign player to restored copy before giving exp
			player = playerArray [0];
			//Wait a frame before changing button states
			yield return null;
			setButtonsInteractable (false);
			yield return StartCoroutine (updateExp(enemy.ExpGiven));
			Debug.Log (player.Level);
			playerArray [0] = player;
			PlayerData.instance.data.Players = playerArray;
			PlayerData.instance.data.giveExpToAll (enemy.ExpGiven / 2, player.Name); //give half exp to all other players
			PlayerData.instance.data.Money += manager.money;
			if (itemReward != null) {
				PlayerData.instance.data.addItem (itemReward);
			}
			Debug.Log ("Money: " + PlayerData.instance.data.Money);
			GlobalFunctions.instance.endBattle ();
			QManagerObj.manager.logQuestVariable (questTypes.defeatEnemy, enemy.Name); //log that the enemy has been defeated
		}
	}

	/// <summary>
	/// Updates the saved and displayed exp
	/// </summary>
	/// <returns>Coroutine function to update exp bar</returns>
	/// <param name="totalExp">The total exp the player has gained</param>
	private IEnumerator updateExp(int totalExp) {
		Debug.Log (player.Name);
		Debug.Log ("totalExp: " + totalExp);
		yield return new WaitForSeconds (1f);
		int gainedExp;
		int remainingExp = totalExp;
		Debug.Log ("remainingExp: " + remainingExp);
		Debug.Log ("playerExp: " + player.Exp);
		Debug.Log ("ExpToNextLevel: " + player.ExpToNextLevel);
		while ( (player.Exp + remainingExp) >= player.ExpToNextLevel) {
			Debug.Log ("In While");
			gainedExp = player.ExpToNextLevel - player.Exp;
			Debug.Log ("gainedExp: " + gainedExp);
			remainingExp -= gainedExp;
			Debug.Log ("remainingExp: " + remainingExp);
			yield return StartCoroutine (updateExpHelper (gainedExp, true));
			expBar.setUpDisplay (0, player.ExpToNextLevel);
		}
		if (remainingExp > 0) {
			Debug.Log ("In If condition");
			gainedExp = remainingExp;
			Debug.Log ("gainedExp: " + gainedExp);
			yield return StartCoroutine (updateExpHelper (gainedExp, false));
		}
	}

	/// <summary>
	/// A helper function for <see cref="updateExp"/>  that performs an exp increase within a single level interval.
	/// Updates text display if player levels up
	/// </summary>
	/// <returns>Coroutines to update exp display and add a delay before scene closes</returns>
	/// <param name="gainedExp">Exp gained within this level interval</param>
	/// <param name="levelledUp">If set to <c>true</c> indicates the player is about to level up.</param>
	private IEnumerator updateExpHelper(int gainedExp, bool levelledUp) {
		Debug.Log ("In Helper, levelledUp: " + levelledUp);
		yield return StartCoroutine (expBar.updateDisplay (player.Exp, player.Exp + gainedExp));
		player.gainExp (gainedExp);
		if (levelledUp) {
			textBox.text = player.Name + " grew to level " + player.Level + "!";
			SoundManager.instance.playSFX ("interact");
		}
		yield return new WaitForSeconds (1.5f);
	}

	/// <summary>
	/// Creates a deep copy of the original player array
	/// </summary>
	/// <returns>The deep copy</returns>
	/// <param name="players">The array of players to be copied</param>
	private Player[] createOriginalCopy (Player[] players) {
		Player[] copy = new Player[6];
		for (int i = 0; i < copy.Length; i++) {
			if (players [i] == null) {
				copy [i] = null;
			} else {
				copy [i] = players [i].Clone ();
			}
		}
		return copy;
	}

	/// <summary>
	/// Restores all players to their original stats, except from the changes in health and magic
	/// </summary>
	/// <returns>The array of players with all stat changes reverted</returns>
	/// <param name="newCopy">The new copy of the player array, with stat changes</param>
	private Player[] restoreOriginalStats (Player[] newCopy) {
		for (int i = 0; i < newCopy.Length; i++) {
			if (originalCopy [i] != null) {
				originalCopy [i].Health = newCopy [i].Health;
				originalCopy [i].Magic = newCopy [i].Magic;
			}
		}
		return originalCopy;
	}

	/// <summary>
	/// Checks if player lost. If so and no player's left return to main menu, reactivating the player so the menu script can find it.
	/// If so and player's left, open switch player menu. Otherwise pass.
	/// [EXTENSION] - Log if a player has died
	/// </summary>
	private IEnumerator checkIfPlayerLost() {
		if (manager.playerFainted()) { //if a player has fainted
			yield return null;
			setButtonsInteractable (false); //stop player from using buttons
			if (PlayerData.instance.data.playersAlive() == 0) { //if no more players left, so game over
				textBox.text = "All players have fainted! Game Over.";
				yield return new WaitForSeconds (2);
				GlobalFunctions.instance.player.SetActive (true); //Make player active so it can be found again in main menu
				//go back to main menu
				SoundManager.instance.playBGM(GlobalFunctions.instance.previousBGM);
				SceneChanger.instance.loadLevel ("mainmenu1");
			} else { //if other players alive
				playerDied = true;
				textBox.text = player.Name + " fainted!";
				yield return new WaitForSeconds (3);
				//load the switch player menu
				SceneManager.LoadSceneAsync ("SwitchPlayer", LoadSceneMode.Additive);
				QManagerObj.manager.logQuestVariable (questTypes.noFainting, ""); //log that a player has fainted
			}
		}
	}

	/// <summary>
	/// Sets all buttons interactivity state
	/// </summary>
	/// <param name="val">Enable buttons if set to <c>true</c>, <c>false</c> otherwise.</param>
	private void setButtonsInteractable(bool val) {
		attackButton.interactable = val;
		playerButton.interactable = val;
		//So both val and canRunAway need to be true to activate run button
		runButton.interactable = val && GlobalFunctions.instance.canRunAway;
	}

	/// <summary>
	/// Setup a standard attack move for the player
	/// </summary>
	public void standardAttack() {
		playerMove = new StandardAttack (manager, player, enemy);
		prepareTurn ();
	}

	/// <summary>
	/// Setup the first special move for the player
	/// [EXTENSION] - Log that a special move has been used
	/// </summary>
	public void special1() {
		QManagerObj.manager.logQuestVariable (questTypes.noSpecialMoves); //log special move use
		player.Special1.setUp (manager, player, enemy);
		playerMove = player.Special1;
		prepareTurn ();
	}

	/// <summary>
	/// Setup the second special move for the player
	/// [EXTENSION] - Log that a special move has been used
	/// </summary>
	public void special2() {
		QManagerObj.manager.logQuestVariable (questTypes.noSpecialMoves); //log the special move use
		player.Special2.setUp (manager, player, enemy);
		playerMove = player.Special2;
		prepareTurn ();
	}

	/// <summary>
	/// Switchs the players.
	/// [EXTENSION] - Log the new player that is being switched in
	/// 			- Also changed order of elements in <see cref="originalCopy"/> to match 
	/// </summary>
	/// <param name="playerIndex">Index of new player in <see cref="DataManager.players"/> array </param>
	public void switchPlayers(int playerIndex) {
		Player newPlayer = PlayerData.instance.data.Players [playerIndex];
		QManagerObj.manager.logQuestVariable (questTypes.onlyOneCharacter, newPlayer.Name); //log that more than one player has been used
		playerMove = new SwitchPlayers (manager, player, newPlayer); //setup playermove
		PlayerData.instance.data.swapPlayers (0, playerIndex); //switch the players
		//update originalCopy to match
		Player temp = originalCopy [0];
		originalCopy [0] = originalCopy [playerIndex];
		originalCopy [playerIndex] = temp;

		prepareTurn();
	}

	/// <summary>
	/// Updates references and re-setup bars to this next player
	/// </summary>
	public void updateToNewPlayer() {
		this.player = manager.Player;
		Debug.Log (player.Name);
		//Update references
		playerHealthBar.setUpDisplay(player.Health, 100);
		playerMagicBar.setUpDisplay (player.Magic, player.MaximumMagic);
		//update sprite shown
		playerSprite.sprite = Sprite.Create (player.Image, new Rect (0.0f, 0.0f, player.Image.width, player.Image.height),
			new Vector2 (0.5f, 0.5f));
		Debug.Log (player.ExpToNextLevel);
		expBar.setUpDisplay (player.Exp, player.ExpToNextLevel);
		
	}

	/// <summary>
	/// Called by any of <see cref="standardAttack"/>, <see cref="special1"/> or <see cref="special2"/>
	/// Uses <see cref="BattleManager.enemyMove"/> to decide on the enemy's move, and hides attack panel and makes
	/// attack button unclickable
	/// [EXTENSION] - If Gorilla, randomly swap to an attack that damages a team member 25% of the time
	/// </summary>
	private void prepareTurn() {
		if (!playerDied) { //so long as the player has not just died, so a move should go ahead
			if (player.Name == "Gorilla") { //if the player is the gorilla
				if (Random.value <= 0.25) { //on a 25% chance
					Player alivePlayer = PlayerData.instance.data.getAlivePlayer (); //get an alive player
					if (alivePlayer == null) { //if there is no other player alive
						playerMove = new StandardAttack (manager, player, player); //gorilla attacks themself
					} else { //if there is at least one other player alive
						playerMove = new StandardAttack (manager, player, alivePlayer); //attack that player
					}
					gorillaMove = true; //signal that a gorilla move is occuring so sound effect can be played
				}
			}
			moveChosen = true;
			attacksPanel.SetActive (false);
			setButtonsInteractable (false);
			BattleButtons.panelActive = false;
		} else { //if player just died
			//Perform the switch character move
			StartCoroutine(performTurn(playerMove));
			playerDied = false;
			setButtonsInteractable (true);
		}
	}


    /// <summary>
	/// Run away from battle if <see cref="BattleManager.ranAway"/> returns true.
	/// Otherwise display appropriate text and make sure run cannot be selected again that move
    /// </summary>
	public void ranAway() {
        if (manager.ranAway(player.Speed,enemy.Speed)) {
			StartCoroutine (runAway ());
        } else {
			textBox.text = "You failed to run away";
			runButton.interactable = false;
        }
    }

	/// <summary>
	/// If <see cref="ranAway"/> return true, then  display message, disable attack button and after 2 seconds
	/// call <see cref="GlobalFunctions.endBattle"/> 
	/// </summary>
	/// <returns>The away.</returns>
	private IEnumerator runAway() {
		textBox.text = "You ran from the battle";
		attackButton.interactable = false;
		yield return new WaitForSeconds (2);
		GlobalFunctions.instance.endBattle ();
	}		

}
