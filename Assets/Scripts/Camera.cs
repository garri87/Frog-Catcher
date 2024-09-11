using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform player;            // Referencia al jugador
    public float distance = 10f;        // Distancia vertical desde el jugador
    public float height = 5f;           // Altura de la cámara
    public float smoothSpeed = 0.125f;  // Velocidad de suavizado

    private Vector3 offset;

    void Start()
    {
        // Calcular el desplazamiento inicial basado en la distancia y la altura
        offset = new Vector3(0, height, -distance);
    }

    void LateUpdate()
    {
        // Calcular la posición deseada de la cámara con respecto al jugador
        Vector3 desiredPosition = player.position + offset;

        // Suavizar el movimiento de la cámara entre su posición actual y la posición deseada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Establecer la posición suavizada
        transform.position = smoothedPosition;

        // Asegurarse de que la cámara mire al jugador
        transform.LookAt(player.position);
    }
}
