using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private const int PLAYER_LAYER = 9;
    private const string PLAYER_TAG = "Player";
    private const string HOOKER_TAG = "Hooker";
    private const string ROOF_TAG = "Roof";
    private const string GOAL_TAG = "Goal";

    private const string NEAR_BIRD_TAG = "NearBird";

    private float _yawSensitivity = 360.0f;
    private float _pitch = 0.0f;
    private float _pitchSensitivity = 180.0f;
    private float _pitchMax = 90.0f;
    private float _ropeLength = 75.0f;
    private float _ropeForce = 0.2f;
    private int _ropeDeployed = 0;
    private bool _dead = false;
    private bool _goalReached = false;
    private float _loadLevelDelay = 2.0f;
    private AudioSource _fxAudioSource = null;
    private AudioSource _targetAudioSource = null;
    private LineRenderer _ropeRenderer = null;
    
    public Camera _camera;
    public Rigidbody _rigidbody;
    public GameObject _target;
    public AudioClip _fireSound1;
    public AudioClip _fireSound2;
    public Material _ropeMaterial1;
    public AudioClip _buCaw = null;
    public Material _ropeMaterial2;
    public AudioClip _collision = null;

    public Hud _hud;
    public GameObject _grapple;
    public string _nextLevel;

    public bool dead
    {
        get { return _dead; }
    }
    public bool goalReached
    {
        get { return _goalReached; }
    }

    // Use this for initialization
    void Start()
    {
        if (!Application.isEditor)
        {
            Cursor.visible = false;
        }

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isEditor)
        {
            Cursor.visible = false;
        }

        // Quit / Restart

        if (Input.GetButtonDown("Quit"))
        {
            Application.Quit();
            return;
        }
        if (Input.GetButtonDown("Restart"))
        {
            Application.LoadLevel(Application.loadedLevel);
            return;
        }

        float deltaTime = Time.deltaTime;
        if (_goalReached || _dead)
        {
            _loadLevelDelay -= deltaTime;
            if (_loadLevelDelay <= 0.0f)
            {
                if (_goalReached && !string.IsNullOrEmpty(_nextLevel))
                {
                    Application.LoadLevel(_nextLevel);
                }
                else
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
            }
            return;
        }
        if (transform.position.y < -60.0f)
        {
            OnDeath();
            return;
        }

        // Inputs
        
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
        int rope = fire1 ? 1 : (fire2 ? 2 : 0);
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, maxDistance: _ropeLength, layerMask: layerMask))
        {
            bool hitHook = true; //(hit.collider.gameObject.tag == HOOKER_TAG || hit.collider.gameObject.tag == ROOF_TAG);
            if (rope == _ropeDeployed)
            {
                _grapple.SetActive(false);
                _ropeDeployed = 0;
            }
            else if (hitHook && rope > 0)
            {
                _grapple.transform.parent = hit.collider.transform;
                _grapple.transform.position = hit.point;
                _fxAudioSource.PlayOneShot(_fireSound1);
                _ropeDeployed = rope;

                var nearbyColliders = Physics.OverlapSphere(transform.position, 3.0f);
                var nearRoof = false;
                foreach (var c in nearbyColliders)
                {
                    if (c.gameObject.tag == ROOF_TAG)
                    {
                        nearRoof = true;
                        break;
                    }
                }
                if (nearRoof)
                {
                    _rigidbody.AddForce(
                        transform.up * 2.0f,
                        ForceMode.VelocityChange
                    );
                }

                _rigidbody.AddForce(
                    _rigidbody.velocity * -0.5f, 
                    ForceMode.VelocityChange
                );
                //_targetAudioSource.Stop();
                //_targetAudioSource.clip = _fireSound2;
                //_targetAudioSource.loop = false;
                //_targetAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
                //_targetAudioSource.spatialize = false;
                //_targetAudioSource.spatialBlend = 0.9f;
                //_targetAudioSource.PlayDelayed(1.0f);
                //_targetAudioSource.PlayDelayed(Mathf.Min(0.1f, hit.distance / _ropeLength * 2.0f));
            }
            _target.transform.position = hit.point;
            _target.SetActive(hitHook);
        }
        else
        {
            if (rope == _ropeDeployed)
            {
                _grapple.SetActive(false);
                _ropeDeployed = 0;
            }
            _target.SetActive(false);
        }

        if (_ropeDeployed > 0)
        {
            //var ropeColor = GetRopeColor(_ropeDeployed);
            var ropeVector = (_grapple.transform.position - transform.position);
            _rigidbody.AddForce(ropeVector * _ropeForce, ForceMode.Acceleration);

            //var grappleScale = new Vector3(
            //    1.0f / _grapple.transform.lossyScale.x,
            //    1.0f / _grapple.transform.lossyScale.y,
            //    1.0f / _grapple.transform.lossyScale.z
            //);
            var grappleScale = Vector3.one;
            var grappleParent = _grapple.transform.parent;
            while (grappleParent != null)
            {
                grappleScale.x /= grappleParent.localScale.x;
                grappleScale.y /= grappleParent.localScale.y;
                grappleScale.z /= grappleParent.localScale.z;
                grappleParent = grappleParent.parent;
            }
            _grapple.transform.rotation = _camera.transform.rotation;
            _grapple.transform.localScale = grappleScale;
            _grapple.SetActive(true);

            Vector3 ropeStart = transform.position + transform.up * -2.0f;
            Vector3 ropeEnd = _grapple.transform.position;
            int segments = 10;
            _ropeRenderer.useWorldSpace = true;
            _ropeRenderer.material = GetRopeMaterial(_ropeDeployed);
            _ropeRenderer.SetWidth(0.1f, 0.1f);
            _ropeRenderer.SetColors(Color.white, Color.white);
            _ropeRenderer.SetVertexCount(1 + segments);
            _ropeRenderer.SetPosition(0, ropeStart);
            for (int i = 1; i < segments; ++i)
            {
                float t = ((float)i) / ((float)segments);
                var a = ropeEnd * t;
                var b = ropeStart * (1 - t);
                _ropeRenderer.SetPosition(i, a + b);
            }
            _ropeRenderer.SetPosition(segments, ropeEnd);
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

    private Material GetRopeMaterial(int rope)
    {
        switch (rope)
        {
            case 1:
                return _ropeMaterial1;
            case 2:
                return _ropeMaterial2;
            default:
                return null;
        }
    }

    public void OnDeath()
    {
        if (!_dead && !_goalReached)
        {
            _dead = true;
            _ropeDeployed = 0;
            _rigidbody.AddForce(_rigidbody.velocity * -0.5f, ForceMode.VelocityChange);
            _grapple.SetActive(false);
            _fxAudioSource.PlayOneShot(_collision, 0.5f);
            _hud.FadeTo(Color.black, _loadLevelDelay * 0.5f);
            _hud.text.text = "OUCH!\n\nYOU SHOULD HAVE KNOWN BY NOW";
        }
    }
    public void OnGoalReached()
    {
        if (!_dead && !_goalReached)
        {
            _goalReached = true;
            _ropeDeployed = 0;
            _rigidbody.AddForce(-_rigidbody.velocity, ForceMode.VelocityChange);
            _grapple.SetActive(false);
            _hud.FadeTo(Color.white, _loadLevelDelay * 0.5f);
            _hud.text.text = "A LITTLE BIT HARDER NOW";
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        switch (col.gameObject.tag)
        {
            case PLAYER_TAG:
                break;
            case ROOF_TAG:
                _rigidbody.AddForce(_rigidbody.velocity * -0.5f, ForceMode.VelocityChange);
                break;
            case GOAL_TAG:
                OnGoalReached();
                break;
            default:
                OnDeath();
                break;
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        switch (col.gameObject.tag)
        {
            case PLAYER_TAG:
                break;
            case GOAL_TAG:
                OnGoalReached();
                break;
			case NEAR_BIRD_TAG:
            	_fxAudioSource.PlayOneShot(_buCaw, 0.5f);
                break;
            default:
                OnDeath();
                break;
        }
    }
}
