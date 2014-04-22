using UnityEngine;
using System.Collections;

public class Helper : MonoBehaviour {
	public tk2dCamera sceneCamera;

	public tk2dCameraAnchor anchorLowerLeft;
	public tk2dCameraAnchor anchorLowerRight;
	public tk2dCameraAnchor anchorUpperLeft;
	public tk2dCameraAnchor anchorUpperRight;

	[HideInInspector]public static Helper instance;

	void Awake() {
		DontDestroyOnLoad(this.gameObject);
		instance = this;
	}

// screen width and height
	public int screenWidthNative {
		get {return sceneCamera.nativeResolutionWidth;}
	}
	
	public int screenHeightNative {
		get {return sceneCamera.nativeResolutionHeight;}
	}


// screen extents
	public Rect screenExtentsNative {
		get {return sceneCamera.NativeScreenExtents;}
	}


// screen center
	public Vector2 screenCenterNative {
		get {return new Vector2(screenWidthNative / 2f, screenHeightNative / 2f);}
	}
}