using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace pratikCeviri
{
    public partial class Ceviri : Form
    {
        HttpClient client;
        CookieContainer cookieJar; //istemcimize belki lazım olur diye cerez tekenesi ekliyorum.
        public string ilkDil = "";
        public string ikinciDil = "";
        public string metin = "";
        public Ceviri()
        {
            InitializeComponent();
            Screen ekran = Screen.FromPoint(Location);
            Location = new Point(ekran.WorkingArea.Right - Width, ekran.WorkingArea.Bottom - Height);
            cookieJar = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler()
            {
                CookieContainer = cookieJar
            };
            client = new HttpClient(handler, true);
            client.DefaultRequestHeaders.Add("referer", "https://translate.google.com/");
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.97 Safari/537.36");
        }       
        public async void cevir()
        {
            textBox1.Text = "Çevriliyor...";
            if (string.IsNullOrEmpty(ilkDil))
            {
                HttpResponseMessage resultat = await client.GetAsync("https://translate.google.com/translate_a/single?client=gtx&sl=auto&tl="+ikinciDil+"&hl=tr&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=sos&dt=ss&dt=t&otf=2&ssel=0&tsel=0&xid=45662847&kc=3&tk=502623.70859&q=" + metin);
                parse(await resultat.Content.ReadAsStringAsync());
            }
            else
            {
                HttpResponseMessage resultat = await client.GetAsync("https://translate.google.com/translate_a/single?client=gtx&sl="+ilkDil+"&tl=" + ikinciDil + "&hl=tr&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=sos&dt=ss&dt=t&otf=2&ssel=0&tsel=0&xid=45662847&kc=3&tk=502623.70859&q=" + metin);
                parse(await resultat.Content.ReadAsStringAsync());
            }
        }
        private void parse(string input)
        {
          
            try
            {
                JArray jsonArray = JArray.Parse(input);
                textBox1.Text = (jsonArray[0][0][0].ToString());
            }
            catch (Exception ex) {
                if (input.Contains("bilgisayar ağınızdan gelen"))
                {
                    Text = "Çeviri Hatası";
                    textBox1.Text = "Muhtemelen üst üste seri bi şekilde çeviri yaptınız ve bu sebepten Google bunu bot olarak algıladı. Modeminizi kapatıp 15sn bekleyin ve tekrar açın veya VPN kullanın.";
                }
                else {
                    Text = "Çeviri Hatası";
                    textBox1.Text = ex.Message;
                }
            }
        }      
        private void Ceviri_Load(object sender, EventArgs e)
        {
            cevir();   
        }
    }
}
