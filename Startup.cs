using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SpeakingMania.Startup))]
namespace SpeakingMania
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}