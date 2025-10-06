using UnityEngine;

public class Candle : MonoBehaviour, IButtonListener
{
    // Variables
    private ButtonInfo _currentButton;

    [Tooltip("Must have exactly 5 Sprites - Start Sprite to End Sprite in order")]
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float totalLifetime;
    [SerializeField] private float burnSpeed = 4.0f;

    private SpriteRenderer _candleRenderer;
    [SerializeField] private Transform flameTransform;

    private Vector3 _flameScaleMin = new Vector3(0.15f, 0.15f, 0.15f);
    private Vector3 _flameScaleMax = new Vector3(0.3f, 0.3f, 0.3f);

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
    }

    public void Setup()
    {
        _candleRenderer.sprite = sprites[0];

        flameTransform.localPosition = new Vector3(-0.165f, 2.483f, 0.0f);
        flameTransform.gameObject.SetActive(true);

        _currentLifetime = totalLifetime;
        _quarterOfTotalLifetime = totalLifetime / 4.0f;
    }

    private void Update()
    {
        if (_currentButton.CurrentState == ButtonState.Held)
        {
            _currentLifetime -= Time.deltaTime * burnSpeed;

            flameTransform.localScale = _flameScaleMax;

            if (_currentLifetime <= 0.0f)
            {
                _candleRenderer.sprite = sprites[4];
                flameTransform.gameObject.SetActive(false);
            }
            else if (_currentLifetime <= _quarterOfTotalLifetime)
            {
                _candleRenderer.sprite = sprites[3];
                flameTransform.localPosition = new Vector3(-0.117f, 1.0f, 0.0f);
            }
            else if (_currentLifetime <= _quarterOfTotalLifetime * 2.0f)
            {
                _candleRenderer.sprite = sprites[2];
                flameTransform.localPosition = new Vector3(-0.125f, 1.471f, 0.0f);
            }
            else if (_currentLifetime <= _quarterOfTotalLifetime * 3.0f)
            {
                _candleRenderer.sprite = sprites[1];
                flameTransform.localPosition = new Vector3(-0.136f, 2.043f, 0.0f);
            }
        }
        else
        {
            _currentLifetime -= Time.deltaTime;
            flameTransform.localScale = _flameScaleMin;
        }
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
