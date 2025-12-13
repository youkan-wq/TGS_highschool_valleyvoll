using UnityEngine;

public static class DataManager
{
    // スタート位置（右側）
    private static Vector2 start_point_position;

    // ゴール位置（左側）
    private static Vector2 end_point_position;

    // 判定位置
    private static Vector2 judgement_point_position;



    public static Vector2 GetStartPointPosition()
    {
        return start_point_position;
    }

    public static void SetStartPointPosition(Vector2 pos)
    {
        this.start_point_position = pos;
    }

    public static Vector2 GetEndPointPosition()
    {
        return end_point_position;
    }

    public static void SetEndPointPosition(Vector2 pos)
    {
        this.end_point_position = pos;
    }

    public static Vector2 GetJudgementPointPosition()
    {
        return judgement_point_position;
    }

    public static void SetJudgementPointPosition(Vector2 pos)
    {
        this.judgement_point_position = pos;
    }

    // ゲームを終了する
    public static void StopGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタなら再生停止
#else
        Application.Quit(); // ビルドなら終了
#endif
    }
}
