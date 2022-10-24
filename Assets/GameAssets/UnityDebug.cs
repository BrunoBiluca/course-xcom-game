using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public interface IBilucaLoggable
    {
        IBilucaLogger Logger { get; set; }
    }

    public interface IBilucaLogger
    {
        void Log(params string[] message);
        void LogHighlight(string highlight, params string[] message);
        void Setup(LogLevels logLevel);

        public enum LogLevels
        {
            None,
            Error,
            Warn,
            Info
        }
    }

    public class DefaultLogger : IBilucaLogger
    {
        public IBilucaLogger.LogLevels LogLevel { get; private set; }

        private bool CanLogInfo => LogLevel >= IBilucaLogger.LogLevels.Info;
        private bool CanLogWarn => LogLevel >= IBilucaLogger.LogLevels.Warn;
        private bool CanLogError => LogLevel >= IBilucaLogger.LogLevels.Error;

        public DefaultLogger(IBilucaLogger.LogLevels logLevel)
        {
            LogLevel = logLevel;
        }

        public void Setup(IBilucaLogger.LogLevels logLevel)
        {
            LogLevel = logLevel;
        }

        public void LogHighlight(string highlight, params string[] message)
        {
            var newMessage = new List<string> {
                $"[{highlight}] - "
            };
            newMessage.AddRange(message);
            Log(newMessage.ToArray());
        }

        public void Log(params string[] message)
        {
            LogInfo(string.Join(" ", message));
        }

        private void LogInfo(string message)
        {
            if(!CanLogInfo) return;
            Debug.Log(message);
        }
    }

    public class UnityDebug : Singleton<UnityDebug>, IBilucaLogger
    {
        [field: SerializeField]
        public IBilucaLogger.LogLevels LogLevel { get; private set; }

        private IBilucaLogger logger;

        protected override void PreAwake()
        {
            LogLevel = IBilucaLogger.LogLevels.Info;
            logger = new DefaultLogger(LogLevel);
        }

        public void Setup(IBilucaLogger.LogLevels logLevel)
        {
            LogLevel = logLevel;
            logger.Setup(LogLevel);
        }

        public void LogHighlight(string highlight, params string[] message)
        {
            logger.Setup(LogLevel);
            logger.LogHighlight(highlight, message);
        }

        public void Log(params string[] message)
        {
            logger.Setup(LogLevel);
            logger.Log(message);
        }
    }
}
