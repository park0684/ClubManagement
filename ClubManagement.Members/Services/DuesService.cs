using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.Models;
using ClubManagement.Members.Repositories;
using ClubManagement.Common.Hlepers;

namespace ClubManagement.Members.Services
{
    public class DuesService : IDuesService
    {
        IDuesRepsotiry _repository;

        public DuesService(IDuesRepsotiry repsotiry)
        {
            _repository = repsotiry;
        }

        public DataTable LoadMemberList(DuesModel model)
        {
            var result = _repository.GetMemberList(model);
            result.Columns.Add("No");
            result.Columns.Add("nonPayment");
            result.Columns["mem_code"].ColumnName = "code";
            result.Columns["mem_name"].ColumnName = "name";
            result.Columns["totaldues"].ColumnName = "totalDues";
            int i = 1;
            foreach (DataRow row in result.Rows)
            {
                row["No"] = i++;
                row["name"] = $"{row["name"].ToString().Trim()}({row["mem_birth"].ToString().Trim()})";
                int dueCount = Convert.ToInt32(row["totalDues"]);
                int paycount = Convert.ToInt32(row["payment"]);
                int free = Convert.ToInt32(row["free"]);
                int nonPay = dueCount - paycount - free;
                row["nonPayment"] = nonPay < 0 ? 0 : nonPay;
            }

            //전체 행 추가
            DataRow newRow = result.NewRow();
            newRow["No"] = 0;
            newRow["code"] = 0;
            newRow["name"] = "[전 체]";
            newRow["payment"] = 0;
            newRow["nonPayment"] = 0;
            result.Rows.InsertAt(newRow, 0);

            return result;
        }

        public DataTable LoadStatementList(DuesModel model)
        {
            int balance = 0;
            if (model.MemberCode == 0)
            {
                //설정 기간 이전 잔액 확인
                var resultRow = _repository.GetBalance(model);
                balance = Convert.ToInt32(resultRow["deus"]) - Convert.ToInt32(resultRow["payment"]);
            }
            
            var result = _repository.GetStateList(model);
            //DataTable 컬럼 설정
            result.Columns.Add("No");
            result.Columns.Add("balance");
            result.Columns.Add("type");
            result.Columns["du_code"].ColumnName = "code";
            result.Columns["du_date"].ColumnName = "date";
            result.Columns["du_detail"].ColumnName = "statement";
            result.Columns["du_apply"].ColumnName = "applay";
            result.Columns["deposit"].ColumnName = "deposit";
            result.Columns["whthdrawal"].ColumnName = "withdrawal";
            if (model.MemberCode == 0)
            {
                DataRow newRow = result.NewRow();
                newRow["No"] = 0;
                newRow["balance"] = balance;
                int beforeBalance = balance;
                int i = 1;
                foreach (DataRow row in result.Rows)
                {
                    row["No"] = i++;
                    row["type"] = MemberHelper.GetDuesType(Convert.ToInt32(row["du_type"]));
                    int deposit = Convert.ToInt32(row["deposit"]);
                    int withdrawal = Convert.ToInt32(row["withdrawal"]);
                    beforeBalance = beforeBalance + deposit - withdrawal;
                    row["balance"] = beforeBalance;
                }

                result.Rows.InsertAt(newRow, 0);
            }
            else
            {
                int i = 1;
                foreach (DataRow row in result.Rows)
                {
                    row["No"] = i++;
                }
            }
            return result;
        }
        public StatementModel LoadStatement(int statementCode)
        {
            var result = _repository.LoadStatmet(statementCode);

            return new StatementModel
            {
                StatementCode = statementCode,
                IsNew = false,
                MemberCode = Convert.ToInt32(result["du_memcode"]),
                StatementDate = Convert.ToDateTime(result["du_date"]),
                StatementAmount = Convert.ToInt32(result["du_pay"]),
                DueCount = Convert.ToInt32(result["du_apply"]),
                Memo = result["du_memo"].ToString().Trim(),
                StatementType = Convert.ToInt32(result["du_type"]),
                StatementDetail = result["du_detail"].ToString().Trim(),
                IsWithdrawal = Convert.ToInt32(result["du_type"]) == 1 || Convert.ToInt32(result["du_type"]) == 3 ? false : true
            };
        }

        public void SaveStatement(StatementModel model)
        {
            if (model.IsNew)
                _repository.InsertStatment(model);
            else
                _repository.UpdateStatment(model);
        }
    }
}
