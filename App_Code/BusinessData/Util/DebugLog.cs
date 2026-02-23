using System;
using System.Collections.Concurrent;

namespace BusinessData.Util
{
    public static class DebugLog
    {
        private static ConcurrentQueue<string> _log = new ConcurrentQueue<string>();
        private static volatile bool _isDone = false;

        public static void Write(string msg)
        {
            _log.Enqueue(DateTime.Now+" - "+msg);
        }

        public static string GetLog()
        {
            return string.Join("\r\n", _log.ToArray());
        }

        public static void Clear()
        {
            _log = new ConcurrentQueue<string>();
            _isDone = false;
        }

        public static void MarkDone()
        {
            _isDone = true;
        }

        public static bool IsDone()
        {
            return _isDone;
        }
    }
}
