using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json.Linq;
using ReactNative.Bridge;
using System;
using System.Collections.Generic;

namespace ReactWindowsApplicationInsights
{
    class ReactNativeApplicationInsightsModule : NativeModuleBase
    {
        private const int SeverityLevelVerbose = (int)SeverityLevel.Verbose;
        private const int SeverityLevelInformation = (int)SeverityLevel.Information;
        private const int SeverityLevelWarning = (int)SeverityLevel.Warning;
        private const int SeverityLevelError = (int)SeverityLevel.Error;
        private const int SeverityLevelCritical = (int)SeverityLevel.Critical;

        private readonly TelemetryClient _telemetryClient = new TelemetryClient();

        public override string Name
        {
            get
            {
                return "ApplicationInsights";
            }
        }

        public override IReadOnlyDictionary<string, object> Constants
        {
            get
            {
                return new Dictionary<string, object>
                {
                    {
                        "SeverityLevel",
                        new Dictionary<string, object>
                        {
                            { "Verbose", SeverityLevelVerbose },
                            { "Information", SeverityLevelInformation },
                            { "Warning", SeverityLevelWarning },
                            { "Error", SeverityLevelError },
                            { "Critical", SeverityLevelCritical },
                        }
                    },
                };
            }
        }

        [ReactMethod]
        public void setup(string instrumentationKey)
        {
            _telemetryClient.InstrumentationKey = instrumentationKey;
        }

        [ReactMethod]
        public void trackDependency(string dependencyName, string commandName, long startTimeUnixMs, long durationMs, bool success)
        {
            var startTime = DateTimeOffset.FromUnixTimeMilliseconds(startTimeUnixMs);
            var duration = TimeSpan.FromMilliseconds(durationMs);
            _telemetryClient.TrackDependency(dependencyName, commandName, startTime, duration, success); 
        }

        [ReactMethod]
        public void trackEvent(string eventName, JObject propertiesJson, JObject metricsJson)
        {
            var properties = ConvertMap<string>(propertiesJson);
            var metrics = ConvertMap<double>(metricsJson);
            _telemetryClient.TrackEvent(eventName, properties, metrics);
        }

        [ReactMethod]
        public void trackException(string message, JObject propertiesJson, JObject metricsJson)
        {
            var exception = new Exception(message);
            var properties = ConvertMap<string>(propertiesJson);
            var metrics = ConvertMap<double>(metricsJson);
            _telemetryClient.TrackException(exception, properties, metrics);
        }

        [ReactMethod]
        public void trackMetric(string name, double value, JObject propertiesJson)
        {
            var properties = ConvertMap<string>(propertiesJson);
            _telemetryClient.TrackMetric(name, value, properties);
        }

        [ReactMethod]
        public void trackPageView(string name)
        {
            _telemetryClient.TrackPageView(name);
        }

        [ReactMethod]
        public void trackRequest(string name, long startTimeUnixMs, long durationMs, string responseCode, bool success)
        {
            var startTime = DateTimeOffset.FromUnixTimeMilliseconds(startTimeUnixMs);
            var duration = TimeSpan.FromMilliseconds(durationMs);
            _telemetryClient.TrackRequest(name, startTime, duration, responseCode, success);
        }
        
        [ReactMethod]
        public void trackTrace(string message, int severity, JObject propertiesJson)
        {
            var severityLevel = (SeverityLevel)severity;
            var properties = ConvertMap<string>(propertiesJson);
            _telemetryClient.TrackTrace(message, severityLevel, properties);
        }

        private static IDictionary<string, T> ConvertMap<T>(JObject json)
        {
            var result = default(IDictionary<string, T>);
            if (json != null)
            {
                result = new Dictionary<string, T>(json.Count);
                foreach (var pair in json)
                {
                    result.Add(pair.Key, pair.Value.Value<T>());
                }
            }

            return result;
        }
    }
}
