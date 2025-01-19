using UnityEngine;

public class PositionFixerFixer : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<PositionFixer>().enabled = false;
        GetComponent<PositionFixer>().enabled = true;
    }
}
