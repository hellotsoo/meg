using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace babibubebo
{
    public partial class Form1 : Form
    {
        string connection;
        string query;
        MySqlConnection connect;
        MySqlCommand command;
        MySqlDataAdapter adapter;
        MySqlDataReader reader;
        DataTable dt_nat = new DataTable();
        DataTable dt_team = new DataTable();
        DataTable dt_M = new DataTable();
        DataTable dt_teamLagi = new DataTable();
        DataTable dt_del = new DataTable();
        DataTable dtPlayer, dtManager, dtNationality, dtTeam;

        public Form1()
        {
            connection = "server=localhost;user=root;pwd=;database=premier_league";
            connect = new MySqlConnection(connection);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dtPlayer = new DataTable();
            dtManager = new DataTable();
            dtTeam = new DataTable();
            dtNationality = new DataTable();
            dataGridView_players.DataSource = dtPlayer;
            dataGridView_managers.DataSource = dtManager;
            updateP();
            updateM();
            
            query = "SELECT nation,nationality_id FROM nationality;";
            command = new MySqlCommand(query,connect);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dt_nat);
            comboBox_nat.DataSource = dt_nat;
            comboBox_nat.ValueMember = "nationality_id";
            comboBox_nat.DisplayMember = "nation";

            query = "SELECT team_name,team_id FROM team;";
            command = new MySqlCommand(query, connect);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dt_team);
            comboBox_tname.DataSource = dt_team;
            comboBox_tname.ValueMember = "team_id";
            comboBox_tname.DisplayMember = "team_name";

            query = "SELECT team_name,team_id FROM team;";
            command = new MySqlCommand(query, connect);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dt_teamLagi);
            comboBox_teamM.DataSource = dt_teamLagi;
            comboBox_teamM.ValueMember = "team_id";
            comboBox_teamM.DisplayMember = "team_name";

            query = "SELECT team_name,team_id FROM team;";
            command = new MySqlCommand(query, connect);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dt_del);
            comboBox_del.DataSource = dt_del;
            comboBox_del.ValueMember = "team_id";
            comboBox_del.DisplayMember = "team_name";
        }

        private void comboBox_teamM_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string manager = $"SELECT m.manager_name, t.team_name, m.birthdate, n.nation from manager m, team t, nationality n, where n.manager_id=t.manager_id and n.nationality_id=m.nationality_id and t.team_id = {comboBox_teamM.SelectedValue.ToString()}';";
            command = new MySqlCommand(manager,connect);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dt_M);
            dataGridView_managers.DataSource = dt_M;
        }

        private void button_edit_Click(object sender, EventArgs e)
        {

        }

        private void button_del_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_del_SelectionChangeCommitted(object sender, EventArgs e)
        {
            button_del.Enabled = true;
            try
            {
                string com = $"SELECT * FROM player WHERE team_id = '{comboBox_del.SelectedValue.ToString()}';";
                command = new MySqlCommand(com, connect);
                adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt_del);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView_del_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string pid = dataGridView_del.CurrentRow.Cells[0].Value.ToString();
        }

        private void updateP()
        {
            dtPlayer.Clear();
            try
            {
                string commandstring = "select * from player";
                command = new MySqlCommand(commandstring, connect);
                adapter = new MySqlDataAdapter(command);
                adapter.Fill(dtPlayer);

                dataGridView_players.DataSource = dtPlayer;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateM()
        {
            try
            {
                string commandstring = "select * from manager";
                command = new MySqlCommand(commandstring, connect);
                adapter = new MySqlDataAdapter(command);
                adapter.Fill(dtManager);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            string pid = textBox_pid.Text;
            string tnum = textBox_tnum.Text;
            string name = textBox_name.Text;
            string nat = comboBox_nat.SelectedValue.ToString();
            string pos = textBox_pos.Text;
            string height = textBox_height.Text;
            string weight = textBox_weight.Text;
            string bdate = dateTimePicker_bdate.Value.ToString("yyyy-MM-dd");
            string tname = comboBox_tname.SelectedValue.ToString();
            string commandlagi = $"insert into player values ('{pid}','{tnum}','{name}','{nat}','{pos}','{height}', '{weight}', '{bdate}', '{tname}',1,0)";
            try
            {
                connect.Open();
                command = new MySqlCommand(commandlagi, connect);
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connect.Close();
                updateP();
            }
        }
    }
}
