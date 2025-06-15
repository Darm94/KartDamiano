using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class WheelSkid : MonoBehaviour {
	
	[SerializeField]
	Rigidbody rb;
	[SerializeField]
	Skidmarks skidmarksController;
	
	[SerializeField][Range(0.01f,100f)] float skidFXSpeed = 0.5f; // Min side slip speed in m/s to start showing a skid
	[SerializeField][Range(0.01f,100f)] float maxSkidIntensity = 20.0f; // m/s where skid opacity is at full intensity
	[SerializeField][Range(0.01f,100f)] float wheelSlipMultiplier = 10.0f; // For wheelspin. Adjust how much skids show

	WheelCollider _wheelCollider;
	WheelHit _wheelHitInfo;

	int _lastSkid = -1; // Array index for the skidmarks controller. Index of last skidmark piece this wheel used
	float _lastFixedUpdateTime;
	
	protected void Awake() {
		_wheelCollider = GetComponent<WheelCollider>();
		_lastFixedUpdateTime = Time.time;
	}

	protected void FixedUpdate() {
		_lastFixedUpdateTime = Time.time;
	}

	protected void LateUpdate() {
		if (_wheelCollider.GetGroundHit(out _wheelHitInfo))
		{
			// Check sideways speed

			// Gives velocity with +z being the car's forward axis
			Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
			float skidTotal = Mathf.Abs(localVelocity.x);

			// Check wheel spin as well

			float wheelAngularVelocity = _wheelCollider.radius * ((2 * Mathf.PI * _wheelCollider.rpm) / 60);
			float carForwardVel = Vector3.Dot(rb.linearVelocity, transform.forward);
			float wheelSpin = Mathf.Abs(carForwardVel - wheelAngularVelocity) * wheelSlipMultiplier;

			// NOTE: This extra line should not be needed and you can take it out if you have decent wheel physics
			// The built-in Unity demo car is actually skidding its wheels the ENTIRE time you're accelerating,
			// so this fades out the wheelspin-based skid as speed increases to make it look almost OK
			wheelSpin = Mathf.Max(0, wheelSpin * (10 - Mathf.Abs(carForwardVel)));

			skidTotal += wheelSpin;

			// Skid if we should
			if (skidTotal >= skidFXSpeed) {
				float intensity = Mathf.Clamp01(skidTotal / maxSkidIntensity);
				// Account for further movement since the last FixedUpdate
				Vector3 skidPoint = _wheelHitInfo.point + (rb.linearVelocity * (Time.time - _lastFixedUpdateTime));
				_lastSkid = skidmarksController.AddSkidMark(skidPoint, _wheelHitInfo.normal, intensity, _lastSkid);
			}
			else {
				_lastSkid = -1;
			}
		}
		else {
			_lastSkid = -1;
		}
	}

}
