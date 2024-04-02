using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Camera.UI.Models;
using Joint.Data.Models;

namespace Camera.UI.Services;


public interface IServerNotificationService
{
    Task<ServerResponse<List<Joint.Data.Models.Notification>>> GetNotification(int cameraId);
    
}

public class ServerNotificationService : IServerNotificationService
{
    private readonly HttpClient _httpClient;

    public ServerNotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<ServerResponse<List<Notification>>> GetNotification(int cameraId)
    {
        try
        {
            var cameras = await _httpClient.GetAsync("api/camera/camera");
            var res = await ServerResponse<List<Joint.Data.Models.Notification>>.CreateFromJson(cameras);
            
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