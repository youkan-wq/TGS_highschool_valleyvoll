using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    private float sensor_value;

    private float start_attack_th = 25;

    void Start()
    {
        sensor_value = 0;
    }

    void Update()
    {
        sensor_value = SensorReader.GetPressure();

        if ((sensor_value > start_attack_th) || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
