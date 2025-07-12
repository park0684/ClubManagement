using ClubManagement.Common.Factory;
using ClubManagement.Members.Presenters;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManagement.Members.Factory
{
    public class MembersViewFactory: IViewFactory
    {
        public Form CreateView(string menuKey)
        {
            switch (menuKey)
            {
                case "MemberList":
                    var view = new MemberListView();
                    var repo = new MemberRepository();
                    var presenter = new MemberListPresenter(view, repo);
                    return view;
                    
                case "DuesManage":
                    var duesView = new DuesManageView();
                    var duesRepo = new DuesRepsotiry();
                    var udesPres = new DuesManagePresenter(duesView, duesRepo);
                    return duesView;
                case "MemberScore":
                    var scoreView = new MemberScoreView();
                    var scoreRepo = new MemberScoreRepository();
                    var scorePres = new MemberScorePresenter(scoreView, scoreRepo);
                    return scoreView;
            }
            return null;

        }
    }
}
