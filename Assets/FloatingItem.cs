using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90;
    [SerializeField] private float floatSpeed = 0.25f;
    [SerializeField] private float floatDelta = 0.25f;
    private Vector3 _startPosition;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        transform.position = _startPosition + floatDelta * Mathf.Sin(Time.time * floatSpeed) * transform.up;
    }
}
