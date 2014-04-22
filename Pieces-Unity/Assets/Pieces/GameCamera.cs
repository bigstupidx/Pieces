using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	public static Camera instance;

	// Use this for initialization
	void Awake () {
		instance = gameObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
