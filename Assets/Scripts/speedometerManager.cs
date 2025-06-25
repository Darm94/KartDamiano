using UnityEngine;

public class speedometerManager : MonoBehaviour
{
    [SerializeField] private RectTransform pinRT;
    [SerializeField] private float maxAngle = 270; // note: angle is 0...270

    private void OnEnable()
    {
        KartController.CarSpeed += UpdateSpeed;
    }

    private void OnDisable()
    {
        KartController.CarSpeed -= UpdateSpeed;
    }

    private void UpdateSpeed(float speedRatio)
    {
        pinRT.rotation = Quaternion.Euler(0, 0, -Mathf.Lerp(0, maxAngle, speedRatio));
    }
}
