using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanteenPOS
{
    public partial class frmprint : Form
    {
        List<Receipt> _list;
        string _total, _cash, _change, _date;
        public frmprint(List<Receipt> dataSource, string total, string cash, string change, string date)
        {
            InitializeComponent();
            _list = dataSource;
            _total = total;
            _cash = cash;
            _change = change;
            _date = date;
        }

        private void frmprint_Load(object sender, EventArgs e)
        {
          
            ReportDataSource source = new ReportDataSource("ds", _list);
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(source);
            Microsoft.Reporting.WinForms.ReportParameter[] para = new Microsoft.Reporting.WinForms.ReportParameter[]
            {
                new Microsoft.Reporting.WinForms.ReportParameter("pTotal",_total),
                new Microsoft.Reporting.WinForms.ReportParameter("pCash",_cash),
                new Microsoft.Reporting.WinForms.ReportParameter("pChange",_change),
                new Microsoft.Reporting.WinForms.ReportParameter("pDate",_date)
            };
            this.reportViewer.LocalReport.SetParameters(para);
            this.reportViewer.RefreshReport();
        }
    }
}
