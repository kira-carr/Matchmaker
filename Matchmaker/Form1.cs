using System.Drawing.Drawing2D;
using System.Reflection;


namespace Matchmaker
{
    public partial class Form1 : Form
    {
        // Dictionary mapping filepaths and labels
        private Dictionary<string, Action<string>> fileSetters;
        private Dictionary<string, Action> fileClearers;

        private readonly string baseDir;
        private readonly string inputDocsDir;
        private readonly string templatePath;

        private string inovarBOMFilePath = "";
        private string cBomFilePath = "";
        private string asBuiltFilePath = "";
        private string faReportFilePath = "";



        public Form1()
        {
            InitializeComponent();


            // Paths safe for single-file and normal runs
            baseDir = AppDomain.CurrentDomain.BaseDirectory;
            inputDocsDir = Path.Combine(baseDir, "InputDocs");
            Directory.CreateDirectory(inputDocsDir);

            templatePath = Path.Combine(inputDocsDir, "FAIR_ImportTemplate_RevC.xlsx");

            EnsureTemplateAvailable(); // Writes embedded resource to templatePath if missing


            fileSetters = new Dictionary<string, Action<string>>
            {
                ["inovarBOM"] = path => 
                    { inovarBOMFilePath = path; 
                        lblInovarBOM.Text = Path.GetFileName(path);
                        lblInovarBOM.ForeColor = SystemColors.HotTrack;
                    },
                ["CBOM"] = path => 
                    { cBomFilePath = path; 
                        lblCBOM.Text = Path.GetFileName(path);
                        lblCBOM.ForeColor = SystemColors.HotTrack;
                    },
                ["asBuilt"] = path => 
                    { asBuiltFilePath = path;
                        lblAsBuilt.Text = Path.GetFileName(path);
                        lblAsBuilt.ForeColor = SystemColors.HotTrack;
                    },
                ["FAReport"] = path =>
                {
                    faReportFilePath = path;
                    lblFAReport.Text = Path.GetFileName(path);
                    lblFAReport.ForeColor = SystemColors.HotTrack;
                }
            };

            fileClearers = new Dictionary<string, Action>
            {
                ["inovarBOM"] = () => 
                    { inovarBOMFilePath = ""; 
                        lblInovarBOM.Text = "(no file selected)";
                        lblInovarBOM.ForeColor = SystemColors.GrayText;
                        btnDeleteInovarBOM.Visible = false; 
                    },
                ["CBOM"] = () => 
                    { cBomFilePath = ""; 
                        lblCBOM.Text = "(no file selected)";
                        lblCBOM.ForeColor = SystemColors.GrayText;
                        btnDeleteCBOM.Visible = false;
                    },
                ["asBuilt"] = () => 
                    { asBuiltFilePath = ""; 
                        lblAsBuilt.Text = "(no file selected)";
                        lblAsBuilt.ForeColor = SystemColors.GrayText;
                        btnDeleteAsBuilt.Visible = false; },
                ["FAReport"] = () =>
                {
                    faReportFilePath = "";
                    lblFAReport.Text = "(no file selected)";
                    lblFAReport.ForeColor = SystemColors.GrayText;
                    btnDeleteFAReport.Visible = false;
                }
            };

        }

        private string PickPdf() {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files|*.pdf";

            if (dialog.ShowDialog() == DialogResult.OK) { 
                return dialog.FileName;
            }
            return "";
        }

        private string PickExcel()  // simplifies file browsing to just show excel files
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel Files|*.xlsx;*.xls";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }

            return "";
        }

        // button handler: each button accepts 
        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            if (sender is not Button btn)
                return; // sender wasn't a button

            if (btn.Tag is not string key)
                return; // Tag wasn't set or wasn't a string

            string filePath = "";

            if (key == "FAReport")
            {    // if processing pdf
                filePath = PickPdf();
            }
            else { // otherwise excel
                filePath = PickExcel();
            }

            if (!string.IsNullOrEmpty(filePath)) {

                HandleFileSelection(key, filePath);
            }

        }

        private void HandleFileSelection(string key, string filePath)
        {

            if (key == "FAReport" && !filePath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {  // confirm is pdf
                MessageBox.Show("FA Report must be a PDF file.",
                                "Invalid File Type",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            else if  (key != "FAReport" && !filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) && !filePath.EndsWith(".xls", StringComparison.OrdinalIgnoreCase)) {
                MessageBox.Show("Upload must be an Excel file.",
                                "Invalid File Type",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // get file
            if (fileSetters.TryGetValue(key, out var setFile)) {
                setFile(filePath);
            }

            // option to remove files after uploading: show "x" button next to file name

            switch (key)
            {
                case "inovarBOM": btnDeleteInovarBOM.Visible = true; break;
                case "CBOM": btnDeleteCBOM.Visible = true; break;
                case "asBuilt": btnDeleteAsBuilt.Visible = true; break;
                case "FAReport": btnDeleteFAReport.Visible = true; break;
            }
            UpdateResetButtonState();


            //  Optional: Prompt to preview raw text dump for FA Report
            //if (key == "FAReport")
            //{
            //    var answer = MessageBox.Show(
            //        "Preview raw text extracted from the FA Report now?",
            //        "FA Report Preview",
            //        MessageBoxButtons.YesNo,
            //        MessageBoxIcon.Question);

            //    if (answer == DialogResult.Yes)
            //    {
            //        _ = PreviewFaRawTextAsync();   // see method below
            //    }
            //}

        }


        private async Task PreviewFaRawTextAsync()
        {
            if (string.IsNullOrWhiteSpace(faReportFilePath))
            {
                MessageBox.Show("No FA Report selected.");
                return;
            }

            try
            {
                // Run the dump off the UI thread
                string rawPath = await Task.Run(() => PdfDebug.DumpRawText(faReportFilePath, inputDocsDir));

                // Let Kira see the file and optionally open it in Notepad
                var open = MessageBox.Show($"Raw PDF text saved:\n{rawPath}\n\nOpen in Notepad?",
                                           "FA Report Raw Text",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Information);

                if (open == DialogResult.Yes)
                {
                    // Start Notepad with the file
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "notepad.exe",
                        Arguments = $"\"{rawPath}\"",
                        UseShellExecute = false
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error dumping raw PDF text:\n" + ex.Message);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            btnReset.Enabled = false;


            // Rounded UI elements
            RoundControl(btnGo, 8);
            RoundControl(btnReset, 8);
            RoundControl(btnInovarBOM, 6);
            RoundControl(btnCBOM, 6);
            RoundControl(btnAsBuilt, 6);
            RoundControl(btnFAReport, 6);
            RoundControl(submissionPanel, 6);

        }

        private void EnsureTemplateAvailable()
        {
            var asm = Assembly.GetExecutingAssembly();

            // 1) Try exact resource name (adjust if your default namespace differs)
            string exactName = "Matchmaker.InputDocs.FAIR_ImportTemplate_RevC.xlsx";

            // 2) Fallback: search by file name (works even if namespace/folder changes)
            using Stream? resourceStream =
                asm.GetManifestResourceStream(exactName) ??
                FindResourceStream(asm, "FAIR_ImportTemplate_RevC.xlsx");

            if (resourceStream == null)
            {
                // Helpful diagnostic in case the resource name doesn't match
                var available = string.Join(Environment.NewLine, asm.GetManifestResourceNames());
                throw new FileNotFoundException(
                    $"Embedded resource for template not found. Looked for '{exactName}'. " +
                    $"Available resource names:\n{available}");
            }

            // Write only if missing or zero-length (optional overwrite logic)
            if (!File.Exists(templatePath) || new FileInfo(templatePath).Length == 0)
            {
                using var outFile = File.Create(templatePath);
                resourceStream.CopyTo(outFile);
            }
        }

        // Finds an embedded resource that ends with the specified file name.
        private static Stream? FindResourceStream(Assembly asm, string endsWithFileName)
        {
            string? name = asm.GetManifestResourceNames()
                              .FirstOrDefault(n => n.EndsWith(endsWithFileName,
                                                StringComparison.OrdinalIgnoreCase));
            return name != null ? asm.GetManifestResourceStream(name) : null;
        }

        private void UpdateResetButtonState() {
            btnReset.Enabled =  // once a file is uploaded, enable reset button
                !string.IsNullOrEmpty(inovarBOMFilePath) || !string.IsNullOrEmpty(cBomFilePath) || !string.IsNullOrEmpty(asBuiltFilePath) || !string.IsNullOrEmpty(faReportFilePath);
        }

        private void RoundControl(Control ctrl, int radius)
        {

            var path = new GraphicsPath();

            int diameter = radius * 2;
            var rect = ctrl.ClientRectangle;

            // Top-left corner
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // Top-right corner
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            // Bottom-right corner
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            // Bottom-left corner
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();

            ctrl.Region = new Region(path);

        }

        private void FileButton_DragEnter(object sender, DragEventArgs e)
        {
            ResetUploadButtonColors();   // clear everything first

            if (sender is not Button btn) {
                return;
            }

            btn.BackColor = Color.FromArgb(230, 240, 255);

            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void FileButton_DragDrop(object sender, DragEventArgs e)
        {

            if (sender is not Button btn) return;
            if (btn.Tag is not string key) return;

            if (e.Data is IDataObject data && data.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
            {
                string filePath = files[0];
                HandleFileSelection(key, filePath);
            }

            btn.BackColor = Color.White;

        }

        private async void btnGo_Click(object sender, EventArgs e)
        {
            // update UI to show click being processed
            btnGo.Enabled = false;
            lblStatus.Text = "Loading ...";

            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 30;


            try
            {

                // Confirm files uploaded 
                if (string.IsNullOrWhiteSpace(inovarBOMFilePath) ||
                    string.IsNullOrWhiteSpace(cBomFilePath) ||
                    string.IsNullOrWhiteSpace(asBuiltFilePath) ||
                    string.IsNullOrWhiteSpace(faReportFilePath))
                {
                    MessageBox.Show("Please upload Inovar BOM, CBOM, As Built, and FA Report files before continuing.");
                    throw new Exception("Missing input file(s).");
                }

                // run engine
                var results = await Task.Run(() =>
                {
                    var loaderFactory = new LoaderFactory();
                    var serviceFactory = new ServiceFactory();

                    var engine = new MatchmakerEngine(loaderFactory, serviceFactory);
                    try
                    {
                        return engine.Process(inovarBOMFilePath, asBuiltFilePath, cBomFilePath, faReportFilePath);
                    }
                    catch (ApplicationException ex)
                    {
                        MessageBox.Show(ex.Message, "File Access Error", MessageBoxButtons.OK);
                        return new List<MatchResult>(); // returns empty list
                    }

                });

                TemplateManager tm = new TemplateManager();

                var template = tm.LoadTemplate(templatePath);
                tm.PopulateTemplate(template, results); //write results to template
                tm.SaveModifiedTemplate(template);      // save updated template

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading Excel files:\n" + ex.Message + "\n\nFor persistent issues, contact kira.carr@spartronics.com");
                return;
            }
            finally
            {
                // reset UI after fail
                lblStatus.Text = "";
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.MarqueeAnimationSpeed = 0;
                progressBar1.Visible = false;

                MessageBox.Show("File saved successfully.");
                btnGo.Enabled = true;
            }

            lblStatus.Text = "";
            btnGo.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.Tag is not string key) return;

            if (fileClearers.TryGetValue(key, out var deleteAction))
                deleteAction();

            UpdateResetButtonState();
        }

        private void submissionPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ResetUploadButtonColors()
        {
            btnInovarBOM.BackColor = Color.White;
            btnCBOM.BackColor = Color.White;
            btnAsBuilt.BackColor = Color.White;
            btnFAReport.BackColor = Color.White;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            fileClearers["inovarBOM"]();
            fileClearers["CBOM"]();
            fileClearers["asBuilt"]();
            fileClearers["FAReport"]();

            UpdateResetButtonState();
        }

    }



}
