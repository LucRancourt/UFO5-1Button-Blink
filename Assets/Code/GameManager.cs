using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // On Press Play on MM 
    public void StartGame()
    {
        Candle.Instance.Setup();
    }

    public void ShowMainMenu()
    {

    }
}
