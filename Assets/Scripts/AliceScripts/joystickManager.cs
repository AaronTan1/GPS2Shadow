using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class joystickManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private static joystickManager _instance;
    public static joystickManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else 
            _instance = this;
    }
    
    private Image joystickImgBg;
    private Image joystickImg;
    private Vector2 posInput;

    private void Start()
    {
        joystickImgBg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickImgBg.rectTransform, eventData.position,
                eventData.pressEventCamera, out posInput)) return;
        
        //solved problem of fast movement speed : dividing the position of the drag by the size of the img
        var sizeDelta = joystickImgBg.rectTransform.sizeDelta;
        posInput.x /= (sizeDelta.x);  
        posInput.y /= (sizeDelta.y);
        /*Debug.Log(posInput.x.ToString() + "/" + posInput.y.ToString());*/ //displays coordinate val

        // normalize
        if (posInput.magnitude > 1.0f)
            posInput = posInput.normalized;

        //joystick move
        joystickImg.rectTransform.anchoredPosition = new Vector2(posInput.x * (sizeDelta.x / 4), posInput.y * (sizeDelta.y / 4));
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

    public float InputHorizontal()
    {
        return posInput.x != 0 ? posInput.x : Input.GetAxis("Horizontal");
    }

    public float InputVertical()
    {
        return posInput.x != 0 ? posInput.y : Input.GetAxis("Vertical");
    }

}
