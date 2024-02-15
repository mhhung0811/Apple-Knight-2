using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPanelInfo : MonoBehaviour
{
    private RectTransform squarePanel;
    void Start()
    {
        squarePanel = GetComponent<RectTransform>();

        float minScreenDimension = Mathf.Min(Screen.width, Screen.height);

        float sizePanel = minScreenDimension * 0.2f;

        squarePanel.sizeDelta = new Vector2 (sizePanel*2, sizePanel);

        squarePanel.anchorMin = new Vector2(0, 1);
        squarePanel.anchorMax = new Vector2(0, 1);
        squarePanel.pivot = new Vector2(0, 1);
    }
}
