using System;
namespace JarvisAPI.Models.Settings
{
	public class UserSettings
	{
		public UserSettings()
		{
		}

        #region GeneralProperties
        public string UserId { get; set; }
		public int Priority { get; set; }
		public int Timeout { get; set; }

		#endregion GeneralProperties

		#region DemoProperties
		public bool OutletOne { get; set; }
		public bool OutletTwo { get; set; }

		public string DefaultActivity { get; set; }
		public int DefaultVolume { get; set; }

		public int DefaultTemperature { get; set; }

		#endregion DemoProperties
	}
}
