using UnityEngine;

public class BallMove : MonoBehaviour
{
    // ボールが「右 → 左」に進むのに何秒かけるか（1秒＝だいたい1拍）
    public float moveTime = 1.0f;

    // 放物線の高さ。数値が大きいほど「ぴょん」と高く飛ぶ
    public float peakHeight = 120f;

    // スタート位置（右側）
    public RectTransform startPos;

    // ゴール位置（左側）
    public RectTransform endPos;

    RectTransform rect;

    // 0〜moveTimeの間で時間を数えるためのタイマー
    float timer = 0;

    void Start()
    {
        // 自分（ボール）のRectTransformを取ってくる
        rect = GetComponent<RectTransform>();

        // ボールをスタート位置に置く
        ResetBall();
    }

    void Update()
    {
        // 毎フレームごとにタイマーを進める
        timer += Time.deltaTime;

        // timer を 0〜1 の範囲に変換（0＝スタート、1＝ゴール）
        float t = timer / moveTime;

        // 1を超えたらゴールしたので、またスタートに戻す
        if (t > 1f)
        {
            ResetBall();
            return;
        }

        // ▼━━ X方向（横）の動き ━━▼
        // startPos → endPos へまっすぐ移動する
        float x = Mathf.Lerp(startPos.anchoredPosition.x,
                             endPos.anchoredPosition.x,
                             t);

        // ▼━━ Y方向（縦）の動き ━━▼
        // 基本の高さ（start と end の間）
        float baseY = Mathf.Lerp(startPos.anchoredPosition.y,
                                 endPos.anchoredPosition.y,
                                 t);

        // 放物線の形を作る（0→1→0 のカーブ）
        // t=0  → 0
        // t=0.5 → 1（頂点）
        // t=1 → 0
        float parabola = -4 * (t - 0.5f) * (t - 0.5f) + 1;

        // baseY に「放物線分の高さ」を足す
        float y = baseY + parabola * peakHeight;

        // 実際にボールを動かす
        rect.anchoredPosition = new Vector2(x, y);
    }

    // ボールをスタート位置に戻す関数
    public void ResetBall()
    {
        timer = 0; // 時間を0に戻す
        rect.anchoredPosition = startPos.anchoredPosition; // スタートにワープ
    }
}
