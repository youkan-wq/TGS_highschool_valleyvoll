using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("制限時間")]
    private float timelimit;

    // スタート位置（右側）
    [SerializeField, Tooltip("スタート位置のオブジェクト")]
    private RectTransform start_point;

    // ゴール位置（左側）
    [SerializeField, Tooltip("ゴール位置のオブジェクト")]
    private RectTransform end_point;

    // 判定位置
    [SerializeField, Tooltip("判定位置のオブジェクト")]
    private RectTransform target_point;

    // ボールオブジェクト
    [SerializeField, Tooltip("ボールのプレハブをセット")]
    private RectTransform ballPrefab;

    // キャンバス
    [SerializeField, Tooltip("キャンバスをセット")]
    private RectTransform canvas;

    // 何秒に１回ボールを出現させるか
    [SerializeField, Tooltip("キャンバスをセット")]
    private int ball_spawn_interval;
    private const int default_ball_spawn_interval = 4;

    private const int fixedUpdate_interval = 50;

    private float default_timeimit = 60f;

    private float time;
    private int count;
    private int ball_count;

    void Awake()
    {
        // DataManagerに基準オブジェクトの位置データをセット
        DataManager.SetStartPointPos(start_point.anchoredPosition);
        DataManager.SetEndPointPos(end_point.anchoredPosition);
        DataManager.SetTargetPointPos(target_point.anchoredPosition);

        // DataManagerの初期化
        DataManager.Initialize();
    }

    void Start()
    {
        if (timelimit <= 0) timelimit = default_timeimit;
        if (ball_spawn_interval <= 0) ball_spawn_interval = default_ball_spawn_interval;
        time = 0;
        count = 0;
        ball_count = 0;
    }

    void Update()
    {
        time += Time.deltaTime;

        if ((time > timelimit) || Input.GetKeyDown(KeyCode.E))
        {
            GoEndScene();
        }
    }

    void FixedUpdate()
    {
        count++;

        if (count % (fixedUpdate_interval * ball_spawn_interval) == 1)
        {
            ball_count++;

            SpawnBall(start_point.anchoredPosition, ball_count);
            DataManager.AddBallList(ball_count);
        }
    }


    private RectTransform SpawnBall(Vector2 anchoredPos, int ballNum)
    {
        if (canvas == null || ballPrefab == null)
        {
            Debug.LogError("canvas または ballPrefab が未設定です", this);
            return null;
        }

        // UIなので親をcanvasにして生成（worldPositionStays=false）
        RectTransform ball = Instantiate(ballPrefab, canvas);
        ball.anchoredPosition = anchoredPos;

        ball.gameObject.name = $"ball_{ballNum}";

        return ball;
    }

    private void GoEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
