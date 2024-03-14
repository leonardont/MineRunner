using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gerencia o spawn de um novo tile e a destruição
/// do atual uma vez que o jogador chega ao fim
/// </summary>
public class TileEndBehaviour : MonoBehaviour
{

    [Tooltip("How much time to wait before destroying" + "o tile depois de chegar ao fim")]
    public float destroyTime = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        // Primeiro, checa se colidiu com o jogador
        if(other.gameObject.GetComponent<PlayerBehaviour>())
        {
            // Se sim, spawna um novo tile
            var gm = GameObject.FindObjectOfType<GameManager>();
            gm.SpawnNextTile();

            // Destrói o tile inteiramente depois de um pequeno atraso
            Destroy(transform.parent.gameObject, destroyTime);
        }
    }
}
