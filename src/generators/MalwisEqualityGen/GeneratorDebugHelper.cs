#if DEBUG
using System.Diagnostics;
#endif

using System;
using System.IO;

namespace EqualityGenerator
{
    public sealed class GeneratorDebugHelper : IDisposable
    {
        /// <summary>
        /// Only available in DEBUG
        /// </summary>
        public GeneratorDebugHelper(DebuggingOutputMode debugMode = DebuggingOutputMode.Debugger, string prefix = "source generator", string nullString = "null")
        {
            Counter = 0;

            DebugMode = debugMode;
            _prefix = prefix;
            _null = nullString;
            
            string path = $"{prefix}_{Guid.NewGuid()}.log";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            //File.Create(path);

            _stream = new StreamWriter(path);
        }
        
        private readonly string _null;
        private readonly StreamWriter _stream;

        private readonly string _prefix;
        internal int Counter { get; set; }
        internal DebuggingOutputMode DebugMode { get; }

        /// <summary>
        /// Only available in DEBUG
        /// </summary>
        public enum DebuggingOutputMode
        {
            Debugger,
            File
        }

#if DEBUG
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

        private void DoCount()
        {
            DebugLine($"DebugCount: {Counter}", false);
            Counter++;
        }

        internal void DebugLine(bool doCount = false) => DebugLine("", doCount);
        internal void DebugLine(object message, bool doCount = false) => DebugLine(message is null ? "null" : message.ToString(), doCount);
        internal void DebugLine(string message, bool doCount = false)
        {
            if (doCount)
            {
                DoCount();
            }
            string msg = $"{_prefix} : {message ?? _null}";

            switch (DebugMode)
            {
                case DebuggingOutputMode.Debugger:
                    Debug.WriteLine(msg);
                    break;
                case DebuggingOutputMode.File:
                    _stream.WriteLine(msg);
                    break;
                default:
                    throw new Exception($"The {nameof(DebuggingOutputMode)} {DebugMode} is currently not supported.");
            }
        }

        internal void DebugLine(object message, string heading, bool doCount = false)
        {
            DebugHeader(heading, true, doCount);
            DebugLine(message, false);
            DebugHeader(heading, false, false);
        }
        
        private void DebugHeader(object heading, bool isHeadingStart, bool doCount = false) => DebugLine($"--- {heading} {(isHeadingStart ? "start" : "end")} ---", doCount);

#else
    internal static void DebugLine(params object[] args) { }
#endif
        public void Dispose() => _stream?.Dispose();

        ~GeneratorDebugHelper() => Dispose();
    }
    
    
}
