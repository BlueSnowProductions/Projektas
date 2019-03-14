using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZaidimasProjektui
{
    public partial class Form1 : Form
    {
        bool einaKairen = false;
        bool einaDesinen = false;
        bool begaDesinen = false;
        bool pasokes = false;
        bool turiRakta = false; // nusako ar paeme rakta 
        //bool fonasKairen = true;
       // bool fonasDesinen = false;

        int pasokimoGreitis = 10;
        int galia = 8; // pasokimo galia 
        int taskai = 0;

        int zaidejoGreitis = 18;
        int fonoGreitis = 8; // Nusako, kokiu greiciu i kaire juda fonas
        int begimoGreitis = 15;

        public Form1()
        {
            InitializeComponent();
        }
        
        
        private void pagrindisnisLaikmatis(object sender, EventArgs e)
        {
            //susieje pasokimo greiti ir zaidejo paveiksliuka su vieta
            Zaidejas.Top += pasokimoGreitis;

            //pastoviai atnaujina zaidejo parametrus
            Zaidejas.Refresh();

            //jei pasokes ir galia < 0, pasokes = false
            if (pasokes && galia < 0)
            {
                pasokes = false;
            }

            // pasokimo metu paskokimoGreitis = -12, galia -1
            if (pasokes)
            {
                pasokimoGreitis = -12;
                galia -= 1;
            }
            else
            {  //jei dar nepasokta
                pasokimoGreitis = 12;
            }
           
            //DOMINYKAI tau pravers!!!!!
           
            //leidzia zaidejui judeti i kaire tik tada, jei ten yra 
            //daugiau kaip 100 pikseliu
            if (einaKairen && Zaidejas.Left > 100)
            {
                Zaidejas.Left -= zaidejoGreitis; 
            }

            // jei eina i desine ir 
            //zaidejo plotis + kaires puses plotis + 100 pikseliu
            //yra maziau negu ekrano ilgis, leidzia zaidejui eit desinen
            //Zaidejas sustos pasiekes kairi krasta?
            if (einaKairen && Zaidejas.Left + (Zaidejas.Width + 100) < this.ClientSize.Width)
            {
                Zaidejas.Left += zaidejoGreitis;
            }

            //NELABAI SUPRANTU, KAS CIA!!! Zemiau
            //jei eina desinen ir fono kairej daugiau 1352, slint fona i kaire
            if ((einaDesinen || begaDesinen) && Fonas.Left > -1353) //ar reikia - nuso?
            {
                if (begaDesinen)
                {
                    fonoGreitis = 15;
                }
                Fonas.Left -= fonoGreitis;

                // ciklas skirtas tikrinti platformoms ir pinigams
                //susrasti pinigai pajudes kairen ?
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && x.Tag == "platforma" || x is PictureBox && x.Tag == "pinigas" || x is PictureBox && x.Tag == "durys" || x is PictureBox && x.Tag == "raktas" || x is PictureBox && x.Tag == "siena")
                    {
                        x.Left -= fonoGreitis;
                    }
                }
            }


            //jei eini i kaire ir fono kairej maziau nei 2, tada judint fona i desine
            if (einaKairen && Fonas.Left < 200)
            {
                Fonas.Left += fonoGreitis;
                // Tas pats tik i desine
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && x.Tag == "platforma" || x is PictureBox && x.Tag == "pinigas" || x is PictureBox && x.Tag == "durys" || x is PictureBox && x.Tag == "raktas" || x is PictureBox && x.Tag == "siena")
                    {
                        x.Left += fonoGreitis;
                    }

                }

            }



            // ciklas skirtas visam valdymui tikrinti
            foreach (Control x in this.Controls)
            {
                // jei turi platforma taga
                if (x is PictureBox && x.Tag == "platforma")
                {
                    //tikrina ar zaidejas stovi ant platformos ir ar pasokes = false
                    if(Zaidejas.Bounds.IntersectsWith(x.Bounds) && !pasokes)
                    {
                        galia = 8;
                        Zaidejas.Top = x.Top - Zaidejas.Height; // pastato zaideja ant pictreBox
                        pasokimoGreitis = 0;
                    }
                }

                //jei picture box lieciasi su pinigu
                if (x is PictureBox && x.Tag == "pinigas")
                {
                    //jei zaidejas lieciasi su pinigu
                    if (Zaidejas.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x); // pasalina piniga
                        taskai++;
                    }
                }

                if (x is PictureBox && x.Tag == "siena")
                {
                    if (Zaidejas.Bounds.IntersectsWith(x.Bounds))
                    {
                        if (einaKairen)
                        {
                            Zaidejas.Left -= zaidejoGreitis;
                            fonoGreitis = 8;
                        }
                        else
                        {
                            fonoGreitis = 0;
                        }
                    }

                }
               
            }

            //Jei zaidejas lieciasi su durim ir turi rakta
            if (Zaidejas.Bounds.IntersectsWith(Durys.Bounds) && turiRakta)
            {
                // pakeist duru paveiksliuka i atviru
                Durys.Image = Properties.Resources.door_open;
                //sustabdomas laikmatis
                Laikmatis.Stop();
                MessageBox.Show("Sveikinu perrejus si neiveikiama lygi, kuris toks sunkus, kad tik beprociai ji gali iveikt!!!!!!!!!!!!!!!!!!!!!!!");
            }
        
            // Kai ziadejas lieciasi su raktu
            if (Zaidejas.Bounds.IntersectsWith(Raktas.Bounds))
            {
                this.Controls.Remove(Raktas);//pasalina rakta
                turiRakta = true; // padaro, kad zaidimas matytu, jog zaidejas turi rakta
            }

            //zaidejo mirtis, jei iskrenta uz formos ribu
            if (Zaidejas.Top + Zaidejas.Height > this.ClientSize.Height + 60)
            {
                Laikmatis.Stop();
                MessageBox.Show("Cha Cha! Tu nesugebejai");

            }
        }
              
        private void mygtukasNuspaustas(object sender, KeyEventArgs e)
        {
            //nustatomi true judesio bool

            //Kaire
            if (e.KeyCode == Keys.Left)
            {
                einaKairen = true;
            }

            //Desine
            if (e.KeyCode == Keys.Right)
            {
                einaDesinen = true;
            }

            //Pasokimas
            if (e.KeyCode == Keys.Space && !pasokes)
            {
                pasokes = true;
            }

            if (e.KeyCode == Keys.M)
            {
                begaDesinen = true;
            }

            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void mygtukasAtleistas(object sender, KeyEventArgs e)
        {
            //nustatomi false judesio bool

            //Kaire
            if (e.KeyCode == Keys.Left)
            {
                einaKairen = false;
            }

            //Desine
            if (e.KeyCode == Keys.Right)
            {
                einaDesinen = false;
            }

            //Pasokimas
            if (pasokes)
            {
                pasokes = false;
            }

            if (e.KeyCode == Keys.M)
            {
                begaDesinen = false;
            }
        }

        private void pictureBox40_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {

        }
    }
}
