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


    private float default_timeimit = 60f;

    private float time;

    void Start()
    {
        if (timelimit <= 0) timelimit = default_timeimit;
        time = 0;

        // DataManagerに基準オブジェクトの位置データをセット
        DataManager.SetStartPointPos(start_point.anchoredPosition);
        DataManager.SetEndPointPos(end_point.anchoredPosition);
        DataManager.SetTargetPointPos(target_point.anchoredPosition);
    }

    void Update()
    {
        time += Time.deltaTime;

        if ((time > timelimit) || Input.GetKeyDown(KeyCode.E))
        {
            GoEndScene();
        }
    }

    private void GoEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
