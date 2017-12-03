using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    public Color LeftMostColor;
    public Color RightMostColor;
    public Text TimeLimit;
    public Text HalayScore;
    public float TimeInSec;
    public HalayorPool HalayorPool;
    private bool GameIsCompleted;

    void Awake()
    {
        Cursor.visible = false;
        _instance = this;
        GameIsCompleted = false;
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
            SceneManager.LoadScene(0);
        if (GameIsCompleted)
        {
            HalayScore.text = "Halay Score\n" + HalayorPool.Halay.GetComponent<SpiralGenerator>().GetScore();
            return;
        }

        TimeInSec -= Time.deltaTime;
        TimeLimit.text = ((int) TimeInSec / 60).ToString("D2")+":" + ((int) TimeInSec % 60).ToString("D2");

        if (TimeInSec <= 0)
            CompleteHalay();
    }
    
    public void SelectNewHalayor()
    {
        if (GameIsCompleted)
            return;
        if (HalayorPool.GetRemaining() <= 0)
        {
            CompleteHalay();
            return;
        }
        GameObject go = HalayorPool.GetRandom();
        go.AddComponent<HalayorControler>();
        go.GetComponent<Halayor>().OnActive();
    }

    public void CompleteHalay()
    {
        GameIsCompleted = true;
    }

    //https://www.alanzucconi.com/2016/01/06/colour-interpolation/2/
    public static Color LerpHSV(ColorHSV a, ColorHSV b, float t)
    {
        // Hue interpolation
        float h = 0;
        float d = b.h - a.h;
        if (a.h > b.h)
        {
            // Swap (a.h, b.h)
            var h3 = b.h;
            b.h = a.h;
            a.h = h3;

            d = -d;
            t = 1 - t;
        }

        if (d > 0.5) // 180deg
        {
            a.h = a.h + 1; // 360deg
            h = (a.h + t * (b.h - a.h)) % 1; // 360deg
        }
        if (d <= 0.5) // 180deg
        {
            h = a.h + t * d;
        }

        // Interpolates the rest
        return new ColorHSV
        (
            h, // H
            a.s + t * (b.s - a.s), // S
            a.v + t * (b.v - a.v), // V
            a.a + t * (b.a - a.a) // A
        );
    }
}
