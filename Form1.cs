using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;



namespace bankauto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=bankautodatabase;Uid=root;Pwd='sensizasla2.';");
      

        //formlar arası veri aktraımı
       public static string tcno, adi, soyadi, dtarihi, telno, email;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            kullancilari_goster();
            textBox1.MaxLength = 11;
            textBox4.MaxLength = 10;
            toolTip1.SetToolTip(this.textBox1, "Lütfen 11 haneli TC kimlik numaranızı giriniz.");// tooltipe uyarı verir
            toolTip1.SetToolTip(this.textBox4, "Telefon numaranızı başında sıfır olmadan yazınız.");
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox3.CharacterCasing = CharacterCasing.Upper;
            comboBox1.Items.Add("KADIN");
            comboBox1.Items.Add("ERKEK");

            comboBox3.Items.Add("PERSONEL");
            comboBox3.Items.Add("MÜŞTERİ");

            label14.Text = Form2.adi + " " + Form2.soyadi;

            conn.Open();
            MySqlCommand ilsorgu = new MySqlCommand("SELECT*FROM il ", conn);
            MySqlDataReader ilokuma = ilsorgu.ExecuteReader();
            while (ilokuma.Read())
            {
                comboBox2.Items.Add(ilokuma["il_ad"]);
               
            }
            conn.Close();


            
        }

       
        private void kullancilari_goster()
        {
            try
            {
               
                conn.Open();
                MySqlDataAdapter kullanicilari_listele = new MySqlDataAdapter("SELECT   customers.id, customers.identity, customers.first_name ," +
                    " customers.last_name, customers.bd_date, customers.phone_no, customers.genger, customers.city , customers.password ," +
                    " customers.person_area FROM customers ", conn);
                DataSet dshafiza = new DataSet();
                kullanicilari_listele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                conn.Close();
            }
            catch (Exception errormes)

            {
                MessageBox.Show(errormes.Message, "Banka Otomasyonu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
           
            }
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void topPage1_temizle()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear(); 
            textBox5.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;

            
        }
        private void btn1Ekle_Click(object sender, EventArgs e)
        {
            bool kayitkontrol=false;
            conn.Open();
            MySqlCommand kayitsorgu = new MySqlCommand("SELECT*FROM customers where identity='"+textBox1.Text+"'", conn);
            MySqlDataReader kayitokuma = kayitsorgu.ExecuteReader();
            while (kayitokuma.Read()){//okunan tc veri tabanında var mı?
                kayitkontrol = true;
                MessageBox.Show("Müşteri sisteme kayıtlı lütfen başka bir Tc Kimlik No giriniz!");
                break;
            }
            conn.Close();

            if (kayitkontrol == false)
            {
               if(textBox1.Text.Length==11 && textBox1.Text != "" &&
                  
              textBox2.Text!=""&& textBox3.Text != ""&& textBox4.Text.Length == 10 && textBox5.Text.Length == 6 && textBox5.Text != ""
              )
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand ekle = new MySqlCommand("insert into customers  (identity,first_name,last_name,bd_date,phone_no,genger,city , password,person_area)" +
                            " values ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + dateTimePicker1.Value + "', '" + textBox4.Text + "'," +
                            " '" + comboBox1.Text + "','" + comboBox2.Text + "' , '" + textBox5.Text + "','" + comboBox3.Text + "' )", conn);
                        ekle.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Kullanıcı başarıyla eklendi!"    , "Banka Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        kullancilari_goster();
                        topPage1_temizle();
                    }
                    catch (Exception errormes)
                    {
                        MessageBox.Show(errormes.Message, "Banka Otomasyonu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                       
                    }
                }
               else
                {
                    MessageBox.Show("alanları eksik doldurdunuz lütfen kontrol edin!", "Banka Otomasyonu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
                && !char.IsSeparator(e.KeyChar);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
                && !char.IsSeparator(e.KeyChar);
        }

        private void btn2Sil_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 11)
            {
                try
                {
                    conn.Open();
                    MySqlCommand sil = new MySqlCommand("delete from customers where identity = '" + textBox1.Text + "'", conn);
                    sil.ExecuteNonQuery();
                    conn.Close();

                   
                    
                            MessageBox.Show("Kullanıcı başarıyla silindi!", "Banka Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                      
                    
                    
                    kullancilari_goster();
                    topPage1_temizle();
                }
                catch (Exception errormes)
                {
                   
                    MessageBox.Show(errormes.Message, "Banka Otomasyonu",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();

                }
            }
            else
            {
                MessageBox.Show("Lütfen 11 haneli TC kimlik numaranızı giriniz!", "Banka Otomasyonu",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        
        }
        private void btn3Guncelle_Click(object sender, EventArgs e)
        {
            

            
                if (textBox1.Text.Length == 11 && textBox1.Text != "" &&

              textBox2.Text != "" && textBox3.Text != "" && textBox4.Text.Length == 10 && textBox5.Text.Length == 6 && textBox5.Text != "")
                {
                    try
                    {
                        conn.Open(); 
                        MySqlCommand guncelle = new MySqlCommand("update  customers set  first_name= '" + textBox2.Text + "', last_name='" + textBox3.Text + "', bd_date='" + dateTimePicker1.Value + "', phone_no='" + textBox4.Text + "', genger='" + comboBox1.Text + "',city='" + comboBox2.Text + "', password = '" + textBox5.Text + "' ,person_area='" + comboBox3.Text + "'  where identity='" + textBox1.Text+"'", conn);
                        guncelle.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Kullanıcı bilgileri başarıyla güncellendi!", "Banka Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        kullancilari_goster();
                        topPage1_temizle();
                    }
                    catch (Exception errormes)
                    {
                        MessageBox.Show(errormes.Message, "Banka Otomasyonu",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();

                    }
                }
                else
                {
                    MessageBox.Show("alanları eksik doldurdunuz lütfen kontrol edin!", "Banka Otomasyonu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
        
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                textBox4.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                comboBox1.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                comboBox2.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                textBox5.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();
                comboBox3.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString();
            }
            catch
            {

            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            conn.Open();
            MySqlCommand musterinocek = new MySqlCommand("SELECT*FROM customer where identity = '" + textBox1.Text + "'", conn);
            
            MySqlDataReader musterioku = musterinocek.ExecuteReader();
            while (musterioku.Read())
            {
                textBox5.Text=musterioku["id"].ToString();

            }
            conn.Close();
        }
    }

}
