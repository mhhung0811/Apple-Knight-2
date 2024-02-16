using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPanelPause : MonoBehaviour
{
    private RectTransform squarePanel;
    void Start()
    {
        squarePanel = GetComponent<RectTransform>();  

        float minScreenDimension = Mathf.Min(Screen.width, Screen.height);

        float panelSize = minScreenDimension * 0.15f;

        squarePanel.sizeDelta = new Vector2 (panelSize, panelSize);

        squarePanel.anchorMin = new Vector2(1, 1);
        squarePanel.anchorMax = new Vector2(1, 1);
        squarePanel.pivot = new Vector2(1, 1);
    }
}
