using UnityEngine;

public class BallMove : MonoBehaviour
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

    // 0〜moveTimeの間で時間を数えるためのタイマー
    private float timer = 0;

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

        float det = ((x1 * x1) * (x2 - x3)) + ((x2 * x2) * (x3 - x1)) + (x3 * x3) * (x1 - x2);
        a = ((y1 * (x2 - x3)) + (y2 * (x3 - x1)) + (y3 * (x1 - x2))) / det;
        b = ((y1 * (x3 * x3 - x2 * x2)) + (y2 * (x1 * x1 - x3 * x3)) + (y3 * (x2 * x2 - x1 * x1))) / det;
        c = ((y1 * (x2 * x2 * x3 - x2 * x3 * x3)) + (y2 * (x3 * x3 * x1 - x3 * x1 * x1)) + (y3 * (x1 * x1 * x2 - x1 * x2 * x2))) / det;

        move_x_distance = x2 - x1;

        timer = 0;
    }

    void Update()
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
    }
}
