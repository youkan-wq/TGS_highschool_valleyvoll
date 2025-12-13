using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SensorReader : MonoBehaviour
{
    [Header("Serial Settings")]
    public string portName = "/dev/cu.usbserial-0001"; // Mac例

    [SerializeField]
    private int baudRate;

    // 圧力センサーの値（他スクリプトから参照可）
    private static float pressure = 0f;

    SerialPort serial;
    Thread readThread;
    bool running = false;
    object lockObj = new object();

    void Start()
    {
        serial = new SerialPort(portName, baudRate);
        serial.ReadTimeout = 50;
        serial.NewLine = "\n";   // 改行区切り

        try
        {
            serial.Open();
            Debug.Log("Serial Opened");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Serial Open Failed: " + e.Message);
            return;
        }

        running = true;
        readThread = new Thread(ReadSerialLoop);
        readThread.Start();
    }

    void ReadSerialLoop()
    {
        while (running && serial != null && serial.IsOpen)
        {
            try
            {
                string line = serial.ReadLine().Trim();

                if (float.TryParse(line, out float value))
                {
                    lock (lockObj)
                    {
                        pressure = value;
                        //Debug.Log(pressure);
                    }
                }
            }
            catch (System.TimeoutException)
            {
                // 無視
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Serial Read Error: " + e.Message);
            }
        }
    }

    // メインスレッドから安全に取得
    public static float GetPressure()
    {
        return pressure;
    }

    void OnDestroy()
    {
        running = false;

        if (readThread != null && readThread.IsAlive)
        {
            readThread.Join();
        }

        if (serial != null && serial.IsOpen)
        {
            serial.Close();
        }
    }
}
