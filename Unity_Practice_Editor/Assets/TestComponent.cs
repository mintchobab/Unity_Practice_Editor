using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent : MonoBehaviour
{
    [Button]
    public void MethodExcuteButton()
    {
        Debug.LogWarning("Public Method");
    }

    [Button]
    private void PrintString()
    {
        Debug.LogWarning("Private Method");
    }
}
