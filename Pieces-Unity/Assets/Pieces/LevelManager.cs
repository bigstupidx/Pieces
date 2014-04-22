using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public GameObject tilePrefab;
	public int tileMargin = 50;
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < 9; i++) {
			Tile.Tiles[i] = ((GameObject)Instantiate(tilePrefab)).GetComponent<Tile>();
		}

		Vector2 screenCenter = Helper.instance.screenCenterNative;

		Tile.Tiles[0].transform.position = new Vector3(screenCenter.x - tileMargin,	screenCenter.y - tileMargin, 	0);
		Tile.Tiles[1].transform.position = new Vector3(screenCenter.x,				screenCenter.y - tileMargin, 	0);
		Tile.Tiles[2].transform.position = new Vector3(screenCenter.x + tileMargin,	screenCenter.y - tileMargin, 	0);
		Tile.Tiles[3].transform.position = new Vector3(screenCenter.x - tileMargin,	screenCenter.y,				 	0);
		Tile.Tiles[4].transform.position = new Vector3(screenCenter.x,				screenCenter.y,					0);
		Tile.Tiles[5].transform.position = new Vector3(screenCenter.x + tileMargin,	screenCenter.y,					0);
		Tile.Tiles[6].transform.position = new Vector3(screenCenter.x - tileMargin,	screenCenter.y + tileMargin,	0);
		Tile.Tiles[7].transform.position = new Vector3(screenCenter.x,				screenCenter.y + tileMargin,	0);
		Tile.Tiles[8].transform.position = new Vector3(screenCenter.x + tileMargin,	screenCenter.y + tileMargin, 	0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
