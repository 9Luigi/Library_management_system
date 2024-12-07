using Microsoft.Extensions.Logging;


namespace Library.Presentation.Controllers
{
    /// <summary>
    /// Controller responsible for managing the enable/disable state of controls in a Windows Form.
    /// </summary>
    class ControlsController
    {
        private readonly ILogger<ControlsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlsController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance to log messages related to control operations.</param>
        public ControlsController()
        {
            _logger = LoggerService.CreateLogger<ControlsController>();
        }

        /// <summary>
        /// Sets the enabled state of controls recursively within a form.
        /// </summary>
        /// <param name="invokerForm">The form that owns the controls to be updated.</param>
        /// <param name="controls">The collection of controls to be updated.</param>
        /// <param name="flag">The state to set (true to enable, false to disable).</param>
        /// <exception cref="ArgumentNullException">Thrown when invokerForm or controls are null.</exception>
        /// <exception cref="Exception">Thrown if any error occurs during the operation.</exception>
        internal async Task SetControlsEnableFlag(Form invokerForm, Control.ControlCollection controls, bool flag)
        {
            try
            {
                if (invokerForm == null || controls == null)
                {
                    _logger.LogError("Form or controls collection is null.");
                    throw new ArgumentNullException(invokerForm == null ? nameof(invokerForm) : nameof(controls));
                }

                // Log the start of the operation
                _logger.LogInformation("Attempting to set controls enabled state to {Flag} for {ControlCount} controls.", flag, controls.Count);

                await Task.Run(() =>
                {
                    invokerForm.Invoke(new Action(() =>
                    {
                        SetEnabledRecursive(controls, flag);
                    }));
                });

                // Log the successful completion of the operation
                _logger.LogInformation("Successfully set controls enabled state to {Flag}.", flag);
            }
            catch (Exception ex)
            {
                // Log the error and throw it
                _logger.LogError(ex, "An error occurred while setting the controls enabled state.");
                throw;
            }
        }

        /// <summary>
        /// Recursively sets the enabled state of controls within a control collection.
        /// </summary>
        /// <param name="controls">The collection of controls to update.</param>
        /// <param name="flag">The state to set (true to enable, false to disable).</param>
        private void SetEnabledRecursive(Control.ControlCollection controls, bool flag)
        {
            try
            {
                foreach (Control item in controls)
                {
                    // Set the enabled state for each control
                    item.Enabled = flag;

                    // Recursively set the enabled state for any child controls
                    if (item.HasChildren)
                    {
                        SetEnabledRecursive(item.Controls, flag);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error if something goes wrong during recursion
                _logger.LogError(ex, "An error occurred while recursively setting the enabled state of controls.");
                throw;
            }
        }
    }
}
