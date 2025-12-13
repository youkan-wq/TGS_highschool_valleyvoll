using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("制限時間")]
    private float timelimit;

    [SerializeField, Tooltip("Canvasをセット")]
    private RectTransform canvasRect;

    [SerializeField, Tooltip("ボールのプレハブをセット")]
    private GameObject ballPrefab;


    [SerializeField, Tooltip("スタート位置（RectTransform）")]
    private RectTransform start_point;

    [SerializeField, Tooltip("ゴール位置（RectTransform）")]
    private RectTransform end_point;

    [SerializeField, Tooltip("判定位置（RectTransform）")]
    private RectTransform judgement_point;


    private float default_timelimit = 60;
    private float time;
    private float count;

    void Awake()
    {
        // DataManagerに値をセット
        if (!ValidateReferences())
        {
            DataManager.SetStartPointPosition(start_point.anchoredPosition);
            DataManager.SetEndPointPosition(end_point.anchoredPosition);
            DataManager.SetJudgementPointPosition(judgement_point.anchoredPosition);
        }
    }

    void Start()
    {
        if (timelimit <= 0) timelimit = default_timelimit;
        time = 0;
        count = 0;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > timelimit)
        {
            GoEndScene();
        }
    }

    void FixedUpdate()
    {
        count++;
        if (count % (50 * 3) == 0)
        {
            BallSpawn(start_point.anchoredPosition);
        }
    }

    private void GoEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }

    public GameObject BallSpawn(Vector2 anchoredPos)
    {
        if (canvasRect == null || ballPrefab == null)
        {
            Debug.LogError("canvasRect または ballPrefab が未設定です", this);
            return null;
        }

        // 親をCanvas直下にして生成（worldPositionStays=falseが重要）
        GameObject go = Instantiate(ballPrefab, canvasRect);
        RectTransform rt = go.GetComponent<RectTransform>();
        if (rt == null)
        {
            Debug.LogError("生成したボールに RectTransform がありません。ballPrefab はUIプレハブですか？", this);
            Destroy(go);
            return null;
        }

        // 位置指定
        rt.anchoredPosition = anchoredPos;

        return go;
    }

    // 参照が正しく取得できるかを検証
    private bool ValidateReferences()
    {
        bool ok = true;

        if (start_point == null)
        {
            Debug.LogError($"{name}: start_point が Inspector で設定されていません。", this);
            ok = false;
        }
        if (end_point == null)
        {
            Debug.LogError($"{name}: end_point が Inspector で設定されていません。", this);
            ok = false;
        }
        if (judgement_point == null)
        {
            Debug.LogError($"{name}: judgement_point が Inspector で設定されていません。", this);
            ok = false;
        }

        if (!ok)
        {
            Debug.LogError($"{name}: 必要な参照が未設定のためゲームを停止します。", this);
            StopGame();
        }

        return ok;
    }
}
