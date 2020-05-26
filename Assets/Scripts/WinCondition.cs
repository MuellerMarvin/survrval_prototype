using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{
    public int burntGrassUntilLose = 100;
    public float secondsUntilWin = 10;
    public float secondsPassed { get; set; }
    public BurntCounter burntCounter;
    public Image gameEndBackground;
    public Text gameFailText;
    public Text gameWinText;
    private bool gameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        secondsPassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        secondsPassed += Time.deltaTime;

        if(secondsPassed > secondsUntilWin && !gameEnded)
        {
            // trigger endscreen win
            gameEndBackground.gameObject.SetActive(true);
            gameWinText.gameObject.SetActive(true);
            Debug.Log("Game won");
            gameEnded = true;
        }

        if(burntCounter.BurntAmount > burntGrassUntilLose && !gameEnded)
        {
            // trigger endscreen lose
            gameEndBackground.gameObject.SetActive(true);
            gameFailText.gameObject.SetActive(true);
            Debug.Log("Game lost");
            gameEnded = true;
        }
    }
}
