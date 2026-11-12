using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public Slider volumeSlider;
    public AudioSource menuMusic;

    void Start()
{
    optionsPanel.SetActive(false);

    float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);

    AudioListener.volume = savedVolume;
    volumeSlider.value = savedVolume;

    menuMusic.loop = true;
    menuMusic.Play();
    Debug.Log("Music playing: " + menuMusic.isPlaying);
Debug.Log("Game volume: " + AudioListener.volume);
}

    public void PlayGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        );
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}