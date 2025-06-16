using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace StudentInfoApp
{
    public partial class StudentPage_Individual : Form
    {
        private int studentId;

        public StudentPage_Individual(int id)
        {
            InitializeComponent();
            this.Load += StudentPage_Individual_Load;
            studentId = id;
        }

        private void StudentPage_Individual_Load(object sender, EventArgs e)
        {
            LoadStudentDetails();
        }

        private void LoadStudentDetails()
        {
            string connStr = "server=localhost;user=root;password=;database=StudentInfoDB;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT s.*, c.courseName, y.yearLvl
                        FROM StudentRecordTB s
                        JOIN CourseTB c ON s.courseId = c.courseId
                        JOIN YearTB y ON s.yearId = y.yearId
                        WHERE s.studentId = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", studentId);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        FullNameTxt.Text = $"{reader["firstName"]} {reader["middleName"]} {reader["lastName"]}";
                        StudentIdTxt.Text = reader["studentId"].ToString();
                        BirthdateTxt.Text = reader["birthdate"].ToString();
                        AgeTxt.Text = reader["age"].ToString();
                        CourseTxt.Text = reader["courseName"].ToString();
                        YearLevelTxt.Text = reader["yearLvl"].ToString();
                        ContactTxt.Text = reader["studContactNo"].ToString();
                        EmailTxt.Text = reader["emailAddress"].ToString();
                        GuardianTxt.Text = $"{reader["guardianFirstName"]} {reader["guardianLastName"]}";
                        NicknameTxt.Text = reader["nickname"].ToString();
                        HobbiesTxt.Text = reader["hobbies"].ToString();
                        AddressTxt.Text = $"{reader["houseNo"]}, {reader["brgyName"]}, {reader["municipality"]}, {reader["province"]}, {reader["region"]}, {reader["country"]}";
                    }
                    else
                    {
                        MessageBox.Show("Student not found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
