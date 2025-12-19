using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    private float sensor_value;

    private float start_attack_th = 25;

    private int start_game_mode = 0;

    void Start()
    {
        sensor_value = 0;
        start_game_mode = 0;
    }

    void Update()
    {
        sensor_value = SensorReader.GetPressure();

        if ((sensor_value > start_attack_th) || Input.GetKeyDown(KeyCode.Space))
        {
            switch (start_game_mode)
            {
                case 0:
                    SceneManager.LoadScene("GameScene");
                    break;

                case 1:
                    SceneManager.LoadScene("SampleGameScene_1");
                    break;

                case 2:
                    SceneManager.LoadScene("SampleGameScene_2");
                    break;

                default:
                    SceneManager.LoadScene("GameScene");
                    break;
            }
        }

        // "シフトキー＋１"キーでサンプルゲームシーン１へ移動
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            start_game_mode = 1;
        }

        // "シフトキー＋２"キーでサンプルゲームシーン２へ移動
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            start_game_mode = 2;
        }
    }
}
