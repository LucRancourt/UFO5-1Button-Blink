using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // Variables
    [SerializeField] private Image panel;
    [SerializeField] private float dividerToSlowFade;
    private bool _timeToFadeIn;
    private float _fadeValue;

    // Functions 
    protected override void Awake()
    {
        base.Awake();

        Color transparent = Color.black;
        transparent.a = 0.0f;
        panel.color = transparent;
    }

    public void StartGame()
    {
        _fadeValue = 0.0f;

        Candle.Instance.Activate();

        Memory memory = FindFirstObjectByType<Memory>(FindObjectsInactive.Include);
        memory.gameObject.SetActive(true);
        memory.Activate();
    }

    public void EndGame()
    {
        _fadeValue = 1.0f;
        _timeToFadeIn = true;
        SetOpacity(_fadeValue);

        MainMenu.Instance.ShowMainMenu();
    }

    private void Update()
    {
        if (_timeToFadeIn)
        {
            _fadeValue -= Time.deltaTime / dividerToSlowFade;
            SetOpacity(_fadeValue);

            if (_fadeValue <= 0.0f)
            {
                _timeToFadeIn = false;
            }
        }
    }

    private void SetOpacity(float alphaValue)
    {
        Color tempColor = panel.color;

        tempColor.a = Mathf.Clamp01(alphaValue);

        panel.color = tempColor;
    }
}
