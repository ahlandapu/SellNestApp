Imports System.Data.Odbc
Public Class FormTransaksi

    Private Sub FormTransaksi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Koneksi()
        Da = New OdbcDataAdapter("SELECT t.id_transaksi, r.nama_pemilik, t.nama_pembeli, t.tanggal_transaksi FROM transaksi t JOIN rumah r ON t.id_rumah = r.id_rumah", Conn)
        Ds = New DataSet()
        Da.Fill(Ds, "transaksi")
        DataGridView1.DataSource = Ds.Tables("transaksi")
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub
End Class