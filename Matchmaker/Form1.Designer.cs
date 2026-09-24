namespace Matchmaker
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lblInovarBOM = new Label();
            lblCBOM = new Label();
            lblAsBuilt = new Label();
            btnInovarBOM = new Button();
            btnCBOM = new Button();
            btnAsBuilt = new Button();
            lblTitle = new Label();
            btnGo = new Button();
            btnDeleteInovarBOM = new Button();
            btnDeleteCBOM = new Button();
            btnDeleteAsBuilt = new Button();
            lblStatus = new Label();
            progressBar1 = new ProgressBar();
            submissionPanel = new Panel();
            btnDeleteFAReport = new Button();
            lblFAReport = new Label();
            btnFAReport = new Button();
            btnReset = new Button();
            lblInstructions = new Label();
            divider = new Label();
            submissionPanel.SuspendLayout();
            SuspendLayout();
            // 
            // lblInovarBOM
            // 
            lblInovarBOM.AutoEllipsis = true;
            lblInovarBOM.ForeColor = SystemColors.GrayText;
            lblInovarBOM.Location = new Point(243, 34);
            lblInovarBOM.Name = "lblInovarBOM";
            lblInovarBOM.Size = new Size(120, 20);
            lblInovarBOM.TabIndex = 2;
            lblInovarBOM.Text = "(no file selected)";
            // 
            // lblCBOM
            // 
            lblCBOM.AutoEllipsis = true;
            lblCBOM.ForeColor = SystemColors.GrayText;
            lblCBOM.Location = new Point(243, 81);
            lblCBOM.Name = "lblCBOM";
            lblCBOM.Size = new Size(120, 20);
            lblCBOM.TabIndex = 3;
            lblCBOM.Text = "(no file selected)";
            // 
            // lblAsBuilt
            // 
            lblAsBuilt.AutoEllipsis = true;
            lblAsBuilt.ForeColor = SystemColors.GrayText;
            lblAsBuilt.Location = new Point(243, 131);
            lblAsBuilt.Name = "lblAsBuilt";
            lblAsBuilt.Size = new Size(120, 20);
            lblAsBuilt.TabIndex = 5;
            lblAsBuilt.Text = "(no file selected)";
            // 
            // btnInovarBOM
            // 
            btnInovarBOM.AllowDrop = true;
            btnInovarBOM.BackColor = SystemColors.ButtonHighlight;
            btnInovarBOM.FlatAppearance.BorderColor = Color.White;
            btnInovarBOM.FlatAppearance.BorderSize = 0;
            btnInovarBOM.FlatStyle = FlatStyle.Flat;
            btnInovarBOM.Location = new Point(29, 31);
            btnInovarBOM.Name = "btnInovarBOM";
            btnInovarBOM.Size = new Size(182, 29);
            btnInovarBOM.TabIndex = 6;
            btnInovarBOM.Tag = "inovarBOM";
            btnInovarBOM.Text = "Upload Inovar BOM";
            btnInovarBOM.UseVisualStyleBackColor = false;
            btnInovarBOM.Click += btnChooseFile_Click;
            btnInovarBOM.DragDrop += FileButton_DragDrop;
            btnInovarBOM.DragEnter += FileButton_DragEnter;
            // 
            // btnCBOM
            // 
            btnCBOM.AllowDrop = true;
            btnCBOM.BackColor = SystemColors.ButtonHighlight;
            btnCBOM.FlatAppearance.BorderColor = Color.White;
            btnCBOM.FlatAppearance.BorderSize = 0;
            btnCBOM.FlatStyle = FlatStyle.Flat;
            btnCBOM.Location = new Point(29, 77);
            btnCBOM.Name = "btnCBOM";
            btnCBOM.Size = new Size(182, 29);
            btnCBOM.TabIndex = 7;
            btnCBOM.Tag = "CBOM";
            btnCBOM.Text = "Upload CBOM";
            btnCBOM.UseVisualStyleBackColor = false;
            btnCBOM.Click += btnChooseFile_Click;
            btnCBOM.DragDrop += FileButton_DragDrop;
            btnCBOM.DragEnter += FileButton_DragEnter;
            // 
            // btnAsBuilt
            // 
            btnAsBuilt.AllowDrop = true;
            btnAsBuilt.BackColor = SystemColors.ButtonHighlight;
            btnAsBuilt.FlatAppearance.BorderColor = Color.White;
            btnAsBuilt.FlatAppearance.BorderSize = 0;
            btnAsBuilt.FlatStyle = FlatStyle.Flat;
            btnAsBuilt.ForeColor = SystemColors.ControlText;
            btnAsBuilt.Location = new Point(29, 123);
            btnAsBuilt.Name = "btnAsBuilt";
            btnAsBuilt.Size = new Size(182, 29);
            btnAsBuilt.TabIndex = 9;
            btnAsBuilt.Tag = "asBuilt";
            btnAsBuilt.Text = "Upload As Built";
            btnAsBuilt.UseVisualStyleBackColor = false;
            btnAsBuilt.Click += btnChooseFile_Click;
            btnAsBuilt.DragDrop += FileButton_DragDrop;
            btnAsBuilt.DragEnter += FileButton_DragEnter;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.FlatStyle = FlatStyle.Flat;
            lblTitle.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            lblTitle.Location = new Point(0, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(969, 80);
            lblTitle.TabIndex = 10;
            lblTitle.Text = "NET INSPECT MATCHMAKER 2.0";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnGo
            // 
            btnGo.BackColor = Color.DarkGreen;
            btnGo.FlatAppearance.BorderSize = 0;
            btnGo.FlatAppearance.MouseOverBackColor = Color.ForestGreen;
            btnGo.FlatStyle = FlatStyle.Flat;
            btnGo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnGo.ForeColor = SystemColors.ButtonHighlight;
            btnGo.Location = new Point(124, 250);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(87, 28);
            btnGo.TabIndex = 12;
            btnGo.Text = "Start";
            btnGo.UseVisualStyleBackColor = false;
            btnGo.Click += btnGo_Click;
            // 
            // btnDeleteInovarBOM
            // 
            btnDeleteInovarBOM.FlatAppearance.BorderColor = Color.White;
            btnDeleteInovarBOM.FlatStyle = FlatStyle.Flat;
            btnDeleteInovarBOM.Font = new Font("Roboto Thin", 9F);
            btnDeleteInovarBOM.ForeColor = SystemColors.ControlDarkDark;
            btnDeleteInovarBOM.Location = new Point(383, 30);
            btnDeleteInovarBOM.Name = "btnDeleteInovarBOM";
            btnDeleteInovarBOM.Size = new Size(25, 29);
            btnDeleteInovarBOM.TabIndex = 14;
            btnDeleteInovarBOM.Tag = "inovarBOM";
            btnDeleteInovarBOM.Text = "x";
            btnDeleteInovarBOM.TextAlign = ContentAlignment.TopCenter;
            btnDeleteInovarBOM.UseVisualStyleBackColor = true;
            btnDeleteInovarBOM.Visible = false;
            btnDeleteInovarBOM.Click += btnDelete_Click;
            // 
            // btnDeleteCBOM
            // 
            btnDeleteCBOM.FlatAppearance.BorderColor = Color.White;
            btnDeleteCBOM.FlatStyle = FlatStyle.Flat;
            btnDeleteCBOM.Font = new Font("Roboto Thin", 9F);
            btnDeleteCBOM.ForeColor = SystemColors.ControlDarkDark;
            btnDeleteCBOM.Location = new Point(383, 77);
            btnDeleteCBOM.Name = "btnDeleteCBOM";
            btnDeleteCBOM.Size = new Size(25, 29);
            btnDeleteCBOM.TabIndex = 15;
            btnDeleteCBOM.Tag = "CBOM";
            btnDeleteCBOM.Text = "x";
            btnDeleteCBOM.TextAlign = ContentAlignment.TopCenter;
            btnDeleteCBOM.UseVisualStyleBackColor = true;
            btnDeleteCBOM.Visible = false;
            btnDeleteCBOM.Click += btnDelete_Click;
            // 
            // btnDeleteAsBuilt
            // 
            btnDeleteAsBuilt.FlatAppearance.BorderColor = Color.White;
            btnDeleteAsBuilt.FlatStyle = FlatStyle.Flat;
            btnDeleteAsBuilt.Font = new Font("Roboto Thin", 9F);
            btnDeleteAsBuilt.ForeColor = SystemColors.ControlDarkDark;
            btnDeleteAsBuilt.Location = new Point(383, 127);
            btnDeleteAsBuilt.Name = "btnDeleteAsBuilt";
            btnDeleteAsBuilt.Size = new Size(25, 29);
            btnDeleteAsBuilt.TabIndex = 16;
            btnDeleteAsBuilt.Tag = "asBuilt";
            btnDeleteAsBuilt.Text = "x";
            btnDeleteAsBuilt.TextAlign = ContentAlignment.TopCenter;
            btnDeleteAsBuilt.UseVisualStyleBackColor = true;
            btnDeleteAsBuilt.Visible = false;
            btnDeleteAsBuilt.Click += btnDelete_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(229, 281);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 20);
            lblStatus.TabIndex = 17;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(243, 256);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(125, 16);
            progressBar1.TabIndex = 18;
            progressBar1.Visible = false;
            // 
            // submissionPanel
            // 
            submissionPanel.BackColor = SystemColors.GradientInactiveCaption;
            submissionPanel.Controls.Add(btnDeleteFAReport);
            submissionPanel.Controls.Add(lblFAReport);
            submissionPanel.Controls.Add(btnFAReport);
            submissionPanel.Controls.Add(btnDeleteAsBuilt);
            submissionPanel.Controls.Add(lblAsBuilt);
            submissionPanel.Controls.Add(btnAsBuilt);
            submissionPanel.Controls.Add(btnReset);
            submissionPanel.Controls.Add(btnInovarBOM);
            submissionPanel.Controls.Add(progressBar1);
            submissionPanel.Controls.Add(lblStatus);
            submissionPanel.Controls.Add(lblInovarBOM);
            submissionPanel.Controls.Add(btnGo);
            submissionPanel.Controls.Add(lblCBOM);
            submissionPanel.Controls.Add(btnDeleteCBOM);
            submissionPanel.Controls.Add(btnCBOM);
            submissionPanel.Controls.Add(btnDeleteInovarBOM);
            submissionPanel.Location = new Point(266, 264);
            submissionPanel.Name = "submissionPanel";
            submissionPanel.Size = new Size(437, 326);
            submissionPanel.TabIndex = 19;
            submissionPanel.Paint += submissionPanel_Paint;
            // 
            // btnDeleteFAReport
            // 
            btnDeleteFAReport.FlatAppearance.BorderColor = Color.White;
            btnDeleteFAReport.FlatStyle = FlatStyle.Flat;
            btnDeleteFAReport.Font = new Font("Roboto Thin", 9F);
            btnDeleteFAReport.ForeColor = SystemColors.ControlDarkDark;
            btnDeleteFAReport.Location = new Point(383, 171);
            btnDeleteFAReport.Name = "btnDeleteFAReport";
            btnDeleteFAReport.Size = new Size(25, 29);
            btnDeleteFAReport.TabIndex = 22;
            btnDeleteFAReport.Tag = "FAReport";
            btnDeleteFAReport.Text = "x";
            btnDeleteFAReport.TextAlign = ContentAlignment.TopCenter;
            btnDeleteFAReport.UseVisualStyleBackColor = true;
            btnDeleteFAReport.Visible = false;
            btnDeleteFAReport.Click += btnDelete_Click;
            // 
            // lblFAReport
            // 
            lblFAReport.AutoEllipsis = true;
            lblFAReport.ForeColor = SystemColors.GrayText;
            lblFAReport.Location = new Point(243, 175);
            lblFAReport.Name = "lblFAReport";
            lblFAReport.Size = new Size(120, 20);
            lblFAReport.TabIndex = 20;
            lblFAReport.Text = "(no file selected)";
            // 
            // btnFAReport
            // 
            btnFAReport.AllowDrop = true;
            btnFAReport.BackColor = SystemColors.ButtonHighlight;
            btnFAReport.FlatAppearance.BorderColor = Color.White;
            btnFAReport.FlatAppearance.BorderSize = 0;
            btnFAReport.FlatStyle = FlatStyle.Flat;
            btnFAReport.ForeColor = SystemColors.ControlText;
            btnFAReport.Location = new Point(29, 169);
            btnFAReport.Name = "btnFAReport";
            btnFAReport.Size = new Size(182, 29);
            btnFAReport.TabIndex = 21;
            btnFAReport.Tag = "FAReport";
            btnFAReport.Text = "Upload FA Report";
            btnFAReport.UseVisualStyleBackColor = false;
            btnFAReport.Click += btnChooseFile_Click;
            btnFAReport.DragDrop += FileButton_DragDrop;
            btnFAReport.DragEnter += FileButton_DragEnter;
            // 
            // btnReset
            // 
            btnReset.BackColor = SystemColors.ButtonHighlight;
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Location = new Point(29, 250);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(87, 28);
            btnReset.TabIndex = 19;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // lblInstructions
            // 
            lblInstructions.Anchor = AnchorStyles.Top;
            lblInstructions.AutoSize = true;
            lblInstructions.BackColor = Color.Transparent;
            lblInstructions.Font = new Font("Segoe UI", 11F);
            lblInstructions.ForeColor = Color.FromArgb(80, 80, 80);
            lblInstructions.Location = new Point(234, 131);
            lblInstructions.MaximumSize = new Size(500, 0);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(475, 50);
            lblInstructions.TabIndex = 20;
            lblInstructions.Text = "Upload files to populate Net Inspect FAIR forms. Always verify results before submitting. ";
            lblInstructions.TextAlign = ContentAlignment.TopCenter;
            // 
            // divider
            // 
            divider.BackColor = Color.FromArgb(200, 200, 200);
            divider.Location = new Point(184, 205);
            divider.Name = "divider";
            divider.Size = new Size(600, 1);
            divider.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 248);
            ClientSize = new Size(969, 680);
            Controls.Add(lblInstructions);
            Controls.Add(divider);
            Controls.Add(submissionPanel);
            Controls.Add(lblTitle);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Matchmaker";
            Load += Form1_Load;
            submissionPanel.ResumeLayout(false);
            submissionPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblInovarBOM;
        private Label lblCBOM;
        private Label lblAsBuilt;
        private Button btnInovarBOM;
        private Button btnCBOM;
        private Button btnAsBuilt;
        private Label lblTitle;
        private Button btnGo;
        private Button btnDeleteInovarBOM;
        private Button btnDeleteCBOM;
        private Button btnDeleteAsBuilt;
        private Label lblStatus;
        private ProgressBar progressBar1;
        private Panel submissionPanel;
        private Button btnReset;
        private Label lblInstructions;
        private Label divider;
        private Button btnDeleteFAReport;
        private Label lblFAReport;
        private Button btnFAReport;
    }
}
