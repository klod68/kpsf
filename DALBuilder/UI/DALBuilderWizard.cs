using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DALBuilder.ApplicationLayer;

namespace DALBuilder.UI
{ 
	public enum WizardState
	 {
		  Welcome,
		  Connect,
		  SelectTables,
		  SaveDirectory,
		  Finish
	 }

	 public partial class DALBuilderWizard : Form
	 {
		  protected WizardState _state;
		  protected bool _tablesSelected = false;
		 private MakeScriptFilesController _controller = null;

		  #region constructor
		  public DALBuilderWizard()
		  {
			   InitializeComponent();
			   _controller = new MakeScriptFilesController();
			

		  }
		  #endregion

		  #region properties

		
		  #endregion

		  #region event handlers
		  private void DALBuilderWizard_Load(object sender, EventArgs e)
		  {
			   SetInitialValues();
		  }
		  private void btnHelp_Click(object sender, EventArgs e)
		  {
			   ShowHelp();
		  }
		  private void btnSelectDeselect_Click(object sender, EventArgs e)
		  {
			   SetSelectUnselectButton();
		  }
   
		  private void btnBrowseDas_Click(object sender, EventArgs e)
		  {
			   ShowBrowseDirectoryDialog(tbDASDirectory);
		  }
		  private void cbServers_SelectedIndexChanged(object sender, EventArgs e)
		  {
			   try
			   {
					GetDatabasesInServer(cbServers.Text);
			   }
			   catch { MessageBox.Show("Cannot login to SQL Server", "There is a problem with the connection"); }
		  }
		  private void btnNext_Click(object sender, EventArgs e)
		  {
			   SetNextWizardState();

		  }
		  private void btnBack_Click(object sender, EventArgs e)
		  {
			   SetBackWizardState();
		  }
		  private void btnCancel_Click(object sender, EventArgs e)
		  {
			   ShowCancelPrompt();
		  }
		  private void btnConnect_Click(object sender, EventArgs e)
		  {
			   TestConnection();
		  }
		  private void ckIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
		  {
			   SetSecurityControls();
		  }
		 
		  private void btnFinish_Click(object sender, EventArgs e)
		  {
			   SubmitSelectedTables();
			   AddConcurrencySupport();              
			   SaveScripts();
		  }
		private void cbDatabases_SelectedIndexChanged(object sender, EventArgs e)
		{
			_tablesSelected = false;
		}
		  #endregion

		  #region methods
		  private void SetInitialValues()
		  {
			   grWelcome.Height = 300;
			   grWelcome.Width = this.Width + 4;
			   grWelcome.Left = -2;
			   grWelcome.Top = 60;
			   _state = WizardState.Welcome;
		  }
		  private void SetNextWizardState()
		  {
			   switch (_state)
			   {
					case WizardState.Welcome:
						 grWelcome.Hide();
						 grConnect.Location = grWelcome.Location;
						 grConnect.Size = grWelcome.Size;
						 grConnect.Show();
						 btnBack.Enabled = true;
					CheckPredefinedSettings();

					GetSqlServersInNetwork();
						 
					_state = WizardState.Connect;
						 break;
					case WizardState.Connect:
						 
						 if (cbServers.Text == string.Empty || cbDatabases.Text == string.Empty)
							  return;
						 if (ckIntegratedSecurity.Checked == false && tbLogin.Text == string.Empty)
							  return;
						 if (!_tablesSelected)
						 {
							  ConnectToDb();
							  GetTables();
							  _tablesSelected = true;
						 }
						 grConnect.Hide();
						 grSelectTables.Location = grConnect.Location;
						 grSelectTables.Size = grConnect.Size;
						 grSelectTables.Show();
					CheckPredefinedSettings();
						 _state = WizardState.SelectTables;
						 break;
					case WizardState.SelectTables:
						 grSelectTables.Hide();
						 grSaveDASDirectory.Location = grSelectTables.Location;
						 grSaveDASDirectory.Size = grSelectTables.Size;
						 grSaveDASDirectory.Show();
					CheckPredefinedSettings();
						 _state = WizardState.SaveDirectory;
						 break;
					case WizardState.SaveDirectory:
						 grSaveDASDirectory.Hide();
					grFinish.Location = grSaveDASDirectory.Location;
					grFinish.Size = grSaveDASDirectory.Size;
					grFinish.Show();
					btnNext.Enabled = false;
					btnFinish.Enabled = true;
					_state = WizardState.Finish;
					break;
			   }
		  }
		  private void SetBackWizardState()
		  {
			   switch (_state)
			   {
					case WizardState.Connect:
						 grWelcome.Show();
						 grConnect.Hide();
						 btnBack.Enabled = false;
						 _state = WizardState.Welcome;
						 break;
					case WizardState.SelectTables:
						 grConnect.Show();
						 grSelectTables.Hide();
						 _state = WizardState.Connect;
						 break;
					case WizardState.SaveDirectory:
						 grSelectTables.Show();
						 grSaveDASDirectory.Hide();
						 _state = WizardState.SelectTables;
						 break;
					case WizardState.Finish:
					grSaveDASDirectory.Show();
						 grFinish.Hide();
						 btnNext.Enabled = true;
					_state = WizardState.SaveDirectory;
					break;
			   }
		  }
		  
		  private void SetSelectUnselectButton()
		  {
			   bool ck = btnSelectDeselect.Text.ToUpper() == "SELECT ALL" ? true : false;
			   btnSelectDeselect.Text = btnSelectDeselect.Text.ToUpper() == "SELECT ALL" ? "Unselect All" : "Select All";

			   for (short i = 0; i < lstTables.Items.Count; i++)
			   {
					lstTables.SetItemChecked(i, ck);
			   }
		  }
		  private void ShowBrowseDirectoryDialog(TextBox addressBox)
		  {
			   DialogResult _result = ScriptFolder.ShowDialog();
			   if (_result == DialogResult.OK)
			   {
					addressBox.Text = ScriptFolder.SelectedPath;
			   }
		  }

		private void CheckPredefinedSettings()
		{
			switch(_state)
			{
				case WizardState.Welcome:
					if (app.Default.Server != string.Empty)
						cbServers.Text = app.Default.Server;
					if (app.Default.Database != string.Empty)
						cbDatabases.Text = app.Default.Database;
					if (app.Default.IntegratedSecurity != false)
						ckIntegratedSecurity.Checked = app.Default.IntegratedSecurity;
						 if (app.Default.IntegratedSecurity == false)
						 {
							  tbLogin.Text = app.Default.UserId;
							  tbPassword.Text = app.Default.Password;
						 }
					break;
				case WizardState.Connect:
					if (app.Default.Optimistic != false)
						rbOptimistic.Checked = true;
					else if (app.Default.PessimisticUserId != false)
					{
						rbPessimistic.Checked = true;
						grPessimistic.Enabled = true;
						rbUniqueIdentifier.Checked = true;
					}
					else if (app.Default.PessimisticUserName != false)
					{
						rbPessimistic.Checked = true;
						grPessimistic.Enabled = true;
						rbVarchar.Checked = true;
					}
					break;
				case WizardState.SelectTables:
					if (app.Default.ScriptFilePath != string.Empty)
						tbDASDirectory.Text = app.Default.ScriptFilePath;
					break;
			}
		}
		  public  void GetSqlServersInNetwork()
		  {
			   try
			   {
					if (cbServers.Items.Count == 0 && cbServers.Text == string.Empty)
					{
					Application.DoEvents(); 

						 WaitWindow _w = new WaitWindow("Trying to locate SQL Servers in your network. Please wait...");
						 _w.Show();
						 _w.Update();
						 
						 this.Cursor = Cursors.WaitCursor;
					Application.DoEvents();
						 DataTable table = _controller.GetServersInNetwork();
						 foreach (DataRow row in table.Rows)
						 {
							  cbServers.Items.Add(row[0].ToString());
						 }
						 this.Cursor = Cursors.Default;
						 _w.Close();
					}
			   }
			   catch { this.Cursor = Cursors.Default; }
		  }
		  private void GetDatabasesInServer(string server)
		  {
			if (cbDatabases.Items.Count == 0 && cbDatabases.Text==string.Empty)
			   {
					this.Cursor = Cursors.WaitCursor;
					try
					{
						 Application.DoEvents(); 
						 DataTable table = _controller.GetDatabases(server);
						 foreach (DataRow row in table.Rows)
						 {
							  cbDatabases.Items.Add(row[0].ToString());
						 }
						 this.Cursor = Cursors.Default;
					}
					catch 
					{
						 this.Cursor = Cursors.Default;
					}
			   }
		  }
		  private void TestConnection()
		  {
			   try
			   {
					if (cbServers.Text == string.Empty || cbServers.Text == string.Empty)
						 return;
					if (ckIntegratedSecurity.Checked == false && tbLogin.Text == string.Empty)
						 return;

					Application.DoEvents(); 
					ConnectToDb();
					MessageBox.Show("Connection configured!", "Connection configured");
			   }
			   catch (Exception ex)
			   {
					MessageBox.Show("Connection failed!\n Exception: " + ex.Message, "Check Your Connection Settings");

			   }
		  }
		  private void ConnectToDb()
		  {
			   _controller.SetConnection(cbServers.Text, cbDatabases.Text, ckIntegratedSecurity.Checked, tbLogin.Text, tbPassword.Text);
		  }
		  private void GetTables()
		  {
			    string[] _tableNames = _controller.GetTableNames();
                if (_tableNames == null)
                return;

			    lstTables.Items.Clear();
			    for (byte i = 0; i < _tableNames.GetLength(0); i++)
			    {
				    lstTables.Items.Add(_tableNames[i]);
			    }

		  }
		  private void SubmitSelectedTables()
		  {
			   string[] _tables = new string[lstTables.CheckedItems.Count];
			   lstTables.CheckedItems.CopyTo(_tables, 0);
			   _controller.SubmitTablesSelected(_tables);
		  }
		  private void AddConcurrencySupport()
		  {
			   _controller.AddConcurrencySupport(rbOptimistic.Checked,rbVarchar.Checked,rbUniqueIdentifier.Checked);
		  }
		  private void SaveScripts()
		  {
			   _controller.SaveScripts(tbDASDirectory.Text);
			   MessageBox.Show("Scripts files has been saved!", "Wizard Finished");
			   Application.Exit();
		  }
		  private void ShowHelp()
		  {
			   string _message = string.Empty;
			   string _title = string.Empty;
			   switch (_state)
			   {
					case WizardState.Welcome:
						 _message = "Welcome to DALBuilder Wizard.\nTo continue press the next button.";
						 _title = "Welcome";
						 break;
					case WizardState.Connect:
						 _message = "Enter your database configuration settings.\nTo test your configuration press the test button.\nTo continue press the next button.";
						 _title = "Connect to Your Database";
						 break;
					case WizardState.SelectTables:
						 _message = "Please select all the tables you want to generate a script\n and press the next button.";
						 _title = "Select Tables";
						 break;
					case WizardState.SaveDirectory:
						 _message = "Provide a valid directory to save the files.\nClick the browse button to search or create a folder.";
						 _title = "Provide a Directory";
						 break;
					case WizardState.Finish:
						 _message = "Your are about to finish the wizard.\nReview your settings and click the finish button.";
						 _title = "Finish";
						 break;
			   }
			   MessageBox.Show(_message, _title);
		  }
		  private void ShowCancelPrompt()
		  {
			   DialogResult yesno = MessageBox.Show("Are you sure you want to cancel the wizard?", "Cancel the Wizard", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			   if (yesno == DialogResult.Yes)
					Application.Exit();
		  }
		  private void SetSecurityControls()
		  {
			   bool disabled = !ckIntegratedSecurity.Checked;
			   tbLogin.Enabled = disabled;
			   tbPassword.Enabled = disabled;
		  }
		  #endregion 

		private void rbPessimistic_CheckedChanged(object sender, EventArgs e)
		{
			if (rbPessimistic.Checked)
				grPessimistic.Enabled = true;
			else
			{
				grPessimistic.Enabled = false;
				rbUniqueIdentifier.Checked = false;
				rbVarchar.Checked = false;
			}
		}

	 }
}