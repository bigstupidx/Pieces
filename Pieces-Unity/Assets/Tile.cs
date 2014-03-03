using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public int bitmask {get; private set;}

	public GameObject piecePrefab;
	public GameObject pieceHolder;

	// Use this for initialization
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
	
	}
}
