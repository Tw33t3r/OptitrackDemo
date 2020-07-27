using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave_manager : MonoBehaviour
{
    [SerializeField] private Text TimerText;
    [SerializeField] private float waitingTime = 20.0f;
    [SerializeField] private float stageTime_1 = 60.0f;
    [SerializeField] private float stageTime_2 = 60.0f;
    [SerializeField] private float stageTime_3 = 60.0f;
    [SerializeField] private float stageTime_4 = 60.0f;
    [SerializeField] private int[] spawntime_1 = { 60, 40, 20 };
    [SerializeField] private int[] spawntime_2 = { 50, 30 };
    [SerializeField] private int[] spawntime_3 = { 50, 30 };
    [SerializeField] private int[] spawntime_4 = { 60 };

    public Transform spawnTransform;
    public Transform monster_1;
    public Transform monster_2;
    public Transform monster_3;
    public Transform monster_4;

    private float timer;
    private bool hold = false;
    private bool end = false;
    private string text;
    private int minuite;
    private int second;
    private int milisecond;

    private int spawncount = 0;
    private bool stage_1 = false;
    private bool stage_2 = false;
    private bool stage_3 = false;
    private bool stage_4 = false;

    void Start()
    {
        timer = waitingTime;
    }

    void Update()
    {
        if (timer > 0.0f && !hold) {
            timer -= Time.deltaTime;
            minuite = (int)(timer / 60);
            second = (int)timer - (minuite*60);
            milisecond = (int)(timer * 100) % 100;
            text = minuite.ToString() + ":" + second.ToString() + ":" + milisecond.ToString();
            TimerText.text = text;
        }
        else if (timer <= 0.0f && !end) {
            spawncount = 0;
            if (!stage_1)
            {
                stage_1 = true;
                timer = stageTime_1;
            }
            else if (!stage_2)
            {
                stage_1 = false;
                stage_2 = true;
                timer = stageTime_2;
            }
            else if (!stage_3)
            {
                stage_2 = false;
                stage_3 = true;
                timer = stageTime_3;
            }
            else if (!stage_4)
            {
                stage_3 = false;
                stage_4 = true;
                timer = stageTime_4;
            }
            else
            {
                stage_4 = false;
                timer = 0.0f;
                TimerText.text = "0:0:0";
                end = true;
            }
        }

        // spawn monsters
        if (stage_1)
        {
            if (spawntime_1.Length > spawncount)
            {
                if ((int)timer <= spawntime_1[spawncount])
                {
                    spawncount++;
                    Instantiate(monster_1, spawnTransform);
                }
            }
        }
        else if (stage_2)
        {
            if (spawntime_2.Length > spawncount)
            {
                if ((int)timer <= spawntime_2[spawncount])
                {
                    spawncount++;
                    Instantiate(monster_2, spawnTransform);
                }
            }
        }
        else if (stage_3)
        {
            if (spawntime_3.Length > spawncount)
            {
                if ((int)timer <= spawntime_3[spawncount])
                {
                    spawncount++;
                    Instantiate(monster_3, spawnTransform);
                }
            }
        }
        else if (stage_4)
        {
            if (spawntime_4.Length > spawncount)
            {
                if (timer <= spawntime_4[spawncount])
                {
                    spawncount++;
                    Instantiate(monster_4, spawnTransform);
                }
            }
        }

    }
}
