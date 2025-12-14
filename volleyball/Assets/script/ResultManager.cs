using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{

    [SerializeField]
    private float move_startScene_time;
    private float default_move_startScene_time = 20;

    private float time;

    void Start()
    {
        if (move_startScene_time <= 0) move_startScene_time = default_move_startScene_time;
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;

        if ((time > move_startScene_time) || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}
