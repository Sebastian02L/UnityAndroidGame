using System;
using System.Collections;
using UnityEngine;

public class PositionFixer : MonoBehaviour
{
    public Camera mainCamera;
    public float offset;

    void Update()
    {
        Vector3 screenPosition = new Vector3(0, Screen.height, 0);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane + 2.0f)
        );
        transform.position = new Vector3(worldPosition.x + offset, transform.position.y, transform.position.z);
    }
}
