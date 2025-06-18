using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour
{
    public GameObject menupanel;
    public GameObject howtoplaypanel;



    void Start()
    {
      menupanel.SetActive(true);
      howtoplaypanel.SetActive(false);


    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        menupanel.SetActive(false);
        howtoplaypanel.SetActive(true);
    }

    public void BackButton()
    {
        menupanel.SetActive(true);
        howtoplaypanel.SetActive(false);
    }


}
