using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace UI.Api
{
    public static class ApiClient
    {
        private static readonly HttpClient _client = new HttpClient();
        private static string _endpointUrl;

        public static void Init(string endpointUrl)
        {
            _endpointUrl = endpointUrl;
        }

        public static async Task<IReadOnlyList<StudentDto>> GetAll(string enrolledIn, string numberOfCourses)
        {
            Result<List<StudentDto>> result = await SendRequest<List<StudentDto>>($"?enrolled={enrolledIn}&number={numberOfCourses}", HttpMethod.Get).ConfigureAwait(false);
            return result.Value;
        }

        public static async Task<Result> Create(StudentDto dto)
        {
            Result result = await SendRequest<string>("/", HttpMethod.Post, dto).ConfigureAwait(false);
            return result;
        }

        public static async Task<Result> Update(StudentDto dto)
        {
            Result result = await SendRequest<string>("/" + dto.Id, HttpMethod.Put, dto).ConfigureAwait(false);
            return result;
        }

        public static async Task<Result> Delete(long id)
        {
            Result result = await SendRequest<string>("/" + id, HttpMethod.Delete).ConfigureAwait(false);
            return result;
        }

        private static async Task<Result<T>> SendRequest<T>(string url, HttpMethod method, object content = null)
             where T : class
        {
            var request = new HttpRequestMessage(method, $"{_endpointUrl}{url}");
            if (content != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage message = await _client.SendAsync(request).ConfigureAwait(false);
            string response = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (message.StatusCode == HttpStatusCode.InternalServerError)
                throw new Exception(response);

            var envelope = JsonConvert.DeserializeObject<Envelope<T>>(response);

            if (!message.IsSuccessStatusCode)
                return Result.Fail<T>(envelope.ErrorMessage);

            T result = envelope.Result;

            if (result == null && typeof(T) == typeof(string))
            {
                result = string.Empty as T;
            }

            return Result.Ok(result);
        }
    }
}
