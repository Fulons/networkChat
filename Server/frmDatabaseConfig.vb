Public Class frmDatabaseConfig

    Public sql As New SQLControl
    Public Sub Loadgrid()
        sql.ExecuteQuery("SELECT * FROM [UserData]")
        If sql.HasException(True) = True Then Exit Sub
        dgvData.DataSource = sql.dataTable
    End Sub
    Private Sub DataGridView1_CellContentClick(sender As Object, e As Windows.Forms.DataGridViewCellEventArgs) Handles dgvData.CellContentClick

    End Sub

    Private Sub frmDatabaseConfig_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Loadgrid()
    End Sub
End Class