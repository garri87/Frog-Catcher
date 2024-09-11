using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform player;            // Referencia al jugador
    public float distance = 10f;        // Distancia vertical desde el jugador
    public float height = 5f;           // Altura de la c�mara
    public float smoothSpeed = 0.125f;  // Velocidad de suavizado

    private Vector3 offset;

    void Start()
    {
        // Calcular el desplazamiento inicial basado en la distancia y la altura
        offset = new Vector3(0, height, -distance);
    }

    void LateUpdate()
    {
        // Calcular la posici�n deseada de la c�mara con respecto al jugador
        Vector3 desiredPosition = player.position + offset;

        // Suavizar el movimiento de la c�mara entre su posici�n actual y la posici�n deseada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Establecer la posici�n suavizada
        transform.position = smoothedPosition;

        // Asegurarse de que la c�mara mire al jugador
        transform.LookAt(player.position);
    }
}
