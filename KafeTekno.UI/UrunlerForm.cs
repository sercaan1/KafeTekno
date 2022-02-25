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
    public partial class UrunlerForm : Form
    {
        private readonly KafeVeri _db;
        private readonly BindingList<Urun> _blUrunler;
        private Urun _duzenlenen;

        public UrunlerForm(KafeVeri db)
        {
            // Program.cs'ye gidip Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR"); diyebiliriz para formatı ve ondalık sayılar kültürü için.

            _db = db;
            _blUrunler = new BindingList<Urun>(_db.urunler);
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false;
            dgvUrunler.DataSource = _blUrunler;
            btnIptal.Visible = false;
            //dgvUrunler.Columns[0].HeaderText = "Ürün Adı"; Bu şekilde headerlar değiştirilebilir.
            //dgvUrunler.Columns[1].HeaderText = "Birim Fiyatı";
            // Kolon ekledik otomatik gelenler auto generated. Edit columnsdan kaynak propertyleri verdik. 24. satırdaki kodu yazdık
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            string ad = txtUrunAd.Text.Trim();

            if (ad == "")
            {
                MessageBox.Show("Bir ürün adı belirtmediniz.");
                return;
            }
            if (_duzenlenen == null)
            {
                _blUrunler.Add(new Urun { UrunAd = ad, BirimFiyat = nudBirimFiyat.Value });
            }
            else
            {
                _duzenlenen.UrunAd = ad;
                _duzenlenen.BirimFiyat = nudBirimFiyat.Value;
            }
            FormuSifirla();
            // btnEkle textten de kontrol edebilirdik anca bu şekilde daha güvenli.
        }
        private void FormuSifirla()
        {
            txtUrunAd.Clear();
            nudBirimFiyat.Value = 0;
            btnIptal.Visible = false;
            btnEkle.Text = "EKLE";
            dgvUrunler.Enabled = true;
            _duzenlenen = null;
        }
        private void dgvUrunler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            DataGridViewRow satir = dgvUrunler.Rows[e.RowIndex];
            _duzenlenen = (Urun)satir.DataBoundItem;
            txtUrunAd.Text = _duzenlenen.UrunAd;
            nudBirimFiyat.Value = _duzenlenen.BirimFiyat;
            dgvUrunler.Enabled = false;
            btnEkle.Text = "KAYDET";
            btnIptal.Visible = true;
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            FormuSifirla();
        }
    }
}
