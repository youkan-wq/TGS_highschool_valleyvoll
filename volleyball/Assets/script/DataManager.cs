using UnityEngine;

public static class DataManager
{
    private static Vector2 start_point_pos;
    private static Vector2 end_point_pos;
    private static Vector2 target_point_pos;

    private static int score;



    public static Vector2 GetStartPointPos()
    {
        return start_point_pos;
    }

    public static Vector2 GetEndPointPos()
    {
        return end_point_pos;
    }

    public static Vector2 GetTargetPointPos()
    {
        return target_point_pos;
    }

    public static void SetStartPointPos(Vector2 pos)
    {
        start_point_pos = pos;
    }

    public static void SetEndPointPos(Vector2 pos)
    {
        end_point_pos = pos;
    }

    public static void SetTargetPointPos(Vector2 pos)
    {
        target_point_pos = pos;
    }



    public static int GetScore()
    {
        return score;
    }

    public static void SetScore(int set_score)
    {
        score = set_score;
    }
}
