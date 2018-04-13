using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayOneByOne : MonoBehaviour {

	private List<GameObject> children;
	private float t;

	public void Reset() {
		t = 0;
		children = new List<GameObject> ();
		for (int i = 0; i < transform.childCount; i++) {
			var child = transform.GetChild (i).gameObject;
			children.Add (child);
			child.SetActive (false);
		}
	}

	// Use this for initialization
	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		t += 3 * Time.deltaTime;
		if (t > 3) {
			children [2].SetActive (true);
		} else if (t > 2) {
			children [1].SetActive (true);
		} else if (t > 1) {
			children [0].SetActive (true);
		}
	}
}
