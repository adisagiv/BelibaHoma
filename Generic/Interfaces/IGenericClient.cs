using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic.Delegates;
using Generic.Models;

namespace Generic.Interfaces
{
    public interface IGenericClient
    {

        event GenericEventHandler<StatusModel<ProgressModel>> OnProgressEvent;

        Task<string> DownloadAsStringAsync(string query);
        Task<string> DownloadAsFileAsync(string path, string query);

        Task<byte[]> DownloadAsDataAsync(string query);
    }
}
