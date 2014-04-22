using UnityEngine;
using System.Collections;

public enum PieceType {
	PieceType_0,
	PieceType_1,
	PieceType_2,
	PieceType_3,
	PieceType_4,
	PieceType_5,
	PieceType_6,
	PieceType_7
}

public class Piece : MonoBehaviour {
	public int bitmask {get; private set;}
	public PieceType pieceType {get; private set;}

	public tk2dSprite sprite;

	void Awake () {
		UpdateForPieceType();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetPieceType(PieceType type) {
		pieceType = type;
		UpdateForPieceType();
	}

	private void UpdateForPieceType() {
		bool xFlip = false;
		float zRotation = 0;
		
		if (pieceType == PieceType.PieceType_0) {
			bitmask = 1 << 0;
			xFlip = false;
			zRotation = 0;
		}
		else if (pieceType == PieceType.PieceType_1) {
			bitmask = 1 << 1;
			xFlip = true;
			zRotation = 90;
		}
		else if (pieceType == PieceType.PieceType_2) {
			bitmask = 1 << 2;
			xFlip = false;
			zRotation = 270;
		}
		else if (pieceType == PieceType.PieceType_3) {
			bitmask = 1 << 3;
			xFlip = true;
			zRotation = 0;
		}
		else if (pieceType == PieceType.PieceType_4) {
			bitmask = 1 << 4;
			xFlip = false;
			zRotation = 180;
		}
		else if (pieceType == PieceType.PieceType_5) {
			bitmask = 1 << 5;
			xFlip = true;
			zRotation = 270;
		}
		else if (pieceType == PieceType.PieceType_6) {
			bitmask = 1 << 6;
			xFlip = false;
			zRotation = 90;
		}
		else if (pieceType == PieceType.PieceType_7) {
			bitmask = 1 << 7;
			xFlip = true;
			zRotation = 180;
		}
		
		sprite.FlipX = xFlip;
		sprite.transform.rotation = Quaternion.Euler(0, 0, zRotation);
	}
}
