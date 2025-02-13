Imports System.Data.Odbc
Imports System.IO
Public Class Form2
    'Public UserRole As String
    Sub TampilData()
        Call Koneksi()
        Da = New OdbcDataAdapter("SELECT * FROM rumah", Conn)
        Ds = New DataSet()
        Da.Fill(Ds, "rumah")
        DataGridView1.DataSource = Ds.Tables("rumah")
    End Sub

    Private Sub btnTambah_Click(sender As Object, e As EventArgs) Handles btnTambah.Click
        Call Koneksi()
        ' Cek apakah semua field wajib telah diisi
        If txtNama.Text = "" Or txtAlamat.Text = "" Or txtLuas.Text = "" Or txtHarga.Text = "" Then
            MessageBox.Show("Semua field wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub ' Hentikan proses jika ada yang kosong
        End If

        Str = "INSERT INTO rumah (nama_pemilik, alamat, luas, harga) VALUES ('" & txtNama.Text & "', '" & txtAlamat.Text & "', '" & txtLuas.Text & "', '" & txtHarga.Text & "')"
        Cmd = New OdbcCommand(Str, Conn)
        Cmd.ExecuteNonQuery()
        MsgBox("Data Berhasil Ditambahkan!", MsgBoxStyle.Information, "Sukses")
        TampilData()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        ' Cek apakah ID Rumah sudah diisi
        If txtID.Text = "" Then
            MessageBox.Show("ID Rumah wajib diisi untuk mengedit data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Call Koneksi()
        Str = "UPDATE rumah SET nama_pemilik='" & txtNama.Text & "', alamat='" & txtAlamat.Text & "', luas='" & txtLuas.Text & "', harga='" & txtHarga.Text & "' WHERE id_rumah='" & txtID.Text & "'"
        Cmd = New OdbcCommand(Str, Conn)
        Cmd.ExecuteNonQuery()
        MsgBox("Data Berhasil Diperbarui!", MsgBoxStyle.Information, "Sukses")
        TampilData()
    End Sub

    Private Sub btnHapus_Click(sender As Object, e As EventArgs) Handles btnHapus.Click
        ' Cek apakah ID Rumah sudah diisi
        If txtID.Text = "" Then
            MessageBox.Show("ID Rumah wajib diisi untuk menghapus data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'Call Koneksi()

        ' Pastikan koneksi tertutup sebelum dibuka
        If Conn.State = ConnectionState.Open Then Conn.Close()

        ' Query untuk mengecek apakah rumah sudah terjual
        Dim queryCek As String = "SELECT COUNT(*) FROM transaksi WHERE id_rumah = ?"
        Dim cmdCek As New OdbcCommand(queryCek, Conn)
        cmdCek.Parameters.AddWithValue("?", txtID.Text)

        'Str = "DELETE FROM rumah WHERE id_rumah='" & txtID.Text & "'"
        'Cmd = New OdbcCommand(Str, Conn)
        'Cmd.ExecuteNonQuery()
        'MsgBox("Data Berhasil Dihapus!", MsgBoxStyle.Information, "Sukses")
        'TampilData()

        Try
            Conn.Open()
            Dim jumlahTransaksi As Integer = Convert.ToInt32(cmdCek.ExecuteScalar())

            If jumlahTransaksi > 0 Then
                MsgBox("Rumah ini sudah terjual dan tidak bisa dihapus!", MsgBoxStyle.Critical, "Error")
            Else
                ' Jika belum terjual, lanjutkan penghapusan
                Dim queryHapus As String = "DELETE FROM rumah WHERE id_rumah = ?"
                Dim cmdHapus As New OdbcCommand(queryHapus, Conn)
                cmdHapus.Parameters.AddWithValue("?", txtID.Text)

                If MessageBox.Show("Apakah Anda yakin ingin menghapus rumah ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                    cmdHapus.ExecuteNonQuery()
                    MsgBox("Data rumah berhasil dihapus.", MsgBoxStyle.Information, "Sukses")
                    TampilData()
                End If
            End If
        Catch ex As Exception
            MsgBox("Terjadi kesalahan: " & ex.Message, MsgBoxStyle.Critical, "Error")
        Finally
            ' Pastikan koneksi selalu tertutup setelah operasi selesai
            If Conn.State = ConnectionState.Open Then Conn.Close()
        End Try
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MessageBox.Show("UserRole saat ini: " & UserRole) ' Debugging

        If UserRole.Trim().ToLower() <> "admin" Then
            MessageBox.Show("Anda login sebagai user biasa. Mode hanya-baca diaktifkan.", "Akses Terbatas", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ' Nonaktifkan semua tombol CRUD
            btnTambah.Enabled = False
            btnEdit.Enabled = False
            btnHapus.Enabled = False
            btnJual.Enabled = False
            txtID.Enabled = False
            txtNama.Enabled = False
            txtAlamat.Enabled = False
            txtHarga.Enabled = False
            txtLuas.Enabled = False
            txtIDBeli.Enabled = False
            txtPembeli.Enabled = False
            DateTimePicker1.Enabled = False
            TransaksiToolStripMenuItem.Enabled = False

        Else
            MessageBox.Show("Anda login sebagai Admin!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        TampilData()
    End Sub

    Private Sub btnTambah_Click_1(sender As Object, e As EventArgs) Handles btnTambah.Click

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub


    Private Sub btnJual_Click_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub cmbFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFilter.SelectedIndexChanged
        Call Koneksi()
        Dim query As String = "SELECT * FROM rumah"
        If cmbFilter.Text = "Tersedia" Then
            query &= " WHERE status='Tersedia'"
        ElseIf cmbFilter.Text = "Terjual" Then
            query &= " WHERE status='Terjual'"
        End If
        Da = New OdbcDataAdapter(query, Conn)
        Ds = New DataSet()
        Da.Fill(Ds, "rumah")
        DataGridView1.DataSource = Ds.Tables("rumah")
    End Sub

    Private Sub TransaksiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TransaksiToolStripMenuItem.Click
        Dim transaksiForm As New FormTransaksi()
        transaksiForm.Show()
    End Sub

    Private Sub KeluarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KeluarToolStripMenuItem.Click
        ' Tampilkan kembali Form1 (Login)
        Form1.Show()

        ' Tutup Form2 (CRUD Rumah)
        Me.Close()
    End Sub

    Private Sub TampilDataRumah()
        ' Pastikan ID Rumah tidak kosong
        If txtID.Text = "" Then
            MsgBox("Masukkan ID Rumah terlebih dahulu!", MsgBoxStyle.Exclamation, "Peringatan")
            Exit Sub
        End If

        ' Koneksi ke database
        Call Koneksi()

        ' Query untuk mencari data berdasarkan ID Rumah
        Str = "SELECT * FROM rumah WHERE id_rumah = '" & txtID.Text & "'"
        Cmd = New OdbcCommand(Str, Conn)
        Rd = Cmd.ExecuteReader()

        ' Jika data ditemukan
        If Rd.Read() Then
            txtNama.Text = Rd("nama_pemilik").ToString()
            txtAlamat.Text = Rd("alamat").ToString()
            txtLuas.Text = Rd("luas").ToString()
            txtHarga.Text = Rd("harga").ToString()
            cmbFilter.Text = Rd("status").ToString()
        Else
            MsgBox("Data rumah tidak ditemukan!", MsgBoxStyle.Exclamation, "Peringatan")
            txtID.Focus()
        End If
    End Sub

    Private Sub txtID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtID.KeyDown
        ' Jika tombol yang ditekan adalah Enter
        If e.KeyCode = Keys.Enter Then
            ' Panggil fungsi untuk menampilkan data
            TampilDataRumah()
        End If
    End Sub

    Private Sub txtID_TextChanged(sender As Object, e As EventArgs) Handles txtID.TextChanged

    End Sub

    Private Sub btnJual_Click(sender As Object, e As EventArgs) Handles btnJual.Click
        If txtIDBeli.Text = "" Or txtPembeli.Text = "" Then
            MsgBox("Masukkan ID Rumah dan nama pembeli!", MsgBoxStyle.Exclamation, "Peringatan")
            Exit Sub
        End If

        Call Koneksi()

        Dim adaRumah As Boolean = False
        Str = "SELECT COUNT(*) FROM rumah WHERE id_rumah = '" & txtIDBeli.Text & "'"
        Cmd = New OdbcCommand(Str, Conn)
        Dim count As Integer = Convert.ToInt32(Cmd.ExecuteScalar())

        If count > 0 Then
            adaRumah = True
        End If

        If Not adaRumah Then
            MsgBox("ID Rumah tidak ditemukan di database!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        ' Cek status rumah terlebih dahulu
        Dim statusRumah As String = ""
        Str = "SELECT status FROM rumah WHERE id_rumah = '" & txtIDBeli.Text & "'"
        Cmd = New OdbcCommand(Str, Conn)
        Rd = Cmd.ExecuteReader()

        If Rd.Read() Then
            statusRumah = Rd("status").ToString()
        Else
            MsgBox("Data rumah tidak ditemukan!", MsgBoxStyle.Exclamation, "Peringatan")
            Exit Sub
        End If
        Rd.Close()

        ' Jika rumah sudah terjual, batalkan transaksi
        If statusRumah = "Terjual" Then
            MsgBox("Rumah ini sudah terjual dan tidak bisa dijual kembali!", MsgBoxStyle.Critical, "Peringatan")
            Exit Sub
        End If

        ' Lanjutkan proses transaksi

        Dim pembeli As String = txtPembeli.Text
        Dim tanggal As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")

        Str = "INSERT INTO transaksi (id_rumah, nama_pembeli, tanggal_transaksi) VALUES ('" & txtIDBeli.Text & "', '" & pembeli & "', '" & tanggal & "')"
        Cmd = New OdbcCommand(Str, Conn)
        Cmd.ExecuteNonQuery()

        ' Update status rumah menjadi "Terjual"
        Str = "UPDATE rumah SET status = 'Terjual' WHERE id_rumah = '" & txtIDBeli.Text & "'"
        Cmd = New OdbcCommand(Str, Conn)
        Cmd.ExecuteNonQuery()

        MsgBox("Transaksi berhasil! Rumah telah terjual.", MsgBoxStyle.Information, "Sukses")

        ' Kosongkan form setelah transaksi
        txtID.Text = ""
        txtNama.Text = ""
        txtAlamat.Text = ""
        txtLuas.Text = ""
        txtHarga.Text = ""
        cmbFilter.Text = ""
        txtPembeli.Text = ""

        'Dim tanggal As String = DateTime.Now.ToString("yyyy-MM-dd")
        'Str = "INSERT INTO transaksi (id_rumah, nama_pembeli, tanggal_transaksi, harga) VALUES ('" & txtIDBeli.Text & "', '" & txtPembeli.Text & "', '" & tanggal & "', '" & txtHarga.Text & "')"
        'Cmd = New OdbcCommand(Str, Conn)
        'Cmd.ExecuteNonQuery()

        ' Update status rumah jadi "Terjual"
        Str = "UPDATE rumah SET status='Terjual' WHERE id_rumah='" & txtIDBeli.Text & "'"
        Cmd = New OdbcCommand(Str, Conn)
        Cmd.ExecuteNonQuery()

        MsgBox("Rumah berhasil dijual!", MsgBoxStyle.Information, "Sukses")
        TampilData()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ' Membersihkan semua input field
        txtID.Clear()
        txtNama.Clear()
        txtAlamat.Clear()
        txtHarga.Clear()
        txtLuas.Clear()

        'cmbFilter.Clear()

        ' Jika ada ComboBox, atur ke nilai default
        'cmbTipeRumah.SelectedIndex = -1

        ' Jika ada DateTimePicker, atur ke tanggal sekarang
        DateTimePicker1.Value = DateTime.Now

        ' Jika ada Checkbox, atur menjadi tidak tercentang
        'chkTersedia.Checked = False

        ' Kembalikan fokus ke input pertama
        txtID.Focus()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim aboutForm As New about()
        aboutForm.Show()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataToolStripMenuItem.Click

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub
End Class