using UnityEngine;
using UnityEngine.InputSystem;

public class FollowPlayerCamera : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform target;

    [Header("Distancia")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 8f;

    [Header("Altura")]
    [SerializeField] private float height = 2.5f;

    [Header("Rotación")]
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float smoothSpeed = 8f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 2f;

    private Vector2 lookInput;

    private float yaw;
    private float pitch = 15f;

    public void OnLook(InputAction.CallbackContext context)
    {
       
        // El mouse se maneja directamente para ambas cámaras.
        if (context.control.device is Mouse)
            return;

        lookInput = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
      
        // este es para el MOUSE
       

        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            lookInput = mouseDelta;
        }


        // El manejo de la rotación de la cámara se hace en LateUpdate para que se ejecute después de que el jugador se haya movido.


        yaw += lookInput.x * sensitivity * Time.deltaTime;
        pitch -= lookInput.y * sensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -20f, 60f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

       
        // ZOOM
      

        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;

            distance -= scroll * zoomSpeed * Time.deltaTime;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

    
        // POSICIÓN
        

        Vector3 offset = rotation * new Vector3(0, height, -distance);

        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}