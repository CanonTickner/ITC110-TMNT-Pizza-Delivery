using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{   
    public int numberOfKeys = 0;

    public int numberOfCoins = 0;

    public int numberOfLives = 3;

    public TextMeshProUGUI keyCounterText;

    public TextMeshProUGUI coinCounterText;

    public TextMeshProUGUI lifeCounterText;

    public Vector3 spawnPoint;
    public GameObject player; // make sure to drag Player into the Inspector for this

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = player.transform.position;
        keyCounterText.SetText("Keys Found: " + numberOfKeys);
        coinCounterText.SetText("Moneys: " + numberOfCoins);
        lifeCounterText.SetText("Lives: " + numberOfLives);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (numberOfCoins >= 10)
        {
            numberOfLives++;
            numberOfCoins = 0;
            lifeCounterText.SetText("Lives: " + numberOfLives);
            coinCounterText.SetText("Moneys: " + numberOfCoins);
        }
    }
}
