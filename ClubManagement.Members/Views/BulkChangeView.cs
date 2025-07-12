using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClubManagement.Members.Views
{
    public partial class BulkChangeView : Form, IBulkChangeView
    {
        public BulkChangeView()
        {
            InitializeComponent();
            ViewEvent();
        }

        public int SelectedItem => ((KeyValuePair<int, string>)cmbItem.SelectedItem).Key;

        public event EventHandler CloseFormEvent;
        public event EventHandler<int> SelectedEvent;

        public void CloseForm()
        {
            this.Close();
        }

        public void SetComboBoxItems(Dictionary<int, string> items)
        {
            cmbItem.DataSource = items.ToList();
            cmbItem.DisplayMember = "Value";
            cmbItem.ValueMember = "Key";
            cmbItem.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        public void ShowMessageBox(string message)
        {
            MessageBox.Show(message);
        }

        private void ViewEvent()
        {
            btnSelect.Click += (s, e) => SelectedEvent(this, SelectedItem);
            btnClose.Click += (s, e) => CloseFormEvent(this, EventArgs.Empty);
        }
    }
}
