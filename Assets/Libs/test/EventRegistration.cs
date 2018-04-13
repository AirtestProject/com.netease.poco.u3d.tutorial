using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EventRegistration : MonoBehaviour
{
	private string currentPlayItemName = "beginPanel";
	private Dictionary<string, GameObject> playPanels = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Start ()
	{
		GameObject.Find ("btn_start").GetComponent<Button> ().onClick.AddListener (select_level);
		GameObject.Find ("btn_back").GetComponent<Button> ().onClick.AddListener (go_back);

		GameObject.Find ("basic").GetComponent<Button> ().onClick.AddListener (strat_playing_basic);
		GameObject.Find ("drag_and_drop").GetComponent<Button> ().onClick.AddListener (start_playing_drag_and_drop);
		GameObject.Find ("list_view").GetComponent<Button> ().onClick.AddListener (start_playing_list_view);
		GameObject.Find ("local_positioning").GetComponent<Button> ().onClick.AddListener (start_playing_local_positioning);
		GameObject.Find ("wait_ui").GetComponent<Button> ().onClick.AddListener (start_playing_wait_ui);
		GameObject.Find ("wait_ui2").GetComponent<Button> ().onClick.AddListener (start_playing_wait_ui2);

		// register panel entries
		playPanels.Add ("beginPanel", GameObject.Find ("beginPanel"));
		playPanels.Add ("levelSelect", GameObject.Find ("levelSelect"));
		playPanels.Add ("playBasic", GameObject.Find ("playBasic"));
		playPanels.Add ("playDragAndDrop", GameObject.Find ("playDragAndDrop"));
		playPanels.Add ("playListView", GameObject.Find ("playListView"));
		playPanels.Add ("playLocalPositioning", GameObject.Find ("playLocalPositioning"));
		playPanels.Add ("playWaitingForUI", GameObject.Find ("playWaitingForUI"));
		playPanels.Add ("playWaitingForUI2", GameObject.Find ("playWaitingForUI2"));

		playPanels ["levelSelect"].SetActive (false);
		playPanels ["playBasic"].SetActive (false);
		playPanels ["playDragAndDrop"].SetActive (false);
		playPanels ["playListView"].SetActive (false);
		playPanels ["playLocalPositioning"].SetActive (false);
		playPanels ["playWaitingForUI"].SetActive (false);
		playPanels ["playWaitingForUI2"].SetActive (false);
	}

	void strat_playing_basic()
	{
		switchPanel ("playBasic");
	}
	void start_playing_drag_and_drop()
	{
		switchPanel ("playDragAndDrop");

		// reset stars
		var stars = GameObject.FindGameObjectsWithTag ("star");
		var x = -0.342f * Screen.width;
		var y = 0.185f * Screen.height;
		foreach (var star in stars) {
			star.GetComponent<TestDragAndDrop> ().Reset (new Vector3 (x, y));
			x += 0.179f * Screen.width;
		}

		// reset score
		GameObject.Find ("scoreVal").GetComponent<Text> ().text = "0";
	}
	void start_playing_list_view()
	{
		switchPanel ("playListView");
	}
	void start_playing_local_positioning()
	{
		switchPanel ("playLocalPositioning");
	}
	void start_playing_wait_ui()
	{
		switchPanel ("playWaitingForUI");
	}
	void start_playing_wait_ui2()
	{
		var display = playPanels ["playWaitingForUI2"].GetComponentInChildren<DisplayOneByOne> ();
		display.Reset ();
		switchPanel ("playWaitingForUI2");
	}

//	
//	{
//		var gamePanel = GameObject.Find (playPanelName).GetComponent<CanvasGroup> ();
//		var bp = GameObject.Find ("levelSelect");
//		var cg = bp.GetComponent<CanvasGroup> ();
//		while (cg.alpha > 0) {
//			cg.alpha -= 0.1f;
//			gamePanel.alpha += 0.1f;
//			yield return new WaitForSeconds (0.05f);
//		}
//		cg.alpha = 0;
//		cg.blocksRaycasts = false;
//		cg.interactable = false;
//		gamePanel.alpha = 1;
//		gamePanel.blocksRaycasts = true;
//		gamePanel.interactable = true;
//	}

	void select_level()
	{
		switchPanel ("levelSelect");
	}
	void go_back()
	{
		if (currentPlayItemName.Equals ("beginPanel")) {
			return;
		} else if (currentPlayItemName.Equals ("levelSelect")) {
			switchPanel ("beginPanel");
		} else {
			switchPanel ("levelSelect");
		}
	}

	void switchPanel(string panelName)
	{
		if (panelName.Equals (currentPlayItemName)) {
			return;
		}
		show (panelName);
		hide (currentPlayItemName);
		currentPlayItemName = panelName;
	}

	void hide(string panelName) 
	{
		var go = playPanels [panelName];
		go.SetActive (false);
		var cg = go.GetComponent<CanvasGroup> ();
		cg.alpha = 0;
		cg.blocksRaycasts = false;
		cg.interactable = false;
	}

	void show(string panelName)
	{
		var go = playPanels [panelName];
		go.SetActive (true);
		var cg = go.GetComponent<CanvasGroup> ();
		cg.alpha = 1;
		cg.blocksRaycasts = true;
		cg.interactable = true;
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}

