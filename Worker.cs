using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using WorkerServiceTest2.SocketService;

namespace WorkerServiceTest2

{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Her skal business logic være.
                SocketServer socketServer = new SocketServer();
                await socketServer.start();
            }
        }
    }
}
