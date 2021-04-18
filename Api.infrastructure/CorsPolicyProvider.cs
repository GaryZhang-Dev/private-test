using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.infrastructure
{
    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ConcurrentDictionary<string, CorsPolicy> _corsPolicies = new ConcurrentDictionary<string, CorsPolicy>();
        private readonly IConfiguration _configuration;

        public CorsPolicyProvider(IDistributedCache distributedCache, IConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _configuration = configuration;
        }

        public async Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
        {
            if (!_corsPolicies.ContainsKey(policyName))
            {

                string[] allowOrigins;
                allowOrigins = _configuration.GetSection("AllowOrigins").Get<string[]>();
                _corsPolicies.AddOrUpdate(policyName, new CorsPolicyBuilder()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .WithOrigins(allowOrigins)
                   .AllowCredentials().Build(), (string key, CorsPolicy corsPolicy) => corsPolicy);
            }
            return _corsPolicies[policyName];
        }
    }
}
