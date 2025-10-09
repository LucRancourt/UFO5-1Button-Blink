using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu>
{
    // Variables
    [SerializeField] private SFX clickSFX;
    [SerializeField] private GameObject panel;
    [SerializeField] private EventSystem eventSystem;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button achievementsButton;
    [SerializeField] private Button quitButton;


    // Functions
    protected override void Awake()
    {
        base.Awake();

        Button[] buttons = GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
            button.onClick.AddListener(PlayClickSFX);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        achievementsButton.onClick.AddListener(OpenAchievements);
        quitButton.onClick.AddListener(QuitGame);

        eventSystem.SetSelectedGameObject(playButton.gameObject);
    }

    public void ShowMainMenu()
    {
        panel.SetActive(true);
        eventSystem.SetSelectedGameObject(playButton.gameObject);
    }

    private void StartGame()
    {
        panel.SetActive(false);
        GameManager.Instance.StartGame();
    }

    private void OpenSettings()
    {
        SettingsMenu.Instance.OpenMenu();
    }

    private void OpenAchievements()
    {
        //AchievementsMenu.Instance.OpenMenu();
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void PlayClickSFX()
    {
        AudioManager.Instance.PlaySound(clickSFX);
    }
}
