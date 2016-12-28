using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using JarvisConsole.Actions;
using JarvisConsole.DataProviders;
using JarvisConsole.DataProviders.Wit;

[assembly: OwinStartup(typeof(WebApplication1.Startup))]

namespace WebApplication1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);           
        }
    }
}
