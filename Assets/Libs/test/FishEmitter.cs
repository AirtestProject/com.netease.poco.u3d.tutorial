using UnityEngine;
using System.Collections;

public class FishEmitter : MonoBehaviour {

	private float t = 0;
	private float _lastT = -1;
	private GameObject fishPrefeb;
	private GameObject jumpStart;
	private GameObject jumpEnd;

	static string[] fish_prefabs = new string[] {
		"jump_fish_prefab_yellow",
		"jump_fish_prefab_blue",
		"jump_fish_prefab_bomb",
	};

	// Use this for initialization
	void Start () {
		jumpStart = GameObject.Find ("jump_start");
		jumpEnd = GameObject.Find ("jump_end");
	}
	
	// Update is called once per frame
	void Update () {
		int t = (int) Time.time;
		if (t % 5 == 0 && _lastT != t) {
			_lastT = t;

			var prefabName = fish_prefabs[Random.Range (0, fish_prefabs.Length)];
			Debug.Log (prefabName);
			var goPrefab = GameObject.Find (prefabName);
			var fish = GameObject.Instantiate (goPrefab);
			fish.name = prefabName.Substring ("jump_fish_prefab_".Length);
			fish.transform.parent = gameObject.transform;
			fish.transform.position = jumpStart.transform.position;
			fish.GetComponent<ParabolicMovement> ().enabled = true;
		}
	}
}
