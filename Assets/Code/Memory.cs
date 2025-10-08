using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Memory : MonoBehaviour
{
    // Variables
    [SerializeField] private float lifetime;
    private float _currentLifetime;
    private bool _isActive;

    [SerializeField] private float secondsToFade = 0.05f;
    private WaitForSeconds _secondsToFade;
    private Color _colorForFade;

    private SpriteRenderer _spriteRenderer;


    // Functions
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _secondsToFade = new WaitForSeconds(secondsToFade);
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        _isActive = false;

        _colorForFade = _spriteRenderer.color;
        _colorForFade.a = 0.0f;
        _spriteRenderer.color = _colorForFade;

        _currentLifetime = lifetime;

        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (!_isActive) return;

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

        while (_spriteRenderer.color.a < 1f)
        {
            _colorForFade.a += 0.1f;
            _spriteRenderer.color = _colorForFade;
            yield return _secondsToFade;
        }

        _isActive = true;
    }

    private IEnumerator FadeOut()
    {
        StopCoroutine(FadeIn());

        _colorForFade = _spriteRenderer.color;

        while (_spriteRenderer.color.a > 0f)
        {
            _colorForFade.a -= 0.1f;
            _spriteRenderer.color = _colorForFade;
            yield return _secondsToFade;
        }

        gameObject.SetActive(false);
    }
}
