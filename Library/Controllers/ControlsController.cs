using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Controllers
{
	class ControlsController
	{
		internal async Task SetControlsEnableFlag(Form invokerForm, Control.ControlCollection controls, bool flag) //Set all controls enabled property according to flag //await in other class
		{
			invokerForm.Invoke(new Action(() =>
			{
				foreach (Control item in controls)
					item.Enabled = flag;
			}));
		}
	}
}
