using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeTekno.DATA
{
    public class Siparis
    {
        public int MasaNo { get; set; }
        public SiparisDurum Durum { get; set; }
        public decimal OdenenTutar { get; set; }
        public DateTime? AcilisZamani { get; set; } = DateTime.Now;
        public DateTime? KapanisZamani { get; set; }
        public List<SiparisDetay> siparisDetaylar { get; set; } = new List<SiparisDetay>();
        public string ToplamTutarTl => $"₺{ToplamTutar():n2}";
        public decimal ToplamTutar()
        {
            return siparisDetaylar.Sum(x => x.Tutar());
        }
    }
}
