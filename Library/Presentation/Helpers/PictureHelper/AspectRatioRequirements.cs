using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Presentation.Controllers.PictureController
{
	public static class AspectRatioRequirements
	{
		public static readonly double ThreeToFour = 3.0 / 4;   // Common portrait aspect ratio
		public static readonly double TwoToThree = 2.0 / 3;     // Common photographic portrait
		public static readonly double FiveToFour = 5.0 / 4;     // Slightly taller, used for portrait display
		public static readonly double NineToSixteen = 9.0 / 16; // Portrait mode used in phone screens and vertical videos
		public static readonly List<double> ValidAspectRatios =
    [
        ThreeToFour,
		TwoToThree,
		FiveToFour,
		NineToSixteen
	];
	}
}
