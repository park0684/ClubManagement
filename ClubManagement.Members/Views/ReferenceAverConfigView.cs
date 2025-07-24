using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManagement.Members.Views
{
    public partial class ReferenceAverConfigView : Form, IReferenceAverConfigView
    {
        public ReferenceAverConfigView()
        {
            InitializeComponent();
            SetButtonTagItem();
            ViewEvent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ControlBox = false;
            this.Text = "기준 에버 기간 설정";
        }

        public event EventHandler CloseFormEvent;
        public event EventHandler<int> SaveEvent;

        public void CloseForm()
        {
            this.Close();
        }

        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += btnSave_Click;
            foreach ( var btn in this.Controls.OfType<Button>())
            {
                if(btn.Tag is ValueTuple<bool, int>)
                {
                    btn.Click += Button_Click;
                    SetButtonUnSelected(btn);
                }
            }
        }

        private void SetButtonTagItem()
        {
            btnOneMonth.Tag = (isSelected: false, interval: 1);
            btnThreeMonth.Tag = (isSelected: false, interval: 3);
            btnSixMonth.Tag = (isSelected: false, interval: 6);
            btnOneYear.Tag = (isSelected: false, interval: 12);
        }
        private void Button_Click(object sender, EventArgs e)
        {
            if(sender is Button clicked && clicked.Tag is ValueTuple<bool, int> tag )
            {
                foreach(var btn in this.Controls.OfType<Button>())
                {
                    if(btn.Tag is ValueTuple<bool, int> t)
                    {
                        bool isMatch = btn == clicked;
                        btn.Tag = (isMatch, t.Item2);
                        if(isMatch)
                        {
                            SetButtonSeleced(btn);
                        }
                        else
                        {
                            SetButtonUnSelected(btn);
                        }
                    }
                    
                }
            }
        }

        private void SetButtonSeleced(Button btn)
        {
            btn.BackColor = Color.FromArgb(126, 112, 247);
            btn.ForeColor = Color.White;
        }
        private void SetButtonUnSelected(Button btn)
        {
            btn.BackColor = Color.White;
            btn.ForeColor = Color.Black;
        }

        public void GetInterval(int interval)
        {
            foreach( var btn in this.Controls.OfType<Button>())
            {

                if (btn.Tag is ValueTuple<bool, int> tag && tag.Item2 == interval)
                {
                    SetButtonSeleced(btn);
                    return;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach( var btn in this.Controls.OfType<Button>())
            {
                if (btn.Tag is ValueTuple<bool, int> tag && tag.Item1 == true)
                {
                    SaveEvent?.Invoke(this, tag.Item2);
                    return;
                }                    
            }
        }
    }
}
