using System;
using System.Net;
using System.Net.Http;

namespace Core.WebApi.Extensions
{
    public static class RequestExtensions
    {
        public static HttpResponseMessage CreateResponse<T>(this HttpRequestMessage request, HttpStatusCode statusCode, T value, DateTimeOffset? lastModifiedValue)
        {
            var httpResponseMessage = request.CreateResponse(statusCode, value);
            httpResponseMessage.Content.Headers.LastModified = lastModifiedValue;
            return httpResponseMessage;
        }

        public static HttpResponseMessage CreateResponse<T>(this HttpRequestMessage request, HttpStatusCode statusCode, T value, DateTime lastModifiedValue)
        {
            var httpResponseMessage = request.CreateResponse(statusCode, value);

            httpResponseMessage.Content.Headers.LastModified = new DateTimeOffset(lastModifiedValue, TimeSpan.Zero);
            return httpResponseMessage;
        }
    }
}