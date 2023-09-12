using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Slideshow : MonoBehaviour
{
    private Stack<Image> slideStack = new Stack<Image>();
    [SerializeField] float slideInterval;
    [SerializeField] float fadeFrame;
    [SerializeField] float fadeTime;
    private void Awake()
    {
        foreach(RectTransform tr in transform)
        {
            slideStack.Push(tr.GetComponent<Image>());
        }
    }
    public void StartSlideShow()
    {
        Image slide = slideStack.Pop();
        StartCoroutine(SlideShow(slide));
    }

    IEnumerator SlideShow(Image slide)
    {
        float fadeAmt = 1f / fadeFrame;
        float fadeInterval = fadeTime / fadeFrame;
        float fadeCounter = fadeFrame;

        yield return new WaitForSeconds(slideInterval);
        while (fadeCounter > 0)
        {
            slide.color = new Color(1, 1, 1, (slide.color.a - fadeAmt));
            yield return new WaitForSeconds(fadeInterval);
            fadeCounter--;
        }
        if (slideStack.Count > 0) {

            StartCoroutine(SlideShow(slideStack.Pop())); 
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
