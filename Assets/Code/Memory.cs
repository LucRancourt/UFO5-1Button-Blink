using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Memory : MonoBehaviour, IButtonListener
{
    // Variables
    [SerializeField] private GameObject pfx;
    [SerializeField] private float lifetime;
    private float _currentLifetime;
   
    private bool _isActive;

    [SerializeField] private float timeToExperience;
    private float _currentExperienceTime;
    private float _timeToRemove;
    private bool _wasExperienced;
    private bool _isButtonHeldDown;

    [SerializeField] private float secondsToFade = 0.05f;
    private WaitForSeconds _secondsToFade;
    private Color _colorForFade;

    private SpriteRenderer _spriteRenderer;


    // Functions
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _secondsToFade = new WaitForSeconds(secondsToFade);
    }

    private void Start()
    {
        var inputObject = FindFirstObjectByType<PlayerInputs>();
        inputObject.RegisterListener(this);
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        _colorForFade = _spriteRenderer.color;
        _colorForFade.a = 0.0f;
        _spriteRenderer.color = _colorForFade;

        _currentLifetime = lifetime;

        _wasExperienced = false;
        _timeToRemove = 0.025f / timeToExperience;
        _isButtonHeldDown = false;

        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (!_isActive) return;

        if (_isButtonHeldDown)
        {
            _currentExperienceTime = Mathf.Clamp(_currentExperienceTime + Time.deltaTime, 0.0f, timeToExperience);
            _colorForFade.a = Mathf.Clamp(_colorForFade.a + _timeToRemove, 0.5f, 1.0f);
        }
        else
        {
            _currentExperienceTime = Mathf.Clamp(_currentExperienceTime - Time.deltaTime, 0.0f, timeToExperience);
            _colorForFade.a = Mathf.Clamp(_colorForFade.a - _timeToRemove, 0.5f, 1.0f);
        }

        _spriteRenderer.color = _colorForFade;


        if (_currentExperienceTime >= timeToExperience && !_wasExperienced)
        {
            _wasExperienced = true;

            GameObject particleInstance = Instantiate(pfx, transform.position, Quaternion.LookRotation(Camera.main.transform.position - transform.position));

            Destroy(particleInstance, particleInstance.GetComponent<ParticleSystem>().main.duration);
        }


        if (_currentLifetime <= 0.0f)
        {
            Deactivate();
        }
        else
        {
            _currentLifetime -= Time.deltaTime;
        }
    }

    public void Deactivate()
    {
        _isActive = false;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        _colorForFade = _spriteRenderer.color;

        while (_spriteRenderer.color.a < 0.5f)
        {
            _colorForFade.a += 0.1f;
            _spriteRenderer.color = _colorForFade;
            yield return _secondsToFade;
        }

        _colorForFade.a = 0.5f;
        _isActive = true;
    }

    private IEnumerator FadeOut()
    {
        StopCoroutine(FadeIn());

        _colorForFade = _spriteRenderer.color;

        while (_spriteRenderer.color.a > 0.0f)
        {
            _colorForFade.a -= 0.1f;
            _spriteRenderer.color = _colorForFade;
            yield return _secondsToFade;
        }

        gameObject.SetActive(false);
    }



    public void ButtonHeld(ButtonInfo heldInfo)
    {

    }

    public void ButtonPressed(ButtonInfo pressedInfo)
    {
        if (!_isActive) return;

        _isButtonHeldDown = true;
    }

    public void ButtonReleased(ButtonInfo releasedInfo)
    {
        if (!_isActive) return;

        _isButtonHeldDown = false;
    }
}
