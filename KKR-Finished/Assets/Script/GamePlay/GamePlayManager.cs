using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
    private int score;
    private int displayedScore;

    public static int coin;

    public static float multiplierScore = 1f;
    public static float multiplierCoin = 1f;


    private float timeElapsed;

    private float scoreIncreaseRate = 100f;

    public TextMeshProUGUI txtScore;
    public TextMeshProUGUI txtCoin;

    public TextMeshProUGUI txtSumScore;
    public TextMeshProUGUI txtSumCoin;

    public TextMeshProUGUI txtMultipilerScore;

    public TextMeshProUGUI txtWaitForPlay;
    public GameObject waitForplay;
    public GameObject startTxt;

    public float coolDownDuration = 3f;

    private bool isCoolingDown = false;
    private bool isPause = false;

    private DatabaseReference dbReference;

    [Header("PAGE")]
    [SerializeField] private GameObject pausePage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject newRecordText;

    public ItemUpgradeData scoreMultipilerData;

    void Start()
    {
        score = 0;
        displayedScore = 0;
        coin = 0;

        waitForplay.SetActive(true);
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        txtMultipilerScore.text = "x" + multiplierScore.ToString();
    }


    void FixedUpdate()
    {
        Singleton.Instance.LoadScore();

        #region Check Score
        if (scoreMultipilerData.currentUpgradeLevel == 0 )
        {
            multiplierScore = 1f;
        }

        if (scoreMultipilerData.currentUpgradeLevel == 1)
        {
            multiplierScore = 2f;
        }

        if (scoreMultipilerData.currentUpgradeLevel == 2)
        {
            multiplierScore = 4f;
        }

        if (scoreMultipilerData.currentUpgradeLevel == 3)
        {
            multiplierScore = 6f;
        }

        if (scoreMultipilerData.currentUpgradeLevel == 4)
        {
            multiplierScore = 8f;
        }
        if (scoreMultipilerData.currentUpgradeLevel == 5)
        {
            multiplierScore = 10f;
        }
        #endregion

        if (waitForplay.activeSelf)
        {
            Singleton.Instance.isReadyToPlay = false;
            isCoolingDown = true;
            PlayerController.forwardSpeed = 0f;
            if (isCoolingDown)
            {
                coolDownDuration -= Time.deltaTime;
                txtWaitForPlay.text = Mathf.RoundToInt(coolDownDuration).ToString();
                if (coolDownDuration <= 0)
                {
                    coolDownDuration = 0;
                    waitForplay.SetActive(false);
                    isCoolingDown = false;
                    PlayerController.forwardSpeed = 20f;
                    startTxt.SetActive(true);
                    Singleton.Instance.isReadyToPlay = true;
                }
            }
        }

        if (!Singleton.Instance.isReadyToPlay)
            return;

        txtMultipilerScore.text = "x" + multiplierScore.ToString();
        timeElapsed += Time.deltaTime;


        if (Singleton.Instance.isMultiplierScore)
            StartCoroutine(CheckLifeTimeMultiplierScore());
        if (Singleton.Instance.isMultiplierCoin)
            StartCoroutine(CheckLifeTimeMultiplierCoin());


        UpdateDisplayedScore();
        IncreaseScore();

        if (PlayerController.isGameover) 
        { 
            Gameover();
        }
    }


    private void Gameover()
    {
        gameOverPanel.SetActive(true);
        if (score >= Singleton.Instance.scorePlayer)
        {
            newRecordText.SetActive(true);
            SendScore();
        }
        else
        {
            newRecordText.SetActive(false);
        }

        StartCoroutine(UpdateScoreAndCoin());

        Debug.Log("Game over");
    }


    IEnumerator CheckLifeTimeMultiplierScore()
    {
        yield return new WaitForSeconds(Singleton.Instance.multiplierScore);
        Singleton.Instance.isMultiplierScore = false;
        multiplierScore /= 2f;
    }
    IEnumerator CheckLifeTimeMultiplierCoin()
    {
        yield return new WaitForSeconds(Singleton.Instance.multiplierScore);
        Singleton.Instance.isMultiplierCoin = false;
        multiplierCoin = 1f;
    }


    private void IncreaseScore()
    {
        if (PlayerController.isGameover)
            return;
            
        score = Mathf.FloorToInt(timeElapsed * scoreIncreaseRate * multiplierScore);
    }

    public void IncreaseCoin()
    {
        Player.player.playerData.coin += coin;
        Player.player.playerData.SavePlayerData();
    }

    private void UpdateDisplayedScore()
    {
        displayedScore = (int)Mathf.Lerp(score, CalculateTargetScore(), Time.deltaTime * 1f);
        txtScore.text = displayedScore.ToString("D10");

        txtCoin.text = CalculateTargetCoin().ToString("D7");
    }

    public void SendScore()
    {
        Singleton.Instance.SentScore(score);
    }



    public void Pause()
    {
        Time.timeScale = 0f;
        pausePage.SetActive(true);
        isPause = true;

    }
   
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePage.SetActive(false);
        isPause = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuGame");

    }

    private IEnumerator UpdateScoreAndCoin()
    {
        float elapsedTime = 0f;
        int targetScore = CalculateTargetScore(); // คำนวณค่าเป้าหมายของคะแนน
        int targetCoin = CalculateTargetCoin(); // คำนวณค่าเป้าหมายของเหรียญ

        while (elapsedTime < 2f)
        {
            // คำนวณค่าใหม่ของคะแนนและเหรียญในเวลาปัจจุบัน
            int currentScore = Mathf.FloorToInt(Mathf.Lerp(0f, targetScore, elapsedTime / 0.5f));
            int currentCoin = Mathf.FloorToInt(Mathf.Lerp(0f, targetCoin, elapsedTime / 0.5f));

            // อัพเดตค่าข้อความของ txtSumScore และ txtSumCoin
            txtSumScore.text = currentScore.ToString("D10");
            txtSumCoin.text = currentCoin.ToString();

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // กำหนดค่าสุดท้ายของคะแนนและเหรียญ
        txtSumScore.text = targetScore.ToString("D10");
        txtSumCoin.text = targetCoin.ToString();

        // เรียกเมธอด Gameover หลังจากนับเสร็จเพียงครั้งเดียว
        //Gameover();
    }
 

    private int CalculateTargetScore()
    {
        int targetScore = score; // คำนวณค่าเป้าหมายของคะแนนที่ผู้เล่นทำได้

        return targetScore; // คำนวณคะแนนที่ถูกต้องตามต้องการ
    }

    private int CalculateTargetCoin()
    {
        int targetCoin = coin;

        return targetCoin; // คำนวณเหรียญที่ถูกต้องตามต้องการ
    }


}
