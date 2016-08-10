/*  Author:     Eric Mouledoux
*   Contatct:   EricMouledoux@gmail.com
*   
*   Usage:
*       Attach script to desired object to rotate
*       Set rotation speed modifier (1 by default)
*       Set desired mouse input to trigger rotation (LMB by default)
*       Pick which asis can rotate (X, Y, Z);
*       Done!
*   
*   Notes:
*       To be used with Unity3D project
*       Must be attached to a UnityEngine.GameObject
*       Will only affect parent object
*       Speed cannot equal 0, but can be less than for inverted rotation
*       If anchored, this object will always look at (set Z to relative position) the anchor object
*/

using UnityEngine;
using System.Collections;

/// <summary>
/// Rotates attached object around its local axis
/// </summary>
public class RotationToMouseTracking : MonoBehaviour
{
    /// <summary>
    /// Called once per frame
    /// </summary>
	void Update()
    {
        if (Input.GetMouseButtonDown(0))    // Waits for the first frame the left mouse button is clicked
        {
            StartCoroutine(MoveToCursor()); // and starts the tracking
        }
    }

    /// <summary>
    /// Reorientates this gameObject based on the movement of the mosuse, while the LMB is held
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToCursor()
    {
        Vector3 initCursorPos = Input.mousePosition;    // The initial position of the mouse

        if (Anchor != null) // If there is an anchor to rotate around
        {
            transform.forward = (Anchor.transform.position - transform.position).normalized;    // Snaps this object to look at (forward) the anchor
            while (Input.GetMouseButton(MouseButton)) // While the MB is being held...
            {
                Vector3 rcp = initCursorPos - Input.mousePosition;  // Relative Cursor Position
                Vector3 np = new Vector3(rcp.y, -rcp.x, 0);         // New relative cursor Position (Inverted X, and no Z)
                np *= (RotationSpeed * Time.deltaTime);             // Modified by speed and time to be used as speed for rotation

                if (RotateX) // If it should rotate around the 'X' axis...
                    transform.RotateAround(Anchor.transform.position, transform.right, np.x);

                if (RotateY)// If it should rotate around the 'Y' axis...
                    transform.RotateAround(Anchor.transform.position, transform.up, np.y);

                np = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);//
                transform.localEulerAngles = np;                                                // Resets Z to account for drifting

                initCursorPos = Input.mousePosition;    // Sets the initial cursor position to the new position
                yield return null;
            }
        }

        else
        {
            while (Input.GetMouseButton(MouseButton)) // While the MB is being held...
            {
                Vector3 rcp = initCursorPos - Input.mousePosition;  // Relative Cursor Position
                Vector3 np = new Vector3(-rcp.y, rcp.x, 0);         // New relative cursor Position (Inverted Y, and no Z)
                np *= (RotationSpeed * Time.deltaTime);             // Modified by speed and time to be used as speed for rotation

                if(RotateX) // If it should rotate around the 'X' axis...
                    transform.RotateAround(transform.position, transform.right, np.x);

                if (RotateY)// If it should rotate around the 'Y' axis...
                    transform.RotateAround(transform.position, transform.up, np.y);

                np = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);//
                transform.localEulerAngles = np;                                                // Resets Z to account for drifting
                
                initCursorPos = Input.mousePosition;    // Sets the initial cursor position to the new position
                yield return null;
            }
        }
    }
    /// <summary>
    /// Modifier to Increse/Decrease/Invert object rotation
    /// </summary>
    [SerializeField] private float RotationSpeed = 1f;
    /// <summary>
    /// Mouse button to be held while rotating object
    /// 0 = LMB (Left)
    /// 1 = RMB (Right)
    /// 2 = MMB (Middle/Wheel)
    /// </summary>
    [SerializeField] private int MouseButton = 0;
    /// <summary>
    /// Ture/False, Should this object rotate around this axis?
    /// </summary>
    [SerializeField] private bool RotateX, RotateY;
    /// <summary>
    /// Object to rotate around instead on inplace (Optional)
    /// </summary>
    [SerializeField] private GameObject Anchor;
}
