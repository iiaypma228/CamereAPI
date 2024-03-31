using Camera.UI.Models;
using Joint.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Camera.UI.Services
{
    public interface ICameraService
    {
        Task<ServerResponse<List<Joint.Data.Models.Camera>>> GetCameras();
        Task<ServerResponse<string>> SaveCamera(Joint.Data.Models.Camera camera);
        Task<ServerResponse<string>> DeleteCamera(string cameraId);

        IObservable<ObservableCollection<Joint.Data.Models.Camera>> CamerasObservable { get; }

    }
    public class CameraService : ICameraService
    {
        public IObservable<ObservableCollection<Joint.Data.Models.Camera>> CamerasObservable
        {
            get => _camerasSubject.SelectMany(async (i) =>
            {
                if (_camerasSubject.Value is null)
                    await GetCameras();
                
                return _camerasSubject.Value!;
            });
        }
        
        private readonly HttpClient _httpClient;

        private BehaviorSubject<ObservableCollection<Joint.Data.Models.Camera>?> _camerasSubject =
            new BehaviorSubject<ObservableCollection<Joint.Data.Models.Camera>?>(null);
        public CameraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<ServerResponse<List<Joint.Data.Models.Camera>>> GetCameras()
        {
            try
            {
                var cameras = await _httpClient.GetAsync("api/camera/camera");
                var res = await ServerResponse<List<Joint.Data.Models.Camera>>.CreateFromJson(cameras);

                if (res.IsSuccess && res.Data != null)
                {
                    _camerasSubject.OnNext(new ObservableCollection<Joint.Data.Models.Camera>(res.Data!));
                }
                else
                {
                    _camerasSubject.OnError(new Exception(res.Error));
                }
                
                return res;
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

        public async Task<ServerResponse<string>> DeleteCamera(string cameraId)
        {
            try
            {
                var cameras = await _httpClient.DeleteAsync($"api/camera/camera?id={cameraId}");
                await this.GetCameras();
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
        

        public async Task<ServerResponse<string>> SaveCamera(Joint.Data.Models.Camera camera)
        {
            try
            {
                var cameras = await _httpClient.PostAsJsonAsync<Joint.Data.Models.Camera>("api/camera/camera", camera);
                await this.GetCameras();
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
