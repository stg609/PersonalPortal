using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Services
{
    public class ServerOptions
    {
        public string Name { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string[] Tags { get; set; }
        public string HealthCheckUrl { get; set; }
    }
}
