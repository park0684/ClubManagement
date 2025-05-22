using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Service;
using ClubManagement.Games.Views;
namespace ClubManagement.Games.Presenters
{
    public class MatchDetailPresenter
    {
        private IMatchDetailView _view;
        private IMatchRepository _repository;
        private MatchModel _model;
        private MatchService _service;

        public MatchDetailPresenter(IMatchDetailView view, IMatchRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new MatchModel();
            this._service = new MatchService(_repository);
            this._view.CloseEvenvt += CloseAction;
            this._view.SaveEvent += SaveMatch;
            _model.IsNew = true;
        }

        public void LoadMatch(int code)
        {
            _model = _service.LoadMatch(code);

            _view.GameTitle = _model.MatchTitle;
            _view.GameHost = _model.MatchHost;
            _view.GameMemo = _model.MatchMemo;
            _view.GameDate = (DateTime)_model.MatchDate;
            _view.GameType = (int)_model.MatchType;
        }
        private void SaveMatch(object sender, EventArgs e)
        {
            _model.MatchTitle = _view.GameTitle;
            _model.MatchHost = _view.GameHost;
            _model.MatchDate = _view.GameDate;
            _model.MatchType = _view.GameType;
            _model.MatchMemo = _view.GameMemo;

            try
            {
                _service.SaveMatch(_model);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
            _view.CloseForm();
        }

        private void CloseAction(object sender, EventArgs e)
        {
            _view.CloseForm();
        }


    }
}
