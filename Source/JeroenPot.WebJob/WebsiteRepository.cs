using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JeroenPot.WebJob
{
    public class WebsiteRepository
    {
        public void MakeRequest(Uri url)
        {
            HttpWebRequest web = (HttpWebRequest)WebRequest.Create(url);
            web.Method = "GET";
            CookieContainer cookieJar = new CookieContainer();
            web.CookieContainer = cookieJar;
            using (WebResponse resp = web.GetResponse())
            {
                using (Stream istrm = resp.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(istrm))
                    {
                        sr.ReadToEnd();
                        sr.Close();
                        resp.Close();
                    }
                }
            }
        }
    }
}
