using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private const int PLAYER_LAYER = 9;

    private float _yawSensitivity = 360.0f;
    private float _pitch = 0.0f;
    private float _pitchSensitivity = 180.0f;
    private float _pitchMax = 90.0f;
    private float _ropeLength = 100.0f;
    private float _ropeForce = 2.0f;
    private int _ropeDeployed = 0;
    private Vector3 _hookPoint = Vector3.zero;
    private AudioSource _fxAudioSource = null;

    public Camera _camera = null;
    public Rigidbody _rigidbody = null;
    public GameObject _target = null;
    public AudioClip _fireSound = null;

    // Use this for initialization
    void Start()
    {
        _pitchMax = Mathf.Clamp(_pitchMax, 0.0f, 90.0f);
        //_hookPoint = transform.position + transform.forward * _ropeLength;
        //_ropeDeployed = 3;
        _fxAudioSource = gameObject.AddComponent<AudioSource>();

        if (!_camera)
        {
            _camera = Camera.main;
        }
        if (!_rigidbody)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        _rigidbody.drag = 0.0f;
        _rigidbody.angularDrag = 0.0f;
        _rigidbody.freezeRotation = true;
        _rigidbody.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Quit / Restart

        if (Input.GetButton("Quit"))
        {
            Application.Quit();
            return;
        }
        if (Input.GetButton("Restart"))
        {
            Application.LoadLevel(Application.loadedLevel);
            return;
        }

        // Inputs

        float deltaTime = Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        bool fire1 = Input.GetButtonDown("Fire1");
        bool fire2 = Input.GetButtonDown("Fire2");

        // Camera

        float deltaYaw = mouseX * _yawSensitivity * deltaTime;
        float deltaPitch = -mouseY * _pitchSensitivity * deltaTime;

        _pitch = Mathf.Clamp(
            _pitch + deltaPitch,
            -_pitchMax, _pitchMax
        );
        transform.Rotate(0.0f, deltaYaw, 0.0f);
        _camera.transform.forward = transform.forward;
        _camera.transform.Rotate(_pitch, 0.0f, 0.0f);

        // Grapple Hook

        int layerMask = ~(1 << PLAYER_LAYER);  // Ignore Player Layer
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, maxDistance: _ropeLength, layerMask: layerMask))
        {
            bool hitHook = (hit.collider.gameObject.tag == "Hooker");
            if ((fire1 || fire2) && hitHook)
            {
                _hookPoint = hit.point;
                _ropeDeployed = fire1 ? 1 : 2;
                _rigidbody.AddForce(
                    _camera.transform.forward * _ropeForce + _rigidbody.velocity * -0.5f, 
                    ForceMode.VelocityChange
                );
            }
            Debug.DrawLine(
                start: transform.position,
                end: hit.point,
                color: hitHook ? Color.white : Color.black,
                duration: 0.0f,
                depthTest: false
            );
            _target.transform.position = hit.point;
            _target.SetActive(true);
        }
        else
        {
            Debug.DrawRay(
                start: _camera.transform.position,
                dir: _camera.transform.forward * _ropeLength,
                color: Color.black,
                duration: 0.0f,
                depthTest: false
            );
            _target.SetActive(false);
        }

        if (fire1 || fire2)
        {
            _fxAudioSource.PlayOneShot(_fireSound);
        }

            if (_ropeDeployed > 0)
        {
            Debug.DrawLine(
                start: transform.position,
                end: _hookPoint,
                color: GetRopeColor(_ropeDeployed),
                duration: 0.0f,
                depthTest: false
            );

            var ropeForce = (_hookPoint - transform.position).normalized * _ropeForce;
            _rigidbody.AddForce(ropeForce, ForceMode.Acceleration);
        }
    }

    private Color GetRopeColor(int rope)
    {
        switch (_ropeDeployed)
        {
            case 1:
                return Color.red;
            case 2:
                return Color.blue;
            default:
                return Color.black;
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Hooker")
        {
            _ropeDeployed = 0;
            _rigidbody.AddForce(_rigidbody.velocity * -0.5f, ForceMode.VelocityChange);
        }
    }
}
