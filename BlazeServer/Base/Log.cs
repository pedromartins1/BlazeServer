using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;

namespace BlazeServer
{
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warning = 4,
        Error = 8,
        Data = 16,
        All = 31
    }

    public static class Log
    {
        private static string _filename;
        private static LogLevel _logLevel;
        private static StringBuilder _writeString;

        /// <summary>
        /// Creates the log file stream and sets the initial log level.
        /// </summary>
        /// <param name="filename">The output filename. This file will be overwritten if 'clear' is set.</param>
        /// <param name="logLevel">The <see cref="LogLevel" /> value which sets the type of messages to output.</param>
        /// <param name="clear">Whether or not to clear the log file on initialization.</param>
        public static void Initialize(string filename, LogLevel logLevel, bool clear)
        {
            _filename = filename;
            _logLevel = logLevel;
            _writeString = new StringBuilder();

            try
            {
                File.Delete(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Logfile couldn't be deleted: {0}", e.ToString());
            }
        }

        /// <summary>
        /// Internal method which writes a message directly to the log file.
        /// </summary>
        private static void Write(String message, LogLevel level)
        {
            StackTrace trace = new StackTrace();
            StackFrame frame = null;

            frame = trace.GetFrame(2);

            string caller = "";

            if (frame != null && frame.GetMethod().DeclaringType != null)
            {
                caller = frame.GetMethod().DeclaringType.Name + ": ";
            }

            switch (level)
            {
                case LogLevel.Debug:
                    message = "DEBUG: " + message;
                    break;
                case LogLevel.Info:
                    message = "INFO: " + message;
                    break;
                case LogLevel.Warning:
                    message = "WARNING: " + message;
                    break;
                case LogLevel.Error:
                    message = "ERROR: " + message;
                    break;
            }

            /*try
            {
                _logWriter = new StreamWriter(_filename, true);
            }
            catch (IOException)
            {
                _logWriter = new StreamWriter(_filename + "." + Process.GetCurrentProcess().Id.ToString(), true);
            }*/

            String text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + " - " + caller + message;

            Console.WriteLine(text);

            if (!MayWriteType(level))
            {
                return;
            }

            _writeString.AppendLine(text);
        }

        /// <summary>
        /// Checks whether the log level contains the specified flag.
        /// </summary>
        /// <param name="type">The <see cref="LogLevel" /> value to check.</param>
        private static bool MayWriteType(LogLevel type)
        {
            return ((_logLevel & type) == type);
        }

        public static void WriteAway()
        {
            String stringToWrite = _writeString.ToString();
            _writeString.Length = 0;

            StreamWriter _logWriter;
            _logWriter = new StreamWriter(_filename, true);
            _logWriter.Write(stringToWrite);
            _logWriter.Flush();
            _logWriter.Close();
            _logWriter.Dispose();
        }

        /// <summary>
        /// Writes data to the log file.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public static void Data(String message)
        {
            Write(message, LogLevel.Data);
        }

        /// <summary>
        /// Writes an error to the log file.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public static void Error(String message)
        {
            Write(message, LogLevel.Error);
        }

        /// <summary>
        /// Writes a warning to the log file.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public static void Warn(String message)
        {
            Write(message, LogLevel.Warning);
        }

        /// <summary>
        /// Writes an informative string to the log file.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public static void Info(String message)
        {
            Write(message, LogLevel.Info);
        }

        /// <summary>
        /// Writes a debug string to the log file.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public static void Debug(String message)
        {
            if (!Configuration.DebugLog)
            {
                return;
            }

            Write(message, LogLevel.Debug);
        }
    }
}
