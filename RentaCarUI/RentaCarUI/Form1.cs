using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RentaCarUI

{
    public partial class Form1 : Form
    {
        // SQL Server Bağlantı Cümlemiz (Connection String)
        string connectionString = @"Server=localhost\SQLEXPRESS01;Database=RentACarDB;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
        }

        // Form ilk açıldığında çalışacak kodlar
        private void Form1_Load(object sender, EventArgs e)
        {
            VerileriGetir();
            ComboBoxlariDoldur();
        }

        // Butona tıklandığında çalışacak kodlar
        private void button1_Click(object sender, EventArgs e)
        {
            VerileriGetir();
        }

        // Veri tabanından verileri çekip DataGridView'a dolduran özel metodumuz
        private void VerileriGetir()
        {
            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();

                    // Daha önce MSSQL'de yazdığımız View'i çağırıyoruz
                    string sorgu = "SELECT * FROM View_AktifKiralamalar";

                    SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                    DataTable dt = new DataTable();
                    da.Fill(dt); // SQL'den gelen verilerle tabloyu doldur

                    // Doldurulan tabloyu ekrandaki DataGridView'a aktar
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veri tabanına bağlanırken bir hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ComboBoxlariDoldur()
        {
            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();

                    // 1. Müşterileri Doldur (Sadece Ad Soyad Gösterip, Arka planda ID tutacağız)
                    SqlDataAdapter daMusteri = new SqlDataAdapter("SELECT MusteriID, Ad + ' ' + Soyad AS AdSoyad FROM Musteriler", baglanti);
                    DataTable dtMusteri = new DataTable();
                    daMusteri.Fill(dtMusteri);
                    cmbMusteri.DisplayMember = "AdSoyad"; // Ekranda görünen kısım
                    cmbMusteri.ValueMember = "MusteriID"; // Kodun kullanacağı arka plan kimliği
                    cmbMusteri.DataSource = dtMusteri;

                    // 2. Sadece 'Musait' Olan Araçları Doldur
                    SqlDataAdapter daArac = new SqlDataAdapter("SELECT AracID, Plaka + ' - ' + Marka AS AracBilgi FROM Araclar WHERE Durum = 'Musait'", baglanti);
                    DataTable dtArac = new DataTable();
                    daArac.Fill(dtArac);
                    cmbArac.DisplayMember = "AracBilgi";
                    cmbArac.ValueMember = "AracID";
                    cmbArac.DataSource = dtArac;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Listeler yüklenirken hata oluştu: " + ex.Message);
                }
            }
        }
        private void btnMusteriKaydet_Click(object sender, EventArgs e)
        {
            // Boş alan kontrolü (Kullanıcı adı veya TC'yi boş geçerse uyaralım)
            if (txtTc.Text == "" || txtAd.Text == "" || txtSoyad.Text == "")
            {
                MessageBox.Show("Lütfen zorunlu alanları (TC, Ad, Soyad) doldurunuz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Hata varsa işlemi burada kes
            }

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();

                    // MSSQL'de kullandığımız INSERT komutunun aynısı. 
                    // Güvenlik için verileri doğrudan değil, @parametreler aracılığıyla gönderiyoruz.
                    string sorgu = "INSERT INTO Musteriler (TCKimlik, Ad, Soyad, Telefon, EhliyetNo) VALUES (@p1, @p2, @p3, @p4, @p5)";

                    SqlCommand komut = new SqlCommand(sorgu, baglanti);

                    // Parametreleri formdaki TextBox'ların içindeki metinlerle (Text) dolduruyoruz
                    komut.Parameters.AddWithValue("@p1", txtTc.Text);
                    komut.Parameters.AddWithValue("@p2", txtAd.Text);
                    komut.Parameters.AddWithValue("@p3", txtSoyad.Text);
                    komut.Parameters.AddWithValue("@p4", txtTelefon.Text);
                    komut.Parameters.AddWithValue("@p5", txtEhliyet.Text);

                    komut.ExecuteNonQuery(); // Sorguyu SQL'e gönder ve çalıştır

                    MessageBox.Show("Müşteri veri tabanına başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Kayıt işleminden sonra formdaki kutuların içini temizleyelim
                    txtTc.Clear();
                    txtAd.Clear();
                    txtSoyad.Clear();
                    txtTelefon.Clear();
                    txtEhliyet.Clear();
                }
                catch (Exception ex)
                {
                    // Eğer TC Kimlik veya Ehliyet No daha önce kaydedilmişse (UNIQUE kısıtlaması) SQL hata fırlatır, burada yakalarız.
                    MessageBox.Show("Kayıt sırasında bir hata oluştu:\n" + ex.Message, "SQL Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAracKaydet_Click(object sender, EventArgs e)
        {
            // Boş alan kontrolü
            if (txtPlaka.Text == "" || txtMarka.Text == "" || txtUcret.Text == "")
            {
                MessageBox.Show("Plaka, Marka ve Ücret alanları boş geçilemez!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();
                    // Dikkat: Durum sütununa doğrudan 'Musait' yazdırıyoruz.
                    string sorgu = "INSERT INTO Araclar (Plaka, Marka, Model, Yil, GunlukUcret, Durum) VALUES (@p1, @p2, @p3, @p4, @p5, 'Musait')";

                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@p1", txtPlaka.Text);
                    komut.Parameters.AddWithValue("@p2", txtMarka.Text);
                    komut.Parameters.AddWithValue("@p3", txtModel.Text);
                    komut.Parameters.AddWithValue("@p4", txtYil.Text);
                    komut.Parameters.AddWithValue("@p5", txtUcret.Text.Replace(",", ".")); // Ondalık sayı hatasını önlemek için

                    komut.ExecuteNonQuery();

                    MessageBox.Show("Araç başarıyla envantere eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Kutuları temizle
                    txtPlaka.Clear(); txtMarka.Clear(); txtModel.Clear(); txtYil.Clear(); txtUcret.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kayıt sırasında hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void btnKirala_Click(object sender, EventArgs e)
        {
            
        {
            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();
                    string sorgu = "INSERT INTO Kiralamalar (AracID, MusteriID, AlisTarihi, TeslimTarihi, ToplamTutar) VALUES (@arac, @musteri, @alis, @teslim, @tutar)";

                    SqlCommand komut = new SqlCommand(sorgu, baglanti);

                    // ComboBox'tan seçilen ismin arka plandaki ID'sini (ValueMember) alıyoruz
                    komut.Parameters.AddWithValue("@arac", cmbArac.SelectedValue);
                    komut.Parameters.AddWithValue("@musteri", cmbMusteri.SelectedValue);

                    // Tarih seçicilerden tarihleri alıyoruz
                    komut.Parameters.AddWithValue("@alis", dtpAlis.Value.Date);
                    komut.Parameters.AddWithValue("@teslim", dtpTeslim.Value.Date);

                    komut.Parameters.AddWithValue("@tutar", txtTutar.Text.Replace(",", "."));

                    komut.ExecuteNonQuery(); // Kaydet!

                    MessageBox.Show("Kiralama Başarılı! MSSQL Trigger'ımız sayesinde aracın durumu otomatik 'Kirada' yapıldı.", "Harika", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Kiralanan araç "Müsait" listesinden düşsün diye listeleri ve tabloyu güncelliyoruz
                    ComboBoxlariDoldur();
                    VerileriGetir();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kiralama işlemi başarısız: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
    }
}