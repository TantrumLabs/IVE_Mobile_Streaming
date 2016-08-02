using UnityEngine;
using System.Collections;

public class CameraToMouseTracking : MonoBehaviour
{
    void Awake()
    {
        MainCamera = Camera.main;
    }

	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine("MouseToCamera");
        }
	}

    IEnumerator MouseToCamera()
    {
        Vector3 initCursorPos = Input.mousePosition;
        Vector3 initCameraRot = MainCamera.transform.localEulerAngles;

        while(Input.GetMouseButton(0))
        {
            ///Relative Cursor Position
            Vector3 rcp = initCursorPos - Input.mousePosition;
            /// New Camera Position
            Vector3 ncp = new Vector3(-rcp.y, rcp.x, 0);

            MainCamera.transform.localEulerAngles += ncp * (CameraSpeed * Time.deltaTime);

            ncp = new Vector3(
                MainCamera.transform.localEulerAngles.x,
                MainCamera.transform.localEulerAngles.y,
                0);

            MainCamera.transform.localEulerAngles = ncp;

            initCursorPos = Input.mousePosition;
            yield return null;
        }
    }

    public float CameraSpeed = 1f;

    private Camera MainCamera;
    private Vector3 m_mousePos;
}
