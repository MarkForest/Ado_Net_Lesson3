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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string connString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
        SqlConnection conn = null;
        SqlDataReader reader;
        DataTable dataTable;

        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(connString);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = textBox1.Text;
                sqlCommand.Connection = conn;
                dataGridView1.DataSource = null;
                conn.Open();
                dataTable = new DataTable();
                reader = sqlCommand.ExecuteReader();

                int line = 0;
                do
                {
                    while (reader.Read())
                    {
                        if(line == 0)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dataTable.Columns.Add(reader.GetName(i));
                            }
                            line++;
                        }
                        DataRow dataRow = dataTable.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dataRow[i] = reader[i];
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                } while (reader.NextResult());
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(conn != null)
                {
                    conn.Close();
                }
                if(reader != null){
                    reader.Close();
                }
            }
        }
    }
}
