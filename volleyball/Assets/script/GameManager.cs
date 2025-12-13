using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("制限時間")]
    private float timelimit;

    private float default_timeimit = 60f;

    private float time;

    void Start()
    {
        if (timelimit <= 0) timelimit = default_timeimit;
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > timelimit)
        {
            GoEndScene();
        }
    }

    private void GoEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
