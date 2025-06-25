using UnityEngine;

public class RotateBlades : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 1, 0); // asse di rotazione (es: Y)
    [SerializeField] private float rotationSpeed = 200f; // gradi al secondo

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
