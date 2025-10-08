using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class Candle : Singleton<Candle>, IButtonListener
{
    // Variables
    private ButtonInfo _currentButton;

    [Tooltip("Must have exactly 5 Sprites - Start Sprite to End Sprite in order")]
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float totalLifetime;
    [SerializeField] private float burnSpeed = 4.0f;
    [SerializeField] private SFX flameWhoosh;

    [SerializeField] private Volume globalVolume;
    [SerializeField] private float vignetteLowLightIntensity = 0.57f;
    [SerializeField] private float vignetteBurnIntensity = 0.43f;
    private Vignette _vignette;

    [SerializeField] private float endSpeed = 1.0f;

    [SerializeField] private SpriteRenderer blackScreen;

    private SpriteRenderer _candleRenderer;
    [SerializeField] private Transform flameTransform;

    private Vector3 _flameScaleMin = new Vector3(0.15f, 0.15f, 0.15f);
    private Vector3 _flameScaleMax = new Vector3(0.3f, 0.3f, 0.3f);
    public bool IsBurning { get; private set; }
    private bool _wasBurning;

    private bool _isDead;

    private float _currentLifetime;
    private float _quarterOfTotalLifetime;


    // Functions
    private void Start()
    {
        _currentButton.CurrentState = ButtonState.Released;
        var inputObject = FindFirstObjectByType<PlayerInputs>();
        inputObject.RegisterListener(this);

        _candleRenderer = GetComponent<SpriteRenderer>();

        Setup();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(transform.position.y - 0.15f, 3.72f));
        sequence.Append(transform.DOMoveY(transform.position.y, 4.35f));
        sequence.SetLoops(-1, LoopType.Yoyo);
    }

    public void Setup()
    {
        _candleRenderer.sprite = sprites[0];

        flameTransform.gameObject.SetActive(true);

        _currentLifetime = totalLifetime;
        _quarterOfTotalLifetime = totalLifetime / 4.0f;

        IsBurning = false;
        _isDead = false;

        globalVolume.profile.TryGet(out _vignette);
    }

    private void Update()
    {
        if (_isDead) return;

        if (IsBurning && !_wasBurning)
        {
            _wasBurning = true;
            AudioManager.Instance.PlaySound(flameWhoosh);
        }

        if (_currentButton.CurrentState == ButtonState.Held)
        {
            IsBurning = true;

            _currentLifetime -= Time.deltaTime * burnSpeed;

            flameTransform.localScale = _flameScaleMax;

            _vignette.intensity.value = vignetteBurnIntensity;
        }
        else
        {
            IsBurning = false;
            _wasBurning = false;

            _currentLifetime -= Time.deltaTime;

            flameTransform.localScale = _flameScaleMin;

            _vignette.intensity.value = vignetteLowLightIntensity;
        }



        if (_currentLifetime <= 0.0f)
        {
            _candleRenderer.sprite = sprites[4];
            flameTransform.gameObject.SetActive(false);

            StartCoroutine(EndScene());
        }
        else if (_currentLifetime <= _quarterOfTotalLifetime)
        {
            _candleRenderer.sprite = sprites[3];

            if (IsBurning)
                flameTransform.localPosition = new Vector3(-0.117f, 1.21f, 0.0f);
            else
                flameTransform.localPosition = new Vector3(-0.117f, 1.0f, 0.0f);
        }
        else if (_currentLifetime <= _quarterOfTotalLifetime * 2.0f)
        {
            _candleRenderer.sprite = sprites[2];

            if (IsBurning)
                flameTransform.localPosition = new Vector3(-0.135f, 1.694f, 0.0f);
            else
                flameTransform.localPosition = new Vector3(-0.125f, 1.471f, 0.0f);
        }
        else if (_currentLifetime <= _quarterOfTotalLifetime * 3.0f)
        {
            _candleRenderer.sprite = sprites[1];

            if (IsBurning)
                flameTransform.localPosition = new Vector3(-0.156f, 2.289f, 0.0f);
            else
                flameTransform.localPosition = new Vector3(-0.136f, 2.043f, 0.0f);
        }
        else
        {
            if (IsBurning)
                flameTransform.localPosition = new Vector3(-0.165f, 2.747f, 0.0f);
            else
                flameTransform.localPosition = new Vector3(-0.165f, 2.483f, 0.0f);
        }
    }

    private IEnumerator EndScene()
    {
        _isDead = true;
        Vector2 center;

        Color color = new Color(0, 0, 0, 0);

        while (_vignette.intensity.value < 1.0f)
        {
            _vignette.intensity.value += 0.005f * endSpeed;

            center = _vignette.center.value;
            center.y -= 0.0013f * endSpeed;
            _vignette.center.value = center;

            color.a += 0.007f * endSpeed;
            blackScreen.color = color;

            yield return new WaitForSeconds(0.025f);
        }


        yield return new WaitForSeconds(1.0f);

        color.a = 255f;
        blackScreen.color = color;
        gameObject.SetActive(false);

        yield return new WaitForSeconds(3.0f);
        GameManager.Instance.ShowMainMenu();
    }

    public void ButtonHeld(ButtonInfo heldInfo)
    {
        _currentButton = heldInfo;
    }

    public void ButtonPressed(ButtonInfo pressedInfo)
    {
        _currentButton = pressedInfo;
    }

    public void ButtonReleased(ButtonInfo releasedInfo)
    {
        _currentButton = releasedInfo;
    }
}
