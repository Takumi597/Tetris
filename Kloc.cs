using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Kloc
    {
        private bool[,] cialoKloca;
        private int rodzaj, wersja,size;
        private Brush kloc, tlo;
        private Pen ramka, tloPen;

        public Kloc(int t_rodzaj, int t_size, SolidBrush[] kolorKloca)
        {
            
            cialoKloca = new bool[4, 4]; // y /x
            rodzaj = t_rodzaj;
            size = t_size;
            wersja = 0;
           
            kloc = kolorKloca[t_rodzaj];
            
            tlo = new SolidBrush(Color.Black);
            ramka = new Pen(Color.Gray);
            tloPen = new Pen(Color.Black);

            switch (t_rodzaj)
            {
                case 1: //kostka
                    cialoKloca[1, 1] = true;
                    cialoKloca[1, 2] = true;
                    cialoKloca[2, 1] = true;
                    cialoKloca[2, 2] = true;

                    break;

                case 2:// L - lewe
                    cialoKloca[2, 0] = true;
                    cialoKloca[0, 1] = true;
                    cialoKloca[2, 1] = true;
                    cialoKloca[1, 1] = true;

                    break;

                case 3: //L-prawo
                    cialoKloca[0, 0] = true;
                    cialoKloca[0, 1] = true;
                    cialoKloca[1, 1] = true;
                    cialoKloca[2, 1] = true;

                    break;
                case 4: // I
                    cialoKloca[1, 0] = true;
                    cialoKloca[1, 1] = true;
                    cialoKloca[1, 2] = true;
                    cialoKloca[1, 3] = true;

                    break;
                case 5:// T
                    cialoKloca[1, 0] = true;
                    cialoKloca[1, 1] = true;
                    cialoKloca[0, 1] = true;
                    cialoKloca[2, 1] = true;

                    break;
                case 6://Z
                    cialoKloca[1, 0] = true;
                    cialoKloca[1, 1] = true;
                    cialoKloca[0, 0] = true;
                    cialoKloca[2, 1] = true;

                    break;
                case 7://S
                    cialoKloca[1, 0] = true;
                    cialoKloca[0, 1] = true;
                    cialoKloca[1, 1] = true;
                    cialoKloca[2, 0] = true;

                    break;


            }
            
        }
        public Kloc(int t_rodzaj, int t_size,int t_wersja, SolidBrush[] kolorKloca) : this(t_rodzaj,t_size, kolorKloca)
        {
            wersja = t_wersja;
        }

        public void printKloc(Graphics plansza, int x, int y)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (cialoKloca[i, j])
                    {
                        plansza.FillRectangle(kloc, x + i * 30, y + j * 30, size, size);
                        plansza.DrawRectangle(ramka, x + i * 30, y + j * 30, size, size);
                    }

                }
            }
        }
        public void unprintKloc(Graphics plansza, int x, int y)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (cialoKloca[i, j])
                    {
                        plansza.FillRectangle(tlo, x + i * 30, y + j * 30, size, size);
                        plansza.DrawRectangle(tloPen, x + i * 30, y + j * 30, size, size);
                    }

                }
            }
        }
        public void perma(int x, int y, int[,] permaMapa)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (cialoKloca[i, j])
                    {
                        permaMapa[x / 30 + i, y / 30 + j] = rodzaj;
                    }
                }
            }
        }
        public bool blokadaPerma(int x, int y, int[,] permaMapa)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (cialoKloca[i, j] == true)// jest element
                    {
                        if (permaMapa[x / 30 + i, y / 30 + j + 1] != 0) // zapelniony elemet w permamampie
                        {
                            return false;
                        }

                    }
                }
            }
            return true;
        }
        public bool blokadaL(int x, int y, int par, int[,] permaMapa)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (cialoKloca[i, j] == true)// jest element
                    {
                        if (x + (i + par) * 30 <= 30)
                        {
                            return false;
                        }

                        if (x / 30 + i < 10) // czy zajete w permamapie
                        {
                            if (permaMapa[x / 30 + i- (1-par), y / 30 + j] != 0) //( 1-par)= 0 przy obrocie i  ( 1-par)=1 przy przesunieciu
                            {
                                return false;
                            }
                        }

                    }
                }
            }
            return true;
        }
        public bool blokadaP(int x, int y, int par, int[,] permaMapa)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (cialoKloca[i, j] == true) // jest element
                    {
                        if (x + (i - par) * 30 >= 300)// za daleko w prawo
                        {
                            return false;
                        }
                        if (x / 30 + i  < 10) // czy zajete w permamapie
                        {
                            if (permaMapa[x / 30 + i+(1-par) , y / 30 + j] != 0) //( 1-par)= 0 przy obrocie i  ( 1-par)=1 przy przesunieciu
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public bool blokadaD(int x, int y, int par)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (cialoKloca[i, j] == true)// jest element
                    {
                        if (y + (j - par) * 30 >= 600)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public void obrotL()
        {
            wersja++;
            bool[,] cialoKlocal = new bool[4, 4];

            if (rodzaj == 4) // jest to l 
            {
                if (wersja % 2 == 0) //pion
                {
                    cialoKlocal[1, 0] = true;
                    cialoKlocal[1, 1] = true;
                    cialoKlocal[1, 2] = true;
                    cialoKlocal[1, 3] = true;
                }
                else //poziom
                {
                    cialoKlocal[0, 1] = true;
                    cialoKlocal[1, 1] = true;
                    cialoKlocal[2, 1] = true;
                    cialoKlocal[3, 1] = true;
                }

                cialoKloca = cialoKlocal;
            }
            else if (rodzaj != 1) // nie jest kostka
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        cialoKlocal[2 - i, j] = cialoKloca[j, i];
                    }
                }
                cialoKloca = cialoKlocal;
            }


        }
        public void obrotP()
        {
            wersja++;
            bool[,] cialoKlocal = new bool[4, 4];
            if (rodzaj == 4) // jest to l 
            {
                if (wersja % 2 == 0) //pion
                {
                    cialoKlocal[1, 0] = true;
                    cialoKlocal[1, 1] = true;
                    cialoKlocal[1, 2] = true;
                    cialoKlocal[1, 3] = true;

                }
                else //poziom
                {
                    cialoKlocal[0, 1] = true;
                    cialoKlocal[1, 1] = true;
                    cialoKlocal[2, 1] = true;
                    cialoKlocal[3, 1] = true;
                }
                cialoKloca = cialoKlocal;
            }
            else if (rodzaj != 1) // nie jest kostka
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        cialoKlocal[i, 2 - j] = cialoKloca[j, i];
                    }
                }
                cialoKloca = cialoKlocal;
            }
        }
        public int getRodzaj (){ return rodzaj; }

      
    }
}
