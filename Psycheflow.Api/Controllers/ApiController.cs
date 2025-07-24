using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Psycheflow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        [HttpGet("healthcheck")]
        public IActionResult GetHealthCheck()
        {
            object healthInfo = new
            {
                status = "Healthy",
                uptime = GetUptime(),
                machine = Environment.MachineName,
                timestamp = DateTime.UtcNow,
                memoryUsageMB = GetMemoryUsageInMB()
            };

            return Ok(healthInfo);
        }

        private string GetUptime()
        {
            using Process process = Process.GetCurrentProcess();
            TimeSpan uptime = DateTime.Now - process.StartTime;
            return uptime.ToString(@"dd\.hh\:mm\:ss");
        }

        private double GetMemoryUsageInMB()
        {
            using var process = Process.GetCurrentProcess();
            return Math.Round(process.WorkingSet64 / (1024.0 * 1024.0), 2);
        }
    }
}
