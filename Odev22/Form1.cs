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

namespace Odev22
{


    public partial class Form1 : Form
    {

        // ARANACAK KELİME LİSTESİ VE ANLAMLARININ TXT PATHLERİ BURADAN DÜZELTİLMELİ ****
        // ***********************************************
        string pathKelime = @"C:\Users\ozanb\Desktop\kelime-listesi.txt";
        string pathAnlam = @"C:\Users\ozanb\Desktop\anlamlar.txt";

        //
        // Değişken Tanımlamaları
        Random rastgele = new Random();
        Char[] kelimeArray = new char[10];
        string alfabe = "abcdefgğhıijklmnoöprsştuüvyz";

        
        
        string kelimeninAnlamı = "t";
        char jokerHarf;
        int i,j,k;
        string bulunanKelime = "t";
        private static Dictionary<String, String[]> satırArray;
        

        private static List<String> satırlar = new List<String>();
        
        private static List<String> bulunanKelimeArray = new List<String>();
      public Form1()
        {
            InitializeComponent();
            random.Visible = false;
           
        }
        //
        // Random Harfleri Üretme
        private void random_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = null;
            listBox2.DataSource = null;
             bulunanKelimeArray.Clear();
             bulunanKelime = "t";
            Array.Clear(kelimeArray,0,kelimeArray.Length);
           
            char[] array = alfabe.ToCharArray();
            for (i = 0; i < 8; i++)
            {
                int r = rastgele.Next(0, 28);
                kelimeArray[i] = array[r];
            }

            listBox1.DataSource = kelimeArray;
        }
        //
        // Hesaplama Fonksiyonu
        private void Hesapla_Click(object sender, EventArgs e)
        {
            TextBox[] tbs = { textBox1, textBox2, textBox3,textBox4, textBox5, textBox6, textBox7, textBox8, };
            listBox2.DataSource = null;
            bulunanKelimeArray.Clear();
            bulunanKelime = "t";
         
            //
            // Kullanıcı kendi girmek istiyormu diye kontrol
            if (manuelButton.Checked == true)
            {
                Array.Clear(kelimeArray, 0, kelimeArray.Length);
                
                for(int i = 0;i<8;i++)
                {
                    if(tbs[i].Text == "")
                    {
                        MessageBox.Show("Textboxlar Boş Olamaz");
                        return;

                    }
                }

                
                kelimeArray[0] = Convert.ToChar(textBox1.Text);
                kelimeArray[1] = Convert.ToChar(textBox2.Text);
                kelimeArray[2] = Convert.ToChar(textBox3.Text);
                kelimeArray[3] = Convert.ToChar(textBox4.Text);
                kelimeArray[4] = Convert.ToChar(textBox5.Text);
                kelimeArray[5] = Convert.ToChar(textBox6.Text);
                kelimeArray[6] = Convert.ToChar(textBox7.Text);
                kelimeArray[7] = Convert.ToChar(textBox8.Text);
            }


            char[] yeniArray = alfabe.ToCharArray();
           

            // Dosyadaki kelimeleri satır satır okuma
            string[] satır123 = File.ReadAllLines(pathKelime, Encoding.UTF8);
            string[] anlamlar = File.ReadAllLines(pathAnlam, Encoding.UTF8);
            // kelimeleri list'e ekleme
            foreach (string satır in satır123)
            {
                satırlar.Add(satır);  

            }

            satırArray = satırlar
              .Select(word => new {
                  Key = String.Concat(word.OrderBy(c => c)),
                  Value = word
              })
              .GroupBy(item => item.Key, item => item.Value)
              .ToDictionary(chunk => chunk.Key, chunk => chunk.ToArray());
            
            ////
            // Oluşan Kelimelerden ve joker harf kullanarak kelime üretme
            for (i = 0; i < 28; i++)
            {
                kelimeArray[9] = yeniArray[i];

                string yeni = new string(kelimeArray);

                string randomHarfler = String.Concat(yeni.OrderBy(c => c));

                var sonucKelime = Enumerable
                  .Range(1, (1 << randomHarfler.Length) - 1)
                  .Select(index => string.Concat(randomHarfler.Where((item, idx) => ((1 << idx) & index) != 0)))
                  .SelectMany(key =>
                  {
                      String[] words;

                      if (satırArray.TryGetValue(key, out words))
                          return words;
                      else
                          return new String[0];
                  })
                  .Distinct()
                  .OrderBy(word => word);


                //
                //
                // En Uzun kelimeyi Bulma
                foreach (string items in sonucKelime)
                {
                    foreach (string satırs in satır123)
                    {
                        if (satırs.Equals(items))
                        {
                            bulunanKelimeArray.Add(satırs);
                            if (satırs.Length > bulunanKelime.Length)
                            {
                                bulunanKelime = satırs;
                                jokerHarf = kelimeArray[9];

                            }
                        }
                    }
                }
            }
            int satırİ = 0, satırJ = 0;
            //KELİMENİN SATIRINI BULMA
            foreach(string satırKelimesi in satır123)
            {
                satırJ++;

                if (satırKelimesi == bulunanKelime)
                    break;

            }
            foreach(string anlamlarListesi in anlamlar)
            {
                kelimeninAnlamı = anlamlarListesi;
                satırİ++;
                if (satırJ == satırİ)
                break;

            }

            label16.Text = kelimeninAnlamı;
          
            //
            ////
            // Kelime harf sayısına göre puanlama ve bulunan harfleri yazdırma aşaması
            int kelimeHarfSayısı = bulunanKelime.Length;
            if(kelimeHarfSayısı == 3)
            {
                label14.Text = "3 Puan";
            }
            else if(kelimeHarfSayısı ==4)
            {
                label14.Text = "4 Puan";
            }
            else if (kelimeHarfSayısı == 5)
            {
                label14.Text = "5 Puan";
            }
            else if (kelimeHarfSayısı == 6)
            {
                label14.Text = "7 Puan";
            }
            else if (kelimeHarfSayısı == 7)
            {
                label14.Text = "9 Puan";
            }
            else if (kelimeHarfSayısı == 8)
            {
                label14.Text = "11 Puan";
            }
            else if (kelimeHarfSayısı == 9)
            {
                label14.Text = "15 Puan";
            }

            label3.Text = bulunanKelime.ToUpper();
            label4.Text = jokerHarf.ToString().ToUpper();
            if(label3.Text.Contains(label4.Text))
            { 
                label4.Text = jokerHarf.ToString().ToUpper(); 
            }
            else
            {
                label4.Text = "Joker Harf Kullanılmadı";
            }

            listBox2.DataSource = bulunanKelimeArray.ToList();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(randomButton.Checked == true)
            {
                groupBox1.Visible = false;
                random.Visible = true;
            }
            else
            {
                groupBox1.Visible = true;
                random.Visible = false;
            }
        }

       
    }
}
