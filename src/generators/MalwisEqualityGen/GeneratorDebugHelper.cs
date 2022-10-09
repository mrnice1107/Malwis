#if DEBUG
using System;
using System.Diagnostics;
#endif

namespace EqualityGenerator
{
    public sealed class GeneratorDebugHelper
    {
#if DEBUG
        private static string _prefix;
        private static string _null;

        internal static int Counter { get; private set; }
        internal static DebuggingOutputMode DebugMode { get; private set; }

        /// <summary>
        /// Only available in DEBUG
        /// </summary>
        public enum DebuggingOutputMode
        {
            Debugger,
            /// <summary>
            /// This is not supported yet...
            /// </summary>
            File
        }

        /// <summary>
        /// Only available in DEBUG
        /// </summary>
        static GeneratorDebugHelper()
        {
            Counter = 0;

            SetupDebugHelper();
        }

        /// <summary>
        /// Only available in DEBUG
        /// </summary>
        private static void SetupDebugHelper(DebuggingOutputMode debugMode = DebuggingOutputMode.Debugger, string prefix = "generator: ", string nullString = "null")
        {
            DebugMode = debugMode;
            _prefix = prefix;
            _null = nullString;
        }

        /// <summary>
        /// Only available in DEBUG
        /// </summary>
        public static void AttachDebugger()
        {
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
        }

        private static void DoCount()
        {
            DebugLine($"DebugCount: {Counter}", false);
            Counter++;
        }

        internal static void DebugLine(bool doCount = false) => DebugLine("", doCount);
        internal static void DebugLine(object message, bool doCount = false) => DebugLine(message is null ? "null" : message.ToString(), doCount);
        internal static void DebugLine(string message, bool doCount = false)
        {
            if (doCount)
            {
                DoCount();
            }
            string msg = $"{_prefix}{message ?? _null}";

            switch (DebugMode)
            {
                case DebuggingOutputMode.Debugger:
                    Debug.WriteLine(msg);
                    break;
                case DebuggingOutputMode.File:
                default:
                    throw new Exception($"The {nameof(DebuggingOutputMode)} {DebugMode} is currently not supported.");
            }
        }

        internal static void DebugLine(object message, string heading, bool doCount = false)
        {
            DebugHeader(heading, true, doCount);
            DebugLine(message, false);
            DebugHeader(heading, false, false);
        }
        internal static void DebugHeader(object heading, bool isHeadingStart, bool doCount = false) => DebugLine($"--- {heading} {(isHeadingStart ? "start" : "end")} ---", doCount);

#else
    internal static void DebugLine(params object[] args) { }
#endif
    }
}
