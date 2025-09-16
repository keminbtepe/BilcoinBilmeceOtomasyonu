using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;

namespace Bilcoin
{
    public partial class Bilcoin : Form
    {
        public Bilcoin()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=ASDASD\\MSSQLSERVER01;Initial Catalog=bilcoin;Integrated Security=True");
        SqlCommand komut = new SqlCommand();
        SqlDataReader oku;
        string kelime, harf,soru;
        int uzunluk;
        int kayitsayisi = -1;
        int bilinenharfler;
        int kalanhak = 5;
        int kalansure = 80;
        string[] harfler;
        int[] sayilar;
        int puan = 80;
        Random randomsayi = new Random();

        
        private void Form1_Load(object sender, EventArgs e)
        {  
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("select count (*) from tlevel1", baglanti);
            kayitsayisi = Convert.ToInt32(cmd.ExecuteScalar());
            komut.Connection = baglanti;
            komut.CommandText = "select * from tlevel1";
            oku = komut.ExecuteReader();
            timer1.Start();
            basaSar();
            labelAktar();
            diziyeAktar();
        }
        void basaSar()
        {
            if (kayitsayisi > 0)
            {

                kayitsayisi -= 1;
                oku.Read();

                label12.Text = oku[1].ToString();
                soru = oku[2].ToString();


                kelime = soru;
                uzunluk = kelime.Length;
                harfler = new string[uzunluk];
                sayilar = new int[uzunluk];
                label2.Text = Convert.ToString(puan);
            }
            else
            {
                MessageBox.Show("SORULAR BİTTİ");
            }
        }
        void labelAktar()
        {
            label1.Text = "";
            for (int i = 1; i <= uzunluk; i++)
            {
                label1.Text += "_";

            }
        }

        void diziyeAktar()
        {
            for (int i = 0; i < uzunluk; i++)
            {
                harfler[i] = kelime.Substring(i, 1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
           
            surehak();
            if (textBox1.Text != "")
            {
                harf = textBox1.Text;

                if (listBox1.Items.IndexOf(textBox1.Text) == -1)
                {
                    listBox1.Items.Add(textBox1.Text);
                    int sorgula = 0;

                    for (int i = 0; i < uzunluk; i++)
                    {
                        if (harf == harfler[i])
                        {
                            string metin = label1.Text;
                            label1.Text = yazdir(metin, i, harf);
                            bilinenharfler++;
                            puan += 5;
                            label2.Text = puan.ToString();

                            sorgula = 1;
                        }
                    }

                    if (sorgula == 0) // Eğer yanlış bilirse
                    {
                        kalanhak--;
                    }
                    textBox1.Text = "";

                }
                else { MessageBox.Show("aynı harf mevcut"); textBox1.Clear(); }



            }

            oyunBitti();
        }
        void oyunBitti()
        {
            if (bilinenharfler == uzunluk)
            {
                bilinenharfler = 0;
                uzunluk = 0;
                timer1.Stop();
               
                MessageBox.Show("TEBRİKLER, Bu Kelimeyi Bildiniz");
                listBox1.Items.Clear();
                
                basaSar();                               
                labelAktar();
                diziyeAktar();





            }

            if (kalansure <= 0 || kalanhak <= 0)
            {
                timer1.Stop();
                MessageBox.Show("ÜZGÜNÜZ KAYBETTİNİZ!");

            }
            if (puan < 0)
            {
                timer1.Stop();
                MessageBox.Show("OYUN BİTTİ! Puanın yetersiz!!");
                Application.Exit();


            }
        }

        static string yazdir(string metin, int indis, string yenideger)
        {
            metin = metin.Remove(indis, 1);
            return metin.Insert(indis, yenideger);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        void surehak()
        {
            label3.Text = " : " + kalansure;
            label4.Text = " : " + kalanhak;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

                   
            MessageBox.Show(oku[3].ToString());
            puan -= 20;
            label2.Text = Convert.ToString(puan);
            pictureBox4.Visible = false;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            kalansure += 15;
            puan -= 5;
            label2.Text = puan.ToString();

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        devam:
            int sayi = randomsayi.Next(harfler.Length);



            if (listBox1.Items.IndexOf(harfler[sayi]) == -1)
            {
                // MessageBox.Show(Convert.ToString(sayi) + "  " + harfler[sayi]); harf bildirimi
                listBox1.Items.Add(harfler[sayi] + "");
                for (int i = 0; i < uzunluk; i++)
                {
                    if (harfler[sayi] == harfler[i])
                    {

                        string metin = label1.Text;
                        puan -= 5;
                        label2.Text = Convert.ToString(puan);
                        label1.Text = yazdir(metin, i, harfler[sayi]);
                        bilinenharfler++;
                    }

                }
            }
            else
            {

                goto devam;
            }




        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(baglanti.State.ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oyunBitti();
            surehak();

            if (kalansure > 0)
            {
                kalansure--;
            }

        }
    }
}
