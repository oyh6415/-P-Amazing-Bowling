using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onReset;

    public static GameManager instance;

    public GameObject readyPannel;
    public Text scoreText;
    public Text bestScoreText;
    public Text messageText;
    public bool isRoundActive = false;

    private int score = 0;

    public ShooterRotator shooterRotator;
    public CamFollow cam;

    private void Awake()
    {
        instance = this;
        UpdateUI();
    }
    private void Start()
    {
        StartCoroutine("RoundRoutine");
    }
    public void AddScore(int newScore)
    {
        score += newScore;
        UpdateBestScore();
        UpdateUI();
    }

    void UpdateBestScore()
    {
        if(GetBestScore()<score)
            PlayerPrefs.SetInt("BestScore", score);
    }

    int GetBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore");
        return bestScore;
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        bestScoreText.text = "Best Score: " + GetBestScore();
    }

    public void OnBallDestroy()
    {
        UpdateUI();
        isRoundActive = false;
    }

    public void RestRound()
    {
        score = 0;
        UpdateUI();

        //라운드를 처음부터 시작
        StartCoroutine("RoundRoutine");
    }

    IEnumerator RoundRoutine()
    {
        //Ready
        onReset.Invoke();

        readyPannel.SetActive(true);
        cam.SetTarget(shooterRotator.transform, CamFollow.State.Idle); //카메라가 따라갈 대상: 발사대
        shooterRotator.enabled = false;

        isRoundActive = false;

        messageText.text = "Ready!";

        yield return new WaitForSeconds(3f);

        //Play
        isRoundActive = true;
        readyPannel.SetActive(false);
        shooterRotator.enabled = true;

        cam.SetTarget(shooterRotator.transform, CamFollow.State.Ready); //카메라 레디 상태: 줌 인

        while (isRoundActive)
        {
            yield return null;
        }

        //End
        readyPannel.SetActive(true);
        shooterRotator.enabled = false;

        messageText.text = "Wait For Next Round!";
        yield return new WaitForSeconds(3f);
        RestRound();
    }
}
