using UnityEngine;

public static class Debugger
{
    public static void Log(string content)
    {
#if UNITY_EDITOR
        Debug.Log(content);
#endif
    }
}
