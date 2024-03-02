using Camera.UI.Models;
using Joint.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Camera.UI.Services
{
    public interface ICameraService
    {
        Task<ServerResponse<List<Joint.Data.Models.Camera>>> GetCameras();
    }
    public class CameraService : ICameraService
    {
        private readonly HttpClient _httpClient;
        public CameraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServerResponse<List<Joint.Data.Models.Camera>>> GetCameras()
        {
            try
            {
                var cameras = await _httpClient.GetAsync("api/camera/camera");
                return await ServerResponse<List<Joint.Data.Models.Camera>>.CreateFromJson(cameras);
            }
            catch (Exception e)
            {
                return new ServerResponse<List<Joint.Data.Models.Camera>>()
                {
                    IsSuccess = false,
                    Error = e.Message
                };
            }
        }
    }

}
