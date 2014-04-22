using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public bool touched = false;

	public int bitmask {get; private set;}

	public GameObject piecePrefab;
	public GameObject pieceHolder;

	private static Tile[] tiles;

	public static Tile[] Tiles {
		get {
			if (tiles == null) tiles = new Tile[9];

			return tiles;
		}
	}

	void Start () {
		bitmask = 0;

		for (int i = 0; i < 8; i++) {
			if (Random.value < 0.5f) {
				Piece newPiece = ((GameObject)Instantiate(piecePrefab, Vector2.zero, Quaternion.identity)).GetComponent<Piece>();
				newPiece.SetPieceType((PieceType)i);
				newPiece.transform.parent = pieceHolder.transform;
				newPiece.transform.localPosition = Vector3.zero;
				bitmask |= newPiece.bitmask;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = GameCamera.instance.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			
			if(Physics.Raycast(ray, out hit)) {
				foreach (Tile tile in Tile.Tiles) {
					if (hit.transform == tile.transform) {
						tile.transform.localScale *= tile.touched ? 0.5f : 2;
						tile.touched = !tile.touched;
					}
				}
			}
		}
	}
}
