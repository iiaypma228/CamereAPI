using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.UI.Localize
{
    public static class CameraLocalize
    {
        public static Dictionary<Joint.Data.Constants.CameraConnection, string> CameraConnection
        {
            get
            {
                return new Dictionary<Joint.Data.Constants.CameraConnection, string>()
                {
                    {Joint.Data.Constants.CameraConnection.Cabel, "cabel" },
                    {Joint.Data.Constants.CameraConnection.Ethernet, "ethernet"}
                };
            }
        }
    }
}
