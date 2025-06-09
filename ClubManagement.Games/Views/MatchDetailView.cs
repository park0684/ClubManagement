using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Common.Hlepers;

namespace ClubManagement.Games.Views
{
    public partial class MatchDetailView : Form,IMatchDetailView
    {
        public MatchDetailView()
        {
            InitializeComponent();
            InitializeComboBox();
            InitializeControlBox();
            ViewEvent();
            this.Text = "모임 상세 내역";
        }

        public string GameTitle
        {
            get { return txtTitle.Text; }
            set { txtTitle.Text = value; }
        }

        public string GameHost
        {
            get { return txtHost.Text; }
            set { txtHost.Text = value; }
        }

        public string GameMemo
        {
            get { return txtMemo.Text; }
            set { txtMemo.Text = value; }
        }

        public DateTime GameDate
        {
            get { return dtpGameDate.Value; }
            set { dtpGameDate.Value = value; }
        }

        public int GameType
        {
            get { return ((KeyValuePair<int, string>)cmbGameType.SelectedItem).Key; }
            set
            {
                int index = cmbGameType.Items.Cast<KeyValuePair<int, string>>().ToList().FindIndex(item => item.Key == value);
                if (index >= 0)
                {
                    cmbGameType.SelectedIndex = index;
                }
            }
        }

        public event EventHandler SaveEvent;
        public event EventHandler CloseEvenvt;

        /// <summary>
        /// 주요 버튼과 콤보박스 이벤트를 바인딩.
        /// </summary>
        private void ViewEvent()
        {
            // 게임 타입 콤보박스 선택 변경 시 → HostTextBoxEvent 호출
            cmbGameType.SelectedIndexChanged += HostTextBoxEvent;

            // 닫기 버튼 클릭 → CloseEvenvt 발생
            btnClose.Click += (s, e) => CloseEvenvt?.Invoke(this, EventArgs.Empty);

            // 저장 버튼 클릭 → SaveEvent 발생
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 게임 타입 콤보박스 초기화.
        /// MatchType 딕셔너리 데이터를 추가.
        /// </summary>
        private void InitializeComboBox()
        {
            foreach (var item in GameHelper.MatchType)
            {
                // Key: int 코드, Value: 표시 문자열
                cmbGameType.Items.Add(new KeyValuePair<int, string>(item.Key, item.Value));
            }

            // 콤보박스 표시와 값 설정
            cmbGameType.DisplayMember = "Value";
            cmbGameType.ValueMember = "Key";
            cmbGameType.DropDownStyle = ComboBoxStyle.DropDownList;

            // 기본 선택 인덱스 (1번째)
            cmbGameType.SelectedIndex = 1;
        }

        /// <summary>
        /// 폼의 ControlBox 및 크기 관련 속성을 초기화.
        /// </summary>
        private void InitializeControlBox()
        {
            ControlBox = false;                     // 상단 ControlBox 제거
            FormBorderStyle = FormBorderStyle.FixedSingle; // 크기 고정
            this.MaximizeBox = false;               // 최대화 버튼 제거
            this.MinimizeBox = false;               // 최소화 버튼 제거
        }

        /// <summary>
        /// 게임 타입이 '정기전' 선택 시 Host 텍스트 자동 입력.
        /// </summary>
        private void HostTextBoxEvent(object sender, EventArgs e)
        {
            int typeIndex = ((KeyValuePair<int, string>)cmbGameType.SelectedItem).Key;

            if (typeIndex == 1)
            {
                txtHost.Text = "정기전";
            }
        }

        /// <summary>
        /// 폼 종료.
        /// </summary>
        public void CloseForm()
        {
            this.Close();
        }

        /// <summary>
        /// 폼을 화면 중앙에서 모달 다이얼로그로 표시.
        /// </summary>
        public void ShowForm()
        {
            Form form = (Form)this;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        /// <summary>
        /// 메시지 박스를 표시.
        /// </summary>
        /// <param name="message">표시할 메시지</param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        
    }
}
