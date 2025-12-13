using UnityEngine;
using TMPro;

public class ResoltScript : MonoBehaviour
{
    [SerializeField, Tooltip("スコアを表示するテキスト")]
    private TextMeshProUGUI scoreText;

    private int score;

    void Start()
    {
        score = DataManager.GetScore();

        string mes = "SCORE : " + score + "!!";
        scoreText.text = mes;
        scoreText.gameObject.SetActive(true);
    }
}
