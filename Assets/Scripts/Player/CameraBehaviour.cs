using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ajustará a câmera para seguir e focar em um alvo
/// </summary>
public class CameraBehaviour : MonoBehaviour
{

    [Tooltip("O objeto que a câmera deverá focar em")]
    public Transform target;

    [Tooltip("Quão longe a câmera estará do alvo")]
    public Vector3 offset = new Vector3(0, 3.50f, -8);

    /// <summary>
    /// Update é chamado uma vez por frame
    /// </summary>
    private void Update()
    {

        // Verifica se o alvo é um objeto válido
        if (target != null)
        {
            // Fixa a posição para longe do alvo
            transform.position = target.position + offset;

            // Muda a rotação para focar no alvo
            transform.LookAt(target);
        }

    }
}
