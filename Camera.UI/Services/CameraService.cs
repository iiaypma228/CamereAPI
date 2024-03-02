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
        Task<ServerResponse<string>> SaveCamera(Joint.Data.Models.Camera camera);
        Task<ServerResponse<string>> DeleteCamera(string cameraId);
    }
    public class CameraService : ICameraService
    {
        private readonly HttpClient _httpClient;
        public CameraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServerResponse<string>> DeleteCamera(string cameraId)
        {
            try
            {
                var cameras = await _httpClient.DeleteAsync($"api/camera/camera?id={cameraId}");
                return await ServerResponse<string>.CreateFromString(cameras);
            }
            catch (Exception e)
            {
                return new ServerResponse<string>()
                {
                    IsSuccess = false,
                    Error = e.Message
                };
            }
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

        public async Task<ServerResponse<string>> SaveCamera(Joint.Data.Models.Camera camera)
        {
            try
            {
                var cameras = await _httpClient.PostAsJsonAsync<Joint.Data.Models.Camera>("api/camera/camera", camera);
                return await ServerResponse<string>.CreateFromString(cameras);
            }
            catch (Exception e)
            {
                return new ServerResponse<string>()
                {
                    IsSuccess = false,
                    Error = e.Message
                };
            }
        }



    }
}
