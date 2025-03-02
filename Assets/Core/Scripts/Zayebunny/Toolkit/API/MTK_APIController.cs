using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Nocci.Zayebunny.Extensions;
using UnityEngine.Networking;

namespace Nocci.Zayebunny.API
{
    public class APIHit
    {
        private TaskCompletionSource<(bool, string)> _requestCompletion = new();

        public string body;
        public string method;
        public Action<string> OnError;
        public Action<string> OnSuccess;
        public string url;

        public APIHit(string body, string method)
        {
            this.body = body;
            this.method = method;
        }

        public async Task<(bool, string)> Hit()
        {
            _requestCompletion = new TaskCompletionSource<(bool, string)>();
            Request().Run();
            await _requestCompletion.Task;
            return _requestCompletion.Task.Result;
        }


        private IEnumerator Request()
        {
            using var request = new UnityWebRequest(new Uri(url));
            request.method = method;

            request.downloadHandler = new DownloadHandlerBuffer();

            if (!string.IsNullOrEmpty(body) && method == "POST")
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
                request.SetRequestHeader("Content-Type", "application/json");
            }

            yield return request.SendWebRequest();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError
                or UnityWebRequest.Result.DataProcessingError)
            {
                OnError?.Invoke(request.error);
                _requestCompletion.SetResult((false, request.error));
            }
            else
            {
                OnSuccess?.Invoke(request.downloadHandler.text);
                _requestCompletion.SetResult((true, request.downloadHandler.text));
            }

            request.Dispose();
        }
    }
}