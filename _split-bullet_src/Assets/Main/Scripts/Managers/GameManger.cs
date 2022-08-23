using UnityEngine;

public class GameManger : MonoBehaviour
{
    private void Awake()
    {
        // Make the game run as fast as possible
        Application.targetFrameRate = 300;
    }
}
