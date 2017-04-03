using System;
using System.Runtime.InteropServices.WindowsRuntime;
namespace JarvisAPI.Models
{
	public class Location 
	{
		#region Constructor
		public Location(double x, double y)
		{
			_x = x;
			_y = y;
		}

		public Location(double x, double y, double z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public Location()
		{
		}

		#endregion

		private double _x;
		private double _y;
		private double _z;

		#region Properties
		public double X
		{
			get { return _x;}
			set
			{
				if(!value.Equals(_x))
				{
					_x = value;
				}
			}
		}

		public double Y
		{
			get { return _y;}
			set
			{
				if(!value.Equals(_y))
				{
					_y = value;
				}

			}
		}

		public double Z
		{
			get { return _z;}
			set
			{
				if(!value.Equals(_z))
				{
					_z = value;
				}
			}
		}

		#endregion
	}
}
