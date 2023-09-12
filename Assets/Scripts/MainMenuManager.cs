using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] bool startWithCutscene;
    [SerializeField] Slideshow slideshow;
    [SerializeField] GameObject menuPanel;
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SoundManager.Instance.PlaySoundCont("Tutorial Bgm"); // for tutorial scene
        }
    }
    public void PlayGame()
    {
        if(startWithCutscene)
        {
            menuPanel.SetActive(false);
            slideshow.StartSlideShow();
            SoundManager.Instance.PlaySoundCont("Level 1 Music");
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                SceneManager.LoadScene(1);
            }
        }
        
    }

    public void SkipScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnMM()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene(0);
        }
    }

}
