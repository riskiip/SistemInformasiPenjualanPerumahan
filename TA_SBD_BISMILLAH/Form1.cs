using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TA_SBD_BISMILLAH
{
    public partial class Form1 : Form
    {
        MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.ConnectionString);

        public Form1()
        {
            InitializeComponent();
            
        }

        public Form1(string coba)
        {
            InitializeComponent();
            login_button.Visible = false;
            logout_button.Visible = false;
            label29.Text = coba;
            sesi = coba;
            if (sesi == "")
                login_button.Visible = true;
            else
                logout_button.Visible = true;
            tabControl1.SelectedIndex = 2;
        }

        public string idprm = "";
        public string idrm = "";
        public string idpmb = "";
        public string idtrns = "";
        public string checklist_fasilitas;
        string sesi = "";



        //load table untuk memperbarui data pada semua gridview
        private void loadTable()
        {
            String queryPerumahan = "Select id_perumahan as \"No. Perumahan\", id_rumah as \"No. Rumah\", jenis_perumahan as \"Jenis Perumahan\", harga_perumahan  as \"Harga Perumahan\" from perumahan";
            String queryPembeli = "Select nik as \"No. Identitas\", nama_pembeli as \"Nama Pembeli\", alamat as \"Alamat\", nomor_telepon as \"No. Telepon\", pekerjaan as \"Pekerjaan\", gaji_pokok as \"Gaji Pokok\" from pembeli";
            String queryRumah = "Select id_rumah as \"No. Rumah\", tipe_rumah as \"Tipe Rumah\", fasilitas as \"fasilitas\", jarak_gerbang as \"Jarak ke gerbang keluar\", harga_rumah as \"Harga rumah\" from rumah";
            String queryTransaksi = "Select id_transaksi as \"Id Transaksi\", id_perumahan \"No. Perumahan\", id_rumah as \"No. Rumah\", nama_beli as \"Nama Pembeli\", nik as \"NIK\", jenis_pembayaran as \"Jenis Pembayaran\", total_harga as \"Total Harga\", lama_bayar as \"Lama Pembayaran\" from transaksi";

            DataSet dsPerumahan = new DataSet();
            DataSet dsRumah = new DataSet();
            DataSet dsPembeli = new DataSet();
            DataSet dsTransaksi = new DataSet();

            DataSet combo = new DataSet();
            DataSet combo2 = new DataSet();
            DataSet combo3 = new DataSet();
            DataSet combo4 = new DataSet();
            DataSet comboJenis = new DataSet();

            //combo box id_rumah
            var da = new MySqlDataAdapter();
            da.SelectCommand = new MySqlCommand("select id_rumah from rumah");
            da.SelectCommand.Connection = conn;
            da.SelectCommand.CommandType = CommandType.Text;
            da.Fill(combo);

            nomrumah_cmb.DisplayMember = "id_rumah";
            nomrumah_cmb.ValueMember = "id_rumah";
            nomrumah_cmb.DataSource = combo.Tables[0];

            //combo box id_perumahan
            da.SelectCommand = new MySqlCommand("select id_perumahan from perumahan");
            da.SelectCommand.Connection = conn;
            da.SelectCommand.CommandType = CommandType.Text;
            da.Fill(combo2);

            jenisperumahan_cmb.DisplayMember = "id_perumahan";
            jenisperumahan_cmb.ValueMember = "id_perumahan";
            jenisperumahan_cmb.DataSource = combo2.Tables[0];

            //combobox nama_pembeli
            da.SelectCommand = new MySqlCommand("select nama_pembeli from pembeli");
            da.SelectCommand.Connection = conn;
            da.SelectCommand.CommandType = CommandType.Text;
            da.Fill(combo3);

            namapembeli_cmb.DisplayMember = "nama_pembeli";
            namapembeli_cmb.ValueMember = "nama_pembeli";
            namapembeli_cmb.DataSource = combo3.Tables[0];

            //combobox nik_nama
            da.SelectCommand = new MySqlCommand("select nik from pembeli");
            da.SelectCommand.Connection = conn;
            da.SelectCommand.CommandType = CommandType.Text;
            da.Fill(combo4);

            nik_cmb.DisplayMember = "nik";
            nik_cmb.ValueMember = "nik";
            nik_cmb.DataSource = combo4.Tables[0];

            

            //Mengisi Tabel Perumahan
            var dataAdapter = new MySqlDataAdapter();
            dataAdapter.SelectCommand = new MySqlCommand(queryPerumahan);
        
            dataAdapter.SelectCommand.Connection = conn;
            dataAdapter.SelectCommand.CommandType = CommandType.Text;
            dataAdapter.Fill(dsPerumahan);
            dataperumahan.DataSource = dsPerumahan.Tables[0];
            dataperumahan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataperumahan.AutoResizeColumns();


            //Mengisi Tabel Rumah
            dataAdapter.SelectCommand = new MySqlCommand(queryRumah);

            dataAdapter.SelectCommand.Connection = conn;
            dataAdapter.SelectCommand.CommandType = CommandType.Text;
            dataAdapter.Fill(dsRumah);
            datarumah.DataSource = dsRumah.Tables[0];
            datarumah.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datarumah.AutoResizeColumns();


            //Mengisi Tabel Pembeli
            dataAdapter.SelectCommand = new MySqlCommand(queryPembeli);

            dataAdapter.SelectCommand.Connection = conn;
            dataAdapter.SelectCommand.CommandType = CommandType.Text;
            dataAdapter.Fill(dsPembeli);
            datapembeli.DataSource = dsPembeli.Tables[0];
            datapembeli.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datapembeli.AutoResizeColumns();

            //Mengisi Tabel Transaksi
            dataAdapter.SelectCommand = new MySqlCommand(queryTransaksi);

            dataAdapter.SelectCommand.Connection = conn;
            dataAdapter.SelectCommand.CommandType = CommandType.Text;
            dataAdapter.Fill(dsTransaksi);
            datatransaksi.DataSource = dsTransaksi.Tables[0];
            datatransaksi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datatransaksi.AutoResizeColumns();
            
        }

        private void generateTiperumahByJenis()
         {
            DataSet combo = new DataSet();
            var da = new MySqlDataAdapter();

            da.SelectCommand = new MySqlCommand("select id_rumah from perumahan");
            da.SelectCommand.Connection = conn;
            da.SelectCommand.CommandType = CommandType.Text;
            da.Fill(combo);
            comboBox6.DisplayMember = "id_rumah";
            comboBox6.ValueMember = "id_rumah";
            comboBox6.DataSource = combo.Tables[0];
        }

        //fungsi button tambah perumahan
        private void tambahPerumahan()
        {
            String insertperumahan = "insert into perumahan (id_perumahan, id_rumah, jenis_perumahan, harga_perumahan) values (null, @nomrumah_cmb, @jenis, @harga)";
            using (MySqlCommand cmd = new MySqlCommand("", conn))
            {
                cmd.Parameters.AddWithValue("@jenis", jenisperumahan.Text);
                cmd.Parameters.AddWithValue("@harga", float.Parse(hargaperumahan.Text));
                cmd.Parameters.AddWithValue("@nomrumah_cmb", nomrumah_cmb.Text);

                conn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = insertperumahan;
                cmd.ExecuteNonQuery();
                conn.Close();

                loadTable();
            }
        }

        //fungsi button tambah rumah
        private void tambahRumah()
        {
            String insertrumah = "insert into rumah (id_rumah, tipe_rumah, fasilitas, jarak_gerbang, harga_rumah) values (null, @tipe_rumah, @fasilitas, @jarak, @harga_rumah)";
            using (MySqlCommand cmd = new MySqlCommand("", conn))
            {
                cmd.Parameters.AddWithValue("@tipe_rumah", tiperumah_cmb.Text);
                checklist_fasilitas = "";
                for (int i = 0; i < fasilitas_chck.Items.Count; i++)
                {

                    if (fasilitas_chck.GetItemChecked(i))
                    {
                        checklist_fasilitas = checklist_fasilitas + "" + fasilitas_chck.Items[i].ToString() + ", ";
                    }
                };
                cmd.Parameters.AddWithValue("@fasilitas", checklist_fasilitas);
                cmd.Parameters.AddWithValue("@jarak", jarakrumah_cmb.Text);
                cmd.Parameters.AddWithValue("@harga_rumah", hargarumah.Text);

                if (string.IsNullOrWhiteSpace(jarakrumah_cmb.Text))
                {
                    MessageBox.Show("Jarak rumah ke gerbang keluar belum ditentukan");
                }

                else if (string.IsNullOrWhiteSpace(hargarumah.Text))
                {
                    MessageBox.Show("Harga rumah belum ditentukan");
                }
                else if (string.IsNullOrWhiteSpace(checklist_fasilitas))
                {
                    MessageBox.Show("Fasilitas masih belum ada");
                }

                conn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = insertrumah;
                cmd.ExecuteNonQuery();
                conn.Close();

                loadTable();
            }
        }

        private void Deleteperumahan()
        {
            string querydelete = "DELETE FROM perumahan WHERE id_perumahan = " + idprm;
            MySqlCommand cmd = new MySqlCommand(querydelete, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            loadTable();
        }

        private void Deletetransaksi()
        {
            string querydelete = "DELETE FROM transaksi WHERE id_transaksi = " + idtrns;
            MySqlCommand cmd = new MySqlCommand(querydelete, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            loadTable();
        }

        //fungsi button tambah pembeli
        private void tambahPembeli()
         {
             String insertpembeli = "insert into pembeli (nik, nama_pembeli, alamat, nomor_telepon, pekerjaan, gaji_pokok) values (@nik, @nama, @alamat, @nohp, @pekerjaan, @gaji)";
             using (MySqlCommand cmd = new MySqlCommand("", conn))
             {
                 cmd.Parameters.AddWithValue("@nama", namapembeli.Text);
                 cmd.Parameters.AddWithValue("@nik", int.Parse(nik_pembeli.Text));
                 cmd.Parameters.AddWithValue("@alamat", alamat.Text);
                 cmd.Parameters.AddWithValue("@nohp", int.Parse(nohp.Text));
                 cmd.Parameters.AddWithValue("@pekerjaan", pekerjaan.Text);
                 cmd.Parameters.AddWithValue("@gaji", gaji_cmb.Text);

                 conn.Open();
                 cmd.CommandType = CommandType.Text;
                 cmd.CommandText = insertpembeli;
                 cmd.ExecuteNonQuery();
                 conn.Close();

                 loadTable();
             }
         }

         // fungsi button tambah transaksi
         private void tambahTransaksi()
         {
             String inserttransaksi = "insert into transaksi (id_transaksi, id_perumahan, id_rumah, nama_beli, nik, jenis_pembayaran, total_harga, lama_bayar) values (null, @idper, @idrum, @nama, @nik, @jenispembayaran, @total, @lama)";
             using (MySqlCommand cmd = new MySqlCommand("", conn))
             {
                 cmd.Parameters.AddWithValue("@idper", jenisperumahan_cmb.Text);
                 cmd.Parameters.AddWithValue("@idrum", comboBox6.Text);
                 cmd.Parameters.AddWithValue("@nama", namapembeli_cmb.Text);
                 cmd.Parameters.AddWithValue("@nik", nik_cmb.Text);
                 cmd.Parameters.AddWithValue("@jenispembayaran", comboBox8.Text);
                 cmd.Parameters.AddWithValue("@total", label_harga.Text);
                 cmd.Parameters.AddWithValue("@lama", lama_bayar.Text);
                 
                 conn.Open();
                 cmd.CommandType = CommandType.Text;
                 cmd.CommandText = inserttransaksi;
                 cmd.ExecuteNonQuery();
                 conn.Close();

                 loadTable();
             }
         }

        private void Deleterumah()
        {
            string querydeleterumah = "DELETE FROM rumah WHERE id_rumah = " + idrm;
            MySqlCommand cmd = new MySqlCommand(querydeleterumah, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            loadTable();
        }

        private void Deletepembeli()
        {
            string querydeletepembeli = "DELETE FROM pembeli WHERE nik = " + idpmb;
            MySqlCommand cmd = new MySqlCommand(querydeletepembeli, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            loadTable();
        }

        private void getIdperumahan(DataGridViewCellEventArgs e)
        {
            DataGridViewRow rowPerumahan = this.dataperumahan.Rows[e.RowIndex];
            idprm = rowPerumahan.Cells[0].Value.ToString();
        }

        private void getrowPerumahan(DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataperumahan.Rows[e.RowIndex];
            idprm = row.Cells[0].Value.ToString();
            jenisperumahan.Text = row.Cells[2].Value.ToString();
            hargaperumahan.Text = row.Cells[3].Value.ToString();
        }


        private void getIdrumah(DataGridViewCellEventArgs e)
        {
            DataGridViewRow rowRumah = this.datarumah.Rows[e.RowIndex];
            idrm = rowRumah.Cells[0].Value.ToString();
        }

        private void getrowRumah(DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.datarumah.Rows[e.RowIndex];
            idrm = row.Cells[0].Value.ToString();
            tiperumah_cmb.Text = row.Cells[1].Value.ToString();
            fasilitas_txt.Text = row.Cells[2].Value.ToString();
            jarakrumah_cmb.Text = row.Cells[3].Value.ToString();
            hargarumah.Text = row.Cells[4].Value.ToString();
        }

        private void getIdpembeli(DataGridViewCellEventArgs e)
        {
            DataGridViewRow rowPembeli = this.datapembeli.Rows[e.RowIndex];
            idpmb = rowPembeli.Cells[0].Value.ToString();
        }

        private void getrowPembeli(DataGridViewCellEventArgs e)
         {
             DataGridViewRow row = this.datapembeli.Rows[e.RowIndex];
             idpmb = row.Cells[0].Value.ToString();
             namapembeli.Text = row.Cells[1].Value.ToString();
             nik_pembeli.Text = row.Cells[0].Value.ToString();
             alamat.Text = row.Cells[2].Value.ToString();
             nohp.Text = row.Cells[3].Value.ToString();
             pekerjaan.Text = row.Cells[4].Value.ToString();
             gaji_cmb.Text = row.Cells[5].Value.ToString();
        }

        private void getIdtransaksi(DataGridViewCellEventArgs e)
        {
            DataGridViewRow rowTransaksi = this.datatransaksi.Rows[e.RowIndex];
            idtrns = rowTransaksi.Cells[0].Value.ToString();
        }

        private void getrowTransaksi(DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.datatransaksi.Rows[e.RowIndex];
            idtrns = row.Cells[0].Value.ToString();
            jenisperumahan_cmb.Text = row.Cells[1].Value.ToString();
            comboBox6.Text = row.Cells[2].Value.ToString();
            namapembeli_cmb.Text = row.Cells[3].Value.ToString();
            nik_cmb.Text = row.Cells[4].Value.ToString();
            comboBox8.Text = row.Cells[5].Value.ToString();
            label_harga.Text = row.Cells[6].Value.ToString();
            lama_bayar.Text = row.Cells[7].Value.ToString();
        }

        private void jumlah_bayar()
        {
            float fee;
            float hargaRumahh;
            float hargaPerumahann;

            
            String getHargarumah = "SELECT harga_rumah FROM rumah WHERE id_rumah = " + comboBox6.Text;
            String getHargaperumahan = "SELECT harga_perumahan FROM perumahan WHERE id_perumahan = " + jenisperumahan_cmb.Text;

            //String updateHarga = "update transaksi set total_harga = @fee WHERE id_rumah =" + idrm +" or id_perumahan = " + idprm +" or id_transaksi = " +idtrns+" ";


            using (MySqlCommand cmd = new MySqlCommand("", conn))

            {
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = getHargarumah;
                    hargaRumahh = float.Parse(cmd.ExecuteScalar().ToString());

                    cmd.CommandText = getHargaperumahan;
                    hargaPerumahann = float.Parse(cmd.ExecuteScalar().ToString());

                    fee = hargaRumahh * hargaPerumahann;
                    label_harga.Text = fee.ToString();
                   
                    cmd.ExecuteScalar();
                    
                }
               
                catch (MySqlException err)
                {
                    MessageBox.Show(err.Message);
                }
                finally
                {
                    conn.Close();
                    loadTable();
                }
            }
        }
        
        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Deletepembeli();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            getrowPerumahan(e);
        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tambahperumahan_Click(object sender, EventArgs e)
        {
            tambahPerumahan();
        }

        private void dataperumahan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            getrowPerumahan(e);
        }

        private void datarumah_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            getrowRumah(e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tambahRumah();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tambahPembeli();
        }

        private void datapembeli_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            getrowPembeli(e);
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void jenisperumahan_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void nik_pembeli_Click(object sender, EventArgs e)
        {
            
        }

        private void nik_pembeli_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void hapusperumahan_Click(object sender, EventArgs e)
        {
            Deleteperumahan();
        }

        private void datarumah_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            getrowRumah(e);
        }

        private void datapembeli_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            getrowPembeli(e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Deleterumah();
        }

        private void namapembeli_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void jenisperumahan_cmb_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            generateTiperumahByJenis();
        }

        private void updateperumahan_Click(object sender, EventArgs e)
        {
            string queryupdate_perumahan = "UPDATE perumahan SET jenis_perumahan = '" + jenisperumahan.Text + "', id_rumah = '" + nomrumah_cmb.Text + "', harga_perumahan = '" + hargaperumahan.Text + "' where id_perumahan = " + idprm;
            MySqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = queryupdate_perumahan;
            cmd.ExecuteNonQuery();
            conn.Close();
            loadTable();
        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            string queryupdate_rumah = "UPDATE rumah SET tipe_rumah = '" + tiperumah_cmb.Text + "',fasilitas = '"+fasilitas_txt.Text+"', jarak_gerbang = '" + jarakrumah_cmb.Text + "', harga_rumah = '"+hargarumah.Text+"' where id_rumah = " + idrm;
            MySqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = queryupdate_rumah;
            cmd.ExecuteNonQuery();
            conn.Close();
            loadTable();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string queryupdate_pembeli = "UPDATE pembeli SET nama_pembeli = '" + namapembeli.Text + "', nik = " + nik_pembeli.Text + ", alamat = '" + alamat.Text + "', nomor_telepon = " + nohp.Text + ", pekerjaan = '" + pekerjaan.Text + "' , gaji_pokok = '"+gaji_cmb.Text+"' where nik = " + idpmb;
            MySqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = queryupdate_pembeli;
            cmd.ExecuteNonQuery();
            conn.Close();
            loadTable();
        }
        
        private void button10_Click(object sender, EventArgs e)
        {
            
            tambahTransaksi();
            
        }

        private void datatransaksi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            getrowTransaksi(e);
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin ingin Log Out?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sesi = "";
                label29.Text = "";
                login_button.Visible = true;
                logout_button.Visible = false;
                tabControl1.SelectedIndex = 2;
            }
        }

        private void cariperumahan_TextChanged(object sender, EventArgs e)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select id_perumahan as \"No. Perumahan\", id_rumah as \"No. Rumah\", jenis_perumahan as \"Jenis Perumahan\", harga_perumahan  as \"Harga Perumahan\" from perumahan WHERE id_perumahan LIKE ('" + "%" + cariperumahan.Text + "%') or jenis_perumahan LIKE ('" + "%" + cariperumahan.Text + "%') or id_rumah LIKE ('" + " % " + cariperumahan.Text + " %') or harga_perumahan LIKE ('" + "%" + cariperumahan.Text + "%')";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dataperumahan.DataSource = dt;
            conn.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select id_rumah as \"No. Rumah\", tipe_rumah as \"Tipe Rumah\", fasilitas as \"fasilitas\", jarak_gerbang as \"Jarak ke gerbang keluar\", harga_rumah as \"Harga rumah\" from rumah WHERE id_rumah LIKE ('" + "%" + textBox4.Text + "%') or tipe_rumah LIKE ('" + "%" + textBox4.Text + "%') or fasilitas LIKE ('" + "%" + textBox4.Text + "%') or jarak_gerbang LIKE ('" + "%" + textBox4.Text + "%') or harga_rumah LIKE ('" + "%" + textBox4.Text + "%')";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            datarumah.DataSource = dt;
            conn.Close();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select nik as \"No. Identitas\", nama_pembeli as \"Nama Pembeli\", alamat as \"Alamat\", nomor_telepon as \"No. Telepon\", pekerjaan as \"Pekerjaan\", gaji_pokok as \"Gaji Pokok\" from pembeli WHERE nama_pembeli LIKE ('" + "%" + textBox9.Text + "%') or nik LIKE ('" + " % " + textBox9.Text + " %') or alamat LIKE ('" + "%" + textBox9.Text + "%') or nomor_telepon LIKE ('" + "%" + textBox9.Text + "%') or pekerjaan LIKE ('" + "%" + textBox9.Text + "%') or gaji_pokok LIKE ('" + "%" + textBox9.Text + "%')";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            datapembeli.DataSource = dt;
            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void total_harga_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string queryupdate_transaksi = "UPDATE transaksi SET id_perumahan = " + jenisperumahan_cmb.Text + ", nama_beli = '" + namapembeli_cmb.Text + "', nik = " + nik_cmb.Text + ", id_rumah = " + comboBox6.Text + ", jenis_pembayaran = '" + comboBox8.Text + "', total_harga = '" + label_harga.Text + "', lama_bayar = '" + lama_bayar.Text + "' where id_transaksi = " + idtrns;
            MySqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = queryupdate_transaksi;
            cmd.ExecuteNonQuery();
            conn.Close();
            loadTable();
        }

        private void datatransaksi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            getrowTransaksi(e);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Deletetransaksi();
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select id_transaksi as \"Id Transaksi\", id_perumahan \"No. Perumahan\", id_rumah as \"No. Rumah\", nama_beli as \"Nama Pembeli\", nik as \"NIK\", jenis_pembayaran as \"Jenis Pembayaran\", total_harga as \"Total Harga\", lama_bayar as \"Lama Pembayaran\" from transaksi WHERE id_transaksi LIKE ('" + "%" + textBox15.Text + "%') or id_perumahan LIKE ('" + " % " + textBox15.Text + " %') or id_rumah LIKE ('" + "%" + textBox15.Text + "%') or nama_beli LIKE ('" + "%" + textBox15.Text + "%') or nik LIKE ('" + "%" + textBox15.Text + "%') or jenis_pembayaran LIKE ('" + "%" + textBox15.Text + "%') or total_harga LIKE ('" + "%" + textBox15.Text + "%') or lama_bayar LIKE ('" + "%" + textBox15.Text + "%')";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            datatransaksi.DataSource = dt;
            conn.Close();
        }

        private void hargarumah_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if ((sesi == "") && ((e.TabPageIndex == 0 )||(e.TabPageIndex == 1) || (e.TabPageIndex == 3)))
            {
                MessageBox.Show("Harus login dulu");
                tabControl1.SelectedIndex = 2;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginn = new Login();
            loginn.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadTable();
        }

        private void cekharga_button_Click(object sender, EventArgs e)
        {
            jumlah_bayar();
            
        }
    }
}
