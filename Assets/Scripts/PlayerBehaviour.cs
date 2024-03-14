using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerBehaviour : MonoBehaviour
{

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

        // Verifica se o player está se movendo para algum lado
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        rb.AddForce(horizontalSpeed, 0, rollSpeed);
        
    }
}
