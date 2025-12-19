using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HitJudge_sample02 : MonoBehaviour
{
    private float judge_great_range = 12f;
    private float judge_good_range = 40f;

    // スコアを表示するテキスト
    public TextMeshProUGUI scoreText;

    // GREAT / GOOD / MISS を表示するテキスト
    [SerializeField, Tooltip("GREAT / GOOD / MISS を表示するテキスト")]
    private TextMeshProUGUI resultText;

    private int great_score = 3;
    private int good_score = 1;
    private int miss_score = 0;
    private int great_magnification = 1;
    private int good_magnification = 1;
    private int miss_magnification = 1;

    private float sensor_value;
    private float sensor_value_before;

    [SerializeField, Tooltip("センサの値がいくつ以上になったら、叩いた判定にするか")]
    private int th = 100;

    [SerializeField]
    private AudioSource seSource;

    [SerializeField, Tooltip("great時の効果音")]
    private AudioClip seClip_great;

    [SerializeField, Tooltip("goot時の効果音")]
    private AudioClip seClip_good;

    [SerializeField, Tooltip("miss時の効果音")]
    private AudioClip seClip_miss;

    // 現在のスコア
    int score = 0;

    private Vector2 target_point_pos;

    private void Start()
    {
        target_point_pos = DataManager.GetTargetPointPos();

        // 最初は判定結果を表示しない
        resultText.gameObject.SetActive(false);

        // 初期スコアを画面に表示
        UpdateScore();

        sensor_value_before = 0;
        score = 0;
    }

    private void Update()
    {
        // 【キーボード操作】
        // スペースキーを押した瞬間に判定を行う
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Judge();
        }


        sensor_value = SensorReader.GetPressure();

        if (sensor_value_before < th && sensor_value >= th)
        {
            Judge();
        }

        sensor_value_before = sensor_value;
    }

    // 叩いた時の判定処理
    private void Judge()
    {
        List<string[]> ball_list = DataManager.ReturnBallListInfo();

        int judge_ball_num = -1;
        float min_distance = float.MaxValue;

        for (int i = 0; i < ball_list.Count; i++)
        {
            float ball_distance = float.Parse(ball_list[i][1]);

            if (ball_distance < min_distance)
            {
                min_distance = ball_distance;
                judge_ball_num = i;
            }
        }

        if (judge_ball_num != -1)
        {
            if (min_distance < judge_great_range)
            {
                // GREAT
                AddScore(great_score * great_magnification, "GREAT!");

                SoundEffect(2);
            }
            else if (min_distance < judge_good_range)
            {
                // GOOD
                AddScore(good_score * good_magnification, "GOOD");

                SoundEffect(1);
            }
            else
            {
                // MISS
                AddScore(miss_score * miss_magnification, "MISS");

                SoundEffect(0);
            }
        }
    }

    // スコアを加算し、画面に結果を表示する処理
    private void AddScore(int add, string msg)
    {
        // スコアを加算
        score += add;

        // スコア表示を更新
        UpdateScore();

        // 判定結果の文字を表示
        resultText.text = msg;
        resultText.gameObject.SetActive(true);

        // 0.4秒後に結果を非表示にする
        Invoke(nameof(HideResult), 0.4f);

        DataManager.SetScore(score);
    }

    // 判定の文字を非表示にする
    private void HideResult()
    {
        resultText.gameObject.SetActive(false);
    }

    // スコアテキストを書き換える
    private void UpdateScore()
    {
        scoreText.text = "SCORE: " + score;
    }

    private void SoundEffect(int type)
    {
        switch (type)
        {
            case 0:     // miss
                seSource.PlayOneShot(seClip_miss);
                break;

            case 1:     // good
                seSource.PlayOneShot(seClip_good);
                break;

            case 2:     // great
                seSource.PlayOneShot(seClip_great);
                break;
        }
    }
}
