using System.Diagnostics;
using UnityEngine;

public class shootingDebug : MonoBehaviour
{
    // Set these flags if your project uses different inputs
    [SerializeField] private bool watchFire1 = true;
    [SerializeField] private bool watchMouse0 = true;
    [SerializeField] private bool watchSpace = false;

    void Update()
    {
        bool fired = (watchFire1 && Input.GetButtonDown("Fire1"))
                     || (watchMouse0 && Input.GetMouseButtonDown(0))
                     || (watchSpace && Input.GetKeyDown(KeyCode.Space));

        if (fired)
        {
            UnityEngine.Debug.Log($"[ShootingDebug] Shoot input detected at time {Time.time}");
            LogManagedStackTrace();
        }
    }

    private void LogManagedStackTrace()
    {
        // Skip this method's frame
        var st = new StackTrace(1, true);
        var frames = st.GetFrames();
        if (frames == null) return;

        UnityEngine.Debug.Log("[ShootingDebug] Managed stack trace (first non-Unity frames):");
        foreach (var f in frames)
        {
            var m = f.GetMethod();
            if (m == null) continue;

            var declaring = m.DeclaringType;
            // Optionally skip Unity internals and system frames
            if (declaring != null && (declaring.Namespace?.StartsWith("UnityEngine") == true || declaring.Namespace?.StartsWith("System") == true))
                continue;

            string file = f.GetFileName() ?? "<no-file>";
            int line = f.GetFileLineNumber();
            UnityEngine.Debug.Log($"{declaring?.FullName}.{m.Name} ({file}:{line})");
        }
    }
}
