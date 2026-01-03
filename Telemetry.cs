using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace WinBox_Maker
{
    public enum TelemetryPackageType
    {
        StartBuild,
        SuccessfulBuild,
        BuildFailed,
        NewProject,
        LoadProject
    };

    internal class Telemetry
    {
        private static readonly HttpClient client = new HttpClient();

        private static string MeasurementId = "G-16YG4VZ93N";
        private static string ApiSecret = "mXgrqkLMSdeCXqTiw3ENqA";
        private static string logPath = "winbox_temp/debug/telemetry.log";

        public static async Task sendTelemetry(TelemetryPackageType telemetryPackageType, string projectFolder)
        {
            TelemetryPolicy telemetryPolicy = Program.winboxSettings.telemetry_policy ?? TelemetryPolicy.doNotSend;
            if (telemetryPolicy == TelemetryPolicy.doNotSend) return;

            object eventParams = collectProjectTelemetry(telemetryPackageType, projectFolder);
            writeTelemetrySendLog(projectFolder, await internalSendTelemetry(telemetryPackageType, eventParams));
        }

        private static object collectProjectTelemetry(TelemetryPackageType telemetryPackageType, string projectFolder)
        {
            var projectTelemetry = new
            {
                timestamp = Program.getTimestamp(),
                project_name = "MyProject",
                status = "success",
                build_time = 42,
                is_debug = true,
                debug_mode = true
            };

            return projectTelemetry;
        }

        private static void writeTelemetrySendLog(string projectFolder, string log)
        {
            Program.appendLog(Path.Combine(projectFolder, logPath), log);
        }

        private static string getTelemetryClientId()
        {
            return Program.winboxSettings.telemetry_client_id ?? "00000000-0000-0000-0000-000000000000";
        }

        private static string getTelemetryPackageEventName(TelemetryPackageType telemetryPackageType)
        {
            return telemetryPackageType switch
            {
                TelemetryPackageType.StartBuild => "start_build",
                TelemetryPackageType.SuccessfulBuild => "successful_build",
                TelemetryPackageType.BuildFailed => "build_failed",
                TelemetryPackageType.NewProject => "new_project",
                TelemetryPackageType.LoadProject => "load_project",
                _ => "unknown"
            };
        }

        private static async Task<string> internalSendTelemetry(TelemetryPackageType telemetryPackageType, object eventParams = null)
        {
            string sendTelemetryResult = "unknown";
            try
            {
                sendTelemetryResult = await rawSendTelemetry(getTelemetryClientId(), getTelemetryPackageEventName(telemetryPackageType), eventParams);
            }
            catch (Exception e)
            {
                sendTelemetryResult = e.ToString();
            }
            return sendTelemetryResult;
        }

        private static async Task<string> rawSendTelemetry(string clientId, string eventName, object eventParams = null)
        {
            var payload = new
            {
                client_id = clientId,
                events = new[]
                {
                new
                {
                    name = eventName,
                    @params = eventParams
                }
            }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://www.google-analytics.com/mp/collect?measurement_id={MeasurementId}&api_secret={ApiSecret}";
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            return response.ToString();
        }
    }
}
