using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private const int PLAYER_LAYER = 9;

    private float _yawSensitivity = 360.0f;
    private float _pitch = 0.0f;
    private float _pitchSensitivity = 180.0f;
    private float _pitchMax = 90.0f;
    private float _ropeLength = 200.0f;
    private float _ropeForce = 8.0f;
    private int _ropeDeployed = 0;
    private Vector3 _hookPoint = Vector3.zero;
    private AudioSource _fxAudioSource = null;
    private AudioSource _targetAudioSource = null;
    private LineRenderer _ropeRenderer = null;
    
    public Camera _camera = null;
    public Rigidbody _rigidbody = null;
    public GameObject _target = null;
    public AudioClip _fireSound1 = null;
    public AudioClip _fireSound2 = null;
    public Material _ropeMaterial;
    public Hud _hud;

    // Use this for initialization
    void Start()
    {
        _pitchMax = Mathf.Clamp(_pitchMax, 0.0f, 90.0f);
        //_hookPoint = transform.position + transform.forward * _ropeLength;
        //_ropeDeployed = 3;
        _fxAudioSource = gameObject.AddComponent<AudioSource>();
        _targetAudioSource = _target.AddComponent<AudioSource>();

        _ropeRenderer = gameObject.AddComponent<LineRenderer>();
        _ropeRenderer.useWorldSpace = true;
        _ropeRenderer.enabled = false;
        _ropeRenderer.material = _ropeMaterial;

        if (!_camera)
        {
            _camera = Camera.main;
        }
        if (!_rigidbody)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        if (!_hud)
        {
            _hud = GetComponent<Hud>();
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
                    _camera.transform.forward * _ropeForce + _rigidbody.velocity * -1.0f, 
                    ForceMode.VelocityChange
                );
                _targetAudioSource.Stop();
                _targetAudioSource.clip = _fireSound2;
                _targetAudioSource.PlayDelayed(Mathf.Min(0.1f, hit.distance / _ropeLength * 2.0f));
            }
            //Debug.DrawLine(
            //    start: transform.position,
            //    end: hit.point,
            //    color: hitHook ? Color.white : Color.black,
            //    duration: 0.0f,
            //    depthTest: false
            //);
            _target.transform.position = hit.point;
            _target.SetActive(true);
        }
        else
        {
            //Debug.DrawRay(
            //    start: _camera.transform.position,
            //    dir: _camera.transform.forward * _ropeLength,
            //    color: Color.black,
            //    duration: 0.0f,
            //    depthTest: false
            //);
            _target.SetActive(false);
        }

        if (fire1 || fire2)
        {
            _fxAudioSource.PlayOneShot(_fireSound1);
        }

        if (_ropeDeployed > 0)
        {
            var ropeColor = GetRopeColor(_ropeDeployed);

            //Debug.DrawLine(
            //    start: transform.position,
            //    end: _hookPoint,
            //    color: ropeColor,
            //    duration: 0.0f,
            //    depthTest: false
            //);

            var ropeForce = (_hookPoint - transform.position).normalized * _ropeForce;
            _rigidbody.AddForce(ropeForce, ForceMode.Acceleration);

            _ropeRenderer.SetVertexCount(2);
            _ropeRenderer.SetPosition(0, transform.position + transform.up * -2.0f);
            _ropeRenderer.SetPosition(1, _hookPoint);
            _ropeRenderer.SetColors(ropeColor, ropeColor);
            _ropeRenderer.SetWidth(0.1f, 0.1f);
            _ropeRenderer.enabled = true;
        }
        else
        {
            _ropeRenderer.enabled = false;
        }
    }

    private Color GetRopeColor(int rope)
    {
        switch (rope)
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
        if (col.gameObject.tag != "Player" && col.gameObject.tag != "Hooker")
        {
            _ropeDeployed = 0;
            _rigidbody.AddForce(_rigidbody.velocity * -0.5f, ForceMode.VelocityChange);
            _hud.FadeTo(Color.red, 1.0f);
            //Application.LoadLevel(Application.loadedLevel);
        }
    }
}
