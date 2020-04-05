using UnityEngine;
using System.Collections;

public class ZoomCopy : MonoBehaviour {

    
    Camera _cam;
    public Camera camera
    {
        get
        {
            if (_cam == null)
                _cam = GetComponent<Camera>();
            return _cam;
        }
    }
	// Update is called once per frame
	void LateUpdate () {
        camera.orthographicSize = Camera.main.orthographicSize;
	}
}
