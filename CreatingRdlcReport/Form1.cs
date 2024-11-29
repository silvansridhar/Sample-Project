using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;

namespace CreatingRdlcReport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Sridhar R\Forms\CreatingRdlcReport\CreatingRdlcReport\RdlcReport.mdf;Integrated Security=True");
        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "Select * from UserInfo";
            SqlCommand cmd = new SqlCommand(sql, Con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource Rpt = new ReportDataSource("DataSet1",dt);
            //reportViewer1.LocalReport.ReportPath = "D:\Sridhar R\Forms\CreatingRdlcReport\CreatingRdlcReport\Report1.rdlc";
            reportViewer1.LocalReport.ReportPath = "Report1.rdlc";
            reportViewer1.LocalReport.DataSources.Add(Rpt);
            reportViewer1.RefreshReport();
        }

        private void LoadData()
        {
            string sql = "Select UserId,UserName,UserLName,,Convert(varchar(10),DOB,101) As DOB,Age,Gender,Address from UserInfo where IsDeleted=0";

            if (Con.State != ConnectionState.Open)
                Con.Open();
            SqlCommand cmd = new SqlCommand(sql, Con);
            SqlDataAdapter lda = new SqlDataAdapter(cmd);
            DataTable Ldt = new DataTable();
            lda.Fill(Ldt);

            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource Rpt1 = new ReportDataSource("DataSet1",Ldt);
            reportViewer1.LocalReport.DataSources.Add(Rpt1);
            reportViewer1.RefreshReport();
        }

        private void FilterData()
        {
            string sql = "Select UserId,UserName,UserLName,Convert(varchar(10),DOB,101) As DOB,Age,Gender,Address  from UserInfo where IsDeleted=0 AND (UserId Like @Searchdata or UserName Like @Searchdata)";

            if (Con.State != ConnectionState.Open)
                Con.Open();
            SqlCommand cmd1 = new SqlCommand(sql, Con);
            cmd1.Parameters.AddWithValue("@Searchdata","%" + textBox1.Text +"%");
            SqlDataAdapter fda = new SqlDataAdapter(cmd1);
            DataTable fdt = new DataTable();
            fda.Fill(fdt);

            if (fdt.Rows.Count == 0)
            {
                MessageBox.Show("No matching records found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource Rpt2 = new ReportDataSource("DataSet1", fdt);
            reportViewer1.LocalReport.DataSources.Add(Rpt2);
            reportViewer1.RefreshReport();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                LoadData();
            }
            else
            {
                FilterData();
            }
        }
    }
}
