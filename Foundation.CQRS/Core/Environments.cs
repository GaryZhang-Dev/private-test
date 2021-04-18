using System;
using System.Collections.Generic;
using System.Text;

namespace Foundation.CQRS.Core
{
    public static class Environments
    {
        public const string EnvironmentKey = "ASPNETCORE_ENVIRONMENT";

        public static bool IsDevelopment() => EnvironmentName == "Development";
        public static bool IsProduction() => EnvironmentName == "Production";
        public static bool IsStaging() => EnvironmentName == "Staging";

        public static void VerifyEnvironmentName()
        {
            if (EnvironmentName.IsNullOrEmpty())
            {
                throw new Exception("EnvironmentName:[ASPNETCOR_ENVIRONMENT is NULL]");
            }
        }
        public static string EnvironmentName => Environment.GetEnvironmentVariable(EnvironmentKey);
    }
}
