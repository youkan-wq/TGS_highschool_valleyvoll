using System;
using UnityEngine;
using System.Collections.Generic;

public static class DataManager
{
    private static Vector2 start_point_pos;
    private static Vector2 end_point_pos;
    private static Vector2 target_point_pos;

    private static int score;

    private static List<string> ball_list = new List<string>();



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



    public static bool AddBallList(string new_ball_name)
    {
        // `new_ball_name`の形式チェック
        if (!BallNameCheck(new_ball_name)) return false;

        // リストに追加
        ball_list.Add(new_ball_name);

        return true;
    }

    public static bool AddBallList(int new_ball_num)
    {
        // 引数の値チェック
        if (new_ball_num < 0)
        {
            Debug.LogError($"番号が不正です: {new_ball_num}");
            return false;
        }

        // リストに追加
        AddBallList($"ball_{new_ball_num}");

        return true;
    }

    public static bool DeleteBallList(string delete_ball_name)
    {
        if (!BallNameCheck(delete_ball_name)) return false;

        // リストから削除 + 結果を返す
        return ball_list.Remove(delete_ball_name);
    }

    public static bool DeleteBallList(int delete_ball_num)
    {
        // 引数の値チェック
        if (delete_ball_num < 0)
        {
            Debug.LogError($"番号が不正です: {delete_ball_num}");
            return false;
        }

        // リストから削除 + 結果を返す
        return DeleteBallList($"ball_{delete_ball_num}");
    }

    private static bool BallNameCheck(string ball_name)
    {
        // nullチェック
        if (string.IsNullOrEmpty(ball_name))
        {
            Debug.LogError("ball_name が null / 空です");
            return false;
        }

        // 先頭が`ball_`で始まるかのチェック
        const string prefix = "ball_";
        if (!ball_name.StartsWith(prefix, StringComparison.Ordinal))
        {
            Debug.LogError($"形式が違います: {ball_name}（ball_n の形にしてください）");
            return false;
        }

        // 後ろの番号が不正な値じゃないかのチェック
        string numPart = ball_name.Substring(prefix.Length);
        if (!int.TryParse(numPart, out int n))
        {
            Debug.LogError($"番号が int ではありません: {ball_name}");
            return false;
        }

        return true;
    }

    public static List<string[]> ReturnBallListInfo()
    {
        return null;
    }

    public static List<string> ReturnBallLIst()
    {
        return ball_list;
    }
}
