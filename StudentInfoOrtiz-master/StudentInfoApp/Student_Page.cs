using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace StudentInfoApp
{
    public partial class Student_Page : Form
    {
        public Student_Page()
        {
            InitializeComponent();
            // Attach the CellClick event handler to ensure it works
            StudentListDGV.CellClick += StudentListDGV_CellClick;
        }

        private void Student_Page_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void LoadStudents()
        {
            string connStr = "server=localhost;user=root;password=;database=StudentInfoDB;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT studentId, 
                               CONCAT(firstName, ' ', middleName, ' ', lastName) AS FullName 
                        FROM StudentRecordTB";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    StudentListDGV.DataSource = dt;

                    // Remove existing View column first (to avoid duplicates)
                    if (StudentListDGV.Columns.Contains("View"))
                        StudentListDGV.Columns.Remove("View");

                    // Add View button column
                    DataGridViewButtonColumn viewBtn = new DataGridViewButtonColumn
                    {
                        Name = "View",
                        HeaderText = "Action",
                        Text = "VIEW",
                        UseColumnTextForButtonValue = true
                    };
                    StudentListDGV.Columns.Add(viewBtn);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading students: " + ex.Message);
                }
            }
        }

        private void StudentListDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && StudentListDGV.Columns[e.ColumnIndex].Name == "View")
                {
                    int studentId = Convert.ToInt32(StudentListDGV.Rows[e.RowIndex].Cells["studentId"].Value);

                    // Debug line to confirm ID
                    // MessageBox.Show("Selected Student ID: " + studentId);

                    StudentPage_Individual detailsForm = new StudentPage_Individual(studentId);
                    detailsForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error handling view: " + ex.Message);
            }
        }
    }
}
