using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace book_oop
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataReader dr;

        int i = 0;
        dbconnection dbconn = new dbconnection();
        public Form1()
        {
            InitializeComponent();
            conn = new MySqlConnection(dbconn.dbconnect());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }
        private void LoadRecord()
        {
            dataGridView1.Rows.Clear();
            conn.Open();
            cmd = new MySqlCommand("SELECT `id`,`nama_buku`, `kode_buku`, `jumlah_buku`, `tanggal`, `deskripsi`, `nama_pembeli`, `no_hp` FROM `db_book`", conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(dataGridView1.Rows.Count + 1, dr["id"].ToString(), dr["nama_buku"].ToString(), dr["kode_buku"].ToString(),dr["jumlah_buku"].ToString(), dr["tanggal"].ToString(), dr["deskripsi"].ToString(), dr["nama_pembeli"].ToString(), dr["no_hp"].ToString());
            }
            dr.Close();
            conn.Close();
        }
        public void clear()
        {
            txt_nama_buku.Clear();
            txt_id.Clear();
            txt_kode.Clear();
            txt_jumlah.Clear();
            txt_tanggal.Value = DateTime.Now;
            txt_deskripsi.Clear();
            txt_nama_pembeli.Clear();
            txt_nohp.Clear();
        }

        private void btn_tambah_Click(object sender, EventArgs e)
        {
            if ((txt_nama_buku.Text == string.Empty) || (txt_kode.Text == string.Empty) || (txt_jumlah.Text == string.Empty) || (txt_tanggal.Text == string.Empty) || (txt_deskripsi.Text == string.Empty) || (txt_nama_pembeli.Text == string.Empty)||(txt_nohp.Text == string.Empty))
            {
                MessageBox.Show("Warning : Required Failed ?", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            else
            {
                resetIncrement();
                string date1 = txt_tanggal.Value.ToString("yyyy-MM-dd");
                conn.Open();
                cmd = new MySqlCommand("INSERT INTO `db_book`(`nama_buku`, `kode_buku`, `jumlah_buku`, `tanggal`, `deskripsi`, `nama_pembeli`,`no_hp`) VALUES (@nama_buku,@kode_buku,@jumlah_buku,@tanggal,@deskripsi,@nama_pembeli,@no_hp)", conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@nama_buku", txt_nama_buku.Text);
                cmd.Parameters.AddWithValue("@kode_buku", txt_kode.Text);
                cmd.Parameters.AddWithValue("@jumlah_buku", txt_jumlah.Text);
                cmd.Parameters.AddWithValue("@tanggal", date1);
                cmd.Parameters.AddWithValue("@deskripsi", txt_deskripsi.Text);
                cmd.Parameters.AddWithValue("@nama_pembeli", txt_nama_pembeli.Text);
                cmd.Parameters.AddWithValue("@no_hp", txt_nohp.Text);

                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    MessageBox.Show("Record Save success !", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Record Save Failed !", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                conn.Close();
                LoadRecord();
                clear();
            }
        }
        public void resetIncrement()
        {
            MySqlScript script = new MySqlScript(conn, "SET @id :=0; Update db_book SET id = @id := (@id+1); " + "ALTER TABLE db_book AUTO_INCREMENT = 1;");
            script.Execute();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            string date1 = txt_tanggal.Value.ToString("yyyy-MM-dd");
            conn.Open();
            cmd = new MySqlCommand("UPDATE `db_book` SET `nama_buku`=@nama_buku,`kode_buku`=@kode_buku,`jumlah_buku`=@jumlah_buku,`tanggal`=@tanggal,`deskripsi`=@deskripsi,`nama_pembeli`=@nama_pembeli,`no_hp`=@no_hp WHERE `id`=@id", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@id", txt_id.Text);
            cmd.Parameters.AddWithValue("@nama_buku", txt_nama_buku.Text);
            cmd.Parameters.AddWithValue("@kode_buku", txt_kode.Text);
            cmd.Parameters.AddWithValue("@jumlah_buku", txt_jumlah.Text);
            cmd.Parameters.AddWithValue("@tanggal", date1);
            cmd.Parameters.AddWithValue("@deskripsi", txt_deskripsi.Text);
            cmd.Parameters.AddWithValue("@nama_pembeli", txt_nama_pembeli.Text);
            cmd.Parameters.AddWithValue("@no_hp", txt_nohp.Text);

            i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                MessageBox.Show("Record Update success !", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Record Update Failed !", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            conn.Close();
            LoadRecord();
            clear();
        }

        private void btn_hapus_Click(object sender, EventArgs e)
        {
            conn.Open();
            cmd = new MySqlCommand("DELETE FROM `db_book` WHERE `id`=@id", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@id", txt_id.Text);


            i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                MessageBox.Show("Record Delete success !", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Record Delete Failed !", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            conn.Close();
            LoadRecord();
            clear();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void txt_cari_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_cari_TextChanged_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            conn.Open();
            cmd = new MySqlCommand("SELECT `id`, `nama_buku`, `kode_buku`, `jumlah_buku`, `tanggal`, `deskripsi`, `nama_pembeli`, `no_hp` FROM db_book WHERE id like '%" + txt_cari.Text + "%' or nama_buku like '%" + txt_cari.Text + "%' or kode_buku like '%" + txt_cari.Text + "%' or jumlah_buku like '%" + txt_cari.Text + "%' or tanggal like '%" + txt_cari.Text + "%' or deskripsi like '%" + txt_cari.Text + "%' or nama_pembeli like '%" + txt_cari.Text + "%' or no_hp like '%" + txt_cari.Text + "%'", conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(dataGridView1.Rows.Count + 1, dr["id"].ToString(), dr["nama_buku"].ToString(), dr["kode_buku"].ToString(), dr["jumlah_buku"].ToString(), dr["tanggal"].ToString(), dr["deskripsi"].ToString(), dr["nama_pembeli"].ToString(), dr["no_hp"].ToString());
            }
            dr.Close();
            conn.Close();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex.ToString());
            txt_id.Text = dataGridView1.Rows[id].Cells[1].Value.ToString();
            txt_nama_buku.Text = dataGridView1.Rows[id].Cells[2].Value.ToString();
            txt_kode.Text = dataGridView1.Rows[id].Cells[3].Value.ToString();
            txt_jumlah.Text = dataGridView1.Rows[id].Cells[4].Value.ToString();

            txt_deskripsi.Text = dataGridView1.Rows[id].Cells[6].Value.ToString();
            txt_nama_pembeli.Text = dataGridView1.Rows[id].Cells[7].Value.ToString();
            txt_nohp.Text = dataGridView1.Rows[id].Cells[8].Value.ToString();
        }
    }
}
