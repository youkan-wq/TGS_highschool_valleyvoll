using UnityEngine;

public class BallMoveScript : MonoBehaviour
{
    [SerializeField, Tooltip("ボールをセット")]
    private GameObject ball_object;
    private RectTransform ball;

    [SerializeField, Tooltip("ボールが右から左に進のにかかる時間[s]")]
    private float moveTime;

    // 基準となるオブジェクトの座標（Vector2）
    private Vector2 start_point_pos;
    private Vector2 end_point_pos;
    private Vector2 judgement_point_pos;
    // 移動するx座標の距離
    private float move_x_distance;

    // ボールの軌道の２次曲線の係数
    private float a, b, c;  // y = ax^2 + bx + c
    // パラメータ（媒介変数）
    private float t;

    private void Awake()
    {
        // ball_object が未設定なら自分自身を使う
        if (ball_object == null) ball_object = gameObject;

        // DataManager が未設定ならシーンから探す
        if (DataManager == null)
        {
            var dm = FindFirstObjectByType<DataManager>();
            if (dm != null) DataManager = dm.gameObject;
        }

        if (DataManager == null)
        {
            Debug.LogError($"{name}: DataManager が見つかりません", this);
            enabled = false;
            return;
        }

        dataManager = DataManager.GetComponent<DataManager>();

        // 値の簡易チェック
        if (moveTime <= 0f)
        {
            Debug.LogError($"{name}: moveTime は 0 より大きい値にしてください。ゲームを停止します。", this);
            dataManager.StopGame();
            return;
        }
    }

    void Start()
    {
        // 各基準となるオブジェクトの座標を取得
        start_point_pos = DataManager.GetStartPointPosition();
        end_point_pos = DataManager.GetEndPointPosition();
        judgement_point_pos = DataManager.GetJudgementPointPosition();

        ball = ball_object.GetComponent<RectTransform>();

        // 座標を示す変数を定義
        float x1 = start_point_pos.x;
        float y1 = start_point_pos.y;
        float x2 = end_point_pos.x;
        float y2 = end_point_pos.y;
        float x3 = judgement_point_pos.x;
        float y3 = judgement_point_pos.y;

        // 行列式の計算
        float det = ((x1 * x1) * (x2 - x3)) + ((x2 * x2) * (x3 - x1)) + (x3 * x3) * (x1 - x2);

        // ボールの軌道の２次曲線の係数を計算
        a = ((y1 * (x2 - x3)) + (y2 * (x3 - x1)) + (y3 * (x1 - x2))) / det;
        b = ((y1 * (x3 * x3 - x2 * x2)) + (y2 * (x1 * x1 - x3 * x3)) + (y3 * (x2 * x2 - x1 * x1))) / det;
        c = ((y1 * (x2 * x2 * x3 - x2 * x3 * x3)) + (y2 * (x3 * x3 * x1 - x3 * x1 * x1)) + (y3 * (x1 * x1 * x2 - x1 * x2 * x2))) / det;

        move_x_distance = x2 - x1;
        t = 0;

        ball.anchoredPosition = new Vector2(x1, y1);
    }

    void FixedUpdate()
    {
        t += Time.fixedDeltaTime;

        if (t > moveTime) Destroy(ball_object);

        float new_x = start_point_pos.x + (move_x_distance * (t / moveTime));
        float new_y = (a * new_x * new_x) + (b * new_x) + c;

        ball.anchoredPosition = new Vector2(new_x, new_y);
    }
}
