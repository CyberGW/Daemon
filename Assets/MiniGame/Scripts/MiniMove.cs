﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ALL ADDED for the ASSESSMENT 3 took some code from assessment 2's PlayerMovemnt.cs
/// <summary>
/// The movement for the minigame character, includes movement and progression control
/// </summary>
public class MiniMove : MonoBehaviour {
    private float speed = 0.1f;
    private string lastMove = "Idle";
    private bool canMove = true;
    private Animator anim;
    private float xstart;
    private float ystart;
    private GameObject controller;
    /// <summary>the number of lives the player has </summary>
    private int lives = 3;

    // Use this for initialization
    void Start () {
        anim = gameObject.GetComponentInParent<Animator>();
        xstart = transform.position.x;
        ystart = transform.position.y;
        controller = GameObject.Find("CarController");
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //hard restart the player can perform 
        if (Input.GetKeyDown(KeyCode.R))
        {
            controller.GetComponent<CarController>().Restart();
            lives = 3;
            transform.position = new Vector2(xstart, ystart);
    }
        //movement controlls 
        if (Input.GetKey(KeyCode.UpArrow))
        {
            move("Up");
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            move("Down");
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move("Left");
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            move("Right");
        }
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            setIdle();
        }
        //when at the top go to the next level
        if (transform.position.y > 0)
            levelUp();

    }

	/// <summary>
	/// [EXTENSION] - Public function to set <see cref="canMove"/> so that the player's movement can be disabled once the game has been won
	/// and the scene is in process of transitioning 
	/// </summary>
	/// <param name="val">The value to set canMove to</param>
	public void setCanMove (bool val) {
		canMove = val;
	}

    public void OnGUI()
    {
        //draw on the GUI how many lives the player has
        Rect bounds = new Rect(150, 45, 340, 140);
        GUI.Label(bounds, "Lives: " + lives + " /3");
    }

    /// <summary>
    /// The player movement, it takes a direction and moves the player while setting the animation
    /// </summary>
    /// <param name="direction"></param>
    public void move(string direction)


    {
        if (canMove)
        {
            walkAnimation(direction);
            Vector2 translation;
            switch (direction)
            {
                case "Up":
                    translation = Vector2.up;
                    break;
                case "Down":
                    translation = Vector2.down;
                    break;
                case "Left":
                    translation = Vector2.left;
                    break;
                case "Right":
                    translation = Vector2.right;
                    break;
                default:
                    translation = Vector2.zero;
                    break;
            }
            transform.Translate(translation * speed);
            
        }
    }
    /// <summary>
    /// Sets the player to the idle animation
    /// </summary>
    private void setIdle()
    {
        if (lastMove != "Idle")
        {
            anim.SetTrigger("Idle" + lastMove);
            lastMove = "Idle";
        }
    }
    /// <summary>
    /// Sets the player to a walking animation with a gicven direction
    /// </summary>
    /// <param name="direction"> the direction of which to move </param>
    private void walkAnimation(string direction)
    {
        if (lastMove != direction)
        {
            anim.SetTrigger("Walk" + direction);
            lastMove = direction;
        }
    }

	/// <summary>
	/// [EXTENSION] - Added sound effect when player hits car
	/// </summary>
	/// <param name="collision">Collision.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
      //reduce lives by 1 and if less than or equal to zero restart the minigame
        if (collision.tag=="Car")
        {
			SoundManager.instance.playSFX ("horn"); //play sound effect
            lives -= 1; //take level
            if (lives<=0) //if used last live
            {
				//restart minigame, setting lvies to 3
                controller.GetComponent<CarController>().Restart();
                lives = 3;

            }
			//set player back to the starting side of the road
            transform.position = new Vector2(xstart, ystart);
        }
    }
    /// <summary>
    /// Reset player location and call <see cref="CarController.levelUp"/> 
    /// </summary>
    private void levelUp()
    {
        transform.position = new Vector2(xstart, ystart);
		controller.GetComponent<CarController> ().levelUp ();
    }
}

