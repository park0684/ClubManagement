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
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public partial class RecordBoardRegistView : Form, IRecordBoardRegistView
    {
        public RecordBoardRegistView()
        {
            InitializeComponent();
            ViewEvent();
            flpOrderList.AutoScroll = true;
            GameOrderList = new List<GameOrderDto>();
        }

        private void ViewEvent()
        {
            btnOrderAdd.Click += (s, e) => AddOrderEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveOrderEvent?.Invoke(this, EventArgs.Empty);
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);
            btnEditPlayer.Click += (s, e) => EditPlayerEvent?.Invoke(this, EventArgs.Empty);
            btnGameSearch.Click += (s, e) => SelectGameEvent?.Invoke(this, EventArgs.Empty);
        }
        private Panel CreateOrderPanel(GameOrderDto order)
        {
            Panel panel = new Panel
            {
                Width = flpOrderList.Width - 20,
                Height = 50,
                BorderStyle = BorderStyle.FixedSingle
            };

            // 순번 Label (패널 추가 시 리스트 순서대로 자동 적용)
            Label lblOrderSeq = new Label
            {
                Text = order.GameSeq.ToString(),
                Width = 30,
                Location = new Point(5, 15)
            };

            // 게임 타입 ComboBox
            ComboBox cmbOrderType = new ComboBox
            {
                Width = 100,
                Location = new Point(40, 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var item in GameHelper.GameType)
            {
                cmbOrderType.Items.Add(new KeyValuePair<int, string>(item.Key, item.Value));
            }
            
            cmbOrderType.DisplayMember = "Value";
            cmbOrderType.ValueMember = "Key";
            cmbOrderType.SelectedItem = cmbOrderType.Items.Cast<KeyValuePair<int, string>>().FirstOrDefault(item => item.Key == order.GameType);
           
            // 참가조 Label
            Label lblCounter = new Label
            {
                Text = "참가인원 / 팀 : ",
                Width = 90,
                Location = new Point(150,15)
            };
            // 단체전 참가조 ComboBox
            ComboBox cmbPlayerCount = new ComboBox
            {
                Width = 60,
                Location = new Point(240, 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            for (int i = 1; i <= 30; i++)
            {
                cmbPlayerCount.Items.Add(i);
            }
            cmbPlayerCount.SelectedItem = order.PlayerCount;
            cmbPlayerCount.SelectedIndexChanged += (s, e) =>
            {
                order.PlayerCount = int.Parse(cmbPlayerCount.SelectedItem.ToString());
            };

            // 삭제 버튼
            Button btnRemove = new Button
            {
                Text = "삭제",
                Width = 60,
                Location = new Point(320, 10)
            };
            btnRemove.Click += (s, e) => RemoveOrderPanel(panel);

            // 컨트롤 추가
            panel.Controls.Add(lblOrderSeq);
            panel.Controls.Add(cmbOrderType);
            panel.Controls.Add(lblCounter);
            panel.Controls.Add(cmbPlayerCount);
            panel.Controls.Add(btnRemove);

            if (order.GameType == 1)
            {
                cmbPlayerCount.Enabled = false;
                cmbPlayerCount.SelectedItem = flpPlayerList.Controls.Count;
            }
                

            cmbOrderType.SelectedIndexChanged += (s, e) =>
            {
                order.GameType = ((KeyValuePair<int, string>)cmbOrderType.SelectedItem).Key;
                int playerCount = flpPlayerList.Controls.Count;
                int groupCount = 0;
                if (order.GameType == 1)
                {
                    cmbPlayerCount.Enabled = false;
                    cmbPlayerCount.SelectedItem = playerCount;
                }
                else
                {

                    groupCount = SetGroupCount(playerCount);

                    cmbPlayerCount.Enabled = true;
                    cmbPlayerCount.SelectedItem = groupCount;
                    
                }
            };
            return panel;
        }
        public void AddNewOrder(GameOrderDto newOrder)
        {
            GameOrderList.Add(newOrder);
            flpOrderList.Controls.Add(CreateOrderPanel(newOrder));
            UpdateOrderSequence(); // 순번 업데이트
        }
        private int SetGroupCount(int playerCount)
        {
            for (int i = 2; i <= playerCount; i++)
            {
                if (playerCount % i == 0)
                {
                    if( i == 2)
                    {
                        //i = 2;
                    }
                    else if (i == playerCount)
                    {
                        playerCount++;
                        i = 1;
                    }
                    else
                    {
                        return i;
                    }
                }
            }
            return playerCount;
        }
        private void RemoveOrderPanel(Panel panel)
        {
            flpOrderList.Controls.Remove(panel);
            UpdateOrderSequence(); // 순번 업데이트
        }
        private void UpdateOrderSequence()
        {
            GameOrderList.Clear();
            var panels = flpOrderList.Controls.OfType<Panel>().ToList();
            for (int i = 0; i < panels.Count; i++)
            {
                var lblOrderSeq = panels[i].Controls.OfType<Label>().FirstOrDefault() as Label;
                var orderType = panels[i].Controls.Find("cmbOrderType", true).FirstOrDefault() as ComboBox;
                var playerCount = panels[i].Controls.Find("cmbPlayerCount", true).FirstOrDefault() as ComboBox;
                if (lblOrderSeq != null)
                {
                    lblOrderSeq.Text = (i + 1).ToString(); // 리스트 인덱스 + 1
                }
                GameOrderDto order = new GameOrderDto
                {
                    GameSeq = Convert.ToInt32(lblOrderSeq.Text),
                    GameType = Convert.ToInt32(orderType?.SelectedItem?.ToString()),
                    PlayerCount = Convert.ToInt32(playerCount?.SelectedItem?.ToString())
                };
                GameOrderList.Add(order);
            }
        }

        public string MatchTitle
        {
            get { return lblMatchTitle.Text; }
            set { lblMatchTitle.Text = value; }
        }

        public int OrderCount
        {
            get { return GameOrderList.Count; }
            set { lblCounter.Text = value.ToString(); }
        }

        public List<GameOrderDto> GameOrderList { get; set; }

        public List<PlayerInfoDto> PlayerList
        {
            get; set;
        }



        public event EventHandler AddOrderEvent;
        public event EventHandler SaveOrderEvent;
        public event EventHandler CloseFormEvent;
        public event EventHandler SelectGameEvent;
        public event EventHandler EditPlayerEvent;

        public void LoadOrder(List<GameOrderDto> orders)
        {
            flpOrderList.Controls.Clear();
            GameOrderList = orders;
            if (GameOrderList.Count == 0 )
                return;
            foreach (var order in GameOrderList)
            {
                flpOrderList.Controls.Add(CreateOrderPanel(order));
            }
            //UpdateOrderSequence();

        }

        public void LoadPlayer(List<PlayerInfoDto> players)
        {
            flpPlayerList.Controls.Clear();
            flpPlayerList.AutoScroll = true;
            foreach (var player in players)
            {
                Button btn = new Button
                {
                    Size = new Size(60, 40),
                    FlatStyle = FlatStyle.System,
                    Text = player.PlayerName,
                    Tag = player.MemberCode,
                    BackColor = Color.FromArgb(54, 178, 221)
                };
                flpPlayerList.Controls.Add(btn);
            }
        }

        public void SearchGame()
        {
            throw new NotImplementedException();
        }

        public void CloseForm()
        {
            this.Close();
        }

        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        public void ShowMessge(string message)
        {
            MessageBox.Show(message, "알림");
        }

        public void SetMatchButton()
        {
            btnGameSearch.Enabled = false;
        }
    }
}
