using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Common.YZExtensions
{
    /// <summary>
    /// 获取客户端真实的Ip地址
    /// </summary>
   public class GetClientIpAddress
    {
        /// <summary>
        /// 得到请求的客户端IP地址（字符串）
        /// </summary>
        public string UserClientIpAddress => GetUserClientIpAddress();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly HttpContext _httpContext;
        public GetClientIpAddress(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = httpContextAccessor.HttpContext;
        }
        protected virtual string GetUserClientIpAddress()
        {
            try
            {
                var ipAddress = string.Empty;
                var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
                ipAddress = httpContext?.Connection?.RemoteIpAddress?.ToString();
                if (ipAddress!=string.Empty)
                {
                    return ipAddress;
                }
                return ipAddress;
            }
            catch (Exception )
            {
                return string.Empty;
            }
        }
    }
}
