using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceConsoleApp
{
    class Program
    {
        static ServiceClient serviceClient;
        static string connectionString = "HostName=iot-hub-home.azure-devices.net;DeviceId=MyHomeLight;SharedAccessKey=J/LlyDHCQP8w1iYegE2Q2SIo7i9co5syyuScwcJ+h9I=";//ConfigurationManager.AppSettings["IoTHubConnString"]; //"HostName=IOTDevTeam.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=FbHax7vXw/5ndA+lh9n7ursSMu4fwzSMX7JiYlU4iMI=";
        static void Main(string[] args)
        {
            Console.WriteLine("Send Cloud-to-Device message\n");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            Console.WriteLine("Press any key to send a C2D message.");
            Console.ReadLine();
            SendCloudToDeviceMessageAsync().Wait();
            Console.ReadLine();
        }
        private async static Task SendCloudToDeviceMessageAsync()
        {
            var commandMessage = new
             Message(Encoding.ASCII.GetBytes("Cloud to device message."));
            await serviceClient.SendAsync(ConfigurationManager.AppSettings["DeviceName"], commandMessage);
        }
    }
}
