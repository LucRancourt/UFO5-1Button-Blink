using System;
using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]

public class Memory : MonoBehaviour, IButtonListener
{
    // Variables
    public string AchievementKey;
    [SerializeField] private GameObject achievementPopup;

    public event Action OnMemoryFadedOut;

    public GameObject FullsizeBackground;
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

    private int _randomOne;


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

        _currentExperienceTime = 0.0f;
        _wasExperienced = false;
        _timeToRemove = 0.025f / timeToExperience;
        _isButtonHeldDown = false;

        gameObject.layer = LayerMask.NameToLayer("Default");

        _randomOne = UnityEngine.Random.Range(0, 1) * 2 - 1;

        _isActive = true;
        //StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (!_isActive) return;

        transform.RotateAround(transform.position, transform.forward, _randomOne * Time.deltaTime * 10.0f);

        if (_isButtonHeldDown && !Candle.Instance.IsDead)
        {
            _currentExperienceTime = Mathf.Clamp(_currentExperienceTime + Time.deltaTime, 0.0f, timeToExperience);
            _colorForFade.a = Mathf.Clamp(_colorForFade.a + _timeToRemove, 0.0f, 1.0f);
        }
        else
        {
            if (!_wasExperienced)
            {
                _currentExperienceTime = Mathf.Clamp(_currentExperienceTime - Time.deltaTime, 0.0f, timeToExperience);
                _colorForFade.a = Mathf.Clamp(_colorForFade.a - _timeToRemove, 0.0f, 1.0f);
            }
        }

        _spriteRenderer.color = _colorForFade;


        if (_currentExperienceTime >= timeToExperience && !_wasExperienced)
        {
            _wasExperienced = true;

            gameObject.layer = LayerMask.NameToLayer("UnVignetted");

            GameObject particleInstance = Instantiate(pfx, transform.position, Quaternion.LookRotation(Camera.main.transform.position - transform.position));

            Destroy(particleInstance, particleInstance.GetComponent<ParticleSystem>().main.duration);

            Invoke("SetBackgroundFull", particleInstance.GetComponent<ParticleSystem>().main.duration + 3.0f);
        }


        if (_currentLifetime <= 0.0f)
        {
            if ((_isButtonHeldDown && !Candle.Instance.IsDead) || _wasExperienced) return;

            Deactivate();
        }
        else
        {
            _currentLifetime -= Time.deltaTime;
        }
    }

    private void SetBackgroundFull()
    {
        if (!GameManager.Instance.IsGameActive)
        {
            TurnOffMemory();
            return;
        }

        Candle.Instance.AddTime(5.0f);

        FullsizeBackground.gameObject.SetActive(true);
        Invoke("SaveAchievement", 2.5f);
        Invoke("SetBackgroundInactive", 5.0f);
    }

    private void SaveAchievement()
    {
        PlayerPrefs.SetInt(AchievementKey, 1);

        achievementPopup.SetActive(true);

        Invoke("SetPopupInactive", 3.0f);
    }

    private void SetPopupInactive()
    {
        achievementPopup.SetActive(false);
    }

    private void SetBackgroundInactive()
    {
        FullsizeBackground.gameObject.SetActive(false);

        if (!GameManager.Instance.IsGameActive || !gameObject.activeSelf)
        {
            TurnOffMemory();
            return;
        }

        Deactivate();
    }

    public void Deactivate()
    {
        _isActive = false;
        FullsizeBackground.gameObject.SetActive(false);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        _colorForFade = _spriteRenderer.color;

        while (_spriteRenderer.color.a < 0.35f)
        {
            _colorForFade.a += 0.1f;
            _spriteRenderer.color = _colorForFade;
            yield return _secondsToFade;
        }

        _colorForFade.a = 0.35f;
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

        OnMemoryFadedOut?.Invoke();
        TurnOffMemory();
    }

    public void TurnOffMemory()
    {
        StopAllCoroutines();
        CancelInvoke();

        achievementPopup.SetActive(false);
        FullsizeBackground.gameObject.SetActive(false);
        _isActive = false;
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
