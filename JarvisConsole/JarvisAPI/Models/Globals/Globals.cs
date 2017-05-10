using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JarvisAPI.Models.Domain;

namespace JarvisAPI.Models.Globals
{
    public static class Globals
    {
        private static Domain.Domain _domain;

        public static Domain.Domain Domain
        {
            get { return _domain; }
            set
            {
                if(value != _domain)
                {
                    _domain = value;
                }
            }
        }
    }
}