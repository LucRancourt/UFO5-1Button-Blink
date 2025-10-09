using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // Variables
    public bool IsGameActive { get; private set; }

    [SerializeField] private Image panel;
    [SerializeField] private float dividerToSlowFade;

    [SerializeField] private Vector2 memorySpawnDelayRange;
    private List<Memory> _memoriesUnseen;
    private List<Memory> _memoriesSeen;

    private bool _timeToFadeIn;
    private float _fadeValue;


    private Vector2 _largeCircleCenter = new Vector2(0.0f, -5.02f);
    private float _largeCircleRadius = 7.25f;

    private Vector2 _smallCircleCenter = new Vector2(1.14f, -4.67f);
    private float _smallCircleRadius = 4.5f;




    // Functions 
    protected override void Awake()
    {
        base.Awake();

        Color transparent = Color.black;
        transparent.a = 0.0f;
        panel.color = transparent;
    }

    private void Start()
    {
        _memoriesUnseen = new List<Memory>(FindObjectsByType<Memory>(FindObjectsInactive.Include, FindObjectsSortMode.None));
        _memoriesSeen = new List<Memory>();

        foreach (Memory memory in _memoriesUnseen)
        {
            memory.OnMemoryFadedOut += ActiveMemoryDespawned;
        }
    }

    public void StartGame()
    {
        IsGameActive = true;
        ResetMemoryPool();

        _fadeValue = 0.0f;

        Candle.Instance.Activate();


        StartCoroutine(SpawnMemory());
    }

    private void ResetMemoryPool()
    {
        for (int i = 0; i < _memoriesSeen.Count; i++)
        {
            _memoriesUnseen.Add(_memoriesSeen[i]);
        }

        _memoriesSeen.Clear();

        foreach (Memory memory in _memoriesUnseen)
        {
            memory.TurnOffMemory();
        }
    }

    private IEnumerator SpawnMemory()
    {
        yield return new WaitForSeconds(Random.Range(memorySpawnDelayRange.x, memorySpawnDelayRange.y));

        if (_memoriesUnseen.Count <= 0)
            ResetMemoryPool();

        if (!IsGameActive)
        {
            ResetMemoryPool();
        }
        else
        {
            int index = Random.Range(0, _memoriesUnseen.Count);

            Vector2 randomPoint;
            
            while (true)
            {
                randomPoint = _largeCircleCenter + Random.insideUnitCircle * _largeCircleRadius;

                if (randomPoint.y >= -5.0f)
                {
                    float distanceToSmallCircleCenterSquared = (randomPoint - _smallCircleCenter).sqrMagnitude;

                    if (distanceToSmallCircleCenterSquared > (_smallCircleRadius * _smallCircleRadius))
                    {
                        break;
                    }
                }
            }

            _memoriesUnseen[index].transform.position = new Vector3(randomPoint.x, randomPoint.y, 0.0f);

            _memoriesUnseen[index].gameObject.SetActive(true);
            _memoriesUnseen[index].Activate();

            _memoriesSeen.Add(_memoriesUnseen[index]);
            _memoriesUnseen.RemoveAt(index);
        }
    }

    private void ActiveMemoryDespawned()
    {
        if (!IsGameActive) return;

        StartCoroutine(SpawnMemory());
    }

    public void EndGame()
    {
        IsGameActive = false;
        StopAllCoroutines();
        ResetMemoryPool();

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
