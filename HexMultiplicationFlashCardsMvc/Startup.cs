using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HexMultiplicationFlashCardsMvc.Startup))]
namespace HexMultiplicationFlashCardsMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
