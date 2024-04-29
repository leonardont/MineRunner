using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] musicArray;
    private int previousMusicIndex, currentMusicIndex;

    void Start()
    {
        /// Verifica as preferências do jogador para encontrar um nível de volume
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        
        PlayMusic();
    }

    void PlayMusic()
    {
        /// Define um índice aleatório dentro do array de músicas para tocar.
        currentMusicIndex = Random.Range(0,musicArray.Length);

        /// Verifica se o índice da música atual é igual ao índice da música anterior, para evitar repetição
        /// Caso o índice não seja igual ao da música anterior, ele continua igual
        /// Caso o índice seja igual ao da música anterior, ele gera um novo índice para que não toque a mesma música novamente
        currentMusicIndex = currentMusicIndex == previousMusicIndex ? currentMusicIndex = (Random.Range(0,musicArray.Length)) : currentMusicIndex;

        /// Define o clipe de áudio dentro do audio source como o índice anterior
        audioSource.clip = musicArray[currentMusicIndex];

        /// Toca o clipe de áudio
        audioSource.Play();

        /// Define o índice da música que está tocando agora como o índice prévio, para evitar repetição da música na próxima chamada
        currentMusicIndex = previousMusicIndex;

        /// Invoca essa mesma função inteira no fim da música atual + 2 segundos
        Invoke("PlayMusic", (audioSource.clip.length + 2f));
    }

}
