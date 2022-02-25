using KafeTekno.DATA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KafeTekno.UI
{
    public partial class SiparisForm : Form
    {
        public event EventHandler<MasaTasindiEventArgs> MasaTasindi;

        private readonly KafeVeri _db;
        private readonly Siparis _siparis;
        private readonly BindingList<SiparisDetay> _blSiparisDetaylar;

        public SiparisForm(KafeVeri db, Siparis siparis)
        {
            _db = db;
            _siparis = siparis;
            _blSiparisDetaylar = new BindingList<SiparisDetay>(_siparis.siparisDetaylar);
            InitializeComponent();
            cboUrun.DataSource = _db.urunler;
            dgvSiparis.DataSource = _blSiparisDetaylar;
            _blSiparisDetaylar.ListChanged += _blSiparisDetaylar_ListChanged;
            MasaNoGuncelle();
            OdemeTutariniGuncelle();
        }

        private void _blSiparisDetaylar_ListChanged(object sender, ListChangedEventArgs e)
        {
            OdemeTutariniGuncelle();
        }

        private void OdemeTutariniGuncelle()
        {
            lblOdemeTutari.Text = _siparis.ToplamTutarTl;
        }

        private void MasaNoGuncelle()
        {
            this.Text = $"Masa {_siparis.MasaNo:00} (Açılış Zamanı: {_siparis.AcilisZamani})";
            lblMasaNo.Text = _siparis.MasaNo.ToString("00");

            cmoMasaNo.DataSource = Enumerable.Range(1, _db.MasaAdet)
                .Where(x => !_db.aktifSiparisler
                .Any(s => s.MasaNo == x))
                .ToList();
            //cmoMasaNo.Items.Clear();
            //for (int i = 1; i <= _db.MasaAdet; i++)
            //{
            //    if (!_db.aktifSiparisler.Any(x => x.MasaNo == i))
            //    {
            //        cmoMasaNo.Items.Add(i);
            //    }
            //}
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (cboUrun.SelectedIndex == -1)
                return;
            Urun urun = (Urun)cboUrun.SelectedItem;

            SiparisDetay sd = new SiparisDetay() { UrunAd = urun.UrunAd, BirimFiyat = urun.BirimFiyat, Adet = (int)nudAdet.Value };
            _blSiparisDetaylar.Add(sd);
            EkleFormunuSifirla();
        }

        private void EkleFormunuSifirla()
        {
            cboUrun.SelectedIndex = 0;
            nudAdet.Value = 1;
        }

        private void dgvSiparis_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                text: "Emin misiniz?",
                caption: "Ürün siliniyor..",
                buttons: MessageBoxButtons.YesNo,
                icon: MessageBoxIcon.Question,
                defaultButton: MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
        private void btnAnasayfa_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            MasayiKapat(SiparisDurum.Odendi, _siparis.ToplamTutar());
        }

        private void btnİptal_Click(object sender, EventArgs e)
        {
            MasayiKapat(SiparisDurum.Iptal);
        }
        private void MasayiKapat(SiparisDurum durum, decimal odenenTutar = 0)
        {
            _siparis.Durum = durum;
            _siparis.KapanisZamani = DateTime.Now;
            _siparis.OdenenTutar = odenenTutar;
            _db.aktifSiparisler.Remove(_siparis);
            _db.gecmisSiparisler.Add(_siparis);
            Close();
        }

        private void btnTasi_Click(object sender, EventArgs e)
        {
            if (cmoMasaNo.SelectedItem == null) return;
            int eski = _siparis.MasaNo;
            int yeni = (int)cmoMasaNo.SelectedItem;
            _siparis.MasaNo = yeni;
            MasaNoGuncelle();
            if (MasaTasindi != null)
            {
                MasaTasindi(this, new MasaTasindiEventArgs(eski, yeni));
            }
            //AnaForm anaForm = (AnaForm)Owner;
            //anaForm.MasaTasi(eski, yeni);
        }
    }
}
