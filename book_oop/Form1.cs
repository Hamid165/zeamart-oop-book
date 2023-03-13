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
            filldataTable();
        }
        DataTable dataTable = new DataTable();

        //Tampilan MySql
        public DataTable getDataTable()
        {
            resetIncrement();
            dataTable.Reset();
            dataTable = new DataTable();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM db_book", conn))
            {
                conn.Open();

                MySqlDataReader reader = command.ExecuteReader();
                dataTable.Load(reader);
            }
            return dataTable;
        }

        public void filldataTable()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RowTemplate.Height = 200;
            dataGridView1.DataSource = getDataTable();
            Column1.DataPropertyName = "id";
            Column2.DataPropertyName = "nama_buku";
            Column3.DataPropertyName = "kode_buku";
            Column4.DataPropertyName = "jumlah_buku";
            Column5.DataPropertyName = "tanggal";
            Column6.DataPropertyName = "deskripsi";
            Column7.DataPropertyName = "nama_pembeli";
            Column8.DataPropertyName = "no_hp";
            Column9.DataPropertyName = "gambar";
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
            pictureBox2.Image=null;
        }

        private void btn_tambah_Click(object sender, EventArgs e)
        {
            if ((txt_nama_buku.Text == string.Empty) || (txt_kode.Text == string.Empty) || (txt_jumlah.Text == string.Empty) || (txt_tanggal.Text == string.Empty) || (txt_deskripsi.Text == string.Empty) || (txt_nama_pembeli.Text == string.Empty)||(txt_nohp.Text == string.Empty)||(pictureBox2.Image==null))
            {
                MessageBox.Show("Warning : Required Failed ?", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                resetIncrement();
                // Convert image to byte array
                byte[] imageData;
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox2.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    imageData = ms.ToArray();
                }

                string date1 = txt_tanggal.Value.ToString("yyyy-MM-dd");
                //conn.Open();
                cmd = new MySqlCommand("INSERT INTO `db_book`(`nama_buku`, `kode_buku`, `jumlah_buku`, `tanggal`, `deskripsi`, `nama_pembeli`,`no_hp`,`gambar`) VALUES (@nama_buku,@kode_buku,@jumlah_buku,@tanggal,@deskripsi,@nama_pembeli,@no_hp,@gambar)", conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@nama_buku", txt_nama_buku.Text);
                cmd.Parameters.AddWithValue("@kode_buku", txt_kode.Text);
                cmd.Parameters.AddWithValue("@jumlah_buku", txt_jumlah.Text);
                cmd.Parameters.AddWithValue("@tanggal", date1);
                cmd.Parameters.AddWithValue("@deskripsi", txt_deskripsi.Text);
                cmd.Parameters.AddWithValue("@nama_pembeli", txt_nama_pembeli.Text);
                cmd.Parameters.AddWithValue("@no_hp", txt_nohp.Text);
                cmd.Parameters.AddWithValue("@gambar", imageData);

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
                filldataTable();
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
            // Convert image to byte array
            byte[] imageData;
            using (MemoryStream ms = new MemoryStream())
            {
                pictureBox2.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                imageData = ms.ToArray();
            }
            cmd = conn.CreateCommand();
            string date1 = txt_tanggal.Value.ToString("yyyy-MM-dd");
            //conn.Open();
            cmd.CommandText = "UPDATE db_book SET nama_buku= @nama_buku, kode_buku = @kode_buku, jumlah_buku = @jumlah_buku, tanggal = @tanggal, deskripsi = @deskripsi ,nama_pembeli = @nama_pembeli,no_hp=@no_hp, gambar=@gambar WHERE id = @id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@id", txt_id.Text);
            cmd.Parameters.AddWithValue("@nama_buku", txt_nama_buku.Text);
            cmd.Parameters.AddWithValue("@kode_buku", txt_kode.Text);
            cmd.Parameters.AddWithValue("@jumlah_buku", txt_jumlah.Text);
            cmd.Parameters.AddWithValue("@tanggal", date1);
            cmd.Parameters.AddWithValue("@deskripsi", txt_deskripsi.Text);
            cmd.Parameters.AddWithValue("@nama_pembeli", txt_nama_pembeli.Text);
            cmd.Parameters.AddWithValue("@no_hp", txt_nohp.Text);
            cmd.Parameters.AddWithValue("@gambar", imageData);

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
            filldataTable();
            clear();
        }

        private void btn_hapus_Click(object sender, EventArgs e)
        {
            //conn.Open();
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
            resetIncrement();
            filldataTable();
            clear();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void txt_cari_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_cari_TextChanged_1(string ValueToFind )
        {
            string searchQuery = "SELECT * FROM db_book WHERE CONCAT (id, nama_buku, kode_buku, jumlah_buku, tanggal, deskripsi, nama_pembeli, no_hp) LIKE '%" + ValueToFind + "%'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(searchQuery, conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;

            dr.Close();
            conn.Close();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex.ToString());
            txt_id.Text = dataGridView1.Rows[id].Cells[0].Value.ToString();
            txt_nama_buku.Text = dataGridView1.Rows[id].Cells[1].Value.ToString();
            txt_kode.Text = dataGridView1.Rows[id].Cells[2].Value.ToString();
            txt_jumlah.Text = dataGridView1.Rows[id].Cells[3].Value.ToString();

            txt_deskripsi.Text = dataGridView1.Rows[id].Cells[5].Value.ToString();
            txt_nama_pembeli.Text = dataGridView1.Rows[id].Cells[6].Value.ToString();
            txt_nohp.Text = dataGridView1.Rows[id].Cells[7].Value.ToString();
            Byte[] img = (Byte[])dataGridView1.CurrentRow.Cells[8].Value;
            MemoryStream ms = new MemoryStream(img);
            pictureBox2.Image = Image.FromStream(ms);

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfd = new OpenFileDialog();
            openfd.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openfd.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = new Bitmap(openfd.FileName);
            }

        }

        public void searchData(string ValueToFind)
        {
            string searchQuery = "SELECT * FROM db_book WHERE CONCAT (id, nama_buku, kode_buku, jumlah_buku, tanggal, deskripsi, nama_pembeli, no_hp,gambar) LIKE '%" + ValueToFind + "%'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(searchQuery, conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            searchData(textBox2.Text);
        }
    }
}
