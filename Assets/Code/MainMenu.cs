using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu>//, IButtonListener
{
    // Variables
    [SerializeField] private SFX clickSFX;
    [SerializeField] private GameObject panel;

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
    }

    public void ShowMainMenu()
    {
        panel.SetActive(true);
    }

    private void StartGame()
    {
        panel.SetActive(false);
        GameManager.Instance.StartGame();
    }

    private void OpenSettings()
    {
        //SettingsMenu.Instance.OpenMenu();
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



    public void ButtonHeld(ButtonInfo heldInfo)
    {
        throw new System.NotImplementedException();
    }

    public void ButtonPressed(ButtonInfo pressedInfo)
    {
        throw new System.NotImplementedException();
    }

    public void ButtonReleased(ButtonInfo releasedInfo)
    {
        throw new System.NotImplementedException();
    }
}
