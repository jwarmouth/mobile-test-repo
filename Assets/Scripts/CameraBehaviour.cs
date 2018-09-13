using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will Adjust the camera to follow and face a target
/// </summary>
public class CameraBehaviour : MonoBehaviour {

    [Tooltip("What should the object be looking at?")]
    public Transform target;

    [Tooltip("How offset will the camera be?")]
    public Vector3 offset = new Vector3 (0, 3, -6);

	/// <summary>
    /// Update is called once per frame
    /// </summary>
	void Update () {
		// Check if target is a valid object
        if (target != null)
        {
            // Set our position to be offset from the target
            Vector3 pos = target.position + offset;
            pos.x = 0;
            transform.position = pos;

            // Change the rotation to face the target
            transform.LookAt(target);
        }
	}
}
