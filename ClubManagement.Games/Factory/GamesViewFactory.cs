using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Common.Factory;
using ClubManagement.Games.Presenters;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Factory
{
    public class GamesViewFactory : IViewFactory
    {
        public Form CreateView(string menuKey)
        {
            switch (menuKey)
            {
                case "MatchList":
                    var matcthView = new MatchListView();
                    var matchRepo = new MatchRepository();
                    var presenter = new MatchListPresenter(matcthView, matchRepo);
                    return matcthView;

                case "ScoreBoardList":
                    var ScoreBoardListView = new ScoreboardListView();
                    var ScoreRepo = new ScoreBoardRepository();
                    var ScorePres = new ScoreBoardListPresenter(ScoreBoardListView, ScoreRepo);
                    return ScoreBoardListView;

            }
            return null;

        }
    }
}
