using UnityEngine;
using TMPro;

public class HitJudge : MonoBehaviour
{
    // 流れてくるボール（UI Image）
    public RectTransform ball;

    // 判定位置の赤丸（UI Image）
    public RectTransform target;

    // 判定できる距離（数値が大きいほど判定がゆるくなる）
    public float judgeRange = 60f;

    // スコアを表示するテキスト
    public TextMeshProUGUI scoreText;

    // GREAT / GOOD / MISS を表示するテキスト
    public TextMeshProUGUI resultText;

    // 現在のスコア
    int score = 0;

    void Start()
    {
        // 最初は判定結果を表示しない
        resultText.gameObject.SetActive(false);

        // 初期スコアを画面に表示
        UpdateScore();
    }

    void Update()
    {
        // 【キーボード操作】
        // スペースキーを押した瞬間に判定を行う
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Judge();
        }
    }

    // 叩いた時の判定処理
    void Judge()
    {
        // ボールとターゲット（赤丸）の横方向の距離を計算
        float distance = Mathf.Abs(ball.anchoredPosition.x - target.anchoredPosition.x);

        // GREAT：ほぼぴったりの位置
        if (distance < judgeRange * 0.3f)
        {
            Debug.Log("GREAT");
            AddScore(3, "GREAT!");
        }
        // GOOD：まあまあ近い
        else if (distance < judgeRange)
        {
            Debug.Log("GOOD");
            AddScore(1, "GOOD");
        }
        // MISS：遠い位置
        else
        {
            Debug.Log("MISS");
            AddScore(0, "MISS");
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
}
