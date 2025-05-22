using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManagement.Games.Views
{
    public partial class NumericPadView : Form,INumericPadView
    {
        public NumericPadView()
        {
            InitializeComponent();
            viewEvent();
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        public int Nember
        {
            get { return Convert.ToInt32(lblNumber.Text); }
            set { lblNumber.Text = value.ToString("#,##0"); }
        }

        public event EventHandler CloseFormEvent;
        public event EventHandler InsertNumberEvent;

        public void CloseForm()
        {
            this.Close();
        }

        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        private void viewEvent()
        {
            btnNumber0.Click += (s, e) => EnterNumber("0");
            btnNumber1.Click += (s, e) => EnterNumber("1");
            btnNumber2.Click += (s, e) => EnterNumber("2");
            btnNumber3.Click += (s, e) => EnterNumber("3");
            btnNumber4.Click += (s, e) => EnterNumber("4");
            btnNumber5.Click += (s, e) => EnterNumber("5");
            btnNumber6.Click += (s, e) => EnterNumber("6");
            btnNumber7.Click += (s, e) => EnterNumber("7");
            btnNumber8.Click += (s, e) => EnterNumber("8");
            btnNumber9.Click += (s, e) => EnterNumber("9"); 
            btnBackSpace.Click += (s, e) => NumberBacksapce();
            btnClear.Click += (s, e) => NumberClear();

            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);
            btnOk.Click += (s, e) =>
            {
                InsertNumberEvent?.Invoke(this, EventArgs.Empty);
                CloseForm();
            };
        }

        private void NumberClear()
        {
            lblNumber.Text = "0";
        }

        private void NumberBacksapce()
        {
            if(lblNumber.Text == "0")
            {
                return;
            }
            else
            {
                int textLength = lblNumber.Text.Length;
                lblNumber.Text = lblNumber.Text.Substring(0, textLength - 1);
            }
        }

        private void EnterNumber(string number)
        {
            if(lblNumber.Text != "0")
            {
                lblNumber.Text += number;
            }
            else
            {
                lblNumber.Text = number;
            }
        }
    }
}
