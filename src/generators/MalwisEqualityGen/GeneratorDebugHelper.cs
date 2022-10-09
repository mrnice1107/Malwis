#if DEBUG
using System.Diagnostics;
#endif

using System;
using System.Collections.Generic;
using System.IO;

namespace EqualityGenerator
{
    internal sealed class GeneratorDebugHelper
    {
        /// <summary>
        /// Only available in DEBUG
        /// </summary>
        internal GeneratorDebugHelper(string outDir, DebuggingOutputMode debugMode = DebuggingOutputMode.Debugger, string prefix = "source generator", string nullString = "null")
        {
            Counter = 0;

            DebugMode = debugMode;
            _prefix = prefix;
            _null = nullString;
            
            _path = Path.Combine(outDir, $"{prefix}_{Guid.NewGuid()}.log");
            _lines = new List<string>();
        }
        
        private readonly string _path;
        private readonly string _null;
        private readonly List<string> _lines;

        private readonly string _prefix;
        internal int Counter { get; set; }
        internal DebuggingOutputMode DebugMode { get; }

        /// <summary>
        /// Only available in DEBUG
        /// </summary>
        internal enum DebuggingOutputMode
        {
            Debugger,
            File
        }

#if DEBUG
        /// <summary>
        /// Only available in DEBUG
        /// </summary>
        internal static void AttachDebugger()
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
                    _lines.Add(msg);
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
    internal void DebugLine(params object[] args) { }
#endif

        internal void Save()
        {
            if (DebugMode != DebuggingOutputMode.File)
            {
                return;
            }

            if (File.Exists(_path))
            {
                File.Delete(_path);
            }

            File.WriteAllLines(_path, _lines);
            throw new Exception($"p: {_path}, lines: {_lines.Count}");
        }
    }
    
    
}
