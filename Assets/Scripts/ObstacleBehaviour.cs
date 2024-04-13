using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleBehaviour : MonoBehaviour
{

    [Tooltip("Quanto esperar para reiniciar o jogo")]
    // ATENÇÃO! CASO O "waitTime" SEJA MAIOR OU IGUAL AO "destroyTime" ENCONTRADO NO SCRIPT DO TILE END (TileEndBehaviour.cs), O JOGADOR NÃO RENASCERÁ QUANDO MORRER, POIS O OBSTÁCULO DESAPARECERÁ ANTES DA FUNÇÃO "ResetGame" SER CHAMADA!
    public float waitTime = 2.0f;

    [Tooltip("Efeito de explosão para mostrar quando tocado")]
    public GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        // Primeiro, checa se colidiu com o jogador
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
    
    /// <summary>
    /// Se o objeto recebe um toque do jogador, spawna partículas de explosão
    /// </summary>
    private void PlayerTouch()
    {
        if (explosion != null)
        {
            var particles = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(particles, 1.0f);
        }

        Destroy(this.gameObject);
    }

}
