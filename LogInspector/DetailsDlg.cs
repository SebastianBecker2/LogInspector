namespace LogInspector
{
    using System;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public partial class DetailsDlg : Form
    {
        public CachedLogEvent? LogEvent { get; set; }

        public DetailsDlg() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            if (LogEvent is null)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            DisplayLogEvent(LogEvent);

            base.OnLoad(e);
        }

        private void DisplayLogEvent(CachedLogEvent logEvent)
        {
            DgvDetails.Rows.Clear();

            _ = DgvDetails.Rows.Add("Timestamp", logEvent.Timestamp.ToString("o"));
            _ = DgvDetails.Rows.Add("Level", logEvent.Level.ToString());
            _ = DgvDetails.Rows.Add("Message", logEvent.Message);
            _ = DgvDetails.Rows.Add("Message Template", logEvent.MessageTemplate.Text);
            _ = DgvDetails.Rows.Add("Exception", logEvent.Exception?.ToString() ?? "None");

            foreach (var property in logEvent.Properties)
            {
                _ = DgvDetails.Rows.Add(property.Key, CachedLogEvent.ToString(property.Value));
            }

            DgvDetails.AutoResizeRows();
        }

        private void BtnCopyToClipboard_Click(object sender, EventArgs e)
        {
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            var json = JsonConvert.SerializeObject(LogEvent!.LogEvent, options);

            Clipboard.SetText(json);
        }
    }
}
