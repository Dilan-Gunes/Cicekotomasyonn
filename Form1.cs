using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
namespace Cicekotomasyon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection cicek = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=cicekotomasyon.mdb.accdb");

        //Button1 kayıt ekle  butonu
        private void Button1_Click(object sender, EventArgs e)
        {
            cicek.Open();
            OleDbCommand komut = new OleDbCommand("INSERT INTO Cicekotomasyon(Turkce, Ingilizce, Adet) VALUES (@turkce, @ingilizce, @adet)", cicek);
            komut.Parameters.AddWithValue("@turkce", textBox1.Text);
            komut.Parameters.AddWithValue("@ingilizce", textBox2.Text);
            komut.Parameters.AddWithValue("@adet", textBox4.Text);
            komut.ExecuteNonQuery();
            cicek.Close();
            MessageBox.Show("Kayit eklendi", "Kayıt" );
            tablo.Clear();
            Listele();
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is TextBox)
                {
                    Controls[i].Text = "";
                }
            }
        }

        //dataGridView verilerin gözükeceyı yer, kare ...
        DataTable tablo = new DataTable();

        private void Listele() //listeleme yontemi
        {
            cicek.Open();
            OleDbDataAdapter adtr= new OleDbDataAdapter("select * from cicekotomasyon", cicek);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            cicek.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Listele();

        }

        //Kayıt Silme butonu button2
        private void Button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                DialogResult result = MessageBox.Show("Seçili kaydı silmek istediğinize emin misiniz?", "Kayıt Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string selectedTurkce = dataGridView1.CurrentRow.Cells["Turkce"].Value.ToString();

                    cicek.Open();
                    OleDbCommand komut = new OleDbCommand("DELETE FROM cicekotomasyon WHERE Turkce = @turkce", cicek);
                    komut.Parameters.AddWithValue("@turkce", selectedTurkce);
                    komut.ExecuteNonQuery();
                    cicek.Close();

                    MessageBox.Show("Kayıt Silindi");
                    Listele();
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz bir satırı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //TextBox3 arama satırı , arama yeri...
        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox3.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                cicek.Open();
                OleDbDataAdapter adtr = new OleDbDataAdapter("SELECT * FROM cicekotomasyon WHERE Turkce LIKE @search OR Ingilizce LIKE @search", cicek);
                adtr.SelectCommand.Parameters.AddWithValue("@search", "%" + searchText + "%");
                DataTable tablo2 = new DataTable();
                adtr.Fill(tablo2);
                dataGridView1.DataSource = tablo2;
                cicek.Close();
            }
            else
            {
                // Eğer arama kutusu boş ise, tüm verileri göstermek için Listele() metodunu çağırabilirsiniz.
                Listele();
            }
        }

        //button3 temizle butonu listbox dakı verileri temizler.
        private void Button3_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox textBox = (TextBox)ctrl;
                    textBox.Clear();
                }
            }
        }
    }
}
