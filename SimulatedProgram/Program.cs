﻿using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedProgram
{
    class Program
    {
        private static DeviceClient s_deviceClient;

        // The device connection string to authenticate the device with your IoT hub.
        // Using the Azure CLI:
        // az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDotnetDevice --output table
        private readonly static string s_connectionString = "HostName=iot-hub-home.azure-devices.net;DeviceId=MyHomeLight;SharedAccessKey=J/LlyDHCQP8w1iYegE2Q2SIo7i9co5syyuScwcJ+h9I=";

        // Async method to send simulated telemetry
        private static async void SendDeviceToCloudMessagesAsync()
        {
            // Initial telemetry values
            double minTemperature = 20;
            double minHumidity = 60;
            Random rand = new Random();

            while (true)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                // Create JSON message
                var telemetryDataPoint = new
                {
                    temperature = currentTemperature,
                    humidity = currentHumidity
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                // Add a custom application property to the message.
                // An IoT hub can filter on these properties without access to the message body.
                message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                // Send the telemetry message
                await s_deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
            }
        }
        private static void Main(string[] args)
        {
            Console.WriteLine("IoT Hub Quickstarts #1 - Simulated device. Ctrl-C to exit.\n");

            // Connect to the IoT hub using the MQTT protocol
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, TransportType.Mqtt);
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
        ////private static void Main(string[] args)
        ////{
        ////    SimulatedDevice.MainSimulatedDevice(args);
        ////}

        //private static DeviceClient s_deviceClient;

        //// The device connection string to authenticate the device with your IoT hub.
        //// Using the Azure CLI:
        //// az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDotnetDevice --output table
        //private readonly static string s_connectionString = ConfigurationManager.AppSettings["IoTHubConnString"];//"HostName=IOTDevTeam.azure-devices.net;SharedAccessKeyName=device;SharedAccessKey=cTTNBNIlMBgl9e+j4fyWX1JIo0lLGEvmdG1AZicrmQU=";

        //// Async method to send simulated telemetry
        //private static async void SendDeviceToCloudMessagesAsync()
        //{
        //    // Initial telemetry values
        //    double minTemperature = 20;
        //    double minHumidity = 60;
        //    Random rand = new Random();

        //    while (true)
        //    {
        //        double currentTemperature = minTemperature + rand.NextDouble() * 15;
        //        double currentHumidity = minHumidity + rand.NextDouble() * 20;

        //        // Create JSON message
        //        var telemetryDataPoint = new
        //        {
        //            temperature = currentTemperature,
        //            humidity = currentHumidity
        //        };
        //        var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
        //        var message = new Message(Encoding.ASCII.GetBytes(messageString));

        //        // Add a custom application property to the message.
        //        // An IoT hub can filter on these properties without access to the message body.
        //        message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

        //        // Send the telemetry message
        //        await s_deviceClient.SendEventAsync(message);
        //        Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

        //        await Task.Delay(1000);
        //    }
        //}
        //public static void Main(string[] args)
        //{
        //    Console.WriteLine("IoT Hub Quickstarts #1 - Simulated device. Ctrl-C to exit.\n");

        //    var sconn = "HostName=kalpanik-iot-hub.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=9znQULKQmCqXp2aF5usIe6F29x6BThMEze1Oj6TlxWw=";
        //    // Connect to the IoT hub using the MQTT protocol
        //    s_deviceClient = DeviceClient.CreateFromConnectionString(sconn, TransportType.Mqtt);
        //    SendDeviceToCloudMessagesAsync();
        //    Console.ReadLine();
        //}

    }
}
