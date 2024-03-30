using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using System;
using System.Transactions;

public class ParkourCounter : MonoBehaviour
{
    public LocomotionTechnique locomotionTech;
    public bool isStageChange;
    // banners
    public GameObject startBanner;
    public GameObject firstBanner;
    public GameObject secondBanner;
    public GameObject finalBanner;
    // coins
    public GameObject firstCoins;
    public GameObject secondCoins;
    public GameObject finalCoins;
    // Object Interaction Task
    public GameObject objIX1;
    public GameObject objIX2;
    public GameObject objIX3;
    // respawn points
    public Transform start2FirstRespawn;
    public Transform first2SecondRespawn;
    public Transform second2FinalRespawn;
    public Vector3 currentRespawnPos;

    public float timeCounter;
    public float timeCountdown;
    public float currentPartTimer;
    public long startTimestamp;
    public bool started;
    private float part1Time;
    private float part2Time;
    private float part3Time;
    public int coinCount;
    public int previousCoinCount;
    
    private int part1Count; // 17
    private int part2Count; // 33
    private int part3Count; // 24
    public bool parkourStart;

    public TMP_Text timeText;
    public TMP_Text coinText;
    public TMP_Text recordText;
    public GameObject timeTextGO;
    public GameObject coinTextGO;
    public GameObject recordTextGO;
    public GameObject endTextGO;
    public AudioSource backgroundMusic;
    public AudioSource endSoundEffect;
    public SelectionTaskMeasure selectionTaskMeasure;

    void Start()
    {
        coinCount = 0;
        timeCounter = 0.0f;
        currentPartTimer = 0.0f;
        timeCountdown = 10 * 60.0f; // TODO: Change to 10 Minutes for final version
        started = false;
        previousCoinCount = 0;
        DateTime currentTime = DateTime.UtcNow;
        startTimestamp = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
        firstBanner.SetActive(false);
        secondBanner.SetActive(false);
        finalBanner.SetActive(false);
        firstCoins.SetActive(false);
        secondCoins.SetActive(false);
        finalCoins.SetActive(false);
        objIX2.SetActive(false);
        objIX3.SetActive(false);
        objIX1.SetActive(false);
        parkourStart = false;
        endTextGO.SetActive(false);
        this.selectionTaskMeasure = this.GetComponent<SelectionTaskMeasure>();
    }

    public void Log(string message, bool study_log = false)
    {
        string filename = "/ParkourCounterLog-" + startTimestamp;
        if (study_log)
        {
            filename += "-study";
        }
        TextWriter tw = new StreamWriter(Application.persistentDataPath + filename + ".csv", true);
        tw.WriteLine(System.DateTime.Now + "," + message);
        tw.Close();
    }

    public void DisableBlockadeForCurrentStage()
    {
        if (locomotionTech.stage == startBanner.name)
        {
            firstBanner.transform.Find("Blockade").gameObject.SetActive(false);
        }
        else if (locomotionTech.stage == firstBanner.name)
        {
            secondBanner.transform.Find("Blockade").gameObject.SetActive(false);
        }
        else if (locomotionTech.stage == secondBanner.name)
        {
            finalBanner.transform.Find("Blockade").gameObject.SetActive(false);
        }
    }

    public void SetTextForCurrentBlockade(string txt)
    {
        if (locomotionTech.stage == startBanner.name)
        {
            firstBanner.transform.Find("Blockade/Canvas/Text").GetComponent<TMP_Text>().text = txt;
        }
        else if (locomotionTech.stage == firstBanner.name)
        {
            secondBanner.transform.Find("Blockade/Canvas/Text").GetComponent<TMP_Text>().text = txt;
        }
        else if (locomotionTech.stage == secondBanner.name)
        {
            finalBanner.transform.Find("Blockade/Canvas/Text").GetComponent<TMP_Text>().text = txt;
        }
    }

    void Update()
    {
        if (isStageChange)
        {
            isStageChange = false;
            if (locomotionTech.stage == startBanner.name && timeCountdown > 0)
            {
                parkourStart = true;
                currentPartTimer = 0.0f;
                endTextGO.SetActive(false);
                startBanner.SetActive(false);
                firstBanner.SetActive(true);
                firstBanner.transform.Find("Blockade").gameObject.SetActive(true);
                for (int i = 0; i < firstCoins.transform.childCount; i++)
                {
                    firstCoins.transform.GetChild(i).gameObject.SetActive(true);
                }
                firstCoins.SetActive(true);
                objIX1.SetActive(true);
                this.GetComponent<SelectionTaskMeasure>().taskUI.transform.position = objIX1.transform.position;
                currentRespawnPos = start2FirstRespawn.position;
            }
            else if (locomotionTech.stage == firstBanner.name)
            {
                firstBanner.SetActive(false);
                firstCoins.SetActive(false);
                objIX1.SetActive(false);
                secondBanner.SetActive(true);
                secondBanner.transform.Find("Blockade").gameObject.SetActive(true);
                for (int i = 0; i < secondCoins.transform.childCount; i++)
                {
                    for (int x = 0; x < secondCoins.transform.GetChild(i).transform.childCount; x++)
                    {
                        secondCoins.transform.GetChild(i).transform.GetChild(x).gameObject.SetActive(true);
                    }
                }
                secondCoins.SetActive(true);
                objIX2.SetActive(true);
                this.GetComponent<SelectionTaskMeasure>().taskUI.transform.position = objIX2.transform.position;
                part1Time = currentPartTimer;
                currentPartTimer = 0.0f;
                part1Count = coinCount - previousCoinCount;
                previousCoinCount = coinCount;
                currentRespawnPos = first2SecondRespawn.position;
                UpdateRecordText(1, part1Time, part1Count, 16);
            }
            else if (locomotionTech.stage == secondBanner.name)
            {
                secondBanner.SetActive(false);
                secondCoins.SetActive(false);
                objIX2.SetActive(false);
                finalBanner.SetActive(true);
                finalBanner.transform.Find("Blockade").gameObject.SetActive(true);
                for (int i = 0; i < finalCoins.transform.childCount; i++)
                {
                    finalCoins.transform.GetChild(i).gameObject.SetActive(true);
                }
                finalCoins.SetActive(true);
                objIX3.SetActive(true);
                this.GetComponent<SelectionTaskMeasure>().taskUI.transform.position = objIX3.transform.position;
                part2Time = currentPartTimer;
                currentPartTimer = 0.0f;
                part2Count = coinCount - previousCoinCount;
                previousCoinCount = coinCount;
                currentRespawnPos = second2FinalRespawn.position;
                UpdateRecordText(2, part2Time, part2Count, 30);
            }
            else if (locomotionTech.stage == finalBanner.name)
            {
                finalCoins.SetActive(false);
                objIX3.SetActive(false);
                part3Time = currentPartTimer;
                currentPartTimer = 0.0f;
                part3Count = coinCount - previousCoinCount;
                previousCoinCount = coinCount;
                UpdateRecordText(3, part3Time, part3Count, 23);
                endTextGO.GetComponent<TMP_Text>().text = "Round finished. Keep going!";
                endTextGO.SetActive(true);
                startBanner.SetActive(true);

                // Re-enable all coins, specifically collected coins
                for (int i = 0; i < firstCoins.transform.childCount; i++)
                {
                    firstCoins.transform.GetChild(i).gameObject.SetActive(true);
                }
                for (int i = 0; i < secondCoins.transform.childCount; i++)
                {
                    for (int x = 0; x < secondCoins.transform.GetChild(i).transform.childCount; x++)
                    {
                        secondCoins.transform.GetChild(i).transform.GetChild(x).gameObject.SetActive(true);
                    }
                }
                for (int i = 0; i < finalCoins.transform.childCount; i++)
                {
                    finalCoins.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        if (parkourStart)
        {
            if (timeCountdown > 0)
            {
                if(started == false)
                {
                    this.Log("start,v"+Application.version);
                    this.Log("start,v"+Application.version, study_log: true);
                    started = true;
                    backgroundMusic.GetComponent<AudioSource>().Play();
                }
                timeCounter += Time.deltaTime;
                currentPartTimer += Time.deltaTime;
                timeCountdown -= Time.deltaTime;
                timeText.text = "time: " + timeCountdown.ToString("F1");
                coinText.text = "coins: " + coinCount.ToString();
            }
            else
            {
                parkourStart = false;

                startBanner.SetActive(false);
                firstBanner.SetActive(false);
                secondBanner.SetActive(false);
                finalBanner.SetActive(false);
                firstCoins.SetActive(false);
                secondCoins.SetActive(false);
                finalCoins.SetActive(false);
                objIX2.SetActive(false);
                objIX3.SetActive(false);
                objIX1.SetActive(false);

                timeTextGO.SetActive(false);
                coinTextGO.SetActive(false);
                recordTextGO.SetActive(false);

                selectionTaskMeasure.DestroyTObjects();
                selectionTaskMeasure.grabLeft.ExitPortal();
                selectionTaskMeasure.grabRight.ExitPortal();
                selectionTaskMeasure.taskUI.SetActive(false);

                endTextGO.GetComponent<TMP_Text>().text = "Parkour Finished!\n" + recordText.text +
                    "\ntotal: " + timeCounter.ToString("F1") + ", " + coinCount.ToString();
                endTextGO.SetActive(true);
                Debug.Log(endTextGO.GetComponent<TMP_Text>().text);
                this.Log("end,");
                this.Log("end,", study_log: true);
                endSoundEffect.Play();
            }
        }       
    }

    void UpdateRecordText(int part, float time, int coinsCount, int coinsInPart)
    {
        string newRecords = "loco" + part.ToString() + ": " + time.ToString("F1") + ", " + coinsCount + "/" + coinsInPart + "\n" +
                            "obj"  + part.ToString() + ": " + (selectionTaskMeasure.partSumTime/5f).ToString("F1") + "," + (selectionTaskMeasure.partSumErr/5).ToString("F2");
        this.Log("stats,obj" + part.ToString() + "-average," + (selectionTaskMeasure.partSumTime / 5f).ToString("F1") + "," + (selectionTaskMeasure.partSumErr / 5).ToString("F2"), study_log: true);
        this.Log("stats,loco" + part.ToString() + "," + time.ToString("F1") + "," + coinsCount + "/" + coinsInPart, study_log: true);
        recordText.text = recordText.text + "\n" + newRecords;
    }

    public int GetStageNumberByName(string stageName)
    {
        if(stageName == startBanner.name)
        {
            return 1;
        }
        else if(stageName == firstBanner.name)
        {
            return 2;
        }
        else if(stageName == secondBanner.name)
        {
            return 3;
        }
        else if(stageName == finalBanner.name)
        {
            // This is not really a stage, more in-between rounds; between final and start
            return 4;
        }
        
        return -1;
    }
}
