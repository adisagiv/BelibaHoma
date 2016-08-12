using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Generic.Delegates;
using Generic.Interfaces;
using Generic.Models;

namespace Generic.Client
{
    public class GenericClient :  IGenericClient, IDisposable
    {
        #region Events

        public event GenericEventHandler<StatusModel<ProgressModel>> OnProgressEvent;

        #endregion

        #region Fields
        
        private Uri _baseAddress;
        private HttpClient _client;

        #endregion

        public GenericClient() : 
            this("")
        {
            
        }

        public GenericClient(string baseAddress)
            : this(new Uri(baseAddress))
        {
            
        }

        public GenericClient(Uri baseAddress)
        {
            _baseAddress = baseAddress;
            var httpClientHandler = new HttpClientHandler
            {
                UseProxy = false
            };
            _client = new HttpClient(httpClientHandler)
            {
                BaseAddress = _baseAddress
            };
        }

        /// <summary>
        /// Download data as a string Async
        /// </summary>
        /// <param name="query">query string</param>
        /// <returns>data as string</returns>
        public async Task<string> DownloadAsStringAsync(string query)
        {
            var result = String.Empty;

            Func<byte[], string> func = (buffer) =>
            {

                var str = System.Text.Encoding.UTF8.GetString(buffer);

                result += str;

                return result;
            };

            result = await GetData(func, query);

            return result;
        }

        /// <summary>
        /// Download data as a file Async
        /// </summary>
        /// <param name="query">query string</param>
        /// <returns>path to file</returns>
        public async Task<string> DownloadAsFileAsync(string path,string query)
        {
            var result = String.Empty;
            using (FileStream writer = new FileStream(path,FileMode.Create))
            {
                using (BinaryWriter bWriter = new BinaryWriter(writer,Encoding.UTF8))
                {
                    Func<byte[], string> func = (buffer) =>
                    {


                        bWriter.Write(buffer);
                        //var bufferChar = System.Text.Encoding.UTF8.GetString(buffer).ToCharArray();
                        //writer.Write(bufferChar);

                        return path;
                    };

                    result = await GetData(func, query);
                }
            }

            return result;
        }

        public async Task<byte[]> DownloadAsDataAsync(string query)
        {
            Func<byte[], byte[]> func = (buffer) =>
            {
                //var bufferChar = System.Text.Encoding.UTF8.GetString(buffer).ToCharArray();
                //writer.Write(bufferChar);

                return buffer;
            };

            byte[] result = await GetData(func, query);


            return result;
        }

        private async Task<T> GetData<T>(Func<byte[], T> writeData, string query)
        {
            T result = default(T);

            using (HttpResponseMessage response = await _client.GetAsync(query,HttpCompletionOption.ResponseHeadersRead))
            {

                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : 0L;
                var canReportProgress = OnProgressEvent != null;
                
                using (HttpContent content = response.Content)
                {
                    using (var stream = await content.ReadAsStreamAsync())
                    {
                        var totalRead = 0L;
                        var buffer = new byte[4096];
                        var isMoreToRead = true;

                        do
                        {
                            //token.ThrowIfCancellationRequested();

                            var read = await stream.ReadAsync(buffer, 0, buffer.Length);

                            if (read == 0)
                            {
                                isMoreToRead = false;
                            }
                            else
                            {
                                var data = new byte[read];
                                buffer.ToList().CopyTo(0, data, 0, read);
                                

                                // save data
                                result = writeData(data);
                                
                                totalRead += read;

                                if (canReportProgress)
                                {
                                    var progress = new ProgressModel {
                                        Progress = totalRead,
                                        Total = total
                                    };
                                    var progressStatus = new StatusModel<ProgressModel>(true,String.Format("Downloaded {0}/{1}, {2:P}", progress.Progress, progress.Total,progress.InRatio ),progress);
                                    
                                    // Sends progress
                                    OnProgressEvent(query, progressStatus);
                                }
                            }
                        } while (isMoreToRead);
                        
                        var status = new StatusModel<T>(true,"Finish downloading",result);

                        //if (OnGotDataEvent != null)
                        //{
                        //    OnGotDataEvent(result,status);
                        //}
                    }
                }
            }

            return result;
        }



        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
