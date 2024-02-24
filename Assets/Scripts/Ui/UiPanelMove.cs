using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPanelMove: MonoBehaviour
{
    public RectTransform squarePanel; // Kéo và thả RectTransform của panel vào đây trong Inspector

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
        squarePanel.anchorMin = Vector2.zero - new Vector2(-0.5f,panelSize*0/Screen.height)*0.1f; // Anchor min ở góc dưới bên trái
        squarePanel.anchorMax = squarePanel.pivot = squarePanel.anchorMin; // Anchor max cũng ở góc dưới bên trái
    }
}
