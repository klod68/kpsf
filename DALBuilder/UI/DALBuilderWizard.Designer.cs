namespace DALBuilder.UI
{
     partial class DALBuilderWizard
     {
          /// <summary>
          /// Required designer variable.
          /// </summary>
          private System.ComponentModel.IContainer components = null;

          /// <summary>
          /// Clean up any resources being used.
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
          /// Required method for Designer support - do not modify
          /// the contents of this method with the code editor.
          /// </summary>
          private void InitializeComponent()
          {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DALBuilderWizard));
            this.grWelcome = new System.Windows.Forms.GroupBox();
            this.lblFirstMessage = new System.Windows.Forms.Label();
            this.grConnect = new System.Windows.Forms.GroupBox();
            this.lblConnectMessage = new System.Windows.Forms.Label();
            this.cbDatabases = new System.Windows.Forms.ComboBox();
            this.cbServers = new System.Windows.Forms.ComboBox();
            this.grLoginPwd = new System.Windows.Forms.GroupBox();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.ckIntegratedSecurity = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMain = new System.Windows.Forms.Label();
            this.grSelectTables = new System.Windows.Forms.GroupBox();
            this.grConcurrency = new System.Windows.Forms.GroupBox();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.rbPessimistic = new System.Windows.Forms.RadioButton();
            this.rbOptimistic = new System.Windows.Forms.RadioButton();
            this.grPessimistic = new System.Windows.Forms.GroupBox();
            this.rbVarchar = new System.Windows.Forms.RadioButton();
            this.rbUniqueIdentifier = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelectDeselect = new System.Windows.Forms.Button();
            this.lblSelectTables = new System.Windows.Forms.Label();
            this.lstTables = new System.Windows.Forms.CheckedListBox();
            this.grSaveDASDirectory = new System.Windows.Forms.GroupBox();
            this.btnBrowseDas = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDASDirectory = new System.Windows.Forms.TextBox();
            this.ScriptFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.grFinish = new System.Windows.Forms.GroupBox();
            this.lblFinish = new System.Windows.Forms.Label();
            this.grWelcome.SuspendLayout();
            this.grConnect.SuspendLayout();
            this.grLoginPwd.SuspendLayout();
            this.grSelectTables.SuspendLayout();
            this.grConcurrency.SuspendLayout();
            this.grPessimistic.SuspendLayout();
            this.grSaveDASDirectory.SuspendLayout();
            this.grFinish.SuspendLayout();
            this.SuspendLayout();
            // 
            // grWelcome
            // 
            this.grWelcome.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grWelcome.Controls.Add(this.lblFirstMessage);
            this.grWelcome.Location = new System.Drawing.Point(26, 36);
            this.grWelcome.Name = "grWelcome";
            this.grWelcome.Size = new System.Drawing.Size(277, 152);
            this.grWelcome.TabIndex = 1;
            this.grWelcome.TabStop = false;
            // 
            // lblFirstMessage
            // 
            this.lblFirstMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstMessage.Location = new System.Drawing.Point(186, 26);
            this.lblFirstMessage.Name = "lblFirstMessage";
            this.lblFirstMessage.Size = new System.Drawing.Size(290, 115);
            this.lblFirstMessage.TabIndex = 0;
            this.lblFirstMessage.Text = resources.GetString("lblFirstMessage.Text");
            // 
            // grConnect
            // 
            this.grConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grConnect.Controls.Add(this.lblConnectMessage);
            this.grConnect.Controls.Add(this.cbDatabases);
            this.grConnect.Controls.Add(this.cbServers);
            this.grConnect.Controls.Add(this.grLoginPwd);
            this.grConnect.Controls.Add(this.label5);
            this.grConnect.Controls.Add(this.btnConnect);
            this.grConnect.Controls.Add(this.ckIntegratedSecurity);
            this.grConnect.Controls.Add(this.label1);
            this.grConnect.Location = new System.Drawing.Point(176, 36);
            this.grConnect.Name = "grConnect";
            this.grConnect.Size = new System.Drawing.Size(218, 139);
            this.grConnect.TabIndex = 1;
            this.grConnect.TabStop = false;
            this.grConnect.Visible = false;
            // 
            // lblConnectMessage
            // 
            this.lblConnectMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectMessage.Location = new System.Drawing.Point(11, 33);
            this.lblConnectMessage.Name = "lblConnectMessage";
            this.lblConnectMessage.Size = new System.Drawing.Size(225, 66);
            this.lblConnectMessage.TabIndex = 22;
            this.lblConnectMessage.Text = "Enter your connection settings:";
            // 
            // cbDatabases
            // 
            this.cbDatabases.FormattingEnabled = true;
            this.cbDatabases.Location = new System.Drawing.Point(301, 60);
            this.cbDatabases.Name = "cbDatabases";
            this.cbDatabases.Size = new System.Drawing.Size(186, 21);
            this.cbDatabases.TabIndex = 24;
            this.cbDatabases.SelectedIndexChanged += new System.EventHandler(this.cbDatabases_SelectedIndexChanged);
            // 
            // cbServers
            // 
            this.cbServers.FormattingEnabled = true;
            this.cbServers.Location = new System.Drawing.Point(301, 33);
            this.cbServers.Name = "cbServers";
            this.cbServers.Size = new System.Drawing.Size(186, 21);
            this.cbServers.TabIndex = 23;
            this.cbServers.SelectedIndexChanged += new System.EventHandler(this.cbServers_SelectedIndexChanged);
            // 
            // grLoginPwd
            // 
            this.grLoginPwd.Controls.Add(this.tbLogin);
            this.grLoginPwd.Controls.Add(this.label2);
            this.grLoginPwd.Controls.Add(this.tbPassword);
            this.grLoginPwd.Controls.Add(this.label3);
            this.grLoginPwd.Location = new System.Drawing.Point(231, 117);
            this.grLoginPwd.Name = "grLoginPwd";
            this.grLoginPwd.Size = new System.Drawing.Size(278, 100);
            this.grLoginPwd.TabIndex = 5;
            this.grLoginPwd.TabStop = false;
            this.grLoginPwd.Text = "Login settings";
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(70, 28);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(167, 20);
            this.tbLogin.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(70, 54);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(167, 20);
            this.tbPassword.TabIndex = 3;
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Login";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(242, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Database";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(434, 253);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 17;
            this.btnConnect.Text = "Test";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // ckIntegratedSecurity
            // 
            this.ckIntegratedSecurity.AutoSize = true;
            this.ckIntegratedSecurity.Location = new System.Drawing.Point(299, 94);
            this.ckIntegratedSecurity.Name = "ckIntegratedSecurity";
            this.ckIntegratedSecurity.Size = new System.Drawing.Size(115, 17);
            this.ckIntegratedSecurity.TabIndex = 4;
            this.ckIntegratedSecurity.Text = "Integrated Security";
            this.ckIntegratedSecurity.UseVisualStyleBackColor = true;
            this.ckIntegratedSecurity.CheckedChanged += new System.EventHandler(this.ckIntegratedSecurity_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(242, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server";
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnHelp.Location = new System.Drawing.Point(47, 372);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(73, 23);
            this.btnHelp.TabIndex = 2;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnBack.Enabled = false;
            this.btnBack.Location = new System.Drawing.Point(219, 371);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(73, 23);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "< Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNext.Location = new System.Drawing.Point(298, 371);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(73, 23);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnFinish.Enabled = false;
            this.btnFinish.Location = new System.Drawing.Point(377, 371);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(73, 23);
            this.btnFinish.TabIndex = 5;
            this.btnFinish.Text = "Finish >>|";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(456, 371);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMain
            // 
            this.lblMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMain.AutoSize = true;
            this.lblMain.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMain.Location = new System.Drawing.Point(39, 9);
            this.lblMain.Name = "lblMain";
            this.lblMain.Size = new System.Drawing.Size(515, 24);
            this.lblMain.TabIndex = 7;
            this.lblMain.Text = "Welcome to the Data Access Layer Builder Wizard";
            // 
            // grSelectTables
            // 
            this.grSelectTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grSelectTables.Controls.Add(this.grConcurrency);
            this.grSelectTables.Controls.Add(this.btnSelectDeselect);
            this.grSelectTables.Controls.Add(this.lblSelectTables);
            this.grSelectTables.Controls.Add(this.lstTables);
            this.grSelectTables.Location = new System.Drawing.Point(358, 46);
            this.grSelectTables.Name = "grSelectTables";
            this.grSelectTables.Size = new System.Drawing.Size(245, 126);
            this.grSelectTables.TabIndex = 1;
            this.grSelectTables.TabStop = false;
            this.grSelectTables.Visible = false;
            // 
            // grConcurrency
            // 
            this.grConcurrency.Controls.Add(this.rbNone);
            this.grConcurrency.Controls.Add(this.label7);
            this.grConcurrency.Controls.Add(this.rbPessimistic);
            this.grConcurrency.Controls.Add(this.rbOptimistic);
            this.grConcurrency.Controls.Add(this.grPessimistic);
            this.grConcurrency.Controls.Add(this.label6);
            this.grConcurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grConcurrency.Location = new System.Drawing.Point(242, 32);
            this.grConcurrency.Name = "grConcurrency";
            this.grConcurrency.Size = new System.Drawing.Size(238, 213);
            this.grConcurrency.TabIndex = 25;
            this.grConcurrency.TabStop = false;
            this.grConcurrency.Text = "Concurrency Support";
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Checked = true;
            this.rbNone.Location = new System.Drawing.Point(19, 28);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(65, 24);
            this.rbNone.TabIndex = 27;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(38, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(171, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "(Add an InUse column.)";
            // 
            // rbPessimistic
            // 
            this.rbPessimistic.AutoSize = true;
            this.rbPessimistic.Location = new System.Drawing.Point(19, 92);
            this.rbPessimistic.Name = "rbPessimistic";
            this.rbPessimistic.Size = new System.Drawing.Size(105, 24);
            this.rbPessimistic.TabIndex = 26;
            this.rbPessimistic.Text = "Pessimistic";
            this.rbPessimistic.UseVisualStyleBackColor = true;
            this.rbPessimistic.CheckedChanged += new System.EventHandler(this.rbPessimistic_CheckedChanged);
            // 
            // rbOptimistic
            // 
            this.rbOptimistic.AutoSize = true;
            this.rbOptimistic.Location = new System.Drawing.Point(19, 58);
            this.rbOptimistic.Name = "rbOptimistic";
            this.rbOptimistic.Size = new System.Drawing.Size(96, 24);
            this.rbOptimistic.TabIndex = 26;
            this.rbOptimistic.Text = "Optimistic";
            this.rbOptimistic.UseVisualStyleBackColor = true;
            // 
            // grPessimistic
            // 
            this.grPessimistic.Controls.Add(this.rbVarchar);
            this.grPessimistic.Controls.Add(this.rbUniqueIdentifier);
            this.grPessimistic.Enabled = false;
            this.grPessimistic.Location = new System.Drawing.Point(36, 123);
            this.grPessimistic.Name = "grPessimistic";
            this.grPessimistic.Size = new System.Drawing.Size(179, 67);
            this.grPessimistic.TabIndex = 6;
            this.grPessimistic.TabStop = false;
            // 
            // rbVarchar
            // 
            this.rbVarchar.AutoSize = true;
            this.rbVarchar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbVarchar.Location = new System.Drawing.Point(10, 15);
            this.rbVarchar.Name = "rbVarchar";
            this.rbVarchar.Size = new System.Drawing.Size(82, 21);
            this.rbVarchar.TabIndex = 5;
            this.rbVarchar.TabStop = true;
            this.rbVarchar.Text = "[varchar]";
            this.rbVarchar.UseVisualStyleBackColor = true;
            // 
            // rbUniqueIdentifier
            // 
            this.rbUniqueIdentifier.AutoSize = true;
            this.rbUniqueIdentifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbUniqueIdentifier.Location = new System.Drawing.Point(10, 38);
            this.rbUniqueIdentifier.Name = "rbUniqueIdentifier";
            this.rbUniqueIdentifier.Size = new System.Drawing.Size(131, 21);
            this.rbUniqueIdentifier.TabIndex = 4;
            this.rbUniqueIdentifier.TabStop = true;
            this.rbUniqueIdentifier.Text = "[uniqueidentifier]";
            this.rbUniqueIdentifier.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(38, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(185, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "(Add a Version [timestamp] column.)";
            // 
            // btnSelectDeselect
            // 
            this.btnSelectDeselect.Location = new System.Drawing.Point(140, 270);
            this.btnSelectDeselect.Name = "btnSelectDeselect";
            this.btnSelectDeselect.Size = new System.Drawing.Size(75, 23);
            this.btnSelectDeselect.TabIndex = 24;
            this.btnSelectDeselect.Text = "Select All";
            this.btnSelectDeselect.UseVisualStyleBackColor = true;
            this.btnSelectDeselect.Click += new System.EventHandler(this.btnSelectDeselect_Click);
            // 
            // lblSelectTables
            // 
            this.lblSelectTables.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectTables.Location = new System.Drawing.Point(16, 33);
            this.lblSelectTables.Name = "lblSelectTables";
            this.lblSelectTables.Size = new System.Drawing.Size(220, 28);
            this.lblSelectTables.TabIndex = 23;
            this.lblSelectTables.Text = "Select the Tables for Scripting.";
            // 
            // lstTables
            // 
            this.lstTables.CheckOnClick = true;
            this.lstTables.FormattingEnabled = true;
            this.lstTables.Location = new System.Drawing.Point(17, 64);
            this.lstTables.Name = "lstTables";
            this.lstTables.Size = new System.Drawing.Size(198, 199);
            this.lstTables.Sorted = true;
            this.lstTables.TabIndex = 0;
            // 
            // grSaveDASDirectory
            // 
            this.grSaveDASDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grSaveDASDirectory.Controls.Add(this.btnBrowseDas);
            this.grSaveDASDirectory.Controls.Add(this.label4);
            this.grSaveDASDirectory.Controls.Add(this.tbDASDirectory);
            this.grSaveDASDirectory.Location = new System.Drawing.Point(20, 194);
            this.grSaveDASDirectory.Name = "grSaveDASDirectory";
            this.grSaveDASDirectory.Size = new System.Drawing.Size(249, 145);
            this.grSaveDASDirectory.TabIndex = 1;
            this.grSaveDASDirectory.TabStop = false;
            this.grSaveDASDirectory.Visible = false;
            // 
            // btnBrowseDas
            // 
            this.btnBrowseDas.Location = new System.Drawing.Point(422, 62);
            this.btnBrowseDas.Name = "btnBrowseDas";
            this.btnBrowseDas.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDas.TabIndex = 2;
            this.btnBrowseDas.Text = "Browse";
            this.btnBrowseDas.UseVisualStyleBackColor = true;
            this.btnBrowseDas.Click += new System.EventHandler(this.btnBrowseDas_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(313, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Select a location for your  files:";
            // 
            // tbDASDirectory
            // 
            this.tbDASDirectory.Location = new System.Drawing.Point(25, 64);
            this.tbDASDirectory.Name = "tbDASDirectory";
            this.tbDASDirectory.Size = new System.Drawing.Size(391, 20);
            this.tbDASDirectory.TabIndex = 0;
            // 
            // ScriptFolder
            // 
            this.ScriptFolder.Description = "Select Where Yow Want to Save Your DAS File";
            // 
            // grFinish
            // 
            this.grFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grFinish.Controls.Add(this.lblFinish);
            this.grFinish.Location = new System.Drawing.Point(370, 245);
            this.grFinish.Name = "grFinish";
            this.grFinish.Size = new System.Drawing.Size(209, 111);
            this.grFinish.TabIndex = 9;
            this.grFinish.TabStop = false;
            this.grFinish.Visible = false;
            // 
            // lblFinish
            // 
            this.lblFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinish.Location = new System.Drawing.Point(224, 38);
            this.lblFinish.Name = "lblFinish";
            this.lblFinish.Size = new System.Drawing.Size(257, 115);
            this.lblFinish.TabIndex = 1;
            this.lblFinish.Text = "You are about to finish the wizard. When finished you will have a DALScripts\r\n.sq" +
    "l file, a mappings.xml file and an optional ConcurrencySupportScript.sql file in" +
    " your selected locations.";
            // 
            // DALBuilderWizard
            // 
            this.ClientSize = new System.Drawing.Size(609, 410);
            this.Controls.Add(this.grSelectTables);
            this.Controls.Add(this.grSaveDASDirectory);
            this.Controls.Add(this.grFinish);
            this.Controls.Add(this.grWelcome);
            this.Controls.Add(this.grConnect);
            this.Controls.Add(this.lblMain);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnHelp);
            this.Name = "DALBuilderWizard";
            this.Text = "DAL Builder Wizard";
            this.Load += new System.EventHandler(this.DALBuilderWizard_Load);
            this.grWelcome.ResumeLayout(false);
            this.grConnect.ResumeLayout(false);
            this.grConnect.PerformLayout();
            this.grLoginPwd.ResumeLayout(false);
            this.grLoginPwd.PerformLayout();
            this.grSelectTables.ResumeLayout(false);
            this.grConcurrency.ResumeLayout(false);
            this.grConcurrency.PerformLayout();
            this.grPessimistic.ResumeLayout(false);
            this.grPessimistic.PerformLayout();
            this.grSaveDASDirectory.ResumeLayout(false);
            this.grSaveDASDirectory.PerformLayout();
            this.grFinish.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

          }

          #endregion

          public System.Windows.Forms.GroupBox grWelcome;
          private System.Windows.Forms.Button btnHelp;
          private System.Windows.Forms.Button btnBack;
          private System.Windows.Forms.Button btnNext;
          private System.Windows.Forms.Button btnFinish;
          private System.Windows.Forms.Button btnCancel;
          private System.Windows.Forms.Label lblMain;
          private System.Windows.Forms.Label lblFirstMessage;
          private System.Windows.Forms.GroupBox grConnect;
          private System.Windows.Forms.Label lblConnectMessage;
          private System.Windows.Forms.GroupBox grLoginPwd;
          private System.Windows.Forms.TextBox tbLogin;
          private System.Windows.Forms.Label label2;
          private System.Windows.Forms.TextBox tbPassword;
          private System.Windows.Forms.Label label3;
          private System.Windows.Forms.Label label5;
          private System.Windows.Forms.Button btnConnect;
          private System.Windows.Forms.CheckBox ckIntegratedSecurity;
          private System.Windows.Forms.Label label1;
          private System.Windows.Forms.GroupBox grSelectTables;
          private System.Windows.Forms.Label lblSelectTables;
          private System.Windows.Forms.CheckedListBox lstTables;
          private System.Windows.Forms.GroupBox grSaveDASDirectory;
          private System.Windows.Forms.Button btnBrowseDas;
          private System.Windows.Forms.Label label4;
          private System.Windows.Forms.TextBox tbDASDirectory;
		private System.Windows.Forms.FolderBrowserDialog ScriptFolder;
          private System.Windows.Forms.GroupBox grFinish;
          private System.Windows.Forms.Label lblFinish;
          private System.Windows.Forms.Button btnSelectDeselect;
          private System.Windows.Forms.ComboBox cbServers;
          private System.Windows.Forms.ComboBox cbDatabases;
		private System.Windows.Forms.GroupBox grConcurrency;
          private System.Windows.Forms.Label label6;
          private System.Windows.Forms.Label label7;
          private System.Windows.Forms.RadioButton rbVarchar;
          private System.Windows.Forms.RadioButton rbUniqueIdentifier;
          private System.Windows.Forms.GroupBox grPessimistic;
		private System.Windows.Forms.RadioButton rbPessimistic;
		private System.Windows.Forms.RadioButton rbOptimistic;
		private System.Windows.Forms.RadioButton rbNone;

     }
}