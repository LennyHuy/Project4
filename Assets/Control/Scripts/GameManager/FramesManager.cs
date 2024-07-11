using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramesManager : MonoBehaviour
{
    [SerializeField] private int targetFrameRate;
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
