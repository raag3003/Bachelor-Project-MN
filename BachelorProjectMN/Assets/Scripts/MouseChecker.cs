using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseChecker : MonoBehaviour
{

    void Update()
    {
        // Count all pointer devices (mouse, touchpad, pen)
        int pointerCount = InputSystem.devices.Count(d => d is Pointer);

        // If more than 1 pointer exists, assume external mouse is connected
        Debug.Log(pointerCount);
    }
}
