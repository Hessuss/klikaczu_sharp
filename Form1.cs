using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Runtime.InteropServices;
using klikaczu_sharp.Properties;
using NDde.Client;

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
        int xdest = 0;
        int ydest = 0;
        long color = 0;
        long colorzer = 0;
        long colorczek = 0;
        System.Diagnostics.Process prc = new System.Diagnostics.Process();// do odpalania przeglądarki
        IntPtr dc = GetWindowDC(IntPtr.Zero);
        Random rnd = new Random();
        Point position = System.Windows.Forms.Control.MousePosition;
        Form2 n = new Form2();
        Boolean konfigurowanie = false;
        string[] zestaw = new string[3] { "", "", "" };

        public static void LeftClick(int x, int y)
        {
            System.Threading.Thread.Sleep(new Random().Next(1, 100));
            Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            System.Threading.Thread.Sleep(new Random().Next(36, 58));
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
        }

        public static void Spadaj(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
        }

        public Form1()
        {
            InitializeComponent();

            licznik = 0;
            n.TopLevel = true;
            n.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            position = System.Windows.Forms.Control.MousePosition;
            color = GetPixel(dc, position.X-1, position.Y-1);
            label3.Text = "X: " + position.X + "  Y: " + position.Y + " K:" + color.ToString();   
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (konfigurowanie == false)
            {
                lblMousePosition.Text = licznik.ToString();
                n.label1.Text = licznik.ToString();
                licznik++;
                praca();
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
            if (timer1.Enabled == true)
            {
                btnLukanie.BackColor = Color.GreenYellow;
            }
            else
            {
                btnLukanie.BackColor = SystemColors.ControlLight;

            }

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

            ini.Write("klikaczu", "refbtnlgx", refbtnlgx.Text);
            ini.Write("klikaczu", "refbtnlgy", refbtnlgy.Text);
            ini.Write("klikaczu", "refbtnpdx", refbtnpdx.Text);
            ini.Write("klikaczu", "refbtnpdy", refbtnpdy.Text);

            ini.Write("klikaczu", "orix", orix.Text);
            ini.Write("klikaczu", "oriy", oriy.Text);
            ini.Write("klikaczu", "orik", orik.Text);

            ini.Write("klikaczu", "czatlgx", czatlgx.Text);
            ini.Write("klikaczu", "czatlgy", czatlgy.Text);
            ini.Write("klikaczu", "czatpdx", czatpdx.Text);
            ini.Write("klikaczu", "czatpdy", czatpdy.Text);

            ini.Write("klikaczu", "friklgx", friklgx.Text);
            ini.Write("klikaczu", "friklgy", friklgy.Text);
            ini.Write("klikaczu", "frikpdx", frikpdx.Text);
            ini.Write("klikaczu", "frikpdy", frikpdy.Text);

            ini.Write("klikaczu", "boksx", boksx.Text);
            ini.Write("klikaczu", "boksy", boksy.Text);
            ini.Write("klikaczu", "boksk", boksk.Text);

            ini.Write("klikaczu", "ptakx", ptakx.Text);
            ini.Write("klikaczu", "ptaky", ptaky.Text);
            ini.Write("klikaczu", "ptakk", ptakk.Text);

            ini.Write("klikaczu", "ramx", niebx.Text);
            ini.Write("klikaczu", "ramy", nieby.Text);
            ini.Write("klikaczu", "ramk", niebk.Text);

            ini.Write("klikaczu", "poblgx", poblgx.Text);
            ini.Write("klikaczu", "poblgy", poblgy.Text);
            ini.Write("klikaczu", "pobpdx", pobpdx.Text);
            ini.Write("klikaczu", "pobpdy", pobpdy.Text);

            ini.Write("klikaczu", "autolgx", autolgx.Text);
            ini.Write("klikaczu", "autolgy", autolgy.Text);
            ini.Write("klikaczu", "autopdx", autopdx.Text);
            ini.Write("klikaczu", "autopdy", autopdy.Text);

            ini.Write("klikaczu", "kwolgx", kwolgx.Text);
            ini.Write("klikaczu", "kwolgy", kwolgy.Text);
            ini.Write("klikaczu", "kwopdx", kwopdx.Text);
            ini.Write("klikaczu", "kwopdy", kwopdy.Text);
            ini.Write("klikaczu", "kwok", kwok.Text);

            ini.Write("klikaczu", "pejlgx", pejlgx.Text);
            ini.Write("klikaczu", "pejlgy", pejlgy.Text);
            ini.Write("klikaczu", "pejpdx", pejpdx.Text);
            ini.Write("klikaczu", "pejpdy", pejpdy.Text);
            ini.Write("klikaczu", "pejk", pejk.Text);

            ini.Write("klikaczu", "odpallgx", odpallgx.Text);
            ini.Write("klikaczu", "odpallgy", odpallgy.Text);
            ini.Write("klikaczu", "odpalpdx", odpalpdx.Text);
            ini.Write("klikaczu", "odpalpdy", odpalpdy.Text);

            ini.Write("klikaczu", "kuniecx", kuniecx.Text);
            ini.Write("klikaczu", "kuniecy", kuniecy.Text);
            ini.Write("klikaczu", "kunieck", kunieck.Text);

            ini.Write("klikaczu", "klolslgx", klolslgx.Text);
            ini.Write("klikaczu", "klolslgy", klolslgy.Text);
            ini.Write("klikaczu", "klolspdx", klolspdx.Text);
            ini.Write("klikaczu", "klolspdy", klolspdy.Text);

            ini.Write("klikaczu", "makslgx", makslgx.Text);
            ini.Write("klikaczu", "makslgy", makslgy.Text);
            ini.Write("klikaczu", "makspdx", makspdx.Text);
            ini.Write("klikaczu", "makspdy", makspdy.Text);

            ini.Write("klikaczu", "czenslgx", czenslgx.Text);
            ini.Write("klikaczu", "czenslgy", czenslgy.Text);
            ini.Write("klikaczu", "czenspdx", czenspdx.Text);
            ini.Write("klikaczu", "czenspdy", czenspdy.Text);

            ini.Write("klikaczu", "manlgx", manlgx.Text);
            ini.Write("klikaczu", "manlgy", manlgy.Text);
            ini.Write("klikaczu", "manpdx", manpdx.Text);
            ini.Write("klikaczu", "manpdy", manpdy.Text);

            ini.Write("klikaczu", "rolgx", rolgx.Text);
            ini.Write("klikaczu", "rolgy", rolgy.Text);
            ini.Write("klikaczu", "ropdx", ropdx.Text);
            ini.Write("klikaczu", "ropdy", ropdy.Text);

            ini.Write("klikaczu", "zamlgx", zamlgx.Text);
            ini.Write("klikaczu", "zamlgy", zamlgy.Text);
            ini.Write("klikaczu", "zampdx", zampdx.Text);
            ini.Write("klikaczu", "zampdy", zampdy.Text);
            
            ini.Write("klikaczu", "rundki", rundki.Text);

            ini.Write("klikaczu", "eklgx", eklgx.Text);
            ini.Write("klikaczu", "eklgy", eklgy.Text);
            ini.Write("klikaczu", "ekpdx", ekpdx.Text);
            ini.Write("klikaczu", "ekpdy", ekpdy.Text);

            ini.Write("klikaczu", "przegladarka", przegladarka.Text);

            ini.Write("klikaczu", "samozapylacz", samozapylacz.Checked.ToString());
        }

        private void siorb_dane()
        {
            INIFile ini = new INIFile("./klikaczu.ini");

            refbtnlgx.Text = ini.Read("klikaczu", "refbtnlgx");
            refbtnlgy.Text = ini.Read("klikaczu", "refbtnlgy");
            refbtnpdx.Text = ini.Read("klikaczu", "refbtnpdx");
            refbtnpdy.Text = ini.Read("klikaczu", "refbtnpdy");

            orix.Text = ini.Read("klikaczu", "orix");
            oriy.Text = ini.Read("klikaczu", "oriy");
            orik.Text = ini.Read("klikaczu", "orik");

            czatlgx.Text = ini.Read("klikaczu", "czatlgx");
            czatlgy.Text = ini.Read("klikaczu", "czatlgy");
            czatpdx.Text = ini.Read("klikaczu", "czatpdx");
            czatpdy.Text = ini.Read("klikaczu", "czatpdy");

            friklgx.Text = ini.Read("klikaczu", "friklgx");
            friklgy.Text = ini.Read("klikaczu", "friklgy");
            frikpdx.Text = ini.Read("klikaczu", "frikpdx");
            frikpdy.Text = ini.Read("klikaczu", "frikpdy");

            boksx.Text = ini.Read("klikaczu", "boksx");
            boksy.Text = ini.Read("klikaczu", "boksy");
            boksk.Text = ini.Read("klikaczu", "boksk");

            ptakx.Text = ini.Read("klikaczu", "ptakx");
            ptaky.Text = ini.Read("klikaczu", "ptaky");
            ptakk.Text = ini.Read("klikaczu", "ptakk");

            niebx.Text = ini.Read("klikaczu", "ramx");
            nieby.Text = ini.Read("klikaczu", "ramy");
            niebk.Text = ini.Read("klikaczu", "ramk");

            poblgx.Text = ini.Read("klikaczu", "poblgx");
            poblgy.Text = ini.Read("klikaczu", "poblgy");
            pobpdx.Text = ini.Read("klikaczu", "pobpdx");
            pobpdy.Text = ini.Read("klikaczu", "pobpdy");

            autolgx.Text = ini.Read("klikaczu", "autolgx");
            autolgy.Text = ini.Read("klikaczu", "autolgy");
            autopdx.Text = ini.Read("klikaczu", "autopdx");
            autopdy.Text = ini.Read("klikaczu", "autopdy");

            kwolgx.Text = ini.Read("klikaczu", "kwolgx");
            kwolgy.Text = ini.Read("klikaczu", "kwolgy");
            kwopdx.Text = ini.Read("klikaczu", "kwopdx");
            kwopdy.Text = ini.Read("klikaczu", "kwopdy");
            kwok.Text = ini.Read("klikaczu", "kwok");

            pejlgx.Text = ini.Read("klikaczu", "pejlgx");
            pejlgy.Text = ini.Read("klikaczu", "pejlgy");
            pejpdx.Text = ini.Read("klikaczu", "pejpdx");
            pejpdy.Text = ini.Read("klikaczu", "pejpdy");
            pejk.Text = ini.Read("klikaczu", "pejk");

            odpallgx.Text = ini.Read("klikaczu", "odpallgx");
            odpallgy.Text = ini.Read("klikaczu", "odpallgy");
            odpalpdx.Text = ini.Read("klikaczu", "odpalpdx");
            odpalpdy.Text = ini.Read("klikaczu", "odpalpdy");

            kuniecx.Text = ini.Read("klikaczu", "kuniecx");
            kuniecy.Text = ini.Read("klikaczu", "kuniecy");
            kunieck.Text = ini.Read("klikaczu", "kunieck");

            klolslgx.Text = ini.Read("klikaczu", "klolslgx");
            klolslgy.Text = ini.Read("klikaczu", "klolslgy");
            klolspdx.Text = ini.Read("klikaczu", "klolspdx");
            klolspdy.Text = ini.Read("klikaczu", "klolspdy");

            makslgx.Text = ini.Read("klikaczu", "makslgx");
            makslgy.Text = ini.Read("klikaczu", "makslgy");
            makspdx.Text = ini.Read("klikaczu", "makspdx");
            makspdy.Text = ini.Read("klikaczu", "makspdy");

            czenslgx.Text = ini.Read("klikaczu", "czenslgx");
            czenslgy.Text = ini.Read("klikaczu", "czenslgy");
            czenspdx.Text = ini.Read("klikaczu", "czenspdx");
            czenspdy.Text = ini.Read("klikaczu", "czenspdy");

            manlgx.Text = ini.Read("klikaczu", "manlgx");
            manlgy.Text = ini.Read("klikaczu", "manlgy");
            manpdx.Text = ini.Read("klikaczu", "manpdx");
            manpdy.Text = ini.Read("klikaczu", "manpdy");

            rolgx.Text = ini.Read("klikaczu", "rolgx");
            rolgy.Text = ini.Read("klikaczu", "rolgy");
            ropdx.Text = ini.Read("klikaczu", "ropdx");
            ropdy.Text = ini.Read("klikaczu", "ropdy");

            zamlgx.Text = ini.Read("klikaczu", "zamlgx");
            zamlgy.Text = ini.Read("klikaczu", "zamlgy");
            zampdx.Text = ini.Read("klikaczu", "zampdx");
            zampdy.Text = ini.Read("klikaczu", "zampdy");

            rundki.Text = ini.Read("klikaczu", "rundki");

            eklgx.Text = ini.Read("klikaczu", "eklgx");
            eklgy.Text = ini.Read("klikaczu", "eklgy");
            ekpdx.Text = ini.Read("klikaczu", "ekpdx");
            ekpdy.Text = ini.Read("klikaczu", "ekpdy");

            przegladarka.Text = ini.Read("klikaczu", "przegladarka");

            if (ini.Read("klikaczu", "samozapylacz") == "true")
            {
                samozapylacz.Checked = true;
            }
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
                    label1.Text = "przycisk refresh przeglądarki LG";
                    if (zestaw[0] != "")
                    {
                        refbtnlgx.Text = zestaw[0];
                        refbtnlgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 2:
                    label1.Text = "przycisk refresh przeglądarki PD";
                    if (zestaw[0] != "")
                    {
                        refbtnpdx.Text = zestaw[0];
                        refbtnpdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 3:
                    label1.Text = "Punkt orientacyjny że refreszło";
                    if (zestaw[0] != "")
                    {
                        orix.Text = zestaw[0];
                        oriy.Text = zestaw[1];
                        orik.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 4:
                    label1.Text = "Zamknięcie czatu LG";
                    if (zestaw[0] != "")
                    {
                        czatlgx.Text = zestaw[0];
                        czatlgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 5:
                    label1.Text = "Zamknięcie czatu pd";
                    if (zestaw[0] != "")
                    {
                        czatpdx.Text = zestaw[0];
                        czatpdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 6:
                    label1.Text = "frikojn LG";
                    if (zestaw[0] != "")
                    {
                        friklgx.Text = zestaw[0];
                        friklgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 7:
                    label1.Text = "frikojn PD";
                    if (zestaw[0] != "")
                    {
                        frikpdx.Text = zestaw[0];
                        frikpdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 8:
                    label1.Text = "Srodek kwadratu od kapczy";
                    if (zestaw[0] != "")
                    {
                        boksx.Text = zestaw[0];
                        boksy.Text = zestaw[1];
                        boksk.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 9:
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
                case 10:
                    label1.Text = "przycisk pobierania kasiory LG";
                    if (zestaw[0] != "")
                    {
                        poblgx.Text = zestaw[0];
                        poblgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 11:
                    label1.Text = "przycisk pobierania kasiory PD";
                    if (zestaw[0] != "")
                    {
                        pobpdx.Text = zestaw[0];
                        pobpdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 12:
                    label1.Text = "przycisk autobecing LG";
                    if (zestaw[0] != "")
                    {
                        autolgx.Text = zestaw[0];
                        autolgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 13:
                    label1.Text = "przycisk autobecing PD";
                    if (zestaw[0] != "")
                    {
                        autopdx.Text = zestaw[0];
                        autopdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 14:
                    label1.Text = "kwota zakladu LG";
                    if (zestaw[0] != "")
                    {
                        kwolgx.Text = zestaw[0];
                        kwolgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 15:
                    label1.Text = "kwota zakladu PD";
                    if (zestaw[0] != "")
                    {
                        kwopdx.Text = zestaw[0];
                        kwopdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 16:
                    label1.Text = "pejałt LG";
                    if (zestaw[0] != "")
                    {
                        pejlgx.Text = zestaw[0];
                        pejlgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 17:
                    label1.Text = "pejałt PD";
                    if (zestaw[0] != "")
                    {
                        pejpdx.Text = zestaw[0];
                        pejpdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 18:
                    label1.Text = "odpalanie losowania LG";
                    if (zestaw[0] != "")
                    {
                        odpallgx.Text = zestaw[0];
                        odpallgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 19:
                    label1.Text = "odpalanie losowania PD";
                    if (zestaw[0] != "")
                    {
                        odpalpdx.Text = zestaw[0];
                        odpalpdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 20:
                    label1.Text = "punkt że koniec kasy";
                    if (zestaw[0] != "")
                    {
                        kuniecx.Text = zestaw[0];
                        kuniecy.Text = zestaw[1];
                        kunieck.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 21:
                    label1.Text = "zamykanie konca kasy LG";
                    if (zestaw[0] != "")
                    {
                        klolslgx.Text = zestaw[0];
                        klolslgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 22:
                    label1.Text = "zamykanie konca kasy PD";
                    if (zestaw[0] != "")
                    {
                        klolspdx.Text = zestaw[0];
                        klolspdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 23:
                    label1.Text = "maksik LG";
                    if (zestaw[0] != "")
                    {
                        makslgx.Text = zestaw[0];
                        makslgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 24:
                    label1.Text = "maksik PD";
                    if (zestaw[0] != "")
                    {
                        makspdx.Text = zestaw[0];
                        makspdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 25:
                    label1.Text = "czens LG";
                    if (zestaw[0] != "")
                    {
                        czenslgx.Text = zestaw[0];
                        czenslgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 26:
                    label1.Text = "czens PD";
                    if (zestaw[0] != "")
                    {
                        czenspdx.Text = zestaw[0];
                        czenspdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 27:
                    label1.Text = "ręczne LG";
                    if (zestaw[0] != "")
                    {
                        manlgx.Text = zestaw[0];
                        manlgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 28:
                    label1.Text = "reczne PD";
                    if (zestaw[0] != "")
                    {
                        manpdx.Text = zestaw[0];
                        manpdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 29:
                    label1.Text = "rolujgo LG";
                    if (zestaw[0] != "")
                    {
                        rolgx.Text = zestaw[0];
                        rolgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 30:
                    label1.Text = "rolujgo PD";
                    if (zestaw[0] != "")
                    {
                        ropdx.Text = zestaw[0];
                        ropdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 31:
                    label1.Text = "zamykanie przegladarki LG";
                    if (zestaw[0] != "")
                    {
                        zamlgx.Text = zestaw[0];
                        zamlgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 32:
                    label1.Text = "zamykanie przegladarki PD";
                    if (zestaw[0] != "")
                    {
                        zampdx.Text = zestaw[0];
                        zampdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 33:
                    label1.Text = " !!! niebieskie tło przycisku startowania pilocia";
                    if (zestaw[0] != "")
                    {
                        orik.Text = zestaw[2];
                        kunieck.Text = zestaw[2];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 34:
                    label1.Text = "ekran LG";
                    if (zestaw[0] != "")
                    {
                        eklgx.Text = zestaw[0];
                        eklgy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 35:
                    label1.Text = "ekran PD";
                    if (zestaw[0] != "")
                    {
                        ekpdx.Text = zestaw[0];
                        ekpdy.Text = zestaw[1];
                        zestawClr();
                        licznik++;
                    }
                    break;
                case 36:
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

        private void loguj()
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"klikaczu.log", true))
            {
                file.WriteLine(DateTime.Now.ToString() + " RND_" + rundka.ToString() + " " + label1.Text);
            }
        }

        private async void praca()
        {
            switch (licznik)
                {
                case 1:
                        label1.Text = (DateTime.Now.ToString() + " RND_" + rundka.ToString());
                        loguj();
                    break;
                // łejtuje 5 tików zanim odpale maszyne
                case 5:
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        label1.Text = "odpalam przeglądarkie";
                    loguj();
                    try
                        {
                            System.Diagnostics.Process.Start(@przegladarka.Text, "https://www.bitsler.com/play/dice/btc");
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);                     
                        }
                    break;
                    
                    case 10: // refresz przegladary
                        label1.Text = "jadymy na refresz";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja
                        xdest = rnd.Next(int.Parse(refbtnlgx.Text), int.Parse(refbtnpdx.Text));
                        ydest = rnd.Next(int.Parse(refbtnlgy.Text), int.Parse(refbtnpdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        if (rundka > 1) { 
                            LeftClick(xdest, ydest);
                        }
                        break;
                    case 20:// suwamy myszę z miejsca sprawdzania kolorku
                        label1.Text = "suwam mysze ";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        xdest = rnd.Next(int.Parse(eklgx.Text), int.Parse(ekpdx.Text));
                        ydest = rnd.Next(0, 100);
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        break;
                    case 30://sprawdzamy czy się refreszło
                        label1.Text = "sprawdzam czy się refreszło ";
                    loguj();
                    colorczek = GetPixel(dc, int.Parse(orix.Text), int.Parse(oriy.Text));
                        n.label4.Text = colorczek.ToString();
                        if (colorczek.ToString() != orik.Text)
                        {
                            licznik = licznik-5; // zmniejsza licznik żeby timer robił pętlę bo pętla do-while i sleepy freezują progsa
                            label1.Text = "nie refreszło ";
                        loguj();
                    }
                        else
                        {
                            label1.Text = "refreszło się ";
                        loguj();
                    }
                        break;
                    case 40: //zamykamy czacik
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja
                        xdest = rnd.Next(int.Parse(czatlgx.Text), int.Parse(czatpdx.Text));
                        ydest = rnd.Next(int.Parse(czatlgy.Text), int.Parse(czatpdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
                        label1.Text = "klikłem klołsczat";
                    loguj();
                    break;
                    case 42: // klikanie ikony get fałcet
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja
                        xdest = rnd.Next(int.Parse(friklgx.Text), int.Parse(frikpdx.Text));
                        ydest = rnd.Next(int.Parse(friklgy.Text), int.Parse(frikpdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
                        label1.Text = "klikłem get facio";
                    loguj();
                    break;
                    case 48://sprawdzamy czy sięczekło
                        label1.Text = "sprawdzam czy jest okienko z boksikiem ";
                    loguj();
                    colorczek = GetPixel(dc, int.Parse(ptakx.Text), int.Parse(ptaky.Text));
                        n.label4.Text = colorczek.ToString();
                        if (colorczek.ToString() != boksk.Text)
                        {
                            licznik = licznik - 5; // zmniejsza licznik żeby timer robił pętlę bo pętla do-while i sleepy freezują progsa
                            label1.Text = "nie ma boksika ";
                        loguj();
                    }
                        else
                        {
                            label1.Text = "jest boksik ";
                        loguj();
                    }
                        break;
                    case 60: //klikanie boksika że jestem człekiem
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja
                        xdest = rnd.Next(int.Parse(boksx.Text)-8, int.Parse(boksx.Text)+8);
                        ydest = rnd.Next(int.Parse(boksy.Text)-8, int.Parse(boksy.Text)+8);
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
                        label1.Text = "klikłem boksika";
                    loguj();
                    break;
                    case 62:// suwamy myszę z miejsca sprawdzania kolorku
                        label1.Text = "suwam mysze ";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        xdest = rnd.Next(int.Parse(poblgx.Text), int.Parse(pobpdx.Text));
                        ydest = rnd.Next(int.Parse(poblgy.Text), int.Parse(pobpdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        break;
                    case int licz when (licz < 80 && licz >= 63)://sprawdzamy czy sięczekło
                    label1.Text = "sprawdzam czy się czekło ";
                    loguj();
                    colorczek = GetPixel(dc, int.Parse(ptakx.Text), int.Parse(ptaky.Text));
                        n.label4.Text = colorczek.ToString();
                        if (colorczek.ToString() == ptakk.Text)
                        {
                            label1.Text = "czekło się ";
                        loguj();
                        licznik = 81;
                        } 
                        else
                        {
                            colorczek = GetPixel(dc, int.Parse(niebx.Text), int.Parse(nieby.Text));
                            if (colorczek.ToString() == niebk.Text)
                            {
                                // wyjebało obrazki wiec wypierdalamy softa
                                label1.Text = "niebieska kapcza ";
                            loguj();
                        }
                            else
                            {
                                licznik = licznik - 5; // zmniejsza licznik żeby timer robił pętlę bo pętla do-while i sleepy freezują progsa
                                label1.Text = "nie czekło ";
                            loguj();
                        }
                            //licznik--;
                        }
                        break;
                    case 80:
                        // pozycja
                        xdest = rnd.Next(int.Parse(zamlgx.Text), int.Parse(zampdx.Text));
                        ydest = rnd.Next(int.Parse(zamlgy.Text), int.Parse(zampdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
                        Application.Exit();
                        break;
                    case 90: //klikanie przycisku getfałcet na okienku czy jestem człekiem
                        label1.Text = "klikam gecia ";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja ustawiliśmy się jużwcześniej
                        //xdest = rnd.Next(int.Parse(poblgx.Text), int.Parse(pobpdx.Text));
                        //ydest = rnd.Next(int.Parse(poblgy.Text), int.Parse(pobpdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
                        break;
                    case 100: //sprawdzanie czy okienko sprawdzające czy jestem człekiem zniknęło
                        // sprawdzam czy ptaszor nadal jest ptaszorem
                        label1.Text = "sprawdzam czy gecio się przemielił ";
                    loguj();
                    colorczek = GetPixel(dc, int.Parse(ptakx.Text), int.Parse(ptaky.Text));
                        n.label4.Text = colorczek.ToString();
                        if (colorczek.ToString() == ptakk.Text)
                        {
                            licznik--; // zmniejsza licznik żeby timer robił pętlę bo pętla do-while i sleepy freezują progsa
                            label1.Text = "jeszcze mieli ";
                        loguj();
                    }
                        else
                        {
                            label1.Text = "przemielił ";
                        loguj();
                    }
                        break;
                    case 110:
                        // klikamy ałtobecing
                        label1.Text = "klikam ałtobecia ";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja
                        xdest = rnd.Next(int.Parse(autolgx.Text), int.Parse(autopdx.Text));
                        ydest = rnd.Next(int.Parse(autolgy.Text), int.Parse(autopdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
                        break;
                    case 120:
                        // klikamy kwotke
                        label1.Text = "klikam kwotke ";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja
                        xdest = rnd.Next(int.Parse(kwolgx.Text), int.Parse(kwopdx.Text));
                        ydest = rnd.Next(int.Parse(kwolgy.Text), int.Parse(kwopdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
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
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja
                        xdest = rnd.Next(int.Parse(pejlgx.Text), int.Parse(pejpdx.Text));
                        ydest = rnd.Next(int.Parse(pejlgy.Text), int.Parse(pejpdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        //wpisuje kwoczke
                        SendKeys.Send(pejk.Text);
                        break;
                    case 140:
                        // odpalamy losowanie
                        label1.Text = "odpalamy losowanie ";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja
                        xdest = rnd.Next(int.Parse(odpallgx.Text), int.Parse(odpalpdx.Text));
                        ydest = rnd.Next(int.Parse(odpallgy.Text), int.Parse(odpalpdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
                        //losowy łejt
                        System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        //await HumanWindMouse(rnd.Next(50, 100), rnd.Next(50, 100));
                        break;
                case 144:
                    label1.Text = "czekam az wypstryka sie z kasy";
                    loguj();
                    break;
                    case 150:
                        // czekamy aż wypstryka się z kasy
                        label1.Text = "czekam az wypstryka sie z kasy";
                        colorzer = GetPixel(dc, int.Parse(kuniecx.Text), int.Parse(kuniecy.Text));
                        n.label4.Text = colorzer.ToString();
                        //label1.Text = colorzer.ToString();
                        if (colorzer.ToString() == kunieck.Text)
                        {
                            //zamykamy modalkiowe okienko
                            //losowy łejt
                            System.Threading.Thread.Sleep(rnd.Next(1, 500));
                            // pozycja
                            xdest = rnd.Next(int.Parse(klolslgx.Text), int.Parse(klolspdx.Text));
                            ydest = rnd.Next(int.Parse(klolslgy.Text), int.Parse(klolspdy.Text));
                            //jadymy
                            await HumanWindMouse(xdest, ydest);
                            //klikanie
                            LeftClick(xdest, ydest);
                            System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        }
                        else
                        {
                            licznik = licznik - 5;
                            if (rnd.Next(1, 50) < 3)
                            {
                                label1.Text = "bede machal";
                                for (int i = 1; i <= rnd.Next(1, 3); i++)
                                {
                                    label1.Text = "macham "+i;
                                    xdest = rnd.Next(int.Parse(eklgx.Text), int.Parse(ekpdx.Text));
                                    ydest = rnd.Next(int.Parse(eklgy.Text), int.Parse(ekpdy.Text));
                                    //jadymy
                                    await HumanWindMouse(xdest, ydest);
                                }
                            }
                        }
                        break;
                    case 160:// klikamy maksiora
                        label1.Text = "klikamy maksiora";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja
                        xdest = rnd.Next(int.Parse(makslgx.Text), int.Parse(makspdx.Text));
                        ydest = rnd.Next(int.Parse(makslgy.Text), int.Parse(makspdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
                        break;
                    case 170:// klikamy czensa
                        label1.Text = "klikamy czensa";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja
                        xdest = rnd.Next(int.Parse(czenslgx.Text), int.Parse(czenspdx.Text));
                        ydest = rnd.Next(int.Parse(czenslgy.Text), int.Parse(czenspdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
                        // i wpisujemy najmniejsza wartosc
                        SendKeys.Send("00.01");
                        break;
                    case 180:// klikamy manbata
                        label1.Text = "klikamy manbata";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                        // pozycja
                        xdest = rnd.Next(int.Parse(manlgx.Text), int.Parse(manpdx.Text));
                        ydest = rnd.Next(int.Parse(manlgy.Text), int.Parse(manpdy.Text));
                        //jadymy
                        await HumanWindMouse(xdest, ydest);
                        //klikanie
                        LeftClick(xdest, ydest);
                    break;
                case 190:// klikamy rolkę
                    label1.Text = "klikamy rolkę";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                    // pozycja
                    xdest = rnd.Next(int.Parse(rolgx.Text), int.Parse(ropdx.Text));
                    ydest = rnd.Next(int.Parse(rolgy.Text), int.Parse(ropdy.Text));
                    //jadymy
                    await HumanWindMouse(xdest, ydest);
                    //klikanie
                    LeftClick(xdest, ydest);
                    break;
                case 195:// klikamy rolkę
                    label1.Text = "klikamy żeśmy siury";
                    loguj();
                    //losowy łejt
                    System.Threading.Thread.Sleep(rnd.Next(1, 500));
                    // pozycja
                    //xdest = rnd.Next(int.Parse(rolgx.Text), int.Parse(ropdx.Text));
                    //ydest = rnd.Next(int.Parse(rolgy.Text), int.Parse(ropdy.Text));
                    //jadymy
                    //await HumanWindMouse(xdest, ydest);
                    //klikanie
                    LeftClick(xdest, ydest);
                    break;
                case 200:// abarot

                        label1.Text = "jedziem od nowa";
                    loguj();
                    label1.Text = DateTime.Now.ToString() + " RND_" + rundka.ToString();
                    loguj();
                    licznik = 6;
                        rundka++;
                        n.label3.Text = "rnd "+rundka.ToString();
                    

                    if (rundka > int.Parse(rundki.Text))
                        {
                            // pozycja
                            xdest = rnd.Next(int.Parse(zamlgx.Text), int.Parse(zampdx.Text));
                            ydest = rnd.Next(int.Parse(zamlgy.Text), int.Parse(zampdy.Text));
                            //jadymy
                            await HumanWindMouse(xdest, ydest);
                            //klikanie
                            LeftClick(xdest, ydest);
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

        private void filesetbtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //System.IO.StreamReader sr = new
                   //System.IO.StreamReader(openFileDialog1.FileName);
                //MessageBox.Show(openFileDialog1.FileName);
                przegladarka.Text = openFileDialog1.FileName;
                //sr.Close();
            }
        }

        private static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public static double Hypot(double x, double y)
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        private async Task HumanWindMouse(double xe, double ye)
        {
            timer2.Enabled = false;
            position = System.Windows.Forms.Control.MousePosition;
            double xs = position.X;
            double ys = position.Y;

            double gravity = 1;
            double wind = 1;
            //double minWait = 0.1;
            //double maxWait = 1;
            double targetArea = 1;
            int _mouseSpeed = 50;

            double veloX = 0,
                veloY = 0,
                windX = 0,
                windY = 0;

            var msp = _mouseSpeed;
            var sqrt2 = Math.Sqrt(2);
            var sqrt3 = Math.Sqrt(3);
            var sqrt5 = Math.Sqrt(5);

            var tDist = (int)Distance(Math.Round(xs), Math.Round(ys), Math.Round(xe), Math.Round(ye));
            var t = (uint)(Environment.TickCount + 10000);

            do
            {
                if (Environment.TickCount > t)
                    break;

                var dist = Hypot(xs - xe, ys - ye);
                wind = Math.Min(wind, dist);

                if (dist < 1)
                    dist = 1;

                var d = (Math.Round(Math.Round((double)tDist) * 0.3) / 7);

                if (d > 25)
                    d = 25;

                if (d < 5)
                    d = 5;

                double rCnc = rnd.Next(6);

                if (rCnc == 1)
                    d = 2;

                double maxStep;

                if (d <= Math.Round(dist))
                    maxStep = d;
                else
                    maxStep = Math.Round(dist);

                if (dist >= targetArea)
                {
                    windX = windX / sqrt3 + (rnd.Next((int)(Math.Round(wind) * 2 + 1)) - wind) / sqrt5;
                    windY = windY / sqrt3 + (rnd.Next((int)(Math.Round(wind) * 2 + 1)) - wind) / sqrt5;
                }
                else
                {
                    windX = windX / sqrt2;
                    windY = windY / sqrt2;
                }

                veloX = veloX + windX;
                veloY = veloY + windY;
                veloX = veloX + gravity * (xe - xs) / dist;
                veloY = veloY + gravity * (ye - ys) / dist;

                if (Hypot(veloX, veloY) > maxStep)
                {
                    var randomDist = maxStep / 2.0 + rnd.Next((int)(Math.Round(maxStep) / 2));
                    var veloMag = Math.Sqrt(veloX * veloX + veloY * veloY);
                    veloX = (veloX / veloMag) * randomDist;
                    veloY = (veloY / veloMag) * randomDist;
                }

                var lastX = (int)Math.Round(xs);
                var lastY = (int)Math.Round(ys);
                xs = xs + veloX;
                ys = ys + veloY;

                

                if (lastX != Math.Round(xs) || (lastY != Math.Round(ys)))
                   Spadaj((int)Math.Round(xs), (int)Math.Round(ys));

                var w = (rnd.Next((int)(Math.Round((double)(100 / msp)))) * 6);

                if (w < 5)
                    w = 5;

                w = (int)Math.Round(w * 0.9);
                await Task.Delay(w);
            } while (!(Hypot(xs - xe, ys - ye) < 1));

            if (Math.Round(xe) != Math.Round(xs) || (Math.Round(ye) != Math.Round(ys)))
                Spadaj((int)Math.Round(xe), (int)Math.Round(ye));

            _mouseSpeed = msp;

            timer2.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Copy window location to app settings
            Settings.Default.WindowLocation = n.Location;

            // Copy window size to app settings
            if (n.WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowSize = n.Size;
            }
            else
            {
                Settings.Default.WindowSize = n.RestoreBounds.Size;
            }

            // Save settings
            Settings.Default.Save();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                bitmap.Save("c:\\test.bmp", ImageFormat.Bmp);
                Color pixelColor = bitmap.GetPixel(int.Parse(szux.Text), int.Parse(szuy.Text));
                this.pictureBox1.BackColor = pixelColor;
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            timer2.Enabled = false;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@przegladarka.Text, "https://www.bitsler.com/play/dice/btc");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            siorb_dane();

            timer2.Enabled = samozapylacz.Checked;
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
