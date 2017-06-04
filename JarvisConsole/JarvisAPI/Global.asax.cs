﻿using JarvisAPI.DataProviders;
using JarvisAPI.DataProviders.Orvibo;
using JarvisAPI.Models.Domain;
using JarvisAPI.Models.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace JarvisAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Logging.Log("Global", "****************************Jarvis started**********************************");

            //NestDataProvider.Initialize();
            //HarmonyDataProvider.Initialize();
            //OrviboDataProvider.Initialize();
            //Globals.Initialize();

            //XmlDataProvider.LoadDomain();
        }
    }
}
