using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Npgsql;

namespace isrgr
{
    enum RoWState

    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }

    //форма добавления 
    public partial class AddForm : Form

    {

        private readonly NpgsqlConnection connection = new NpgsqlConnection("Host=localhost; Port=5432; User Id=postgres; Password=12345; Database=isrgr");


        public AddForm()

        {

            InitializeComponent();

        }



        private void Change()

        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id_teach = textBox1.Text;
            var t_name = textBox2.Text;
            var t_surname = textBox3.Text;
            int id_job;
            var employment_date = textBox5.Text;
            var phone_num = textBox6.Text;
            int id_dep;



            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if ((int.TryParse(textBox4.Text, out id_job) && (id_job < 6)) && ((int.TryParse(textBox7.Text, out id_dep)) && (id_dep < 5)))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(id_teach, t_name, t_surname, id_job, employment_date, phone_num, id_dep);
                    dataGridView1.Rows[selectedRowIndex].Cells[6].Value = RoWState.Modified;
                }
                else
                {
                    MessageBox.Show("Ошибка формата записи!");
                }
            }
        }

            private void label5_Click(object sender, EventArgs e)
            {

            }

            private void AddForm_Load(object sender, EventArgs e)
            {

            }

            private void button1_Click(object sender, EventArgs e)
            {
            connection.Open();

            string queryString;

            queryString =

                     @"SELECT id_teach ,t_name as Имя,t_surname AS Фамилия, 
                     id_job AS ID_Должность, 
                     employment_date as дата_приема, phone_number as Номер_телефона , 
                     id_dep as Кафедра from teachers; 
                     ";

            var query = new NpgsqlCommand(queryString, connection);
            var reader = query.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);

            dataGridView1.DataSource = table;

            connection.Close();
            
            }
            private void deleteRow()
            {

                int index = dataGridView1.CurrentCell.RowIndex;

                if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
                {
                   dataGridView1.Rows[index].Cells[6].Value = RoWState.Deleted;

                  return;
                }

             dataGridView1.Rows[index].Cells[6].Value = RoWState.Deleted;

            }


            private void Update()
            {

                connection.Open();

                for (int index = 0; index < dataGridView1.Rows.Count; index++)
                {
                    var rowState = (RoWState)dataGridView1.Rows[index].Cells[6].Value;

                    if (rowState == RoWState.Existed)
                    continue;

                    if (rowState == RoWState.Deleted)
                    {
                        var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                        var deleteQuery = $"delete from teachers where id_teach={id}";


                        var query = new NpgsqlCommand(deleteQuery, connection);
                        var reader = query.ExecuteReader();
                        var table = new DataTable();
                        table.Load(reader);

                        dataGridView1.DataSource = table;

                    }

                    if (rowState == RoWState.Modified)
                    {   
                        var id_teach = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                        var t_name = textBox2.Text;
                        var t_surname = textBox3.Text;
                        var id_job = Convert.ToInt32(dataGridView1.Rows[index].Cells[3].Value);
                        var employment_date = textBox5.Text;
                        var phone_number = textBox6.Text;
                        var id_dep = Convert.ToInt32(dataGridView1.Rows[index].Cells[6].Value);


                        var updateQuery = $"update teachers set t_name='{t_name}'," +
                         $"t_surname='{t_surname}',id_job={id_job},employment_date='{employment_date}'," +
                         $"phone_number='{phone_number}' , id_dep='{id_dep}' where id_teach={id_teach};";

                        var query = new NpgsqlCommand(updateQuery, connection);
                        var reader = query.ExecuteReader();
                        var table = new DataTable();
                        table.Load(reader);

                        dataGridView1.DataSource = table;
                    }
            }
                connection.Close();
                Close();
            }


        private void button2_Click(object sender, EventArgs e)
        {
            base.Update();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            connection.Open();

            var t_name = textBox2.Text;
            var t_surname = textBox3.Text;
            int id_job;
            var employment_date = textBox5.Text;
            var phone_num = textBox6.Text;
            int id_dep;

            if ((int.TryParse(textBox4.Text, out id_job) && (id_job < 6)) && ((int.TryParse(textBox7.Text, out id_dep)) && (id_dep < 5)))
            {
                var queryString =
                     $"INSERT INTO teachers(t_name,t_surname,id_job,employment_date,phone_number,id_dep)" +
                     $" values('{t_name}','{t_surname}',{id_job},'{employment_date}','{phone_num}',{id_dep})";

                var query = new NpgsqlCommand(queryString, connection);

                query.ExecuteReader();

                MessageBox.Show("Запись успешно создана!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                if (int.TryParse(textBox7.Text, out id_dep) && (id_dep < 5))
                {
                    MessageBox.Show("ID кафедры целое число! И меньше 5!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (int.TryParse(textBox4.Text, out id_job) && (id_job < 6))
                {
                    MessageBox.Show("ID должности целое число! И меньше 6!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            connection.Close();
            Close();
        

        }

        private void button4_Click(object sender, EventArgs e)
        {
            deleteRow();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Change();
        }
        
        int indexRow;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex; // get the selected Row Index 

            DataGridViewRow row = dataGridView1.Rows[indexRow];


            textBox1.Text = row.Cells[0].Value.ToString();

            textBox2.Text = row.Cells[1].Value.ToString();

            textBox3.Text = row.Cells[2].Value.ToString();

            textBox4.Text = row.Cells[3].Value.ToString();

            textBox5.Text = row.Cells[4].Value.ToString();

            textBox6.Text = row.Cells[5].Value.ToString();

            textBox7.Text = row.Cells[6].Value.ToString();
        }
    }
}
