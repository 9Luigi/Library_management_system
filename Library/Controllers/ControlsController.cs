using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Controllers
{
	class ControlsController
	{
		internal async Task SetControlsEnableFlag(Form invokerForm, Control.ControlCollection controls, bool flag)
		{
			await Task.Run(() =>
			{
				invokerForm.Invoke(new Action(() =>
				{
					SetEnabledRecursive(controls, flag);
				}));
			});
		}
		
		private void SetEnabledRecursive(Control.ControlCollection controls, bool flag)//recursion
		{
			foreach (Control item in controls)
			{
				item.Enabled = flag;

				if (item.HasChildren)
				{
					SetEnabledRecursive(item.Controls, flag);
				}
			}
		}
	}
}
