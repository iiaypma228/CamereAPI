using System.Net;
using System.Net.Http.Json;
using Joint.Data.Constants;
using Camera = Joint.Data.Models.Camera;

namespace TestAPI;

public class CameraTest : BaseTest
{
    private Joint.Data.Models.Camera _newCamera = new Joint.Data.Models.Camera
    {
        Id = 0,
        UserId = 0,
        User = null,
        Name = "NEW CAMERA UNIT TEST",
        Connection = CameraConnection.Ethernet,
        ConnectionData = "192.168.22.18:5090",
        Notifications = null
    };

    [Fact]
    public void TestCreateCamera()
    {
        var res = _httpClient.PostAsJsonAsync($"/api/camera/camera", _newCamera).Result;
        
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        
    }

    [Fact]
    public void TestCameraGet()
    {
        var res = _httpClient.GetAsync($"/api/camera/camera").Result;
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
    
    [Fact]
    public void TestCameraGetById()
    {
        var res = _httpClient.GetAsync($"/api/camera/camera?id=1").Result;
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
    
}