using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverImg;
    public GameObject textObj;
    TMP_Text text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverImg.SetActive(true);
        hoverImg.transform.position = transform.position;
        text = textObj.GetComponent<TMP_Text>();
        text.color = new Color32(197, 67, 67, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverImg.SetActive(false);
        text = textObj.GetComponent<TMP_Text>();
        text.color = new Color32(57, 20, 38, 255);
    }
}
