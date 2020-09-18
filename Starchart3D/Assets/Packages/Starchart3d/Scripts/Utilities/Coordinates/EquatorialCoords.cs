using UnityEngine;
using System;
namespace Starchart3D
{

	[Serializable]
	//https://en.wikipedia.org/wiki/Celestial_coordinate_system#Equatorial_system
	public struct EquatorialCoords
	{
		[Range(0, 24)]
		public double rightAscention;//HOURS
		[Range(-90, 90)]
		public double declination;
		public double radius;

		public EquatorialCoords(double rightAscention, double declination, double radius = 1)
		{
			this.rightAscention = rightAscention;
			this.declination = declination;
			this.radius = radius;
		}


		public EquatorialCoords(Quaternion rotation)
		{
			var euler = rotation.eulerAngles;
			this.declination = -StarMath.DegZero360To90(euler.x);
			this.rightAscention = (360 - euler.y) * StarMath.deg2hours;
			this.radius = 1;
		}

		public HorizontalCoords ToHorizontal(GeographicCoords geographicCoords, double localSiderealTime)
		{

			double ha = (localSiderealTime - rightAscention) * StarMath.hours2deg;

			double x_sid = StarMath.Cos_d(ha) * StarMath.Cos_d(declination);
			double y_sid = StarMath.Sin_d(ha) * StarMath.Cos_d(declination);
			double z_sid = StarMath.Sin_d(declination);

			double x_hor = x_sid * StarMath.Sin_d(geographicCoords.latitude) - z_sid * StarMath.Cos_d(geographicCoords.latitude);
			double y_hor = y_sid;
			double z_hor = x_sid * StarMath.Cos_d(geographicCoords.latitude) + z_sid * StarMath.Sin_d(geographicCoords.latitude);

			double az = StarMath.Atan2_d(y_hor, x_hor) + 180;
			double alt = StarMath.Atan2_d(z_hor, Math.Sqrt(x_hor * x_hor + y_hor * y_hor));

			return new HorizontalCoords(alt, az);
		}

		public override string ToString()
		{
			return $"ra: {rightAscention}\tdec: {declination}";
		}
	}

}