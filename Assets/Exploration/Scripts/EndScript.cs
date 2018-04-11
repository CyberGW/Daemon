using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to manage the end credits screen
/// </summary>
public class EndScript : MonoBehaviour {

	/// <summary>
	/// [EXTENSION] - Start the <see cref="creditScroller"/> coroutine as soon as scene loads 
	/// </summary>
	public void Start() {
		StartCoroutine (creditScroller ());
	}

	/// <summary>
	/// [EXTENSION] - Scroll the credits up each frame, and call <see cref="End"/> when finished 
	/// </summary>
	/// <returns>The scroller.</returns>
	private IEnumerator creditScroller() {
		RectTransform transform = gameObject.GetComponent<RectTransform> (); //get rectTransform object
		while (transform.anchoredPosition.y < 1174) { //while some credits text is on screen
			//move the credits up 1 unit per frame
			transform.anchoredPosition = new Vector2 (transform.anchoredPosition.x, transform.anchoredPosition.y + 1);
			yield return null; //wait till next frame
		}
		End (); //call end when finished
	}

    /// <summary>
    /// Ends the scene taking you to the menu
    /// </summary>
	public void End() {
        SceneChanger.instance.loadLevel("mainmenu1",new Vector2(0,0));
    }

}
