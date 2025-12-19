using UnityEngine;

public class BallMove_sample01 : MonoBehaviour
{
    // ボールが「右 → 左」に進むのに何秒かけるか（1秒＝だいたい1拍）
    [SerializeField, Tooltip("ボールがスタートからゴールまで何秒で移動するか[s]")]
    private float moveTime = 1.0f;

    private Vector2 start_point_pos;
    private Vector2 start_end_pos;
    private Vector2 start_target_pos;

    private float a, b, c;

    private float move_x_distance;

    private RectTransform rect;

    // ボールが叩かれたかまだか（0:まだ叩かれていない、1:叩かれた）
    private int ball_state = 0;

    // 0〜moveTimeの間で時間を数えるためのタイマー
    private float timer = 0;


    // --- 叩かれた後の演出用 ---
 [SerializeField] private float hitFlyTime = 0.5f;      // 奥へ飛ぶ時間
[SerializeField] private float hitMoveX = 200f;        // 右へ流す量（好みで）
[SerializeField] private float hitMoveY = 120f;        // 上へ上げる量
[SerializeField] private float hitScaleTo = 0.15f;     // 小さくなる（奥へ）
[SerializeField] private float hitRotateDeg = 720f;    // 回転

private Vector2 hitStartPos;
private Vector2 hitEndPos;
private Vector3 hitStartScale;
private float hitTimer;
private float hitStartRotZ;

    void Start()
    {
        // 自分（ボール）のRectTransformを取ってくる
        rect = GetComponent<RectTransform>();

        start_point_pos = DataManager.GetStartPointPos();
        start_end_pos = DataManager.GetEndPointPos();
        start_target_pos = DataManager.GetTargetPointPos();

        float x1, y1, x2, y2, x3, y3;
        x1 = start_point_pos.x;
        y1 = start_point_pos.y;
        x2 = start_end_pos.x;
        y2 = start_end_pos.y;
        x3 = start_target_pos.x;
        y3 = start_target_pos.y;

        // 放物線の式の係数a,b,cを計算する
        float det = ((x1 * x1) * (x2 - x3)) + ((x2 * x2) * (x3 - x1)) + (x3 * x3) * (x1 - x2);
        a = ((y1 * (x2 - x3)) + (y2 * (x3 - x1)) + (y3 * (x1 - x2))) / det;
        b = ((y1 * (x3 * x3 - x2 * x2)) + (y2 * (x1 * x1 - x3 * x3)) + (y3 * (x2 * x2 - x1 * x1))) / det;
        c = ((y1 * (x2 * x2 * x3 - x2 * x3 * x3)) + (y2 * (x3 * x3 * x1 - x3 * x1 * x1)) + (y3 * (x1 * x1 * x2 - x1 * x2 * x2))) / det;

        move_x_distance = x2 - x1;

        // 変数の初期化
        timer = 0;
        ball_state = 0;
    }

    void Update()
    {
        if (ball_state == 0)
        {
            // 毎フレームごとにタイマーを進める
            timer += Time.deltaTime;

            // ゴール地点を通りすぎて少ししたら、このオブジェクトを削除
            if (timer > (moveTime + 1))
            {
                Destroy(gameObject);
                DataManager.DeleteBallList(gameObject.name);
            }
            // X方向（横）の動き
            float x = start_point_pos.x + (move_x_distance * (timer / moveTime));

            // Y方向（縦）の動き
            float y = a * x * x + b * x + c;

            // 実際にボールを動かす
            rect.anchoredPosition = new Vector2(x, y);
        } else if (ball_state == 1)
        {
            // ボールが叩かれた後の処理
            hitTimer += Time.deltaTime;
            float t = Mathf.Clamp01(hitTimer / hitFlyTime);

            // イージング（最初速く、最後ゆっくり）
            float e = 1f - Mathf.Pow(1f - t, 3f); // EaseOutCubic

            // 位置：斜め上へ
            rect.anchoredPosition = Vector2.Lerp(hitStartPos, hitEndPos, e);

            // スケール：小さく（奥へ飛ぶ感じ）
            float s = Mathf.Lerp(hitStartScale.x, hitScaleTo, e);
            rect.localScale = new Vector3(s, s, 1f);

            // 回転：くるくる
            float rot = hitStartRotZ + hitRotateDeg * e;
            rect.localEulerAngles = new Vector3(0f, 0f, rot);

            // 時間が来たら消す（リストからも削除）
            if (t >= 1f)
            {
                DataManager.DeleteBallList(gameObject.name);
                Destroy(gameObject);
            }
        }
    }

    public void SetBallState()
    {
        if (ball_state == 1) return; // 二重呼び防止

        ball_state = 1;

        // 叩かれた瞬間の状態を保存
        hitStartPos = rect.anchoredPosition;
        hitEndPos = hitStartPos + new Vector2(hitMoveX, hitMoveY);

        hitStartScale = rect.localScale;
        hitTimer = 0f;

        hitStartRotZ = rect.localEulerAngles.z;
    }
}

