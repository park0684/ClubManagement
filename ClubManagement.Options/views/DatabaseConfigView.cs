using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManagement.Options.views
{
    public partial class DatabaseConfigView : Form,IDatabaseConfigView
    {
        public DatabaseConfigView()
        {
            InitializeComponent();
            this.Text = "데이터베이스 연결 설정";
            ControlBox = false;
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

        public void CloseForm()
        {
            this.Close();
        }

        public void ShowForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            ShowDialog();
        }

        private void ViewEvent()
        {
            btnClose.Click += (e, s) => CloseFromEvent?.Invoke(this, EventArgs.Empty);
            btnConnection.Click += (e, s) => TestEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
