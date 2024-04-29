using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerBehaviour : MonoBehaviour
{
    public AudioClip hitSound;
    public AudioClip deathSound;

    /// <summary> 
    /// Referência para o componente Rigidbody
    /// </summary>
    private Rigidbody rb;

    [Tooltip("Quão rápido o player se moverá para a esquerda/direita")]
    [Range(0,10)]
    public float dodgeSpeed = 5;

    [Tooltip("Quão rápido o player se moverá para frente automaticamente")]
    [Range(0,10)]
    public float rollSpeed = 5;

    public enum MobileHorizontalMovement
    {
        Accelerometer,
        ScreenTouch
    }

    [Tooltip("Qual tipo de movimento horizontal deve ser usado")]
    public MobileHorizontalMovement horizontalMovement = MobileHorizontalMovement.Accelerometer;

    // Start é chamado antes do primeiro frame de update
    void Start()
    {
        // Definir o componente Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// FixedUpdate é um local exemplar para
    /// incluir cálculos de física acontecendo
    /// ao longo de um período de tempo.
    /// </summary>
    void FixedUpdate()
    {
        /// Verifica se o player está se movendo para algum lado com o teclado
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        
        /// Verifica se o jogo está rodando no editor da Unity ou em uma build standalone.
        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
            /// Se o mouse estiver pressionado (ou se a tela estiver sendo tocada no celular)
            if (Input.GetMouseButton(0))
            {
                Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                horizontalSpeed = CalculateMovement(screenPos);
            }
        #elif UNITY_IOS || UNITY_ANDROID
            switch (horizontalMovement)
            {
                case MobileHorizontalMovement.Accelerometer:
                    /// Moverá o jogador baseado na direção do acelerômetro do dispositivo
                    horizontalSpeed = Input.acceleration.x * dodgeSpeed;
                    break;
                
                case MobileHorizontalMovement.ScreenTouch:
                    /// Verifica se o Input registrou mais de zero toques na tela
                    if (Input.touchCount > 0)
                    {
                        /// Guarda o primeiro touch detectado
                        Touch touch = Input.touches[0];
                        horizontalSpeed = CalculateMovement(touch.position);
                    }
                    break;
            }
        #endif

        rb.AddForce(horizontalSpeed, 0, rollSpeed);
    }

    /// <summary>
    /// Calculará para onde mover o jogador horizontalmente
    /// </summary>
    /// <param name="screenPos">A posição que o jogador tocou/clicou no screen space</param>
    /// <returns>A direção a mover o jogador no eixo x</returns>
    private float CalculateMovement(Vector3 screenPos)
    {
        /// Captura a referência para a câmera, para conversão entre espaços
        var cam = Camera.main;

        /// Converte a posição do mouse para uma range de 0 a 1
        var viewPos = cam.ScreenToViewportPoint(screenPos);

        float xMove = 0;

        /// Se pressionar o lado direito da tela...
        if (viewPos.x < 0.5f)
        {
            xMove = -1;
        }
        /// Se pressionar o lado esquerdo da tela...
        else
        {
            xMove = 1;
        }

        /// Substitui a horizontalSpeed com o valor próprio para celulares ou mouse
        return xMove * dodgeSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /// Primeiro, checa se colidiu com o obstáculo
        if(collision.gameObject.GetComponent<ObstacleBehaviour>())
        {
            /// Executa a função "PlayClipAtPoint", que toca um som no exato ponto da morte do jogador
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, 1.0f);
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, 1.0f);
        }
    }
}
