Alur Login
User membuka aplikasi → Muncul Form1 (Login Form).
User memasukkan username & password → Tekan tombol Login.
Sistem mengecek database users:
Jika benar → Masuk ke Form2 (Halaman Utama CRUD Rumah).
Jika salah → Muncul pesan error "Login Gagal!".

Alur Pengelolaan Data Rumah (CRUD)
Di dalam Form2 (CRUD Rumah):

Menampilkan daftar rumah dalam DataGridView.
User dapat melakukan:
Tambah Rumah
Isi form (txtNama, txtAlamat, txtLuas, txtHarga).
Klik Tambah → Data rumah masuk ke database (rumah).
Edit Rumah
Pilih rumah di DataGridView.
Ubah data lalu klik Edit → Data diperbarui di database.
Hapus Rumah
Pilih rumah → Klik Hapus → Data dihapus dari database.

Alur Penjualan Rumah
User memilih rumah yang ingin dijual di DataGridView.
User memasukkan ID rumah yang mau dibeli dan nama pembeli (txtPembeli).
User klik tombol "Jual Rumah" → Sistem akan:
Menyimpan transaksi ke tabel transaksi.
Mengubah status rumah menjadi "Terjual".
Memperbarui tampilan DataGridView.

Alur Filter Rumah
User ingin melihat rumah berdasarkan statusnya.
User memilih opsi filter di cmbFilter (Tersedia/Terjual).
Sistem menampilkan daftar rumah sesuai filter.

Alur Melihat Riwayat Transaksi
User membuka halaman histori transaksi (FormTransaksi).
Sistem mengambil data transaksi dari database.
User melihat daftar rumah yang sudah terjual beserta pembelinya.