using LiveCharts;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using System.Reflection;

namespace MountainFinance
{
    public partial class FormMain : Form
    {
        public string startDate = "2021-01-01";
        public string endDate = DateTime.Now.ToString("yyyy-mm-dd");
        public string lastStartDate = "";
        public string lastEndDate = "";
        public FormExpenseAdd formAdd;
        public Panel activePanel;
        private DataManager dm = DataManager.GetSingleton();
        private LoginManager lm = LoginManager.GetSingleton();
        private ExcelManager em = new ExcelManager();

        public FormMain()
        {
            InitializeComponent();
            activePanel = panelDashboard;
        }

        public void PanelHandler(Panel visiblePanel, Panel PanelButton)
        {
            List<Panel> panels = new List<Panel> { panelDashboard, panelAccounting, panelReports, panelSaving, panelReports };
            List<Panel> panelButtons = new List<Panel> {panelButtonDashboard,panelButtonAccounting, panelButtonSavings,
                                                                                 panelButtonInvestment, panelButtonReports };
            if (activePanel != visiblePanel)
            {
                foreach (var item in panels)
                {
                    item.Visible = false;
                    item.SuspendLayout();
                }

                visiblePanel.Visible = true;
                visiblePanel.ResumeLayout();
                panelControl.BringToFront();
                foreach (var item in panelButtons)
                {
                    item.BackColor = Color.FromArgb(192, 192, 255);
                }
                PanelButton.BackColor = Color.White;

                activePanel = visiblePanel;
            }

        }



        //---------------------Accounting--------------------------------
        private void panelButtonAccounting_Click(object sender, EventArgs e)
        {
            PanelHandler(panelAccounting, panelButtonAccounting);
            this.dataGridViewExpenses.DataSource = dm.SqlGetData(startDate, endDate, 1);
            dataGridViewExpenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewExpenses.AllowUserToAddRows = false;
            this.dataGridViewIncome.DataSource = dm.SqlGetData(startDate, endDate, 2);
            dataGridViewIncome.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewIncome.AllowUserToAddRows = false;
        }


        private void buttonAddExpense_Click(object sender, EventArgs e)
        {
            FormExpenseAdd formAdd = new FormExpenseAdd();
            formAdd.ShowDialog();
            this.dataGridViewExpenses.DataSource = dm.SqlGetData(startDate, endDate, 1);
        }

        private void buttonAddIncome_Click(object sender, EventArgs e)
        {
            FormIncomeAdd formAdd = new FormIncomeAdd();
            formAdd.ShowDialog();
            this.dataGridViewIncome.DataSource = dm.SqlGetData(startDate, endDate, 2);
        }

        private void buttonDeleteExpense_Click(object sender, EventArgs e)
        {
            DeleteTransaction(dataGridViewExpenses);

        }

        private void buttonDeleteIncome_Click(object sender, EventArgs e)
        {
            DeleteTransaction(dataGridViewIncome);
        }

        private void DeleteTransaction(DataGridView datagrid)
        {

            foreach (DataGridViewRow row in datagrid.SelectedRows)
            {
                int id = Int32.Parse(row.Cells[0].Value.ToString());
                datagrid.Rows.RemoveAt(row.Index);
                MessageBox.Show($"Transaction with ID {id} has been deleted");
                datagrid.Refresh();
                dm.SqlDelete(id, "transactions");
            }
        }

        //---------------------Savings--------------------------------

        private void panelButtonSavings_Click(object sender, EventArgs e)
        {
            PanelHandler(panelSaving, panelButtonSavings);
            RefreshPanelSaving();

        }

        private void buttonAddSaving_Click(object sender, EventArgs e)
        {
            FormAddSaving formAdd = new FormAddSaving();
            formAdd.ShowDialog();
            RefreshPanelSaving();
        }

        private void buttonRemoveSaving_Click(object sender, EventArgs e)
        {
            FormRemoveSaving formAdd = new FormRemoveSaving();
            formAdd.ShowDialog();
            RefreshPanelSaving();
        }

        private void buttonGoalSet_Click(object sender, EventArgs e)
        {
            FormSetSavingGoal formAdd = new FormSetSavingGoal();
            formAdd.ShowDialog();
            this.labelGoalSetInfo.Text = dm.SqlGetSavingGoal();
            RefreshPanelSaving();
        }

        private void ChartSavingLoad()
        {
            try
            {
                // create series and populate them with data
                var series1 = new LiveCharts.Wpf.LineSeries()
                {
                    Title = "Savings",
                    Values = new LiveCharts.ChartValues<int>(dm.ChartDataintFlow($"with saving_s as (select  sum (amount) as s, s.date as d from dbo.saving s where s.date between '{startDate}' and '{endDate}' group by s.date)," +
                                                                                $" transactions_s as (select  sum(amount) as t1, userId, t.date as d from dbo.transactions t where type = 'income' and t.date  between '{startDate}' and '{endDate}' group by t.date, userId)" +
                                                                                " select case when s.d is null then 0 else s.s end as saving_amount from transactions_s t full outer join saving_s s on s.d = t.d ")),
                };


                var values = new LiveCharts.ChartValues<int>();
                int length = (dm.ChartDataintFlow($"with saving_s as (select sum (amount) as s, s.date as d from dbo.saving s where s.date between '{startDate}' and '{endDate}' group by s.date)," +
                                                                                $" transactions_s as (select sum(amount) as t1, userId, t.date as d from dbo.transactions t where type = 'income' and t.date  between '{startDate}' and '{endDate}' group by t.date, userId)" +
                                                                                " select case when (select s0.s from saving_s s0 where s0.d = (SELECT TOP 1 s1.d FROM saving_s s1 where s1.d < t.d and s1.d is not null  ORDER BY s1.d desc)) is null then 0 " +
                                                                                " when s.d is null then (select s0.s from saving_s s0 where s0.d = (SELECT TOP 1 s1.d FROM saving_s s1 where s1.d < t.d and s1.d is not null  ORDER BY s1.d desc)) " +
                                                                                "else s.s end as saving_amount from transactions_s t full outer join saving_s s on s.d = t.d ").Count());
                for (int v = 0; v < length; v++)
                {
                    values.Add(Convert.ToInt32(dm.SqlGetSavingGoal()));
                }
                var seriesGoal = new LiveCharts.Wpf.LineSeries()
                {
                    Title = "Goal",

                    Values = new LiveCharts.ChartValues<int>(values),
                };

                var values1 = new LiveCharts.ChartValues<int>();
                for (int v = 0; v < length; v++)
                {

                    int c = (DateTime.Now.Year - DateTime.Parse(startDate).Year) * 12 + DateTime.Now.Month - DateTime.Parse(startDate).Month;
                    if (c == 0)
                    {
                        c = 1;
                    }
                    values1.Add(Int32.Parse(dm.SqlGetDataSum(startDate, endDate, "saving"))
                                                        / c);
                }
                var seriesAvg = new LiveCharts.Wpf.LineSeries()
                {
                    Title = "Average",

                    Values = new LiveCharts.ChartValues<int>(values1),
                };



                var series2 = new LiveCharts.Wpf.LineSeries()
                {
                    Title = "Income",
                    Values = new LiveCharts.ChartValues<int>(dm.ChartDataintFlow($"with saving_s as (select  sum (amount) as s, s.date as d from dbo.saving s where s.date between '{startDate}' and '{endDate}' group by s.date), " +
                                                                                $"transactions_s as ((select  userId, sum(amount) as t1, t.date as d from dbo.transactions t where type = 'income' and  t.date between '{startDate}' and '{endDate}' group by t.date,userId)) " +
                                                                                "select case when(select t0.t1 from transactions_s t0 where t0.d = (SELECT  TOP 1 t1.d   FROM transactions_s t1  where t1.d < s.d and t1.d is not null    ORDER BY t1.d desc)) is null and t.d is null then 0 when t.d is null then 0 else t.t1 end as in_amount  from transactions_s t full outer join saving_s s on s.d = t.d ")),
                };


                this.chartSaving.Series.Clear();
                this.chartSaving.Series.Add(series1);
                this.chartSaving.Series.Add(seriesGoal);
                this.chartSaving.Series.Add(seriesAvg);
                this.chartSaving.Series.Add(series2);

                this.chartSaving.AxisX.Clear();
                this.chartSaving.AxisX.Add(new LiveCharts.Wpf.Axis
                {
                    Title = "Date",
                    Labels = dm.ChartDatastring($"with saving_s as (select  sum (amount) as s, s.date as d from dbo.saving s where s.date between '{startDate}' and '{endDate}' group by s.date), " +
                                                 $"transactions_s as ((select  sum(amount) as t1, userId, t.date as d from dbo.transactions t where t.date between '{startDate}' and '{endDate}' and type = 'income' group by t.date, userId)) " +
                                                 "select case when t.d is null then s.d else t.d end as d from transactions_s t full outer join saving_s s on s.d = t.d "),
                });

                chartSaving.LegendLocation = LegendLocation.Bottom;
            }
            catch (FormatException e)
            {
                MessageBox.Show($"Add data to be able to show them! {e}");
            }

        }

        private void RefreshPanelSaving()
        {
            try
            {
                int c = (DateTime.Now.Year - DateTime.Parse(startDate).Year) * 12 + DateTime.Now.Month - DateTime.Parse(startDate).Month;
                if (c <= 0)
                {
                    c = 1;
                }
                this.labelSavingsSetInfo.Text = dm.SqlGetDataSum(startDate, endDate, "saving");
                this.labelGoalSetInfo.Text = dm.SqlGetSavingGoal();
                this.labelAvgSavingInfo.Text = Convert.ToString(Int32.Parse(dm.SqlGetDataSum(startDate, endDate, "saving"))
                                                        / c);
                this.labelEarningRatioInfo.Text = Convert.ToString(Math.Round(
                                                        (Double.Parse(dm.SqlGetDataSum(startDate, endDate, "saving")) /
                                                         Double.Parse(dm.SqlGetDataSum(startDate, endDate, "income"))) * 100)) + "%";
            }
            catch (FormatException e)
            {
                MessageBox.Show($"{e}");
            }
            ChartSavingLoad();


        }



        //---------------------Dashboard--------------------------------
        private void IntervalUpdate()
        {

            if (lastStartDate != this.datetimePickerFrom.Value.ToString("yyyy-MM-dd"))
            {
                lastStartDate = this.datetimePickerFrom.Value.ToString("yyyy-MM-dd");
            }
            if (lastStartDate != this.datetimePickerTo.Value.ToString("yyyy-MM-dd"))
            {
                lastEndDate = this.datetimePickerTo.Value.ToString("yyyy-MM-dd");
            }

            startDate = lastStartDate;
            endDate = lastEndDate;

        }
        private void panelDashboard_Click(object sender, EventArgs e)
        {
            PanelHandler(panelDashboard, panelButtonDashboard);
            ChartDashboardLoad();
            IntervalUpdate();
            InfoLabelDashboardLoad();

        }


        private void datetimePickerFrom_ValueChanged(object sender, EventArgs e)
        {

        }
        private void datetimePickerTo_ValueChanged(object sender, EventArgs e)
        {
        }

        private void buttonApplyInterval_Click(object sender, EventArgs e)
        {
            IntervalUpdate();
            ChartDashboardLoad();
            InfoLabelDashboardLoad();
        }

        private void ChartDashboardLoad()
        {
            try
            {
                // create series and populate them with data
                var seriesSaving = new LiveCharts.Wpf.LineSeries()
                {
                    Title = "Savings",
                    Values = new LiveCharts.ChartValues<int>(dm.ChartDataintFlow($"with saving_s as (select  sum(amount) as s, userId, s.date as d from dbo.saving s where s.date between '{startDate}' and '{endDate}' group by s.date, userId), " +
                                                                                $"transactions_s as ((select sum(amount) as t1, userId, t.date as d from dbo.transactions t where  t.date between '{startDate}' and '{endDate}'  group by t.date,userId))" +
                                                                                "select case when s.d is null then 0 else s.s end as saving_amount" +
                                                                                " from transactions_s t full outer join saving_s s on s.d = t.d")),
                };

                var seriesIncome = new LiveCharts.Wpf.LineSeries()
                {
                    Title = "Income",
                    Values = new LiveCharts.ChartValues<int>(dm.ChartDataintFlow($"with saving_s as (select  sum(amount) as s, s.date as d from dbo.saving s where s.date between '{startDate}' and '{endDate}' group by s.date), " +
                                                                              $"transactions_s as ((select  sum(amount) as t1, userId, t.date as d from dbo.transactions t where type = 'income' and t.date between '{startDate}' and '{endDate}' group by t.date,userId))," +
                                                                              $"transactions_e as (  (select  sum(amount) as e  , userId, t.date as d  from dbo.transactions t  where type = 'expense' and t.date between '{startDate}' and '{endDate}' group by t.date,userId)) " +
                                                                              " select case when(select t0.t1 from transactions_s t0 where t0.d = (SELECT  TOP 1 t1.d   FROM transactions_s t1  where t1.d < s.d and t1.d is not null    ORDER BY t1.d desc)) is null and t.d is null then 0 when t.d is null then 0 else t.t1 end as in_amount  " +
                                                                              "from transactions_s t full outer join transactions_e e on e.d =t.d full outer join saving_s s on s.d = t.d or s.d = e.d ")),
                };

                var seriesExpense = new LiveCharts.Wpf.LineSeries()
                {
                    Title = "Expense",
                    Values = new LiveCharts.ChartValues<int>(dm.ChartDataintFlow($"with saving_s as (select   sum (amount) as s,s.date as d from dbo.saving s where s.date between '{startDate}' and '{endDate}'   group by s.date)," +
                                                                             $"transactions_i as ( (select  sum(amount) as i , userId,t.date as d from dbo.transactions t where type = 'income' and t.date between '{startDate}' and '{endDate}' group by t.date,userId)), " +
                                                                             $"transactions_e as (  (select  sum(amount) as e  , userId, t.date as d  from dbo.transactions t  where type = 'expense' and t.date between '{startDate}' and '{endDate}' group by t.date,userId)) " +
                                                                             "select  case   when (select e0.e from transactions_e e0 where e0.d = (SELECT   TOP 1 e.d  FROM transactions_e e1 where e1.d<s.d and e1.d is not null  ORDER BY e1.d desc)) is null and  e.d  is null then 0   when e.d is null then 0 else e.e end as expense " +
                                                                             "from transactions_i i full outer join transactions_e e on e.d = i.d full outer join saving_s s on s.d = i.d or s.d = e.d order by i.d")),
                };


                var seriesBalance = new LiveCharts.Wpf.LineSeries()
                {
                    Title = "Balance",
                    Values = new LiveCharts.ChartValues<int>(dm.ChartDataintFlow($"with saving_s as (select sum (amount) as s,s.date as d from dbo.saving s where s.date between '{startDate}' and '{endDate}'   group by s.date)," +
                                                                             $"transactions_i as ( (select sum(amount) as i , userId, t.date as d from dbo.transactions t where type = 'income' and t.date between '{startDate}' and '{endDate}' group by t.date,userId)), " +
                                                                             $"transactions_e as (  (select  sum(amount) as e  , userId, t.date as d  from dbo.transactions t  where type = 'expense' and t.date between '{startDate}' and '{endDate}' group by t.date,userId)) " +
                                                                             $"select  (case    when (select i0.i from transactions_i i0 where i0.d = (SELECT  TOP 1 i.d  FROM transactions_i i1  where i1.d<s.d and i1.d is not null   ORDER BY i1.d desc)) is null and  i.d  is null then 0 when i.d is null then (select i0.i from transactions_i i0 where i0.d = (SELECT  TOP 1 i.d  FROM transactions_i i1  where i1.d<s.d and i1.d is not null  ORDER BY i1.d desc)) else i.i end ) " +
                                                                             $"- ( case   when (select e0.e from transactions_e e0 where e0.d = (SELECT   TOP 1 e.d  FROM transactions_e e1 where e1.d<s.d and e1.d is not null  ORDER BY e1.d desc)) is null and  e.d  is null then 0   when e.d is null then  (select e0.e from transactions_e e0 where e0.d = (SELECT TOP 1 e.d  FROM transactions_e e1   where e1.d<s.d and e1.d is not null ORDER BY e1.d desc)) else e.e end) as balance_value from transactions_i i full outer join transactions_e e on e.d = i.d full outer join saving_s s on s.d = i.d or s.d = e.d order by i.d")),

                };

                this.chartDashboard.Series.Clear();
                this.chartDashboard.Series.Add(seriesBalance);
                this.chartDashboard.Series.Add(seriesExpense);
                this.chartDashboard.Series.Add(seriesIncome);
                this.chartDashboard.Series.Add(seriesSaving);

                this.chartDashboard.AxisX.Clear();
                this.chartDashboard.AxisX.Add(new LiveCharts.Wpf.Axis
                {
                    Title = "Date",
                    Labels = dm.ChartDatastring($"with saving_s as (select sum (amount) as s, s.date as d from dbo.saving s where s.date between '{startDate}' and '{endDate}'  group by s.date), " +
                                                $"transactions_s as ((select  sum(amount) as t1, userId, t.date as d from dbo.transactions t where type = 'income' and t.date between '{startDate}' and '{endDate}' group by t.date,userId)) " +
                                                "select case when t.d is null then s.d else t.d end as d from transactions_s t full outer join saving_s s on s.d = t.d "),
                });

                chartSaving.LegendLocation = LegendLocation.Bottom;
            }
            catch (FormatException e)
            {
                MessageBox.Show($"Add data to be able to show them!");
            }

        }

        private void InfoLabelDashboardLoad() {
            try
            {
                labelIncomeAmount.Text = dm.SqlGetDataSum(startDate, endDate, "income");
                labelExpensesAmount.Text = dm.SqlGetDataSum(startDate, endDate, "expense");
                labelSavingsAmount.Text = dm.SqlGetDataSum(startDate, endDate, "savings");
                labelBalanceAmount.Text = (Convert.ToInt32(dm.SqlGetDataSum(startDate, endDate, "income")) - Convert.ToInt32(dm.SqlGetDataSum(startDate, endDate, "expense"))).ToString();
                labelProfitAmount.Text = "developing";
            }
            catch (FormatException)
            {
                labelIncomeAmount.Text = "0";
                labelExpensesAmount.Text = "0";
                labelSavingsAmount.Text = "0";
                labelBalanceAmount.Text = "0";
                labelProfitAmount.Text = "0";
            }

        }

        //---------------------Other--------------------------------

        private void panelButtonReport_Click(object sender, EventArgs e)
        {
            PanelHandler(panelReports, panelButtonReports);
        }

        private void panelButtonInvestment_Click(object sender, EventArgs e)
        {
            PanelHandler(panelInvestment, panelButtonInvestment);
        }



        private void buttonAddInvestment_Click(object sender, EventArgs e)
        {
            FormInvestmentAdd formAdd = new FormInvestmentAdd();
            formAdd.Show();
        }

        private void buttonWithrawInvestment_Click(object sender, EventArgs e)
        {
            FormWithdrawInvestment formAdd = new FormWithdrawInvestment();
            formAdd.Show();
        }

        private void panelReports_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelSaving_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelMainInfo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelButtonAccounting_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'mfDbDataSet.saving' table. You can move, or remove it, as needed.
            //this.savingTableAdapter.Fill(this.mfDbDataSet.saving);
            IntervalUpdate();
            ChartDashboardLoad();
            InfoLabelDashboardLoad();
            var t = Convert.ToString(lm.SqlGetUsername());
            labelUserName.Text = t;

        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }


        private void pictureBoxDailyReport_Click(object sender, EventArgs e)
        {
            labelDailyReport.BackColor = Color.DarkGray;
            Cursor.Current = Cursors.WaitCursor;
            em.ExportToExcel(DateTime.Today.AddDays(-1).ToString("yyyy-MM-d"), DateTime.Now.ToString("yyyy-MM-d"), "all");
            labelDailyReport.BackColor = Color.White;

        }

        private void pictureBoxWeeklyReport_Click(object sender, EventArgs e)
        {
            labelWeeklyReport.BackColor = Color.DarkGray;
            Cursor.Current = Cursors.WaitCursor;
            em.ExportToExcel(DateTime.Today.AddDays(-7).ToString("yyyy-MM-d"), DateTime.Now.ToString("yyyy-MM-d"),"all");
            labelWeeklyReport.BackColor = Color.White;
        }

        private void pictureBoxMonthlyReport_Click(object sender, EventArgs e)
        {
            labelMonthlyReport.BackColor = Color.DarkGray;
            Cursor.Current = Cursors.WaitCursor;
            em.ExportToExcel(DateTime.Today.AddMonths(-1).ToString("yyyy-MM-d"), DateTime.Now.ToString("yyyy-MM-d"), "all");
            labelMonthlyReport.BackColor = Color.White;
        }

        private void pictureBoxYearlyReport_Click(object sender, EventArgs e)
        {
            labelYearlyReport.BackColor = Color.DarkGray;
            Cursor.Current = Cursors.WaitCursor;
            em.ExportToExcel(DateTime.Today.AddYears(-1).ToString("yyyy-MM-d"), DateTime.Now.ToString("yyyy-MM-d"), "all");
            labelYearlyReport.BackColor = Color.White;
        }

        private void buttonGenerateReport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            em.ExportToExcel(dateTimePickerStartDateGen.Value.ToString("yyyy-MM-d"), dateTimePickerEndDateGen.Value.ToString("yyyy-MM-d"), comboBoxReportGen.Text);
        }

        private void buttonChooseExcel_Click(object sender, EventArgs e)
        {
            em.ImportDataFromExcel();
            labelExcelName.Text = em.path;

        }

        private void buttonGetSampleExcel_Click(object sender, EventArgs e)
        {
            em.CreateSampleExcel();
           
        }

        private void buttonUploadDb_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            em.InsertExcelToDb();
            MessageBox.Show("Upload to database done!");
            labelExcelName.Text = "yourexcel.xlsx";
        }
    }
}
