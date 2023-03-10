using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace webapp.Helper {
    public class WebAppTelemetryInitializer : ITelemetryInitializer {
        public void Initialize(ITelemetry telemetry) {
            if(string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName)) {
                telemetry.Context.Cloud.RoleName = "webapp-frontend";
            }
        }
    }
}
