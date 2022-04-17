
namespace XtractPro.Utils.JsonQueryGenerator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sbrMain = new System.Windows.Forms.StatusStrip();
            this.sbrMainLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabJsonIn = new System.Windows.Forms.TabControl();
            this.tabJsonInPage = new System.Windows.Forms.TabPage();
            this.txtJsonIn = new System.Windows.Forms.TextBox();
            this.cboFiles = new System.Windows.Forms.ComboBox();
            this.tabQuery = new System.Windows.Forms.TabControl();
            this.tabQueryPage = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabResults = new System.Windows.Forms.TabControl();
            this.tabResultsPage = new System.Windows.Forms.TabPage();
            this.lvwResults = new System.Windows.Forms.ListView();
            this.tabJsonOut = new System.Windows.Forms.TabControl();
            this.tabJsonOutPage = new System.Windows.Forms.TabPage();
            this.txtJsonOut = new System.Windows.Forms.TextBox();
            this.cboQueryTypes = new System.Windows.Forms.ComboBox();
            this.sbrMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabJsonIn.SuspendLayout();
            this.tabJsonInPage.SuspendLayout();
            this.tabQuery.SuspendLayout();
            this.tabQueryPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tabResultsPage.SuspendLayout();
            this.tabJsonOut.SuspendLayout();
            this.tabJsonOutPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // sbrMain
            // 
            this.sbrMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.sbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbrMainLabel});
            this.sbrMain.Location = new System.Drawing.Point(0, 700);
            this.sbrMain.Name = "sbrMain";
            this.sbrMain.Size = new System.Drawing.Size(1306, 26);
            this.sbrMain.TabIndex = 2;
            this.sbrMain.Text = "statusStrip1";
            // 
            // sbrMainLabel
            // 
            this.sbrMainLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.sbrMainLabel.Name = "sbrMainLabel";
            this.sbrMainLabel.Size = new System.Drawing.Size(1291, 20);
            this.sbrMainLabel.Spring = true;
            this.sbrMainLabel.Text = " ";
            this.sbrMainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabJsonIn);
            this.splitContainer1.Panel1.Controls.Add(this.cboFiles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabQuery);
            this.splitContainer1.Panel2.Controls.Add(this.cboQueryTypes);
            this.splitContainer1.Size = new System.Drawing.Size(1306, 700);
            this.splitContainer1.SplitterDistance = 364;
            this.splitContainer1.TabIndex = 3;
            // 
            // tabJsonIn
            // 
            this.tabJsonIn.Controls.Add(this.tabJsonInPage);
            this.tabJsonIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabJsonIn.Location = new System.Drawing.Point(0, 28);
            this.tabJsonIn.Multiline = true;
            this.tabJsonIn.Name = "tabJsonIn";
            this.tabJsonIn.SelectedIndex = 0;
            this.tabJsonIn.Size = new System.Drawing.Size(364, 672);
            this.tabJsonIn.TabIndex = 0;
            // 
            // tabJsonInPage
            // 
            this.tabJsonInPage.Controls.Add(this.txtJsonIn);
            this.tabJsonInPage.Location = new System.Drawing.Point(4, 29);
            this.tabJsonInPage.Name = "tabJsonInPage";
            this.tabJsonInPage.Size = new System.Drawing.Size(356, 639);
            this.tabJsonInPage.TabIndex = 1;
            this.tabJsonInPage.Text = "JSON In";
            this.tabJsonInPage.UseVisualStyleBackColor = true;
            // 
            // txtJsonIn
            // 
            this.txtJsonIn.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtJsonIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJsonIn.Font = new System.Drawing.Font("Courier New", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtJsonIn.Location = new System.Drawing.Point(0, 0);
            this.txtJsonIn.Multiline = true;
            this.txtJsonIn.Name = "txtJsonIn";
            this.txtJsonIn.ReadOnly = true;
            this.txtJsonIn.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJsonIn.Size = new System.Drawing.Size(356, 639);
            this.txtJsonIn.TabIndex = 1;
            this.txtJsonIn.WordWrap = false;
            // 
            // cboFiles
            // 
            this.cboFiles.BackColor = System.Drawing.SystemColors.Window;
            this.cboFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiles.FormattingEnabled = true;
            this.cboFiles.Location = new System.Drawing.Point(0, 0);
            this.cboFiles.Name = "cboFiles";
            this.cboFiles.Size = new System.Drawing.Size(364, 28);
            this.cboFiles.TabIndex = 1;
            this.cboFiles.SelectedIndexChanged += new System.EventHandler(this.cboFiles_SelectedIndexChanged);
            // 
            // tabQuery
            // 
            this.tabQuery.Controls.Add(this.tabQueryPage);
            this.tabQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabQuery.Location = new System.Drawing.Point(0, 28);
            this.tabQuery.Name = "tabQuery";
            this.tabQuery.SelectedIndex = 0;
            this.tabQuery.Size = new System.Drawing.Size(938, 672);
            this.tabQuery.TabIndex = 0;
            // 
            // tabQueryPage
            // 
            this.tabQueryPage.Controls.Add(this.splitContainer2);
            this.tabQueryPage.Location = new System.Drawing.Point(4, 29);
            this.tabQueryPage.Name = "tabQueryPage";
            this.tabQueryPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabQueryPage.Size = new System.Drawing.Size(930, 639);
            this.tabQueryPage.TabIndex = 0;
            this.tabQueryPage.Text = "Query";
            this.tabQueryPage.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txtQuery);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(924, 633);
            this.splitContainer2.SplitterDistance = 312;
            this.splitContainer2.TabIndex = 1;
            // 
            // txtQuery
            // 
            this.txtQuery.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery.Font = new System.Drawing.Font("Courier New", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtQuery.Location = new System.Drawing.Point(0, 0);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ReadOnly = true;
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtQuery.Size = new System.Drawing.Size(924, 312);
            this.txtQuery.TabIndex = 0;
            this.txtQuery.WordWrap = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer3.Panel1.Controls.Add(this.tabResults);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer3.Panel2.Controls.Add(this.tabJsonOut);
            this.splitContainer3.Size = new System.Drawing.Size(924, 317);
            this.splitContainer3.SplitterDistance = 609;
            this.splitContainer3.TabIndex = 1;
            // 
            // tabResults
            // 
            this.tabResults.Controls.Add(this.tabResultsPage);
            this.tabResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResults.Location = new System.Drawing.Point(0, 0);
            this.tabResults.Name = "tabResults";
            this.tabResults.SelectedIndex = 0;
            this.tabResults.Size = new System.Drawing.Size(609, 317);
            this.tabResults.TabIndex = 0;
            // 
            // tabResultsPage
            // 
            this.tabResultsPage.Controls.Add(this.lvwResults);
            this.tabResultsPage.Location = new System.Drawing.Point(4, 29);
            this.tabResultsPage.Name = "tabResultsPage";
            this.tabResultsPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabResultsPage.Size = new System.Drawing.Size(601, 284);
            this.tabResultsPage.TabIndex = 0;
            this.tabResultsPage.Text = "Results";
            this.tabResultsPage.UseVisualStyleBackColor = true;
            // 
            // lvwResults
            // 
            this.lvwResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwResults.FullRowSelect = true;
            this.lvwResults.HideSelection = false;
            this.lvwResults.Location = new System.Drawing.Point(3, 3);
            this.lvwResults.MultiSelect = false;
            this.lvwResults.Name = "lvwResults";
            this.lvwResults.Size = new System.Drawing.Size(595, 278);
            this.lvwResults.TabIndex = 0;
            this.lvwResults.UseCompatibleStateImageBehavior = false;
            this.lvwResults.View = System.Windows.Forms.View.Details;
            this.lvwResults.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwResults_MouseClick);
            // 
            // tabJsonOut
            // 
            this.tabJsonOut.Controls.Add(this.tabJsonOutPage);
            this.tabJsonOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabJsonOut.Location = new System.Drawing.Point(0, 0);
            this.tabJsonOut.Name = "tabJsonOut";
            this.tabJsonOut.SelectedIndex = 0;
            this.tabJsonOut.Size = new System.Drawing.Size(311, 317);
            this.tabJsonOut.TabIndex = 1;
            // 
            // tabJsonOutPage
            // 
            this.tabJsonOutPage.Controls.Add(this.txtJsonOut);
            this.tabJsonOutPage.Location = new System.Drawing.Point(4, 29);
            this.tabJsonOutPage.Name = "tabJsonOutPage";
            this.tabJsonOutPage.Size = new System.Drawing.Size(303, 284);
            this.tabJsonOutPage.TabIndex = 1;
            this.tabJsonOutPage.Text = "JSON Out";
            this.tabJsonOutPage.UseVisualStyleBackColor = true;
            // 
            // txtJsonOut
            // 
            this.txtJsonOut.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtJsonOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJsonOut.Font = new System.Drawing.Font("Courier New", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtJsonOut.Location = new System.Drawing.Point(0, 0);
            this.txtJsonOut.Multiline = true;
            this.txtJsonOut.Name = "txtJsonOut";
            this.txtJsonOut.ReadOnly = true;
            this.txtJsonOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJsonOut.Size = new System.Drawing.Size(303, 284);
            this.txtJsonOut.TabIndex = 11;
            this.txtJsonOut.WordWrap = false;
            // 
            // cboQueryTypes
            // 
            this.cboQueryTypes.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboQueryTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQueryTypes.FormattingEnabled = true;
            this.cboQueryTypes.Location = new System.Drawing.Point(0, 0);
            this.cboQueryTypes.Name = "cboQueryTypes";
            this.cboQueryTypes.Size = new System.Drawing.Size(938, 28);
            this.cboQueryTypes.TabIndex = 1;
            this.cboQueryTypes.SelectedIndexChanged += new System.EventHandler(this.cboQueryType_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1306, 726);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.sbrMain);
            this.Name = "MainForm";
            this.Text = "Snowflake JSON Query Generator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.sbrMain.ResumeLayout(false);
            this.sbrMain.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabJsonIn.ResumeLayout(false);
            this.tabJsonInPage.ResumeLayout(false);
            this.tabJsonInPage.PerformLayout();
            this.tabQuery.ResumeLayout(false);
            this.tabQueryPage.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabResults.ResumeLayout(false);
            this.tabResultsPage.ResumeLayout(false);
            this.tabJsonOut.ResumeLayout(false);
            this.tabJsonOutPage.ResumeLayout(false);
            this.tabJsonOutPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip sbrMain;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabJsonIn;
        private System.Windows.Forms.TabControl tabQuery;
        private System.Windows.Forms.TabPage tabQueryPage;
        private System.Windows.Forms.ListView lvwResults;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.TabControl tabResults;
        private System.Windows.Forms.TabPage tabResultsPage;
        private System.Windows.Forms.ComboBox cboFiles;
        private System.Windows.Forms.ComboBox cboQueryTypes;
        private System.Windows.Forms.ToolStripStatusLabel sbrMainLabel;
        private System.Windows.Forms.TabControl tabJsonOut;
        private System.Windows.Forms.TabPage tabJsonInPage;
        private System.Windows.Forms.TabPage tabJsonOutPage;
        private System.Windows.Forms.TextBox txtJsonIn;
        private System.Windows.Forms.TextBox txtJsonOut;
    }
}

