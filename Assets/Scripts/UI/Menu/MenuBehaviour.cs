using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuBehaviour : MonoBehaviour
{
    public AudioClip click;
    AudioSource audioSource;

    [SerializeField] Slider volumeSlider;
    [SerializeField] TMPro.TMP_Dropdown graphicsDropdown;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        Debug.Log(PlayerPrefs.GetInt("graphics"));

        /// Verifica se o jogador já alterou o volume
        if(!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1);
            LoadData();
        }
        else
        {
            LoadData();
        }

        /// Verifica se o jogador já alterou os gráficos
        if(!PlayerPrefs.HasKey("graphics"))
        {
            PlayerPrefs.SetInt("graphics", 1);
            LoadData();
        }
        else
        {
            LoadData();
        }
    }

    /// Toca som de click para feedback sonoro da UI
    public void PlayClickSound()
    {
        audioSource.PlayOneShot(click);
    }

    /// Define o volume dos sons e músicas do jogo
    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveData();
    }

    /// Define o nível de qualidade gráfica do jogo
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        SaveData();
    }

    /// Sai do jogo
    public void QuitGame()
    {
        Application.Quit();
    }
    
    /// Carrega opções do jogador
    /// Volume e gráficos
    private void LoadData()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        graphicsDropdown.value = PlayerPrefs.GetInt("graphics");
    }

    /// Salva opções do jogador
    private void SaveData()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.SetInt("graphics", graphicsDropdown.value);
    }
}
