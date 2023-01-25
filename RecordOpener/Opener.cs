namespace RecordOpener
{
    using Microsoft.Crm.Sdk.Messages;
    using System;
    using System.Windows.Forms;
    using XrmToolBox.Extensibility;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="XrmToolBox.Extensibility.PluginControlBase" />
    public partial class Opener : PluginControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Opener" /> class.
        /// </summary>
        public Opener()
        {
            InitializeComponent();
        }

        #region Base tool implementation

        /// <summary>
        /// Processes the who am i request.
        /// </summary>
        public void ProcessWhoAmI()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving your user id...",
                Work = (w, e) =>
                {
                    var request = new WhoAmIRequest();
                    var response = (WhoAmIResponse)Service.Execute(request);

                    e.Result = response.UserId;
                },
                ProgressChanged = e =>
                {
                    // If progress has to be notified to user, use the following method:
                    SetWorkingMessage("Message to display");
                },
                PostWorkCallBack = e =>
                {
                    MessageBox.Show(string.Format("You are {0}", (Guid)e.Result));
                },
                AsyncArgument = null,
                IsCancelable = true,
                MessageWidth = 340,
                MessageHeight = 150
            });
        }

        /// <summary>
        /// BTNs the close click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnCloseClick(object sender, EventArgs e)
        {
            CloseTool(); // PluginBaseControl method that notifies the XrmToolBox that the user wants to close the plugin
            // Override the ClosingPlugin method to allow for any plugin specific closing logic to be performed (saving configs, canceling close, etc...)
        }

        private void BtnWhoAmIClick(object sender, EventArgs e)
        {
            ExecuteMethod(ProcessWhoAmI); // ExecuteMethod ensures that the user has connected to CRM, before calling the call back method
        }

        #endregion Base tool implementation

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelWorker(); // PluginBaseControl method that calls the Background Workers CancelAsync method.

            MessageBox.Show("Cancelled");
        }
    }
}
