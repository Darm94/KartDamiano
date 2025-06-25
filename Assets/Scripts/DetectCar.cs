using UnityEngine;

public class DetectCar : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer _meshRenderer;
    private float _appearDelay = 5;
    private AudioSource _audioSource;

    [SerializeField] ParticleSystem particleSystem;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag, other.gameObject);

        if (other.CompareTag("Car"))
        {
            _collider.enabled = false;
            _meshRenderer.enabled = false;
            _audioSource.Play();

            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();

            Invoke(nameof(ReenableCoin), _appearDelay);
        }
    }

    private void ReenableCoin()
    {
        _collider.enabled = true;
        _meshRenderer.enabled = true;
        particleSystem.gameObject.SetActive(false);
    }
}
