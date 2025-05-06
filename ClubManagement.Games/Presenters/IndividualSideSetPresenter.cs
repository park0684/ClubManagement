using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class IndividualSideSetPresenter
    {
        IIndividualSideSetView _view;
        IRecordBoardRepository _repository;
        RecordBoardModel _model;

        public IndividualSideSetPresenter(IIndividualSideSetView view, IRecordBoardRepository repository, RecordBoardModel model)
        {
            _view = view;
            _repository = repository;
            _model = model;
            _view.CloseEvent += CloseForm;
            _view.SaveEvent += SetIndividaul;

            LoadIndividualSet();
        }

        private void SetIndividaul(object sender, EventArgs e)
        {
            if (_view.Prize1st.GetValueOrDefault() == 0)
            {
                _view.ShowMessage("입력한 금액을 확인해 주세요"); 
                return;
            }
            _model.IndividaulSideSet.Clear();
            IndividaulSetDto First = new IndividaulSetDto
            {
                Rank = 1,
                Prize = (int)_view.Prize1st,
                Handi = (int)_view.Handi1st,
            };
            _model.IndividaulSideSet.Add(First);
            if (_view.Prize2nd.GetValueOrDefault() != 0)
            {
                IndividaulSetDto Second = new IndividaulSetDto
                {
                    Rank = 2,
                    Prize = (int)_view.Prize2nd,
                    Handi = (int)_view.Handi2nd,
                };
                _model.IndividaulSideSet.Add(Second); ;
                if (_view.Prize3rd.GetValueOrDefault() != 0)
                {
                    IndividaulSetDto therd = new IndividaulSetDto
                    {
                        Rank = 3,
                        Prize = (int)_view.Prize3rd,
                        Handi = (int)_view.Handi3rd,
                    };
                    _model.IndividaulSideSet.Add(therd);
                }
            }

            try
            {
                _repository.UpdateIndividualSet(_model);
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
        private void LoadIndividualSet()
        {
            DataTable resultTable = new DataTable();
            resultTable = _repository.LoadIndividualSideSet(_model.MatchCode);
            if( resultTable.Rows.Count == 0)
            {
                _view.Prize1st = 0;
                _view.Prize2nd = 0;
                _view.Prize3rd = 0;
                _view.Handi1st = 0;
                _view.Handi2nd = 0;
                _view.Handi3rd = 0;

            }
            else
            {
                
                foreach(DataRow row in resultTable.Rows)
                {
                    IndividaulSetDto setter = new IndividaulSetDto
                    {
                        Rank = (int)row["ind_rank"],
                        Prize = (int)row["ind_prize"],
                        Handi = (int)row["ind_handi"]
                    };
                    _model.IndividaulSideSet.Add(setter);
                    switch(setter.Rank)
                    {
                        case 1:
                            _view.Prize1st = setter.Prize;
                            _view.Handi1st = setter.Handi;
                            break;
                        case 2:
                            _view.Prize2nd = setter.Prize;
                            _view.Handi2nd = setter.Handi;
                            break;
                        case 3:
                            _view.Prize3rd = setter.Prize;
                            _view.Handi3rd = setter.Handi;
                            break;
                    }
                }
            }
        }
    }
}
