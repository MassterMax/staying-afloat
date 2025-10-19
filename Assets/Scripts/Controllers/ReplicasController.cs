using System.Collections.Generic;
using UnityEngine;

public static class ReplicasController
{
    private static Dictionary<string, string> replicas = new Dictionary<string, string>()
    {
        { "next", "Продолжить..." },
        { "day_1", "День 1. Привет." },
        { "day_3", "День 3. test." },
        { "day_5", "День 5. Dsdadasd." },
    };

    public static string Get(string id)
    {
        if (replicas.TryGetValue(id, out string line))
            return line;
        else
        {
            Debug.LogWarning($"[DialogueReplicas] Реплика с id '{id}' не найдена!");
            return $"[MISSING REPLICA: {id}]";
        }
    }

    public static void Add(string id, string text)
    {
        if (!replicas.ContainsKey(id))
            replicas.Add(id, text);
        else
            replicas[id] = text;
    }
}
