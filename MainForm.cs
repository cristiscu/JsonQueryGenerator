using System;
using System.Drawing;
using System.IO;
using System.Data;
using System.Windows.Forms;
using Snowflake.Data.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XtractPro.Utils.JsonQueryGenerator
{
    public partial class MainForm : Form
    {
        private QueryGenerator _queryGen = new();

        public MainForm() => InitializeComponent();

        private void WriteStatus(string msg = "", bool isError = false)
        {
            sbrMainLabel.Text = msg;
            sbrMainLabel.ForeColor = isError ? Color.Red : SystemColors.WindowText;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // populate Files combo
            cboFiles.Items.Add("(select a file)");
            foreach (var filename in Directory.GetFiles("files", "*.json"))
                cboFiles.Items.Add(Path.GetFileNameWithoutExtension(filename));
            cboFiles.SelectedIndex = 1;

            // populte Query Types combo
            foreach (var s in QueryGenerator.QueryTypes)
                if (!string.IsNullOrEmpty(s))
                    cboQueryTypes.Items.Add(s);
            cboQueryTypes.SelectedIndex = 1;
        }

        // change current JSON IN content/file
        private void cboFiles_SelectedIndexChanged(object sender, EventArgs e) => RenderNewFile();
        private void RenderNewFile()
        {
            // reset all
            tabJsonIn.SelectedIndex = tabQuery.SelectedIndex = 0;
            txtJsonIn.Text = "";
            txtQuery.Text = "";
            lvwResults.Clear();

            // change current JSON IN content/file
            var path = $"files/{cboFiles.Text}.json";
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                txtJsonIn.Text = JToken.Parse(json).ToString(Formatting.Indented);
                GenerateQuery();
            }
        }

        // change/generate SQL query
        private void cboQueryType_SelectedIndexChanged(object sender, EventArgs e) => GenerateQuery();
        private void GenerateQuery() => FillResults(
            txtQuery.Text = _queryGen.GetQuery(txtJsonIn.Text, cboQueryTypes.Text));

        // run current query and fill results
        private void FillResults(string query)
        {
            // reset all
            lvwResults.Clear();
            txtJsonOut.Text = "";
            WriteStatus();

            if (string.IsNullOrEmpty(query)
                || query.Contains("[ERROR]"))
                return;

            // (try to) run SQL query
            DataTable table = null;
            try { table = RunQuery(query); }
            catch (Exception ex)
            {
                WriteStatus(ex.Message, true);
                return;
            }

            // fill list of results
            lvwResults.BeginUpdate();
            var width = (lvwResults.Width - 5) / table.Columns.Count;
            foreach (DataColumn column in table.Columns)
            {
                var col = lvwResults.Columns.Add(column.Caption);
                col.Width = width;
            }
            foreach (DataRow row in table.Rows)
            {
                var item = lvwResults.Items.Add(row[0].ToString());
                for (var i = 1; i < table.Columns.Count; i++)
                    item.SubItems.Add(row[i].ToString());
            }
            lvwResults.EndUpdate();

            // auto-select first JSON objects from first row
            if (lvwResults.Items.Count > 0)
            {
                var item = lvwResults.Items[0];
                item.Selected = true;
                for (var i = 0; i < item.SubItems.Count; i++)
                {
                    var text = item.SubItems[i].Text;
                    if (text.StartsWith("{") || text.StartsWith("["))
                    {
                        ShowResultJson(text);
                        break;
                    }
                }
            }
        }

        // run Snowflake SQL query an return a table with results
        private static DataTable RunQuery(string query)
        {
            // TODO: save a SNOWFLAKE_CONNSTR environment variable with the following content:
            // account=your-snowflake-account;user=your-username;password=your-password;
            var connStr = Environment.GetEnvironmentVariable("SNOWFLAKE_CONNSTR");

            using (var conn = new SnowflakeDbConnection())
            {
                conn.ConnectionString = connStr;
                conn.Open();
                using (var cmd = new SnowflakeDbCommand(conn))
                {
                    cmd.CommandText = query;
                    var table = new DataTable();
                    using (var adapter = new SnowflakeDbDataAdapter(cmd))
                        adapter.Fill(table);
                    conn.Close();
                    return table;
                }
            }
        }

        // show JSON OUT with Results cell content
        private void lvwResults_MouseClick(object sender, MouseEventArgs e)
        {
            var lvwInfo = lvwResults.HitTest(e.X, e.Y);
            var item = lvwInfo.Item;
            var subitem = lvwInfo.SubItem;
            if (item == null && subitem == null) return;

            var json = subitem != null ? subitem.Text : item.Text;
            ShowResultJson(json);
        }
        private void ShowResultJson(string json)
        {
            try
            {
                txtJsonOut.Text = json.StartsWith("{") || json.StartsWith("[")
                    ? JToken.Parse(json).ToString(Formatting.Indented) : "";
            }
            catch { txtJsonOut.Text = ""; }
        }
    }
}
