using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.Views;
using ClubManagement.Games.Repositories;

namespace ClubManagement.Games.Presenters
{
    public class NumericPadPresenter
    {
        INumericPadView _view;
        Action<int> _onNumberEntered;

        public NumericPadPresenter(INumericPadView view, Action<int> onNumericEntered, int initialNumber)
        {
            _view = view;
            _onNumberEntered = onNumericEntered;
            _view.Nember = initialNumber;

            _view.InsertNumberEvent += OnInsertNumber;
            _view.CloseFormEvent += CloseForm;
        }

        /// <summary>
        /// 폼을 닫는 이벤트 처리.
        /// </summary>
        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        /// <summary>
        /// 번호 입력 완료 시 호출.
        /// 번호 입력 이벤트를 발생시키고 폼을 닫음.
        /// </summary>
        private void OnInsertNumber(object sender, EventArgs e)
        {
            // 번호 입력 이벤트(_onNumberEntered)가 등록되어 있으면 호출
            _onNumberEntered?.Invoke(_view.Nember);

            // 입력 완료 후 폼 닫기
            _view.CloseForm();
        }
    }
}
