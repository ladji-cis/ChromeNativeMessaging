using System;
using System.Text;
using System.IO;
using System.Text.Json;

namespace cs_client
{
    public class NativeMessagingHost
    {
        private readonly Stream _stdIn;
        private readonly Stream _stdOut;
        private readonly LogWriter _logger;

        public NativeMessagingHost(Stream stdIn, Stream stdOut, LogWriter logger)
        {
            _stdIn = stdIn;
            _stdOut = stdOut;
            _logger = logger;
        }

        public int NextMessageLength()
        {
            int returnValue = 0;

            byte[] Buffer = new byte[3 - 0 + 1];

            int outputLength = _stdIn.Read(Buffer, 0, sizeof(int));

            _logger.LogWrite(string.Concat("Get Next Message", outputLength.ToString()));

            if (outputLength > 0)
            {
                returnValue = BitConverter.ToInt32(Buffer, 0);

                _logger.LogWrite(string.Concat("Return Value", returnValue.ToString()));
            }
            return returnValue;
        }

        public string ReadMessage(int msgLen)
        {
            byte[] Buffer = new byte[msgLen + 1];

            int outputLength = _stdIn.Read(Buffer, 0, msgLen);

            string chars = Encoding.UTF8.GetString(Buffer, 0, outputLength);

            _logger.LogWrite(string.Concat("Read Message", chars.ToString()));

            return chars;
        }

        public void WriteMessage(string msg)
        { 
            var message = string.Format("{{\"echo\": \"{0}\"}}", msg);

            byte[] Buffer = Encoding.UTF8.GetBytes(message);

            byte[] bufferSize = BitConverter.GetBytes(Buffer.Length);

            _stdOut.Write(bufferSize, 0, bufferSize.Length);

            _stdOut.Write(Buffer, 0, Buffer.Length);

            _logger.LogWrite(string.Concat("Send Message", message));
        }


        public string GetInputMessage(string strJson)
        {
            string reply;

            try
            {
                var msgObj = JsonDocument.Parse(strJson);

                reply = msgObj.RootElement.GetProperty("text").GetString();
            }
            catch (Exception ex)
            {
                reply = "ERROR (GetInfo): " + ex.Message;
                _logger.LogWrite(reply);
            }

            return reply;
        }

        public void Listen()
        {
            var msgLen = NextMessageLength();

            if (msgLen > 0)
            {
                var msg = ReadMessage(msgLen);

                if ( !string.IsNullOrEmpty(msg))
                {
                    var reply = GetInputMessage(msg);

                    //... Do Some Work Here!

                    WriteMessage(reply);
                }
            }
        }
    }
}
