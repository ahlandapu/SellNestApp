Imports System.Data.Odbc

Public Class Form1

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Call Koneksi()

        Try
            Koneksi()

            Dim query As String = "SELECT role FROM users WHERE username = ? AND password = ?"
            Cmd = New OdbcCommand(query, Conn)
            Cmd.Parameters.AddWithValue("?", txtUsername.Text)
            Cmd.Parameters.AddWithValue("?", txtPassword.Text)

            Rd = Cmd.ExecuteReader()

            If Rd.Read() Then
                UserRole = Rd("role").ToString().Trim() ' Pastikan tidak ada spasi atau karakter tak terlihat
                MessageBox.Show("Anda login sebagai: " & UserRole) ' Debugging

                Form2.Show()
                Me.Hide()
            Else
                MessageBox.Show("Username atau password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            Rd.Close()
            Conn.Close()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If Conn IsNot Nothing AndAlso Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click

    End Sub
End Class