using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeTekno.DATA
{
    public class KafeVeri
    {
        public int MasaAdet { get; set; } = 20;
        public List<Urun> urunler { get; set; } = new List<Urun>();
        public List<Siparis> aktifSiparisler { get; set; } = new List<Siparis>();
        public List<Siparis> gecmisSiparisler { get; set; } = new List<Siparis>();
    }
}
