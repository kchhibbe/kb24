﻿using UnityEngine;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour
{
    private Text fpsText;
    private float deltaTime;

    private void Awake()
    {
        fpsText = GetComponent<Text>();
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
