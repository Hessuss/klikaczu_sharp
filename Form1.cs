using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

using System.Runtime.InteropServices;

namespace klikaczu_sharp
{

    public partial class Form1 : Form
    {
        [DllImport("gdi32")]
        public static extern uint GetPixel(IntPtr hDC, int XPos, int YPos);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        // zmienne globalne
        private int _Licznik;
        public int licznik
        {
            
            get { return this._Licznik; }

            set
            {
                this._Licznik = value;
            }
        }

        int rundka = 1; // do pętlenia rundek
        int kurx = 0;
        long color = 0;
        long colorzer = 0;
        long colorczek = 0;
        IntPtr dc = GetWindowDC(IntPtr.Zero);
        Random rnd = new Random();
        Point position = System.Windows.Forms.Control.MousePosition;
        Form2 n = new Form2();
        Boolean konfigurowanie = false;
        string[] zestaw = new string[3] { "", "", "" };

        public static void LeftClick(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            System.Threading.Thread.Sleep(36);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
        }

        public static void Spadaj(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
        }

        public Form1()
        {
            InitializeComponent();

            siorb_dane();

            licznik = 0;
            n.TopLevel = true;
            n.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            position = System.Windows.Forms.Control.MousePosition;
            lblMousePosition.Text = "X: " + position.X +
                "  " +
                "Y: " + position.Y;            

            color = GetPixel(dc, position.X, position.Y);
            label1.Text = color.ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (konfigurowanie == false)
            {
                timer2.Enabled = false;
                lblMousePosition.Text = licznik.ToString();
                n.label1.Text = licznik.ToString();
                licznik++;
                praca();
                timer2.Enabled = true;
            }
            else // konfigurowanie włączone
            {
                lblMousePosition.Text = licznik.ToString();
                konfiguracja();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer2.Enabled = !timer2.Enabled;
            n.label3.Text = "rnd 1";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            INIFile ini = new INIFile("./klikaczu.ini");

            ini.Write("klikaczu", "refbtnx", refbtnx.Text);
            ini.Write("klikaczu", "refbtny", refbtny.Text);

            ini.Write("klikaczu", "refx", refx.Text);
            ini.Write("klikaczu", "refy", refy.Text);
            ini.Write("klikaczu", "refk", refk.Text);

            ini.Write("klikaczu", "czatx", czatx.Text);
            ini.Write("klikaczu", "czaty", czaty.Text);

            ini.Write("klikaczu", "tankx", tankx.Text);
            ini.Write("klikaczu", "tanky", tanky.Text);

            ini.Write("klikaczu", "tankk", tankk.Text);

            ini.Write("klikaczu", "ptakx", ptakx.Text);
            ini.Write("klikaczu", "ptaky", ptaky.Text);
            ini.Write("klikaczu", "ptakk", ptakk.Text);

            ini.Write("klikaczu", "pobx", pobx.Text);
            ini.Write("klikaczu", "poby", poby.Text);

            ini.Write("klikaczu", "odpalk", odpalk.Text);

            ini.Write("klikaczu", "odpalx", odpalx.Text);
            ini.Write("klikaczu", "odpaly", odpaly.Text);

            ini.Write("klikaczu", "autox", autox.Text);
            ini.Write("klikaczu", "autoy", autoy.Text);

            ini.Write("klikaczu", "kwox", kwox.Text);
            ini.Write("klikaczu", "kwoy", kwoy.Text);
            ini.Write("klikaczu", "kwok", kwok.Text);

            ini.Write("klikaczu", "mnox", mnox.Text);
            ini.Write("klikaczu", "mnoy", mnoy.Text);
            ini.Write("klikaczu", "mnok", mnok.Text);

            ini.Write("klikaczu", "kasx", kasx.Text);
            ini.Write("klikaczu", "kasy", kasy.Text);
            ini.Write("klikaczu", "kask", kask.Text);

            ini.Write("klikaczu", "maksx", maksx.Text);
            ini.Write("klikaczu", "maksy", maksy.Text);

            ini.Write("klikaczu", "czensx", czensx.Text);
            ini.Write("klikaczu", "czensy", czensy.Text);

            ini.Write("klikaczu", "manx", manx.Text);
            ini.Write("klikaczu", "many", many.Text);

            ini.Write("klikaczu", "rolx", rolx.Text);
            ini.Write("klikaczu", "roly", roly.Text);

            ini.Write("klikaczu", "zamx", zamx.Text);
            ini.Write("klikaczu", "zamy", zamy.Text);

        }

        private void siorb_dane()
        {
            INIFile ini = new INIFile("./klikaczu.ini");

            refbtnx.Text = ini.Read("klikaczu", "refbtnx");
            refbtny.Text = ini.Read("klikaczu", "refbtny");

            refx.Text = ini.Read("klikaczu", "refx");
            refy.Text = ini.Read("klikaczu", "refy");
            refk.Text = ini.Read("klikaczu", "refk");

            czatx.Text = ini.Read("klikaczu", "czatx");
            czaty.Text = ini.Read("klikaczu", "czaty");

            tankx.Text = ini.Read("klikaczu", "tankx");
            tanky.Text = ini.Read("klikaczu", "tanky");

            tankk.Text = ini.Read("klikaczu", "tankk");

            ptakx.Text = ini.Read("klikaczu", "ptakx");
            ptaky.Text = ini.Read("klikaczu", "ptaky");
            ptakk.Text = ini.Read("klikaczu", "ptakk");

            pobx.Text = ini.Read("klikaczu", "pobx");
            poby.Text = ini.Read("klikaczu", "poby");

            odpalk.Text = ini.Read("klikaczu", "odpalk");

            odpalx.Text = ini.Read("klikaczu", "odpalx");
            odpaly.Text = ini.Read("klikaczu", "odpaly");

            autox.Text = ini.Read("klikaczu", "autox");
            autoy.Text = ini.Read("klikaczu", "autoy");

            kwox.Text = ini.Read("klikaczu", "kwox");
            kwoy.Text = ini.Read("klikaczu", "kwoy");
            kwok.Text = ini.Read("klikaczu", "kwok");

            mnox.Text = ini.Read("klikaczu", "mnox");
            mnoy.Text = ini.Read("klikaczu", "mnoy");
            mnok.Text = ini.Read("klikaczu", "mnok");

            kasx.Text = ini.Read("klikaczu", "kasx");
            kasy.Text = ini.Read("klikaczu", "kasy");
            kask.Text = ini.Read("klikaczu", "kask");

            maksx.Text = ini.Read("klikaczu", "maksx");
            maksy.Text = ini.Read("klikaczu", "maksy");

            czensx.Text = ini.Read("klikaczu", "czensx");
            czensy.Text = ini.Read("klikaczu", "czensy");

            manx.Text = ini.Read("klikaczu", "manx");
            many.Text = ini.Read("klikaczu", "many");

            rolx.Text = ini.Read("klikaczu", "rolx");
            roly.Text = ini.Read("klikaczu", "roly");

            zamx.Text = ini.Read("klikaczu", "zamx");
            zamy.Text = ini.Read("klikaczu", "zamy");
        }


        private void zestawClr()
        {
            zestaw[0] = "";
            zestaw[1] = "";
            zestaw[2] = "";
        }

        private void konfiguracja()
        {
            switch (licznik)
            {
                case 1:
                    label1.Text = "przycisk refresh przeglądarki";
                    if (zestaw[0] != "")
                    {
                        refbtnx.Text = zestaw[0];
                        refbtny.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 2:
                    label1.Text = "Punkt orientacyjny że refreszło";
                    if (zestaw[0] != "")
                    {
                        refx.Text = zestaw[0];
                        refy.Text = zestaw[1];
                        refk.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 3:
                    label1.Text = "Zamknięcie czatu";
                    if (zestaw[0] != "")
                    {
                        czatx.Text = zestaw[0];
                        czaty.Text = zestaw[1];
                        //refk.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 4:
                    label1.Text = "Tankowanie";
                    if (zestaw[0] != "")
                    {
                        tankx.Text = zestaw[0];
                        tanky.Text = zestaw[1];
                        //refk.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 5:
                    label1.Text = "Srodek kwadratu od kapczy";
                    if (zestaw[0] != "")
                    {
                        //tankx.Text = zestaw[0];
                        //tanky.Text = zestaw[1];
                        tankk.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 6:
                    label1.Text = "Zielone na ptaszku";
                    if (zestaw[0] != "")
                    {
                        ptakx.Text = zestaw[0];
                        ptaky.Text = zestaw[1];
                        ptakk.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 7:
                    label1.Text = "przycisk pobierania kasiory";
                    if (zestaw[0] != "")
                    {
                        pobx.Text = zestaw[0];
                        poby.Text = zestaw[1];
                        //ptakk.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 8:
                    label1.Text = "przycisk autobecing";
                    if (zestaw[0] != "")
                    {
                        autox.Text = zestaw[0];
                        autoy.Text = zestaw[1];
                        //ptakk.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 9:
                    label1.Text = "kolorek z przycisku return to bejs";
                    if (zestaw[0] != "")
                    {
                        //odpalx.Text = zestaw[0];
                        //odpaly.Text = zestaw[1];
                        odpalk.Text = zestaw[2];
                        kask.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 10:
                    label1.Text = " !!! niebieskie tło przycisku startowania pilocia";
                    if (zestaw[0] != "")
                    {
                        odpalx.Text = zestaw[0];
                        odpaly.Text = zestaw[1];
                        //ptakk.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 11:
                    label1.Text = "1mm przed kwotą zakładu";
                    if (zestaw[0] != "")
                    {
                        kwox.Text = zestaw[0];
                        kwoy.Text = zestaw[1];
                        //ptakk.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 12:
                    label1.Text = "pejałt";
                    if (zestaw[0] != "")
                    {
                        mnox.Text = zestaw[0];
                        mnoy.Text = zestaw[1];
                        //ptakk.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 13:
                    label1.Text = "mnożnik";
                        zestawClr();
                        licznik++;
                    break;
                case 14:
                    label1.Text = "przycisk ok,kasa się skończyła";
                    if (zestaw[0] != "")
                    {
                        kasx.Text = zestaw[0];
                        kasy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 15:
                    label1.Text = "przycisk maks";
                    if (zestaw[0] != "")
                    {
                        maksx.Text = zestaw[0];
                        maksy.Text = zestaw[1];
                        //kask.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 16:
                    label1.Text = "przycisk czens";
                    if (zestaw[0] != "")
                    {
                        czensx.Text = zestaw[0];
                        czensy.Text = zestaw[1];
                        //kask.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 17:
                    label1.Text = "przycisk pojedynczy zaklad";
                    if (zestaw[0] != "")
                    {
                        manx.Text = zestaw[0];
                        many.Text = zestaw[1];
                        //kask.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 18:
                    label1.Text = "rolujgo";
                    if (zestaw[0] != "")
                    {
                        rolx.Text = zestaw[0];
                        roly.Text = zestaw[1];
                        //kask.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 19:
                    label1.Text = "przycisk zamknięcia przeglądarki";
                    if (zestaw[0] != "")
                    {
                        zamx.Text = zestaw[0];
                        zamy.Text = zestaw[1];
                        //kask.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 20:
                    label1.Text = "konfignięte, zapisz stan!!!";
                    konfigurowanie = !konfigurowanie;
                    timer2.Enabled = !timer2.Enabled;
                    btnKonfig.BackColor = SystemColors.ControlLight;
                    licznik = 0;
                    break;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            licznik = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            siorb_dane();
        }

        private void praca()
        {
                switch (licznik)
                {
                    // łejtuje 5 tików zanim odpale maszyne

                    case 10: // refresz przegladary
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        //klikanie
                        LeftClick(int.Parse(refbtnx.Text), int.Parse(refbtny.Text));
                        label1.Text = "klikłem refresz";
                        break;
                    case 20:// suwamy myszę z miejsca sprawdzania kolorku
                        label1.Text = "suwam mysze ";
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        Spadaj(rnd.Next(50, 200), rnd.Next(50, 200));
                        break;
                    case 30://sprawdzamy czy się refreszło
                        label1.Text = "sprawdzam czy się refreszło ";
                        colorczek = GetPixel(dc, int.Parse(refx.Text), int.Parse(refy.Text));
                        if (colorczek.ToString() != refk.Text)
                        {
                            licznik = licznik-5; // zmniejsza licznik żeby timer robił pętlę bo pętla do-while i sleepy freezują progsa
                            label1.Text = "nie refreszło ";
                        }
                        else
                        {
                            label1.Text = "refreszło się ";
                        }
                        break;
                    case 40: //zamykamy czacik
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        //klikanie
                        LeftClick(int.Parse(czatx.Text), int.Parse(czaty.Text));
                        label1.Text = "klikłem klołsczat";
                        break;
                    case 42: // klikanie ikony get fałcet
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        //klikanie
                        LeftClick(int.Parse(tankx.Text), int.Parse(tanky.Text));
                        label1.Text = "klikłem get facio";
                        break;
                    case 48://sprawdzamy czy sięczekło
                        label1.Text = "sprawdzam czy jest okienko z boksikiem ";
                        colorczek = GetPixel(dc, int.Parse(ptakx.Text), int.Parse(ptaky.Text));
                        if (colorczek.ToString() != tankk.Text)
                        {
                            licznik = licznik - 5; // zmniejsza licznik żeby timer robił pętlę bo pętla do-while i sleepy freezują progsa
                            label1.Text = "nie ma boksika ";
                        }
                        else
                        {
                            label1.Text = "jest boksik ";
                        }
                        break;
                    case 60: //klikanie boksika że jestem człekiem
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        LeftClick(int.Parse(ptakx.Text), int.Parse(ptaky.Text));
                        label1.Text = "klikłem boksika";
                        break;
                    case 62:// suwamy myszę z miejsca sprawdzania kolorku
                        label1.Text = "suwam mysze ";
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        Spadaj(rnd.Next(50, 200), rnd.Next(50, 200));
                        break;
                    case 68://sprawdzamy czy sięczekło
                        label1.Text = "sprawdzam czy się czekło ";
                        colorczek = GetPixel(dc, int.Parse(ptakx.Text), int.Parse(ptaky.Text));
                        if (colorczek.ToString() != ptakk.Text)
                        {
                            licznik = licznik - 5; // zmniejsza licznik żeby timer robił pętlę bo pętla do-while i sleepy freezują progsa
                            label1.Text = "nie czekło ";
                        } else
                        {
                            label1.Text = "czekło się ";
                        }
                        break;
                    case 90: //klikanie przycisku getfałcet na okienku czy jestem człekiem
                        label1.Text = "klikam gecia ";
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        LeftClick(int.Parse(pobx.Text), int.Parse(poby.Text));
                        break;
                    case 100: //sprawdzanie czy okienko sprawdzające czy jestem człekiem zniknęło
                        // sprawdzam czy ptaszor nadal jest ptaszorem
                        label1.Text = "sprawdzam czy gecio się przemielił ";
                        colorczek = GetPixel(dc, int.Parse(ptakx.Text), int.Parse(ptaky.Text));
                        if (colorczek.ToString() == ptakk.Text)
                        {
                            licznik--; // zmniejsza licznik żeby timer robił pętlę bo pętla do-while i sleepy freezują progsa
                            label1.Text = "jeszcze mieli ";
                        }
                        else
                        {
                            label1.Text = "przemielił ";
                        }
                        break;
                    case 110:
                        // klikamy ałtobecing
                        label1.Text = "klikam ałtobecia ";
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        LeftClick(int.Parse(autox.Text), int.Parse(autoy.Text));
                        break;
                    case 120:
                        // klikamy kwotke
                        label1.Text = "klikam kwotke ";
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        LeftClick(int.Parse(kwox.Text), int.Parse(kwoy.Text));
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // sendamy zaznacz wszystko
                        SendKeys.Send("^(a)");
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        //wpisuje kwoczke
                        SendKeys.Send(kwok.Text);
                        break;
                    case 130:
                        // klikamy kwotke
                        label1.Text = "klikam mnowznik ";
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        LeftClick(int.Parse(mnox.Text), int.Parse(mnoy.Text));
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        //wpisuje kwoczke
                        SendKeys.Send(mnok.Text);
                        break;
                    case 140:
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // odpalamy losowanie
                        label1.Text = "odpalamy losowanie ";
                        LeftClick(int.Parse(odpalx.Text), int.Parse(odpaly.Text));
                        Spadaj(12, 15); // spadamy z miejsca sprawdzania koloru
                        break;
                    case 150:
                        // czekamy aż wypstryka się z kasy
                        label1.Text = "czekam az wypstryka sie z kasy";

                        colorzer = GetPixel(dc, int.Parse(kasx.Text), int.Parse(kasy.Text));
                        //label1.Text = colorzer.ToString();
                        if (colorzer.ToString() == kask.Text)
                        {
                            //zamykamy modalkiowe okienko
                            LeftClick(int.Parse(kasx.Text), int.Parse(kasy.Text));
                            System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        } else
                        {
                            licznik = licznik - 5;
                        }
                        break;
                    case 160:// klikamy maksiora
                    label1.Text = "klikamy maksiora";
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // klikamy maksiora
                        LeftClick(int.Parse(maksx.Text), int.Parse(maksy.Text));
                        break;
                    case 170:// klikamy czensa
                    label1.Text = "klikamy czensa";
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // klikamy czensa
                        LeftClick(int.Parse(czensx.Text), int.Parse(czensy.Text));
                        // i wpisujemy najmniejsza wartosc
                        SendKeys.Send("00.01");
                        break;
                    case 180:// klikamy manbata
                    label1.Text = "klikamy manbata";
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // klikamy czensa
                        LeftClick(int.Parse(manx.Text), int.Parse(many.Text));
                        break;
                    case 190:// klikamy rolkę
                    label1.Text = "klikamy rolkę";
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // klikamy czensa
                        LeftClick(int.Parse(rolx.Text), int.Parse(roly.Text));
                        break;
                    case 200:// abarot
                    label1.Text = "jedziem od nowa";
                    licznik = 0;
                        rundka++;
                        n.label3.Text = "rnd "+rundka.ToString();
                        if (rundka > int.Parse(rundki.Text))
                        {
                            LeftClick(int.Parse(zamx.Text), int.Parse(zamy.Text));
                            Application.Exit();
                        }
                        break;
                }
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            lblMousePosition.Text = licznik.ToString();
            praca();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            licznik--;
            lblMousePosition.Text = licznik.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            licznik++;
            lblMousePosition.Text = licznik.ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            lblMousePosition.Text = licznik.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            licznik = int.Parse(textBox1.Text);
        }

        private void label1_TextChanged(object sender, EventArgs e)
        {
            n.label2.Text = this.label1.Text;
        }

            private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.T)       // Ctrl-t
            {
                position = System.Windows.Forms.Control.MousePosition;

                zestaw[0] = position.X.ToString();
                zestaw[1] = position.Y.ToString();
                zestaw[2] = GetPixel(dc, position.X, position.Y).ToString();
            }
        }

        private void konfig_Click(object sender, EventArgs e)
        {
            konfigurowanie = !konfigurowanie;
            timer2.Enabled = !timer2.Enabled;
            if(konfigurowanie == true)
            {
                btnKonfig.BackColor = Color.GreenYellow;
                licznik = 1;
            } else
            {
                btnKonfig.BackColor = SystemColors.ControlLight;
                licznik = 0;
            }
        }



        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.M)       // Ctrl-t
            {
                kurx = rnd.Next(-1, 2);
                label1.Text = kurx.ToString();

                this.Cursor = new Cursor(Cursor.Current.Handle);
                Cursor.Position = new Point(Cursor.Position.X + (kurx), Cursor.Position.Y);
                //Cursor.Clip = new Rectangle(this.Location, this.Size);
            }
        }
    }

    class INIFile
    {

        private string filePath;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
        string key,
        string val,
        string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
        string key,
        string def,
        StringBuilder retVal,
        int size,
        string filePath);

        public INIFile(string filePath)
        {
            this.filePath = filePath;
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value.ToLower(), this.filePath);
        }

        public string Read(string section, string key)
        {
            StringBuilder SB = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", SB, 255, this.filePath);
            return SB.ToString();
        }

        public string FilePath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
        }
    }
}
