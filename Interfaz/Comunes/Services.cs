namespace Interfaz.Comunes
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web.Script.Serialization;

    public class Services
    {
        public ResponseData CallGet(string url, int timeout = 0)
        {
            var responseData = new ResponseData();
            try
            {
                timeout = timeout == 0 ? 20000 : timeout;

                WebRequest wrGETURL = WebRequest.Create(url);
                wrGETURL.Method = "GET";
                wrGETURL.Timeout = timeout;
                wrGETURL.ContentType = @"application/json; charset=utf-8";
                wrGETURL.ContentLength = 0;

                HttpWebRequest request = wrGETURL as HttpWebRequest;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            string responseString = reader.ReadToEnd();
                            responseData.ErrorCode = 0;
                            responseData.Json = responseString;
                        }
                    }
                    else
                    {
                        responseData.StatusCode = (int)response.StatusCode;
                        responseData.ErrorDescription = response.StatusDescription;
                        responseData.ErrorCode = 2;
                    }
                }
            }
            catch (Exception error)
            {
                responseData.ErrorDescription = error.Message;
                responseData.ErrorCode = 1;
            }
            return responseData;
        }

        public ResponseData CallPost<T>(T model, string url, int timeout = 0)
        {
            var responseData = new ResponseData();
            try
            {
                timeout = timeout == 0 ? 20000 : timeout;
                var json = this.Serialize<T>(model);
                byte[] data = UTF8Encoding.UTF8.GetBytes(json);

                WebRequest wrGETURL = WebRequest.Create(url);
                wrGETURL.Method = "POST";
                wrGETURL.Timeout = timeout;
                wrGETURL.ContentType = @"application/json; charset=utf-8";
                wrGETURL.ContentLength = data.Length;

                HttpWebRequest request = wrGETURL as HttpWebRequest;

                using (var postStream = request.GetRequestStream())
                {
                    postStream.Write(data, 0, data.Length);
                    using (var response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                string responseString = reader.ReadToEnd();
                                responseData.ErrorCode = 0;
                                responseData.Json = responseString;
                            }
                        }
                        else
                        {
                            responseData.StatusCode = (int)response.StatusCode;
                            responseData.ErrorDescription = response.StatusDescription;
                            responseData.ErrorCode = 2;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                responseData.ErrorDescription = error.Message;
                responseData.ErrorCode = 1;
            }
            return responseData;
        }

        private string Serialize<T>(T entity)
        {
            return new JavaScriptSerializer().Serialize(entity);
        }

        public T Deserialize<T>(string json) where T : new()
        {
            var entity = new JavaScriptSerializer().Deserialize<T>(json);
            return entity;
        }
    }
}