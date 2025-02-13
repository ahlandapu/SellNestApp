Imports System.Data.Odbc
Module Module1
    Public Conn As OdbcConnection
    Public Cmd As OdbcCommand
    Public Da As OdbcDataAdapter
    Public Ds As DataSet
    Public Rd As OdbcDataReader
    Public Str As String
    Public UserRole As String

    Public Sub Koneksi()
        Try
            Conn = New OdbcConnection("DSN=db_rumah")
            Conn.Open()
        Catch ex As Exception
            MsgBox("Koneksi Gagal: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
End Module
