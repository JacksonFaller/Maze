using UnityEngine;

namespace Assets.Scripts
{
    public class ZoomView : MonoBehaviour
    {
        [SerializeField]
        [Range(10, 60)]
        private int _minFov;

        [SerializeField]
        [Range(70, 120)]
        private int _maxFov;

        [SerializeField]
        private float _sensitivity;

        private Camera _camera;
        private float _fov;

        // Use this for initialization
        void Start ()
        {
            _camera = Camera.main;
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                _fov = Camera.main.fieldOfView;
                _fov -= Input.GetAxis("Mouse ScrollWheel") * _sensitivity;
                _fov = Mathf.Clamp(_fov, _minFov, _maxFov);
                _camera.fieldOfView = _fov;
            }
        }
    }
}
