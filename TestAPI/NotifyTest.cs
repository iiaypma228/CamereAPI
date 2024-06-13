using System.Net;
using System.Net.Http.Json;
using Joint.Data.Constants;

namespace TestAPI;

public class NotifyTest : BaseTest
{
    private Joint.Data.Models.Notification _newNotify = new Joint.Data.Models.Notification
    {
        Id = 0,
        UserId = 0,
        User = null,
        Name = "NEW TEST NOTIFY",
        NotificationType = NotificationType.SMS,
        Data = "1234556789"
    };

        [Fact]
    public void TestCreateNotify()
    {
        var res = _httpClient.PostAsJsonAsync($"/api/Notification/notify", _newNotify).Result;
        
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        
    }

    [Fact]
    public void TestNotifyGet()
    {
        var res = _httpClient.GetAsync($"/api/Notification/notify").Result;
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
    
    [Fact]
    public void TestNotifyGetById()
    {
        var res = _httpClient.GetAsync($"/api/Notification/notify?id=1").Result;
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
    
}