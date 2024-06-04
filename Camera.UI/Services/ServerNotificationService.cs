using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Camera.UI.Models;
using Joint.Data.Models;

namespace Camera.UI.Services;


public interface IServerNotificationService
{
    Task<ServerResponse<string>> Save(Notification notification);
    Task<ServerResponse<string>> Delete(Notification notification);
    Task<ServerResponse<List<Notification>>> GetAll();
    Task<ServerResponse<List<Notification>>> GetByCamera(int cameraIndex);

    Task<ServerResponse<string>> LinkWithCamera(int cameraIndex, int notificationId);

    Task<ServerResponse<string>> UnlinkWithCamera(int cameraIndex, int notificationId);
    
    IObservable<ObservableCollection<Joint.Data.Models.Notification>> NotificationsObservable { get; }
}

public class ServerNotificationService : IServerNotificationService
{
    private readonly HttpClient _httpClient;
    private BehaviorSubject<ObservableCollection<Joint.Data.Models.Notification>?> _notificationsSubject =
        new (null);

    public async Task<ServerResponse<string>> LinkWithCamera(int cameraIndex, int notificationId)
    {
       
        try
        {
            var notifies = await _httpClient.PostAsync($"api/camera/linkNotify?cameraId={cameraIndex}&notificationId={notificationId}", new StringContent(""));
            var res = await ServerResponse<List<Notification>>.CreateFromString(notifies);
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

    public async Task<ServerResponse<string>> UnlinkWithCamera(int cameraIndex, int notificationId)
    {
       
        try
        {
            var notifies = await _httpClient.DeleteAsync($"api/camera/linkNotify?cameraId={cameraIndex}&notificationId={notificationId}");
            var res = await ServerResponse<List<Notification>>.CreateFromString(notifies);
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
    
    public IObservable<ObservableCollection<Joint.Data.Models.Notification>> NotificationsObservable
    {
        get => _notificationsSubject.SelectMany(async (i) =>
        {
            if (_notificationsSubject.Value is null)
                await GetAll();
                
            return _notificationsSubject.Value!;
        });
    }
    
    public ServerNotificationService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }
    
    
    public async Task<ServerResponse<string>> Save(Notification notification)
    {
        try
        {
            var notifies = await _httpClient.PostAsJsonAsync("api/notification/notify", notification);
            var res = await ServerResponse<List<Notification>>.CreateFromString(notifies);
            await GetAll();
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

    public async Task<ServerResponse<string>> Delete(Notification notification)
    {
        try
        {
            var notifies = await _httpClient.DeleteAsync($"api/notification/notify?id={notification.Id}");
            await this.GetAll();
            return await ServerResponse<string>.CreateFromString(notifies);
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
            if (res.IsSuccess)
            {
                _notificationsSubject.OnNext(new ObservableCollection<Notification>(res.Data));
            }
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
            var notifies = await _httpClient.GetAsync($"api/notification/notifyByCamera?cameraId={cameraIndex}");
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