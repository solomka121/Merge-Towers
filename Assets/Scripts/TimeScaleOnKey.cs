using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleOnKey : MonoBehaviour
{
    [SerializeField] private float timeScaleModified = 5f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = timeScaleModified;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Time.timeScale = 1f;
        }
    }
}
