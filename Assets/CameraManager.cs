using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Serializable]
    class CameraInformation
    {
        [SerializeField] private CinemachineCamera camera;
        [SerializeField] private bool fog = true;

        public CinemachineCamera Camera => camera;
        public bool Fog => fog;
    }

    [SerializeField] private CameraInformation[] cameras;
    private CinemachineCamera _currentCamera;
    private int _currentIndex = 0;

    private void Start()
    {
        _currentCamera = cameras[0].Camera;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _currentIndex = (_currentIndex + 1) % cameras.Length;

            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].Camera.Priority = -1;

                if (i == _currentIndex)
                {
                    cameras[i].Camera.Priority = 1;
                    _currentCamera = cameras[i].Camera;
                }
            }

            RenderSettings.fog = cameras[_currentIndex].Fog;
        }
    }
    
}
