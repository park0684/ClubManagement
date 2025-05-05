using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Members.Models;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.Views;

namespace ClubManagement.Members.Presenters
{
    public class MemberDetailPresenter
    {
        IMemberDetailView _view;
        IMemberRepository _repository;
        MemberModel _model;

        public MemberDetailPresenter(IMemberDetailView view, IMemberRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new MemberModel();
            this._view.SaveEvent += MemberInfoSave;
            this._view.CloseFormEvent += CloseFome;
            this._model.IsNew = true;
        }

        /// <summary>
        /// 닫기 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseFome(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        /// <summary>
        /// 저장 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberInfoSave(object sender, EventArgs e)
        {
            bool isError = ValidateInput();
            if (isError)
                return;

            _model.MemberName = _view.MemberName;
            _model.Status = _view.Status ?? 0;
            _model.Gender = _view.Gender ?? 0;
            _model.Position = _view.Position ?? 0;
            _model.Birth = _view.Birth;
            _model.AccessDate = (DateTime)_view.AccessDate;
            _model.SecessDate = (DateTime)_view.SecessDate;
            _model.Memo = _view.Memo;
            try
            {
                if (_model.IsNew)
                {
                    _repository.InsertMember(_model);
                }
                else
                {
                    _repository.UpdateMember(_model);
                }
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 조호된 회원 더블 클릭 시 회원코드를 넘겨 받아 회원 정보 조회
        /// </summary>
        /// <param name="memCode"></param>
        public void GetMemberInfo(int memCode)
        {
            DataRow row = _repository.LoadMemberInfo(memCode);

            _model.IsNew = false;
            _model.Code = memCode;
            _model.MemberName = row["mem_name"].ToString().Trim();
            _model.Status = Convert.ToInt32(row["mem_status"]);
            _model.Gender = Convert.ToInt32(row["mem_gender"]);
            _model.Position = Convert.ToInt32(row["mem_position"]);
            _model.Birth = row["mem_birth"].ToString().Trim();
            _model.Memo = row["mem_memo"].ToString().Trim();
            _model.AccessDate = (DateTime)row["mem_access"];
            _model.SecessDate = row["mem_secess"] == DBNull.Value ? DateTime.Now : (DateTime)row["mem_secess"];

            _view.MemberName = _model.MemberName;
            _view.Gender = _model.Gender;
            _view.Birth = _model.Birth;
            _view.Position = _model.Position;
            _view.Status = _model.Status;
            _view.IsPro = _model.IsPro;
            _view.AccessDate = _model.AccessDate;
            _view.SecessDate = _model.SecessDate;
            _view.Memo = _model.Memo;
        }

        /// <summary>
        /// 저장 이벤트 실행 시 미입력 여부 확인
        /// </summary>
        /// <returns></returns>
        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(_view.MemberName))
            {
                _view.ShowMessage("회원명을 입력하세요");
                return true;
            }
            if (string.IsNullOrEmpty(_view.Birth))
            {
                _view.ShowMessage("생년을 입력하세요");
                return true;
            }
            return false;
        }
    }
}
