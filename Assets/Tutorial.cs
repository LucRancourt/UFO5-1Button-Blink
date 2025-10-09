using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour, IButtonListener
{
    private void Start()
    {
        var inputObject = FindFirstObjectByType<PlayerInputs>();
        inputObject.RegisterListener(this);
    }

    public void ButtonHeld(ButtonInfo heldInfo)
    {

    }

    public void ButtonPressed(ButtonInfo pressedInfo)
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ButtonReleased(ButtonInfo releasedInfo)
    {

    }
}
