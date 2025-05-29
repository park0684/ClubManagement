using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManagement.DBConfig.views
{
    public partial class DatabaseConfigView : Form, IDatabaseConfigView
    {
        public DatabaseConfigView()
        {
            InitializeComponent();
            this.ControlBox = false;
            ViewEvent();
        }

        public string Address
        {
            get { return txtAddress.Text; }
            set { txtAddress.Text = value; }
        }
        public int Port 
        {
            get { return Convert.ToInt32(txtPort.Text); }
            set { txtPort.Text = value.ToString(); }
        }
        public string User 
        { 
            get { return txtUser.Text; } 
            set { txtUser.Text = value; } 
        }
        public string Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }
        public string Database
        {
            get { return txtDatabase.Text; }
            set { txtDatabase.Text = value; }
        }

        public event EventHandler CloseFromEvent;
        public event EventHandler SaveEvent;
        public event EventHandler TestEvent;

        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseFromEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
            btnConnection.Click += (s, e) => TestEvent?.Invoke(this, EventArgs.Empty);
        }
        public void CloseForm()
        {
            this.Close();
        }

        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowDialog();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public bool ShowConfirmation(string message)
        {
            return MessageBox.Show(message, "확인", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }
    }
}
