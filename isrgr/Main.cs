using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace isrgr
{
    public partial class Main : Form
    {
        private readonly NpgsqlConnection connection = new NpgsqlConnection ("Host=localhost; Port=5432; User Id=postgres; Password=12345; Database=isrgr");

        public Main()
        {
            InitializeComponent();

            //button4_Click(this, new EventArgs());
        }

    

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           // var addForm = new AddForm();

           // addForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connection.Open();

            var queryString =

                @"SELECT id_dep as id, dep_name as Кафедра from departments;";



            var query = new NpgsqlCommand(queryString, connection);

            var reader = query.ExecuteReader();

            var table = new DataTable();

            table.Load(reader);

            dataGridView1.DataSource = table;

            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //выдает список преподавателей всех или заданной кафедры в textBox3

            connection.Open();

            string queryString;

            if (textBox3.Text == "")

            {
                queryString =

                    @"SELECT id_teach ,t_name as Имя,t_surname AS Фамилия, phone_number as Номер_телефона, 

                    job_titel AS Должность,  dep_name as Кафедра FROM teachers 

                    inner join departments ON teachers.id_dep=departments.id_dep 

                    inner join job_titel ON teachers.id_job=job_titel.id_job";

            }

            else

            {
                queryString =

                   @"SELECT id_teach ,t_name as Имя,t_surname AS Фамилия, phone_number as Номер_телефона, 

                   job_titel AS Должность,  dep_name as Кафедра FROM teachers 

                   inner join departments ON teachers.id_dep=departments.id_dep 

                   inner join job_titel ON teachers.id_job=job_titel.id_job 

                         where dep_name='" + textBox3.Text + "'";
            }

            var query = new NpgsqlCommand(queryString, connection);

            var reader = query.ExecuteReader();

            var table = new DataTable();

            table.Load(reader);

            dataGridView1.DataSource = table;

            connection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //5)кнопка, которая, выдает названия кафедры для заданного преподавателя и все его данные. 

            connection.Open();

            var queryString =

                @"SELECT  id_teach AS id, dep_name as Кафедра, job_titel AS Должность, employment_date AS Дата_приема, 

                    phone_number AS Номер_телефона from teachers 

                    inner join departments ON teachers.id_dep=departments.id_dep 

                    inner join job_titel ON teachers.id_job=job_titel.id_job where t_name='" + textBox1.Text + "'and t_surname='" +

                    textBox2.Text + "'";

            var query = new NpgsqlCommand(queryString, connection);

            var reader = query.ExecuteReader();

            var table = new DataTable();

            table.Load(reader);

            dataGridView1.DataSource = table;

            connection.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            connection.Open();

            var queryString =
                 @"SELECT id_dis, dis_name as Дисциплина, dis_code as Код_дисциплины, dis_sem as Семестр
                 from list_of_dis
                 inner join departments on list_of_dis.id_dep=departments.id_dep
                 where dep_name='"+textBox3.Text+"'";

            var query= new NpgsqlCommand(queryString, connection);
            var reader = query.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;

            connection.Close();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
