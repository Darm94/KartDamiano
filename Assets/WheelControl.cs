using UnityEngine;

public class WheelControl : MonoBehaviour
{
    [SerializeField] Transform wheelModel;

    private WheelCollider _wheelCollider;
    public WheelCollider WheelCollider => _wheelCollider;

    [SerializeField] bool steerable; // "true"
    [SerializeField] bool motorized; // "true"

    Vector3 position;
    Quaternion rotation;

    public bool Steerable => steerable;
    public bool Motorized => motorized;

    // Start is called before the first frame update
    private void Start()
    {
        _wheelCollider = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        _wheelCollider.GetWorldPose(out position, out rotation);
        if (wheelModel)
        {
            // wheelModel.transform.position = position;
            wheelModel.transform.localRotation = Quaternion.Euler(rotation.eulerAngles.x, 0, 0);
        }
    }
}
