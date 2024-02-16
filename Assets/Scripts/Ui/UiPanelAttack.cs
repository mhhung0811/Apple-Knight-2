using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPanelAttack : MonoBehaviour
{
    public RectTransform squarePanel;
    void Start()
    {
        squarePanel = GetComponent<RectTransform>();
        // Lấy kích thước của màn hình
        float minScreenDimension = Mathf.Min(Screen.width, Screen.height);

        // Tính toán kích thước của panel
        float panelSize = minScreenDimension * 0.5f;

        // Đặt kích thước của panel
        squarePanel.sizeDelta = new Vector2(panelSize, panelSize);

        // Thiết lập anchor của panel
        squarePanel.anchorMin = new Vector2(1, 0); // Anchor min ở góc dưới bên trái
        squarePanel.anchorMax = squarePanel.pivot = squarePanel.anchorMin;
    }
}
