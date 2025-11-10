Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.ServiceLocation
Imports System.Data.Common
Imports System.Data
Imports System.Data.SqlClient
Imports WebMatrix.Util
Public Class Errores
    Private db As Database
    Public Sub New()
        db = DatabaseFactory.CreateDatabase()
    End Sub
    Public Sub InsertarErrores(ByVal dt As DataSet)
        Dim Er As New VOErrores()
        Dim connection As DbConnection = db.CreateConnection()
        connection.Open()
        Dim transaccion As DbTransaction = connection.BeginTransaction()
        Dim i As Integer

        Dim comando As DbCommand
        Dim Dr As SqlDataReader

        Dim EncuestasCargadas As Integer
        Dim EncuestasNoCargadas As Integer
        EncuestasCargadas = 0
        EncuestasNoCargadas = 0

        i = 5  'Leer primera fila de datos
        Er.Estudio = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(3))
        Er.Mes = Convert.ToInt32(dt.Tables(0).Rows(i).ItemArray.GetValue(1))
        ShowNotification("Estudio : " & Er.Estudio.ToString() & " - " & "MES : " & Er.Mes.ToString(), WebMatrix.ShowNotifications.InfoNotification)

        Try
            For i = 5 To dt.Tables(0).Rows.Count - 1
                'If i = 31 Then
                ' MsgBox("PARAR")
                'End If

                'VALIDAR SI EXISTE ESTE ESTUDIO y ESTA ENCUESTA - No la incluye
                Er.Estudio = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(3))
                Er.Encuesta = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(6))

                comando = db.GetStoredProcCommand("MBO_OPCampoExisteEstudioEncuesta")
                db.AddInParameter(comando, "Estudio", System.Data.DbType.Decimal, Er.Estudio)
                db.AddInParameter(comando, "Encuesta", System.Data.DbType.Decimal, Er.Encuesta)
                Dr = db.ExecuteReader(comando, transaccion)
                Try
                    If Not Dr.Read Then
                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(0) Is DBNull.Value) Then
                            i = dt.Tables(0).Rows.Count
                        Else
                            Er.Año = Convert.ToInt32(dt.Tables(0).Rows(i).ItemArray.GetValue(0))
                            Er.Mes = Convert.ToInt32(dt.Tables(0).Rows(i).ItemArray.GetValue(1))
                            Er.Unidad = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(2))
                            Er.Estudio = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(3))
                            Er.Encuesta = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(6))
                            Er.Ciudad = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(7))
                            Er.Encuestador = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(8))
                            Er.Supervisor = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(9))
                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(10) Is DBNull.Value) Then
                                Er.Error1 = 0
                            Else
                                Er.Error1 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(10))
                            End If
                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(12) Is DBNull.Value) Then
                                Er.Pregunta1 = ""
                            Else
                                Er.Pregunta1 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(12))
                            End If
                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(13) Is DBNull.Value) Then
                                Er.Error2 = 0
                            Else
                                Er.Error2 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(13))
                            End If
                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(15) Is DBNull.Value) Then
                                Er.Pregunta2 = ""
                            Else
                                Er.Pregunta2 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(15))
                            End If
                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(16) Is DBNull.Value) Then
                                Er.Error3 = 0
                            Else
                                Er.Error3 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(16))
                            End If
                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(18) Is DBNull.Value) Then
                                Er.Pregunta3 = ""
                            Else
                                Er.Pregunta3 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(18))
                            End If

                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(19) Is DBNull.Value) Then
                                Er.Error4 = 0
                            Else
                                Er.Error4 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(19))
                            End If
                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(21) Is DBNull.Value) Then
                                Er.Pregunta4 = ""
                            Else
                                Er.Pregunta4 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(21))
                            End If

                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(22) Is DBNull.Value) Then
                                Er.Error5 = 0
                            Else
                                Er.Error5 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(22))
                            End If
                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(24) Is DBNull.Value) Then
                                Er.Pregunta5 = ""
                            Else
                                Er.Pregunta5 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(24))
                            End If

                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(25) Is DBNull.Value) Then
                                Er.Error6 = 0
                            Else
                                Er.Error6 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(25))
                            End If
                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(27) Is DBNull.Value) Then
                                Er.Pregunta6 = ""
                            Else
                                Er.Pregunta6 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(27))
                            End If

                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(28) Is DBNull.Value) Then
                                Er.Error7 = 0
                            Else
                                Er.Error7 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(28))
                            End If
                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(30) Is DBNull.Value) Then
                                Er.Pregunta7 = ""
                            Else
                                Er.Pregunta7 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(30))
                            End If

                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(31) Is DBNull.Value) Then
                                Er.Disp1 = 0
                            Else
                                Er.Disp1 = Convert.ToInt32(dt.Tables(0).Rows(i).ItemArray.GetValue(31))
                            End If

                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(33) Is DBNull.Value) Then
                                Er.Disp2 = 0
                            Else
                                Er.Disp2 = Convert.ToInt32(dt.Tables(0).Rows(i).ItemArray.GetValue(33))
                            End If

                            If (dt.Tables(0).Rows(i).ItemArray.GetValue(35) Is DBNull.Value) Then
                                Er.FechaRecepcion = ""
                            Else
                                Er.FechaRecepcion = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(35))
                            End If

                            comando = db.GetStoredProcCommand("MBO_OPCampoInsertarErrores")
                            db.AddInParameter(comando, "Año", System.Data.DbType.Int32, Er.Año)
                            db.AddInParameter(comando, "Mes", System.Data.DbType.Int32, Er.Mes)
                            db.AddInParameter(comando, "Unidad", System.Data.DbType.[String], Er.Unidad)
                            db.AddInParameter(comando, "Estudio", System.Data.DbType.Decimal, Er.Estudio)
                            db.AddInParameter(comando, "Encuesta", System.Data.DbType.Decimal, Er.Encuesta)
                            db.AddInParameter(comando, "Ciudad", System.Data.DbType.Decimal, Er.Ciudad)
                            db.AddInParameter(comando, "Encuestador", System.Data.DbType.Decimal, Er.Encuestador)
                            db.AddInParameter(comando, "Supervisor", System.Data.DbType.Decimal, Er.Supervisor)
                            db.AddInParameter(comando, "Error1", System.Data.DbType.Int32, Er.Error1)
                            db.AddInParameter(comando, "Pregunta1", System.Data.DbType.[String], Er.Pregunta1)
                            db.AddInParameter(comando, "Error2", System.Data.DbType.Int32, Er.Error2)
                            db.AddInParameter(comando, "Pregunta2", System.Data.DbType.[String], Er.Pregunta2)
                            db.AddInParameter(comando, "Error3", System.Data.DbType.Int32, Er.Error3)
                            db.AddInParameter(comando, "Pregunta3", System.Data.DbType.[String], Er.Pregunta3)
                            db.AddInParameter(comando, "Error4", System.Data.DbType.Int32, Er.Error4)
                            db.AddInParameter(comando, "Pregunta4", System.Data.DbType.[String], Er.Pregunta4)
                            db.AddInParameter(comando, "Error5", System.Data.DbType.Int32, Er.Error5)
                            db.AddInParameter(comando, "Pregunta5", System.Data.DbType.[String], Er.Pregunta5)
                            db.AddInParameter(comando, "Error6", System.Data.DbType.Int32, Er.Error6)
                            db.AddInParameter(comando, "Pregunta6", System.Data.DbType.[String], Er.Pregunta6)
                            db.AddInParameter(comando, "Error7", System.Data.DbType.Int32, Er.Error7)
                            db.AddInParameter(comando, "Pregunta7", System.Data.DbType.[String], Er.Pregunta7)
                            db.AddInParameter(comando, "Disp1", System.Data.DbType.Int32, Er.Disp1)
                            db.AddInParameter(comando, "Disp2", System.Data.DbType.Int32, Er.Disp2)
                            db.AddInParameter(comando, "FechaRecepcion", System.Data.DbType.[String], Er.FechaRecepcion)

                            db.ExecuteNonQuery(comando, transaccion)
                            EncuestasCargadas = EncuestasCargadas + 1
                        End If
                    Else
                        EncuestasNoCargadas = EncuestasNoCargadas + 1
                    End If

                Catch ex As Exception
                    transaccion.Rollback()
                    ShowNotification("Linea : " + (i + 1).ToString() + " - " + ex.Message.Replace("{", "").Replace("}", ""), WebMatrix.ShowNotifications.InfoNotification)
                    'Throw New Exception("Linea : " + (i + 1).ToString() + " - " + ex.Message.Replace("{", "").Replace("}", ""))
                Finally
                    If (connection.State = ConnectionState.Open) Then
                        connection.Close()
                    End If
                End Try
            Next
            transaccion.Commit()
            ShowNotification("Encuestas Cargadas : " & (EncuestasCargadas).ToString() & " - " & "Encuestas NO Cargadas : " & (EncuestasNoCargadas).ToString(), WebMatrix.ShowNotifications.InfoNotification)

        Catch ex As Exception
            transaccion.Rollback()
            ShowNotification("Linea : " + (i + 1).ToString() + " - " + ex.Message.Replace("{", "").Replace("}", ""), WebMatrix.ShowNotifications.InfoNotification)
            'Throw New Exception("Linea : " + (i + 1).ToString() + " - " + ex.Message.Replace("{", "").Replace("}", ""))
        Finally
            If (connection.State = ConnectionState.Open) Then
                connection.Close()
            End If
        End Try

    End Sub

End Class
