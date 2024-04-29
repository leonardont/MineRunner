using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    private UIManagerScript UIManager;
    public GameObject playerDeathParticles;

    private void OnCollisionEnter(Collision collision)
    {
        /// Primeiro, checa se colidiu com o jogador
        if(collision.gameObject.GetComponent<PlayerBehaviour>())
        {
            /// Destrói o jogador
            Destroy(collision.gameObject);

            Instantiate(playerDeathParticles, collision.gameObject.transform.position, transform.rotation);

            UIManager = GameObject.FindWithTag("UI Manager").GetComponent<UIManagerScript>();
            UIManager.gameOver();
        }
    }
}
