using UnityEngine;
using TMPro;

public class HitJudge : MonoBehaviour
{
    // 流れてくるボール（UI Image）
    public RectTransform ball;

    // 判定位置の赤丸（UI Image）
    public RectTransform target;

    // 判定できる距離（数値が大きいほど判定がゆるくなる）
    public float judgeRange = 40f;

    // スコアを表示するテキスト
    public TextMeshProUGUI scoreText;

    // GREAT / GOOD / MISS を表示するテキスト
    [SerializeField, Tooltip("GREAT / GOOD / MISS を表示するテキスト")]
    private TextMeshProUGUI resultText;

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

    void Start()
    {
        // 最初は判定結果を表示しない
        resultText.gameObject.SetActive(false);

        // 初期スコアを画面に表示
        UpdateScore();

        sensor_value_before = 0;
        score = 0;
    }

    void Update()
    {
        // 【キーボード操作】
        // スペースキーを押した瞬間に判定を行う
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Judge();
        }


        sensor_value = SensorReader.GetPressure();

        if (sensor_value_before == 0 && sensor_value >= th)
        {
            Judge();
        }

        sensor_value_before = sensor_value;
    }

    // 叩いた時の判定処理
    void Judge()
    {
        // ボールとターゲット（赤丸）の横方向の距離を計算
        float distance = Mathf.Abs(ball.anchoredPosition.x - target.anchoredPosition.x);

        // GREAT：ほぼぴったりの位置
        if (distance < judgeRange * 0.3f)
        {
            //Debug.Log("GREAT");
            AddScore(3, "GREAT!");

            SoundEffect(2);
        }
        // GOOD：まあまあ近い
        else if (distance < judgeRange)
        {
            //Debug.Log("GOOD");
            AddScore(1, "GOOD");

            SoundEffect(1);
        }
        // MISS：遠い位置
        else
        {
            //Debug.Log("MISS");
            AddScore(0, "MISS");

            SoundEffect(0);
        }
    }

    // スコアを加算し、画面に結果を表示する処理
    void AddScore(int add, string msg)
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
    void HideResult()
    {
        resultText.gameObject.SetActive(false);
    }

    // スコアテキストを書き換える
    void UpdateScore()
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
