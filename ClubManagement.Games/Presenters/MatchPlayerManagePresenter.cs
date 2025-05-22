using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Service;
using ClubManagement.Games.Views;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ClubManagement.Games.Presenters
{
    public class MatchPlayerManagePresenter
    {
        IMatchPlayerManageView _view;
        IMatchRepository _repository;
        MatchModel _model;
        MatchSearchModel _searchModel;
        MatchService _service;

        public MatchPlayerManagePresenter(IMatchPlayerManageView view, IMatchRepository repository)
        {
            _view = view;
            _repository = repository;
            _model = new MatchModel();
            _searchModel = new MatchSearchModel();
            _service = new MatchService(_repository);
            this._view.SearchMemberEvent += SearchMember;;
            this._view.AddGuestEvent += GuestAdd;
            this._view.PlayerAddEvent += PlayerAdd;
            this._view.PlayerRemoveEvent += PlayerRemove;
            this._view.SavePlayerListEvent += PlayerUpdate;
            this._view.CloseEvent += CloseForm;
        }

        private void GuestAdd(object sender, EventArgs e)
        {
            IGuestAddView view = new GuestAddView();
            IMatchRepository repository = new MatchRepository();
            GuestAddPresenter presenter = new GuestAddPresenter(view);
            List<PlayerInfoDto> guests = new List<PlayerInfoDto>();
            view.ShowForm();
            guests = presenter.getGuestList();
            if (guests == null || guests.Count == 0)
                return;
            _model.PlayerList.AddRange(guests);
            _view.SetPlayerList(_model.PlayerList);
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        private void PlayerRemove(object sender, PlayerButtonEventArgs e)
        {
            string memberName = e.MemberName;
            int memberCode = (int)e.MemberCode;

            var playerToRemove = _model.PlayerList.FirstOrDefault(p => p.MemberCode == memberCode && p.PlayerName == memberName);
            if (playerToRemove != null)
            {
                _model.PlayerList.Remove(playerToRemove);
                if (memberCode != 0)
                    _view.UpdateButtonColor(memberCode, false);
                _view.SetPlayerList(_model.PlayerList);
            }
        }

        private void PlayerAdd(object sender, PlayerButtonEventArgs e)
        {
            string memberName = e.MemberName;
            int memberCode = (int)e.MemberCode;

            if (_model.PlayerList.Any(p => p.MemberCode == memberCode))
            {
                return;
            }

            var member = _model.MemberList.FirstOrDefault(m => m.MemberCode == memberCode);
            if (member == null)
                return;
            _model.PlayerList.Add(new PlayerInfoDto
            {
                MemberCode = member.MemberCode,
                PlayerName = member.PlayerName,
                Gender = member.Gender,
                IsPro = member.IsPro,
                IsSelected = member.IsSelected,
                Handycap = member.Handycap
            });
            _view.UpdateButtonColor(memberCode, true);
            _view.SetPlayerList(_model.PlayerList);
        }
        public void PlayerManageCall(int gamecode)
        {
            _model.MatchCode = gamecode;
            LoadPlayer(gamecode);
            LoadMember();
        }

        private void PlayerUpdate(object sender, EventArgs e)
        {
            try
            {
                _service.PlayerUpdate(_model);
                _view.CloseForm();
            }
            catch (Exception ex) 
            { 
                _view.ShowMessage(ex.Message); 
            }
        }

        private void SearchMember(object sender, EventArgs e)
        {
            _searchModel.SearchWord = _view.SearchWord;
            _searchModel.IncludeSecessMember = _view.SecessMember;
            LoadMember();
        }

        private void LoadMember()
        {
            try
            {
                _model.MemberList = _service.LoadMember(_searchModel, _model.PlayerList);
                _view.SetMemberList(_model.MemberList);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }
        private void LoadPlayer(int code)
        {
            try
            {
                _model.PlayerList = _service.LoadPlayer(code);
                _view.SetPlayerList(_model.PlayerList);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }
    }
}
