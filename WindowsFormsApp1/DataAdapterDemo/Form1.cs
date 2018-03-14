using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DataAdapterDemo
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection = null;
        SqlDataAdapter sqlDataAdapter = null;
        DataSet dataSet = null;
        SqlCommandBuilder sqlCommandBuilder = null;
        public Form1()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            try
            {
                dataSet = new DataSet();
                string sqlQuery = textBox1.Text;
                sqlDataAdapter = new SqlDataAdapter(sqlQuery, sqlConnection);
                sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                Debug.WriteLine(sqlCommandBuilder.GetInsertCommand().CommandText);
                Debug.WriteLine(sqlCommandBuilder.GetUpdateCommand().CommandText);
                Debug.WriteLine(sqlCommandBuilder.GetDeleteCommand().CommandText);

                SqlCommand UpdateCommand =
                    new SqlCommand("update Authors set Price = @pPrice where id = @pId", sqlConnection);
                UpdateCommand.Parameters.Add(new SqlParameter("@pPrice", SqlDbType.Int));
                UpdateCommand.Parameters["@pPrice"].SourceVersion = DataRowVersion.Current;
                UpdateCommand.Parameters["@pPrice"].SourceColumn = "Price";
                UpdateCommand.Parameters.Add(new SqlParameter("@pId", SqlDbType.Int));
                UpdateCommand.Parameters["@pId"].SourceVersion = DataRowVersion.Original;
                UpdateCommand.Parameters["@pId"].SourceColumn = "id";

                sqlDataAdapter.UpdateCommand = UpdateCommand;

                Debug.WriteLine(sqlCommandBuilder.GetInsertCommand().CommandText);
                Debug.WriteLine(sqlCommandBuilder.GetUpdateCommand().CommandText);
                Debug.WriteLine(sqlCommandBuilder.GetDeleteCommand().CommandText);

                dataGridView1.DataSource = null;
                sqlDataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables["table1"];
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = null;
            sqlDataAdapter.Update(dataSet, "table");
            dataSet.Reset();
            sqlDataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables["table"];
        }
    }
}
