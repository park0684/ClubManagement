using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Common.Menu;
using ClubManagement.View;
using ClubManagement.Common.Factory;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ClubManagement.Presenter
{
    public class MainPresenter
    {
        IMainView _view;
        IViewFactory _viewFactory;
        private readonly Dictionary<string, Form> _cachedViews = new Dictionary<string, Form>();
        public MainPresenter(IMainView view, IViewFactory viewFactory)
        {
            _view = view;
            _viewFactory = viewFactory;
            _view.DatabaseConnectEvent += ExecuteDatabaseConfig;
        }

        private void ExecuteDatabaseConfig(object sender, EventArgs e)
        {
            try
            {
                string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClubManagement.DBConfig.exe");
                if (File.Exists(exePath))
                {
                    Process.Start(exePath);  // 비동기 실행 (모달 아님)
                }
                else
                {
                    _view.ShowMessage("설정 프로그램을 찾을 수 없습니다.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage("설정 프로그램 실행 중 오류: " + ex.Message);
            }
        }

        public void LoadMenu()
        {
            List<MenuItemInfo> items = MenuConfiguration.GetMenuItems(HandleMenuClick);
            Dictionary<string, List<MenuItemInfo>> grouped = new Dictionary<string, List<MenuItemInfo>>();

            foreach (var item in items)
            {
                if (!grouped.ContainsKey(item.Category))
                    grouped[item.Category] = new List<MenuItemInfo>();
                grouped[item.Category].Add(item);
            }

            _view.SetMenuItems(grouped);
        }

        private void HandleMenuClick(string menuKey)
        {
            if(!_cachedViews.TryGetValue(menuKey, out var form) || form.IsDisposed)
            {
                form = _viewFactory.CreateView(menuKey);
                if (form != null)
                    _cachedViews[menuKey] = form;
            }

            if (form != null)
                _view.LoadContent(form);
        }
    }
}
