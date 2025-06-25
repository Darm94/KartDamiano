using UnityEngine;


public class GoalGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            // Notify player static stats
            PlayerProgress.Instance.RegisterGatePassed();
            gameObject.SetActive(false);

        }
    }
}