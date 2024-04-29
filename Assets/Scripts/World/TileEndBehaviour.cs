using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gerencia o spawn de um novo tile e a destruição
/// do atual uma vez que o jogador chega ao fim
/// </summary>
public class TileEndBehaviour : MonoBehaviour
{

    [Tooltip("Quanto tempo esperar antes de destruir o tile depois de chegar ao fim")]
    /// ATENÇÃO! CASO O "destroyTime" SEJA MENOR OU IGUAL AO "waitTime" ENCONTRADO NO SCRIPT DO OBSTÁCULO (ObstacleBehaviour.cs), O JOGADOR NÃO RENASCERÁ QUANDO MORRER, 
    /// POIS O OBSTÁCULO DESAPARECERÁ ANTES DA FUNÇÃO "ResetGame" SER CHAMADA!
    public float destroyTime = 4.0f;

    private void OnTriggerEnter(Collider other)
    {
        /// Primeiro, checa se colidiu com o jogador
        if(other.gameObject.GetComponent<PlayerBehaviour>())
        {
            /// Se sim, spawna um novo tile
            var gm = GameObject.FindObjectOfType<GameManager>();
            gm.SpawnNextTile();

            // Destrói o tile inteiramente depois de um pequeno atraso
            Destroy(transform.parent.gameObject, destroyTime);
        }
    }
}
