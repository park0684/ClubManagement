using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.DBConfig.views;
using ClubManagement.DBConfig.Presenters;

namespace ClubManagement.DBConfig
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                IDatabaseConfigView view = new DatabaseConfigView();
                var resenter = new DatabaseConfigPresenter(view);

                Application.Run((Form)view);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}\n{ex.StackTrace}" );
            }
            
        }
    }
}
