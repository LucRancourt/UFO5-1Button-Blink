using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MemoryMenu : Singleton<MemoryMenu>
{
    // Variables
    [SerializeField] private GameObject memoryMenu;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button backButton;

    private Memory[] _memories;
    [Tooltip("Each should have: TitleText, IconSprite")]
    [SerializeField] private GameObject[] memorySections;
    [SerializeField] private Sprite blankMemory;


    // Functions
    private void Start()
    {
        _memories = FindObjectsByType<Memory>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        backButton.onClick.AddListener(CloseMenu);
    }

    public void OpenMenu()
    {
        memoryMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(backButton.gameObject);

        for (int i = 0; i < _memories.Length; i++)
        {
            if (PlayerPrefs.GetInt(_memories[i].AchievementKey) == 1)
            {
                memorySections[i].GetComponent<TextMeshProUGUI>().SetText(_memories[i].AchievementKey);
                memorySections[i].GetComponent<SpriteRenderer>().sprite = _memories[i].GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                memorySections[i].GetComponent<TextMeshProUGUI>().SetText("???");
                memorySections[i].GetComponent<SpriteRenderer>().sprite = blankMemory;
            }
        }
    }

    private void CloseMenu()
    {
        memoryMenu.SetActive(false);
        MainMenu.Instance.ShowMainMenu();
    }
}