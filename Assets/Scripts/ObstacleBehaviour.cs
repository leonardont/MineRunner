using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleBehaviour : MonoBehaviour
{

    [Tooltip("How long to wait before restarting the game")]
    public float waitTime = 2.0f;

    private void OnCollisionEnter(Collision collision)
    {
        // Primeiro, checa se colidiu com o jogador.
        if(collision.gameObject.GetComponent<PlayerBehaviour>())
        {
            // Destrói o jogador
            Destroy(collision.gameObject);

            // Chama a função ResetGame depois que o waitTime (tempo de espera) passou
            Invoke("ResetGame", waitTime);
        }
    }

    /// <summary>
    /// Reiniciará o nível atualmente carregado
    /// </summary>
    private void ResetGame()
    {
        // Pega o nome do nível atual
        string sceneName = SceneManager.GetActiveScene().name;

        // Reinicia o nível atual
        SceneManager.LoadScene(sceneName);
    }
    
}
