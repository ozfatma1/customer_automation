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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=bankautodatabase;Uid=root;Pwd='sensizasla2.';");
        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = " Kullanıcı Girişi";
            textBox1.MaxLength = 11;
            textBox2.MaxLength = 6;
        }
        public static string tcno, adi, soyadi, dtarihi, telno, email;

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool kayitkontrol= false;
            conn.Open();
            MySqlCommand selectsorgu = new MySqlCommand("SELECT*FROM customers where identity='" + textBox1.Text + "' ", conn);
            MySqlDataReader kayitokuma = selectsorgu.ExecuteReader();

            while (kayitokuma.Read())
            {
                if (kayitokuma["person_area"].ToString() == "PERSONEL")
                {
                    if (kayitokuma["identity"].ToString() == textBox1.Text && kayitokuma["password"].ToString() == textBox2.Text)
                    {
                        tcno = kayitokuma.GetValue(1).ToString();
                        adi = kayitokuma.GetValue(2).ToString();
                        soyadi = kayitokuma.GetValue(3).ToString();
                        this.Hide();
                        Form1 frm1 = new Form1();
                        frm1.Show();
                        MessageBox.Show("PERSONEL GİRİŞİ YAPTINIZ!", "Banka Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                        
                    }
                    else {

                        if (kayitokuma["identity"].ToString() == textBox1.Text && kayitokuma["password"].ToString() != textBox2.Text)
                        {
                            MessageBox.Show("Geçersiz şifre lütfen tekrar deneyin!", "Banka Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                        else    
                                { MessageBox.Show("kullanıcı bulunumadı,lütfen geçerli bir TC kimlik no giriniz!", "Banka Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                                            
                       }

                }

                else
                {
                    if (kayitokuma["identity"].ToString() == textBox1.Text && kayitokuma["password"].ToString() == textBox2.Text)
                    { 

                    tcno = kayitokuma.GetValue(0).ToString();
                    adi = kayitokuma.GetValue(1).ToString();
                    soyadi = kayitokuma.GetValue(2).ToString();
                    this.Hide();
                    Form3 frm3 = new Form3();
                    frm3.Show();
                        MessageBox.Show("MÜŞTERİ GİRİŞİ YAPTINIZ!", "Banka Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break; 
                    
                    }
                    else
                    {
                        if (kayitokuma["identity"].ToString() == textBox1.Text && kayitokuma["password"].ToString() != textBox2.Text)
                        {
                            MessageBox.Show("Geçersiz şifre lütfen tekrar deneyin!", "Banka Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                        else
                        { MessageBox.Show("kullanıcı bulunumadı,lütfen geçerli bir TC kimlik no giriniz!", "Banka Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error); }

                        
                    }
                }

            }
            conn.Close();
        }
    }
}
