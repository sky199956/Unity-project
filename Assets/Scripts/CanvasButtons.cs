using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;

    private void Start()
    {
        if (PlayerPrefs.GetString("music") == "No" && gameObject.name == "Volume")
        {
            GetComponent<Image>().sprite = musicOff;
        }
    }
    public void RestartGame()
    {
        if (PlayerPrefs.GetString("music") != "No")
        GetComponent<AudioSource>().Play();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        SceneManager.LoadScene("Shop");
    }
    public void CloseShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        SceneManager.LoadScene("SampleScene");
    }
    public void LoadInsta()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        Application.OpenURL("https://www.instagram.com/iladubkov56440/?hl=ru");
    }
    public void MusicWork()
    {
        if (PlayerPrefs.GetString("music") == "No")
        {
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<Image>().sprite = musicOn;
        } else
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
}
