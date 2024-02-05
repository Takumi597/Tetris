
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Media;
using Castle.Core.Internal;

namespace Tetris
{
    public partial class Tetris : Form
    {
        private Graphics plansza;
        private SolidBrush frame, tlo;
        private SolidBrush[] kolorKloca;
        private Pen ramka;
        private Random rnd;

        private int x, y, size, predkosc, clcker, punkty;
        private int[,] mapa;
        private char kierunek;
        public Tetris()
        {
            InitializeComponent();

            plansza = pictureBox1.CreateGraphics();

            rnd = new Random();

            frame = new SolidBrush(Color.Gray);
            tlo = new SolidBrush(Color.Black);
            kolorKloca = new SolidBrush[8];
            ramka = new Pen(Color.Black);

            kolorKloca[0] = tlo;
            kolorKloca[1] = new SolidBrush(Color.DarkBlue);
            kolorKloca[2] = new SolidBrush(Color.Cyan);
            kolorKloca[3] = new SolidBrush(Color.Orange);
            kolorKloca[4] = new SolidBrush(Color.Yellow);
            kolorKloca[5] = new SolidBrush(Color.Green);
            kolorKloca[6] = new SolidBrush(Color.Purple);
            kolorKloca[7] = new SolidBrush(Color.Red);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Thread roboczy = new Thread(() => muzyka());
            roboczy.Start();
            
        }
        private void btStart_KeyDown(object sender, KeyEventArgs e)
        {
            kierunek = (char)e.KeyValue;
        }
        private void btGra(object sender, EventArgs e)
        {

            btStart.Text = "RESET";

            prepare();

            Stopwatch zegar = new Stopwatch();
            Stopwatch zegarRuch = new Stopwatch();
            zegar.Start();
            zegarRuch.Start();

            Kloc klocek = new Kloc(rnd.Next() % 7 + 1,size, kolorKloca);
            //Kloc klocek = new Kloc(4, size, kolorKloca);
            klocek.printKloc(plansza, x, y);

            while (true) // petla gry
            {

                if (zegar.ElapsedMilliseconds > 2000 / predkosc)// predkosc gry // logika
                {
                    if (klocek.blokadaD(x, y, 0) == true && klocek.blokadaPerma(x, y, mapa) == true)// w dół
                    {
                        klocek.unprintKloc(plansza, x, y);
                        y += 30;
                        klocek.printKloc(plansza, x, y);

                    }
                    else// nowy klocek
                    {
                        klocek.perma(x, y, mapa);

                        rzad();

                        x = 150;
                        y = 30;

                        int rodzaj = rnd.Next() % 7 + 1;
                        while (klocek.getRodzaj() == rodzaj)
                        {
                            rodzaj = rnd.Next() % 7 + 1;
                        }

                        klocek = new Kloc(rodzaj, size, kolorKloca);
                        klocek.printKloc(plansza, x, y);
                        if (!klocek.blokadaPerma(x, y, mapa))
                        {
                            MessageBox.Show("PRZEGRAŁEŚ !!!");
                            break;
                        }

                        punkty += predkosc * predkosc;
                        lbPkt.Text = punkty + "";
                    }
                    zegar.Restart();
                }

                if (zegarRuch.ElapsedMilliseconds > 100) // obsuga ruchu
                {
                    //kierunek = 'Q';
                    clcker++;
                    lbCzas.Text = clcker / 10 + " s";
                    progressBar1.Value = (clcker % 600) / 6;
                    lbProgress.Text = progressBar1.Value + " %";
                    if (clcker % 600 == 0)
                    {
                        predkosc++;
                        lbPredkosc.Text = predkosc + "";
                    }
                    ruch(ref klocek);

                    zegarRuch.Restart();
                }
                Application.DoEvents();
                Thread.Sleep(1);
            }
        }
        private void rzad()
        {
            for (int i = 20; i > 0; i--)
            {
                bool pelenRzad = true;

                for (int j = 1; j < 11; j++) // czy rzad pelen
                {
                    if (mapa[j, i] == 0)
                    {
                        pelenRzad = false;
                        break;
                    }
                }
                if (pelenRzad)
                {
                    punkty += predkosc * predkosc * 20;

                    for (int k = i; k > 1; k--)
                    {
                        for (int j = 1; j < 11; j++) // czy rzad pelen
                        {

                            mapa[j, k] = mapa[j, k - 1];
                            plansza.FillRectangle(kolorKloca[mapa[j, k]], j * 30, (k) * 30, size, size);
                            plansza.DrawRectangle(ramka, j * 30, (k) * 30, size, size);

                            Application.DoEvents();
                        }

                    }
                    i++;
                }
            }
        }
        private void prepare()
        {
            x = 150;
            y = 30;
            size = 30;
            clcker = 0;
            punkty = 0;
            kierunek = ' ';
            predkosc = 1;// jeden raz na 3 sek
            mapa = new int[11, 22];

            lbPredkosc.Text = predkosc + "";
            lbPkt.Text = punkty + "";

            plansza = pictureBox1.CreateGraphics();
            plansza.FillRectangle(tlo, 0, 0, 500, 700);

            for (int i = 0; i < 12; i++)
            {
                plansza.FillRectangle(frame, i * 30, 0 * 30, 30, 30);
                plansza.DrawRectangle(ramka, i * 30, 0 * 30, 30, 30);
            }
            for (int i = 0; i < 12; i++)
            {
                plansza.FillRectangle(frame, i * 30, 630, 30, 30);
                plansza.DrawRectangle(ramka, i * 30, 630, 30, 30);
            }
            for (int i = 0; i < 22; i++)
            {
                plansza.FillRectangle(frame, 0, i * 30, 30, 30);
                plansza.DrawRectangle(ramka, 0, i * 30, 30, 30);
            }
            for (int i = 0; i < 22; i++)
            {
                plansza.FillRectangle(frame, 330, i * 30, 30, 30);
                plansza.DrawRectangle(ramka, 330, i * 30, 30, 30);
            }
        }
        private void ruch(ref Kloc klocek)
        {
            switch (kierunek)
            {
                case 'A':
                    if (klocek.blokadaL(x, y, 0, mapa))
                    {
                        klocek.unprintKloc(plansza, x, y);
                        x -= 30;
                        klocek.printKloc(plansza, x, y);
                        kierunek = ' ';
                    }
                    break;


                case 'Q':

                    klocek.unprintKloc(plansza, x, y);
                    klocek.obrotL();
                    if (klocek.blokadaL(x, y, 1, mapa) == false || klocek.blokadaP(x, y, 1, mapa) == false || klocek.blokadaD(x, y, 1) == false)
                    {
                        klocek.obrotP();
                    }
                    klocek.printKloc(plansza, x, y);
                    kierunek = ' ';
                    break;

                case 'E':

                    klocek.unprintKloc(plansza, x, y);
                    klocek.obrotP();
                    if (klocek.blokadaL(x, y, 1, mapa) == false || klocek.blokadaP(x, y, 1, mapa) == false || klocek.blokadaD(x, y, 1) == false)
                    {
                        klocek.obrotL();
                    }
                    klocek.printKloc(plansza, x, y);
                    kierunek = ' ';
                    break;

                case 'D':

                    if (klocek.blokadaP(x, y, 0, mapa))
                    {
                        klocek.unprintKloc(plansza, x, y);
                        x += 30;
                        klocek.printKloc(plansza, x, y);
                        kierunek = ' ';
                    }
                    break;

                case 'S':

                    if (y < 620)
                    {
                        y += 30;
                        if (klocek.blokadaD(x, y, 1) && klocek.blokadaPerma(x, y - 30, mapa))
                        {
                            y -= 30;
                            klocek.unprintKloc(plansza, x, y);
                            y += 30;
                            klocek.printKloc(plansza, x, y);
                        }
                        else
                        {
                            y -= 30;
                        }
                        kierunek = ' ';
                    }
                    break;

                case 'U':

                    klocek.unprintKloc(plansza, x, y);
                    klocek = new Kloc(rnd.Next() % 7 + 1, size, kolorKloca);
                    // klocek = new Kloc(klocek.rodzaj+1);
                    klocek.printKloc(plansza, x, y);
                    kierunek = ' ';
                    break;

                case 'I':

                    for (int k = 20; k > 10; k--)
                    {
                        for (int j = 1; j < 10; j++) // czy rzad pelen
                        {
                            mapa[j, k] = rnd.Next() % 7 + 1;

                            plansza.FillRectangle(kolorKloca[mapa[j, k]], j * 30, (k) * 30, size, size);
                            plansza.DrawRectangle(ramka, j * 30, (k) * 30, size, size);
                            Application.DoEvents();
                            //Thread.Sleep(1);
                        }
                    }
                    break;

                case 'O':

                    predkosc++;
                    kierunek = ' ';
                    lbPredkosc.Text = predkosc + "";
                    break;

                case 'P':

                    for (int k = 20; k > 10; k--)
                    {
                        for (int j = 1; j < 11; j++) // czy rzad pelen
                        {
                            mapa[j, k] = rnd.Next() % 7 + 1;

                            plansza.FillRectangle(kolorKloca[mapa[j, k]], j * 30, (k) * 30, size, size);
                            plansza.DrawRectangle(ramka, j * 30, (k) * 30, size, size);
                            Application.DoEvents();
                            //Thread.Sleep(1);
                        }
                    }
                    break;
            }
        }
        public void muzyka()
        {
            SoundPlayer muzyka = new SoundPlayer("THEME_OF_SOLID_SNAKE_SUPER_SMASH_BROS_BRAWL.wav");
            muzyka.PlayLooping();
            if (chbMusic.Checked == false)
            {
                muzyka.Stop();
            }
        }
    }
}
