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

        /// <summary>
        /// 개인 사이드 세트를 설정하고 DB에 저장.
        /// 1등 상금은 필수, 2등과 3등은 금액 입력 시에만 추가.
        /// </summary>
        private void SetIndividaul(object sender, EventArgs e)
        {
            // 1등 상금이 입력되지 않은 경우 사용자 알림 후 종료
            if (_view.Prize1st.GetValueOrDefault() == 0)
            {
                _view.ShowMessage("입력한 금액을 확인해 주세요");
                return;
            }

            // 기존 데이터 초기화
            _model.IndividaulSideSet.Clear();

            // 1등 세트 생성 및 추가
            IndividaulSetDto First = new IndividaulSetDto
            {
                Rank = 1,
                Prize = (int)_view.Prize1st,
                Handi = (int)_view.Handi1st,
            };
            _model.IndividaulSideSet.Add(First);

            // 2등 금액이 입력된 경우 2등 세트 생성 및 추가
            if (_view.Prize2nd.GetValueOrDefault() != 0)
            {
                IndividaulSetDto Second = new IndividaulSetDto
                {
                    Rank = 2,
                    Prize = (int)_view.Prize2nd,
                    Handi = (int)_view.Handi2nd,
                };
                _model.IndividaulSideSet.Add(Second);

                // 3등 금액이 입력된 경우 3등 세트 생성 및 추가
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
                // DB에 개인 사이드 세트 정보 저장
                _repository.UpdateIndividualSet(_model);

                // 저장 후 폼 닫기
                _view.CloseForm();
            }
            catch (Exception ex)
            {
                // 예외 발생 시 사용자에게 메시지 표시
                _view.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 폼 종료 처리.
        /// </summary>
        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        /// <summary>
        /// DB에서 개인 사이드 세트 정보를 로드하고 뷰와 모델에 반영.
        /// </summary>
        private void LoadIndividualSet()
        {
            // DB에서 데이터 로드
            DataTable resultTable = _repository.LoadIndividualSideSet(_model.MatchCode);

            if (resultTable.Rows.Count == 0)
            {
                // 데이터가 없으면 뷰 초기화
                _view.Prize1st = 0;
                _view.Prize2nd = 0;
                _view.Prize3rd = 0;
                _view.Handi1st = 0;
                _view.Handi2nd = 0;
                _view.Handi3rd = 0;
            }
            else
            {
                // 로드된 각 세트 데이터를 모델과 뷰에 반영
                foreach (DataRow row in resultTable.Rows)
                {
                    IndividaulSetDto setter = new IndividaulSetDto
                    {
                        Rank = (int)row["ind_rank"],
                        Prize = (int)row["ind_prize"],
                        Handi = (int)row["ind_handi"]
                    };
                    _model.IndividaulSideSet.Add(setter);

                    // 뷰에 Rank에 따라 값 반영
                    switch (setter.Rank)
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
