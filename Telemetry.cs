using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        public static async Task sendTelemetry(TelemetryPackageType telemetryPackageType, string projectFolder)
        {
            TelemetryPolicy telemetryPolicy = Program.winboxSettings.telemetry_policy ?? TelemetryPolicy.doNotSend;
            if (telemetryPolicy == TelemetryPolicy.doNotSend) return;

            var eventParams = new
            {
                project_name = "MyProject",
                status = "success",
                build_time = 42,
                is_debug = true
            };

            await rawSendTelemetry(getTelemetryClientId(), getTelemetryPackageEventName(telemetryPackageType), eventParams);
        }

        private static string getTelemetryClientId()
        {
            return Program.winboxSettings.telemetry_client_id ?? "00000000-0000-0000-0000-000000000000";
        }

        private static string getTelemetryPackageEventName(TelemetryPackageType telemetryPackageType)
        {
            switch (telemetryPackageType)
            {
                case TelemetryPackageType.StartBuild:
                    return "Start build";
                case TelemetryPackageType.SuccessfulBuild:
                    return "Successful build";
                case TelemetryPackageType.BuildFailed:
                    return "Build failed";
                case TelemetryPackageType.NewProject:
                    return "New project";
            }

            return "unknown";
        }

        private static async Task rawSendTelemetry(string clientId, string eventName, object eventParams = null)
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
        }
    }
}
