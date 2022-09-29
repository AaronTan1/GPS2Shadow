using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class joystickManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image joystickImgBg;
    private Image joystickImg;
    private Vector2 posInput;

    void Start()
    {
        joystickImgBg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickImgBg.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out posInput))
        {
            //solved problem of fast movement speed : dividing the position of the drag by the size of the img
            posInput.x = posInput.x / (joystickImgBg.rectTransform.sizeDelta.x);  
            posInput.y = posInput.y / (joystickImgBg.rectTransform.sizeDelta.y);
            /*Debug.Log(posInput.x.ToString() + "/" + posInput.y.ToString());*/ //displays coordinate val

            // normalize
            if(posInput.magnitude > 1.0f)
            {
                posInput = posInput.normalized;
            }

            //joystick move
            joystickImg.rectTransform.anchoredPosition = new Vector2(
                posInput.x * (joystickImgBg.rectTransform.sizeDelta.x / 4), 
                posInput.y * (joystickImgBg.rectTransform.sizeDelta.y / 4));
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        posInput = Vector2.zero;
        joystickImg.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float inputHorizontal()
    {
        if (posInput.x != 0)
            return posInput.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float inputVertical()
    {
        if (posInput.x != 0)
            return posInput.y;
        else
            return Input.GetAxis("Vertical");
    }

}
