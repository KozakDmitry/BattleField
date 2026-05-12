using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class TimeManager 
{
    public static Action<bool> Action_TimeChanged;

    static List<Type> _blockParents = new();
    public static DateTime GetTime()
    {
        return  DateTime.Now;
    }

    public static int GetDays(DateTime SavedDate)
    {
        return GetTime().Day - SavedDate.Day;
    }

    public static string GetTimeFormated(TimeSpan Time, bool isShort = false)
    {
        if (Time.Days > 0) return Time.Days + "d " + Time.Hours + "h";
        else if (Time.Hours > 0) return Time.Hours + "h " + Time.Minutes + "m";
        else return Time.Minutes + "m " + Time.Seconds + (isShort ? "" : ("." + Time.Milliseconds.ToString("000"))) + "s";
    }

    public static void ResetBlocks()
    {
        Time.timeScale = 1f;
        _blockParents.Clear();
    }

    public static void SetPause(bool isPause, Type type)
    {
        if (isPause) _blockParents.Add(type);
        if (!isPause) _blockParents.Remove(type);

        Time.timeScale = _blockParents.Count > 0 ? 0 : 1;
        Action_TimeChanged?.Invoke(isPause);
    }

    public static async UniTask StartTimer(float duration, Action<float> onTick, CancellationToken token)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            elapsed += Time.unscaledDeltaTime;
            onTick(Mathf.Max(0, duration - elapsed));
        }
    }

}
