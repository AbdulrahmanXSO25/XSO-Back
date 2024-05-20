using System.Net;

namespace XSOBack
{
    public class WebApplication
    {
        private uint? _port;

        private HttpListener _listener = new HttpListener();

        private Router _router;

        private bool _isRunning = true;

        public WebApplication(Router router)
        {
            _router = router;
        }

        public void SetPort(uint port)
        {
            _port = port;
        }

        private void CheckPort()
        {
            if (_port == null)
                throw new Exception("Port cannot be null!");
        }

        public void Start()
        {
            CheckPort();

            _listener.Prefixes.Add($"http://localhost:{_port}/");
            _listener.Start();
            XSOBackUtilities.XSOBackLog($"XSOBack App is listening on http://localhost:{_port}");

            Thread shutdownThread = new Thread(() =>
            {
                Console.ReadKey();
                Stop();
            });
            shutdownThread.Start();

            try
            {
                while (_isRunning)
                {
                    try
                    {
                        HttpListenerContext context = _listener.GetContext();
                        _router.HandleRequest(context);
                    }
                    catch (Exception ex)
                    {
                        XSOBackUtilities.XSOBackLog($"Error occurred: {ex.Message}");
                        if (!_listener.IsListening) break;
                    }
                }
            }
            finally
            {
                Shutdown();
            }

            shutdownThread.Join();
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
        }

        private void Shutdown()
        {
            _listener.Close();
            XSOBackUtilities.XSOBackLog("Server is shutting down..");
        }
    }
}