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

    public enum MobileHorizMovement
    {
        Accelerometer,
        ScreenTouch
    }

    [Tooltip("Qual tipo de movimento horizontal deve ser usado")]
    public MobileHorizMovement horizMovement = MobileHorizMovement.Accelerometer;

    [Header("Propriedades de escalonamento")]

    [Tooltip("O tamanho mínimo (em unidades Unity) que o jogador deve ter")]
    public float minScale = 0.5f;

    [Tooltip("O tamanho máximo (em unidades Unity) que o jogador deve ter")]
    public float maxScale = 3.0f;

    /// <summary>
    /// A escala atual do jogador
    /// </summary>
    private float currentScale = 1;

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

        // Verifica se o player está se movendo para algum lado com o teclado
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        
        // Verifica se o jogo está rodando no editor da Unity ou em uma build standalone.
        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
            // Se o mouse estiver pressionado (ou se a tela estiver sendo tocada no celular)
            if (Input.GetMouseButton(0))
            {
                Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                TouchObjects(screenPos);
            }
        #elif UNITY_IOS || UNITY_ANDROID
            switch (horizMovement)
            {
                case MobileHorizMovement.Accelerometer:
                    // Moverá o jogador baseado na direção do acelerômetro do dispositivo
                    horizontalSpeed = Input.acceleration.x * dodgeSpeed;
                    break;
                
                case MobileHorizMovement.ScreenTouch:
                    // Verifica se o Input registrou mais de zero toques na tela
                    if (Input.touchCount > 0)
                    {
                        // Guarda o primeiro touch detectado
                        Touch touch = Input.touches[0];
                        horizontalSpeed = CalculateMovement(touch.position);
                        TouchObjects(touch.position);
                        ScalePlayer();
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
        // Captura a referência para a câmera, para conversão entre espaços
        var cam = Camera.main;

        // Converte a posição do mouse para uma range de 0 a 1
        var viewPos = cam.ScreenToViewportPoint(screenPos);

        float xMove = 0;

        // Se pressionar o lado direito da tela...
        if (viewPos.x < 0.5f)
        {
            xMove = -1;
        }
        // Se pressionar o lado esquerdo da tela...
        else
        {
            xMove = 1;
        }

        // Substitui a horizontalSpeed com o valor próprio para celulares ou mouse
        return xMove * dodgeSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Primeiro, checa se colidiu com o obstáculo
        if(collision.gameObject.GetComponent<ObstacleBehaviour>())
        {
            // Executa a função "PlayClipAtPoint", que toca um som no exato ponto da morte do jogador
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, 1.0f);
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, 1.0f);
        }
    }

    /// <summary>
    /// Mudará a escala do jogador através de eventos de touch (toque) de zoom in e out
    /// </summary>
    private void ScalePlayer()
    {
        /// Deve ter dois touches para verificar se está mexendo na escala do objeto
        if (Input.touchCount != 2)
        {
            return;
        }
        else
        {
            // Guarda os touches detectados
            Touch touch0 = Input.touches[0];
            Touch touch1 = Input.touches[1];

            Vector2 t0Pos = touch0.position;
            Vector2 t0Delta = touch0.deltaPosition;

            Vector2 t1Pos = touch1.position;
            Vector2 t1Delta = touch1.deltaPosition;

            // Calcula a posição anterior de cada touch no frame
            Vector2 t0Prev = t0Pos - t0Delta;
            Vector2 t1Prev = t1Pos - t1Delta;

            // Calcula a distância (ou magnitude)d entre os touches em cada frame
            float prevTDeltaMag = (t0Prev - t1Prev).magnitude;
            float tDeltaMag = (t0Pos - t1Pos).magnitude;

            // Calcula a diferença nas distâncias entre cada frame
            float deltaMagDiff = prevTDeltaMag - tDeltaMag;

            // Mantém a mudança consistente, não sendo influenciada pelo framerate
            float newScale = currentScale;
            newScale -= (deltaMagDiff * Time.deltaTime);

            // Garante que o novo valor é válido
            newScale = Mathf.Clamp(newScale, minScale, maxScale);

            // Atualiza a escala do jogador
            transform.localScale = Vector3.one * newScale;

            // Define a escala atual para o próximo frame 
            currentScale = newScale;
        }
    }

    /// <summary>
    /// Determinará se o jogador está tocando um objeto e, caso esteja, chamará eventos
    /// </summary>
    /// <param name="screenPos">A posição do touch no espaço da tela</param>
    private static void TouchObjects(Vector2 screenPos)
    {
        // Converte a posição em um raio
        Ray touchRay = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        // Cria uma LayerMask que colidirá com todos os canais possíveis
        int layerMask = ~0;

        // O jogador está tocando um objeto com colisor?
        if (Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            // Chama a função PlayerTouch se existe em um componente fixado à esse objeto
            hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Determinará se o jogador está tocando um objeto e, caso esteja, chamará eventos
    /// </summary>
    /// <param name="touch">O evento touch</param>
    private static void TouchObjects(Touch touch)
    {
        // Converte a posição em um raio
        Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        // Cria uma LayerMask que colidirá com todos os canais possíveis
        int layerMask = ~0;

        // O jogador está tocando um objeto com colisor?
        if (Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            // Chama a função PlayerTouch se existe em um componente fixado à esse objeto
            hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }
    }

}
