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
    internal class Telemetry
    {
        private static string MeasurementId = "G-16YG4VZ93N";
        private static string ApiSecret = "mXgrqkLMSdeCXqTiw3ENqA";

        public static async Task sendTelemetry()
        {
            TelemetryPolicy telemetryPolicy = Program.winboxSettings.telemetry_policy ?? TelemetryPolicy.doNotSend;
            if (telemetryPolicy == TelemetryPolicy.doNotSend) return;


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
