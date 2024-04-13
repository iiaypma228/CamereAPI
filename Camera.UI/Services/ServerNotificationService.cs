using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Camera.UI.Models;
using Joint.Data.Models;

namespace Camera.UI.Services;

public interface IServerNotificationService
{
    Task<ServerResponse<string>> Save(Notification notification);

    Task<ServerResponse<List<Notification>>> GetAll();

    Task<ServerResponse<List<Notification>>> GetByCamera(int cameraIndex);
    
}

public class ServerNotificationService : IServerNotificationService
{
    private readonly HttpClient _httpClient;

    public ServerNotificationService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }
    
    
    public async Task<ServerResponse<string>> Save(Notification notification)
    {
        try
        {
            var notifies = await _httpClient.PostAsJsonAsync("api/notification/notify", notification);
            var res = await ServerResponse<List<Joint.Data.Models.Notification>>.CreateFromString(notifies);
            
            return res;
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

    public async Task<ServerResponse<List<Notification>>> GetAll()
    {
        try
        {
            var notifies = await _httpClient.GetAsync("api/notification/notify");
            var res = await ServerResponse<List<Joint.Data.Models.Notification>>.CreateFromJson(notifies);
            
            return res;
        }
        catch (Exception e)
        {
            return new ServerResponse<List<Joint.Data.Models.Notification>>()
            {
                IsSuccess = false,
                Error = e.Message
            };
        }
    }

    public async Task<ServerResponse<List<Notification>>> GetByCamera(int cameraIndex)
    {
        try
        {
            var notifies = await _httpClient.GetAsync($"api/notification/notify?cameraId={cameraIndex}");
            var res = await ServerResponse<List<Joint.Data.Models.Notification>>.CreateFromJson(notifies);
            
            return res;
        }
        catch (Exception e)
        {
            return new ServerResponse<List<Joint.Data.Models.Notification>>()
            {
                IsSuccess = false,
                Error = e.Message
            };
        }
    }
}