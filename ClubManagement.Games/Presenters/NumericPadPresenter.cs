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

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        private void OnInsertNumber(object sender, EventArgs e)
        {
            _onNumberEntered?.Invoke(_view.Nember);
            _view.CloseForm();
        }
    }
}
