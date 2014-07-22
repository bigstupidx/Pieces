using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	public static Camera gameCam;

	// Use this for initialization
	void Awake () {
		gameCam = gameObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
