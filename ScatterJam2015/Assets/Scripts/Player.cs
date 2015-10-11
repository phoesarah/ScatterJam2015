using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private const int PLAYER_LAYER = 9;
    private const string PLAYER_TAG = "Player";
    private const string HOOKER_TAG = "Hooker";
    private const string ROOF_TAG = "Roof";

    private float _yawSensitivity = 360.0f;
    private float _pitch = 0.0f;
    private float _pitchSensitivity = 180.0f;
    private float _pitchMax = 90.0f;
    private float _ropeLength = 200.0f;
    private float _ropeForce = 0.2f;
    private int _ropeDeployed = 0;
    private bool _dead = false;
    private AudioSource _fxAudioSource = null;
    private AudioSource _targetAudioSource = null;
    private LineRenderer _ropeRenderer = null;
    
    public Camera _camera;
    public Rigidbody _rigidbody;
    public GameObject _target;
    public AudioClip _fireSound1;
    public AudioClip _fireSound2;
    public Material _ropeMaterial;
    public Hud _hud;
    public GameObject _grapple;

    public bool dead
    {
        get { return _dead; }
    }

    // Use this for initialization
    void Start()
    {
        _pitchMax = Mathf.Clamp(_pitchMax, 0.0f, 90.0f);
        //_hookPoint = transform.position + transform.forward * _ropeLength;
        //_ropeDeployed = 3;
        _fxAudioSource = gameObject.AddComponent<AudioSource>();
        _targetAudioSource = _target.AddComponent<AudioSource>();

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

        _ropeRenderer = gameObject.AddComponent<LineRenderer>();
        _ropeRenderer.useWorldSpace = true;
        _ropeRenderer.enabled = false;
        _ropeRenderer.material = _ropeMaterial;
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
            bool hitHook = true; //(hit.collider.gameObject.tag == HOOKER_TAG || hit.collider.gameObject.tag == ROOF_TAG);
            if ((fire1 || fire2) && hitHook)
            {
                _grapple.transform.parent = hit.collider.transform;
                _grapple.transform.position = hit.point;

                _ropeDeployed = fire1 ? 1 : 2;
                //_rigidbody.AddForce(
                //    _rigidbody.velocity * -0.5f, 
                //    ForceMode.VelocityChange
                //);
                _fxAudioSource.PlayOneShot(_fireSound1);
                //_targetAudioSource.Stop();
                //_targetAudioSource.clip = _fireSound2;
                //_targetAudioSource.loop = false;
                //_targetAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
                //_targetAudioSource.spatialize = false;
                //_targetAudioSource.spatialBlend = 0.9f;
                //_targetAudioSource.PlayDelayed(1.0f);
                //_targetAudioSource.PlayDelayed(Mathf.Min(0.1f, hit.distance / _ropeLength * 2.0f));
            }
            //Debug.DrawLine(
            //    start: transform.position,
            //    end: hit.point,
            //    color: hitHook ? Color.white : Color.black,
            //    duration: 0.0f,
            //    depthTest: false
            //);
            _target.transform.position = hit.point;
            _target.SetActive(hitHook);
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

            var ropeVector = (_grapple.transform.position - transform.position);
            _rigidbody.AddForce(ropeVector * _ropeForce, ForceMode.Acceleration);

            var grappleScale = Vector3.one;
            var grappleParent = _grapple.transform.parent;
            while (grappleParent != null)
            {
                grappleScale.x /= grappleParent.localScale.x;
                grappleScale.y /= grappleParent.localScale.y;
                grappleScale.z /= grappleParent.localScale.z;
                grappleParent = grappleParent.parent;
            }
            _grapple.transform.forward = ropeVector.normalized;
            _grapple.transform.localScale = grappleScale;
            _grapple.SetActive(true);

            _ropeRenderer.SetVertexCount(2);
            _ropeRenderer.SetPosition(0, transform.position + transform.up * -2.0f);
            _ropeRenderer.SetPosition(1, _grapple.transform.position);
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
        if (col.gameObject.tag != PLAYER_TAG && col.gameObject.tag != ROOF_TAG)
        {
            _dead = true;
            _ropeDeployed = 0;
            _rigidbody.AddForce(_rigidbody.velocity * -0.5f, ForceMode.VelocityChange);
            _grapple.SetActive(false);
            _hud.FadeTo(Color.white, 2.0f);
            //Application.LoadLevel(Application.loadedLevel);
        }
    }
}
