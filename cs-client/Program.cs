using System;
using System.Text.Json.Serialization;

namespace cs_client
{
    public class Program
    {
        public static void Main()
        {
            NativeMessagingHost host = new NativeMessagingHost(Console.OpenStandardInput(), Console.OpenStandardOutput(), new LogWriter("NativeMessagingHost"));

            host.Listen();
        }
    }
}
