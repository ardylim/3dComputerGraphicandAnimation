Public Class Form1
    Const PI As Double = 3.14285714
    Const deg2rad As Double = PI / 180
    Const sin5 As Double = 0.08715574
    Const cos5 As Double = 0.99619469
    
    Structure TPoint
        Dim x, y, z, w As Double
    End Structure

    Structure TLine
        Dim P1, P2 As Integer
    End Structure

    Dim bmp As Bitmap
    Dim P1, P2 As Point
    Dim arbi As TPoint
    Dim v(7), vr(7), vs(7), rs(7), vv(7) As TPoint
    Dim edge(11) As TLine
    Dim vt(3, 3) As Double
    Dim st(3, 3) As Double
    Dim dt(3, 3) As Double
    Dim wt(3, 3) As Double
    Dim temp(3, 3) As Double
    Dim rotate(3, 3) As Double
    Dim dtrans(3, 3) As Double
    Dim choice As Integer
    Dim phi, teta, alpha, deg As Double
    Dim a, b, c, d As Double
    Dim x, y, z As Double
    Dim colour As Color

    Sub SetPoint(ByRef P As TPoint, ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
        P.x = X
        P.y = Y
        P.z = Z
        P.w = 1
    End Sub
    Sub SetLine(ByRef L As TLine, ByVal n1 As Integer, ByVal n2 As Integer)
        L.P1 = n1
        L.P2 = n2
    End Sub
    Sub SetColMatrix(ByRef M(,) As Double, ByVal col As Integer, ByVal a As Double, ByVal b As Double, ByVal c As Double, ByVal d As Double)
        M(0, col) = a
        M(1, col) = b
        M(2, col) = c
        M(3, col) = d
    End Sub
    Sub DrawCube()
        Dim i, index1, index2 As Integer
        For i = 11 To 0 Step -1
            index1 = edge(i).P1
            index2 = edge(i).P2
            Drawline(vs(index1).x, vs(index1).y, vs(index2).x, vs(index2).y, Color.Black)
            Select Case i
                Case 5, 9, 10
                    Drawline(vs(index1).x, vs(index1).y, vs(index2).x, vs(index2).y, Color.Blue)
                Case 0, 1, 2, 3
                    Drawline(vs(index1).x, vs(index1).y, vs(index2).x, vs(index2).y, Color.Red)
            End Select
        Next
    End Sub
    Sub hidecube()
        Dim i, index1, index2 As Integer
        For i = 0 To 11
            index1 = edge(i).P1
            index2 = edge(i).P2
            Drawline(vs(index1).x, vs(index1).y, vs(index2).x, vs(index2).y, Color.White)
        Next
    End Sub
    Sub Pset(ByVal x As Double, ByVal y As Double, ByVal colour As System.Drawing.Color)
        If x > 0 And y > 0 And x < 500 And y < 500 Then
            bmp.SetPixel(x, y, colour)
        End If
    End Sub
    Sub Drawline(ByVal px1 As Double, ByVal py1 As Double, ByVal px2 As Double, ByVal py2 As Double, ByVal colour As System.Drawing.Color)
       
        Dim dx, dy, tdy, tdx, d, dR, dUR, x, y, x1, y1, x2, y2 As Integer
        Dim c As Color
        x1 = px1
        y1 = py1
        x2 = px2
        y2 = py2
        c = colour
        dx = x2 - x1
        dy = y2 - y1
        x = x1
        y = y1

        tdx = Math.Abs(dx)
        tdy = Math.Abs(dy)

        If tdx >= tdy Then

            d = 2 * tdy - tdx
            dR = 2 * tdy
            dUR = 2 * (tdy - tdx)

            Do
                Pset(x, y, c)
                If dx > 0 Then
                    x = x + 1
                ElseIf dx < 0 Then
                    x = x - 1
                End If

                If d <= 0 Then
                    d = d + dR
                ElseIf d > 0 Then
                    d = d + dUR
                    If dy < 0 Then
                        y = y - 1
                    ElseIf dy > 0 Then
                        y = y + 1
                    End If
                Else
                    If dy > 0 Then
                        d = d + dR
                    ElseIf dy < 0 Then
                        d = d + dUR
                        y = y - 1
                    End If
                End If
            Loop Until x = x2

        Else

            d = tdy - 2 * tdx
            dR = -2 * tdx
            dUR = 2 * (tdy - tdx)


            Do
                Pset(x, y, c)
                If dy > 0 Then
                    y = y + 1
                ElseIf dy < 0 Then
                    y = y - 1
                End If

                If d >= 0 Then
                    d = d + dR
                ElseIf d < 0 Then
                    d = d + dUR
                    If dx < 0 Then
                        x = x - 1
                    ElseIf dx > 0 Then
                        x = x + 1
                    End If
                Else
                    If dx < 0 Then
                        d = d + dR
                        x = x - 1
                    ElseIf dx > 0 Then
                        d = d + dUR
                    End If
                End If
            Loop Until y = y2
        End If
    End Sub

    Function MultiplyMatrix(ByRef P As TPoint, M(,) As Double) As TPoint
        Dim temp As TPoint
        temp.x = P.x * M(0, 0) + P.y * M(1, 0) + P.z * M(2, 0) + P.w * M(3, 0)
        temp.y = P.x * M(0, 1) + P.y * M(1, 1) + P.z * M(2, 1) + P.w * M(3, 1)
        temp.z = P.x * M(0, 2) + P.y * M(1, 2) + P.z * M(2, 2) + P.w * M(3, 2)
        temp.w = P.x * M(0, 3) + P.y * M(1, 3) + P.z * M(2, 3) + P.w * M(3, 3)
        Return temp
    End Function

    Sub MultiMatrix(M1(,) As Double, M2(,) As Double, M3(,) As Double)
        M3(0, 0) = M1(0, 0) * M2(0, 0) + M1(0, 1) * M2(1, 0) + M1(0, 2) * M2(2, 0) + M1(0, 3) * M2(3, 0)
        M3(0, 1) = M1(0, 0) * M2(0, 1) + M1(0, 1) * M2(1, 1) + M1(0, 2) * M2(2, 1) + M1(0, 3) * M2(3, 1)
        M3(0, 2) = M1(0, 0) * M2(0, 2) + M1(0, 1) * M2(1, 2) + M1(0, 2) * M2(2, 2) + M1(0, 3) * M2(3, 2)
        M3(0, 3) = M1(0, 0) * M2(0, 3) + M1(0, 1) * M2(1, 3) + M1(0, 2) * M2(2, 3) + M1(0, 3) * M2(3, 3)
        M3(1, 0) = M1(1, 0) * M2(0, 0) + M1(1, 1) * M2(1, 0) + M1(1, 2) * M2(2, 0) + M1(1, 3) * M2(3, 0)
        M3(1, 1) = M1(1, 0) * M2(0, 1) + M1(1, 1) * M2(1, 1) + M1(1, 2) * M2(2, 1) + M1(1, 3) * M2(3, 1)
        M3(1, 2) = M1(1, 0) * M2(0, 2) + M1(1, 1) * M2(1, 2) + M1(1, 2) * M2(2, 2) + M1(1, 3) * M2(3, 2)
        M3(1, 3) = M1(1, 0) * M2(0, 3) + M1(1, 1) * M2(1, 3) + M1(1, 2) * M2(2, 3) + M1(1, 3) * M2(3, 3)
        M3(2, 0) = M1(2, 0) * M2(0, 0) + M1(2, 1) * M2(1, 0) + M1(2, 2) * M2(2, 0) + M1(2, 3) * M2(3, 0)
        M3(2, 1) = M1(2, 0) * M2(0, 1) + M1(2, 1) * M2(1, 1) + M1(2, 2) * M2(2, 1) + M1(2, 3) * M2(3, 1)
        M3(2, 2) = M1(2, 0) * M2(0, 2) + M1(2, 1) * M2(1, 2) + M1(2, 2) * M2(2, 2) + M1(2, 3) * M2(3, 2)
        M3(2, 3) = M1(2, 0) * M2(0, 3) + M1(2, 1) * M2(1, 3) + M1(2, 2) * M2(2, 3) + M1(2, 3) * M2(3, 3)
        M3(3, 0) = M1(3, 0) * M2(0, 0) + M1(3, 1) * M2(1, 0) + M1(3, 2) * M2(2, 0) + M1(3, 3) * M2(3, 0)
        M3(3, 1) = M1(3, 0) * M2(0, 1) + M1(3, 1) * M2(1, 1) + M1(3, 2) * M2(2, 1) + M1(3, 3) * M2(3, 1)
        M3(3, 2) = M1(3, 0) * M2(0, 2) + M1(3, 1) * M2(1, 2) + M1(3, 2) * M2(2, 2) + M1(3, 3) * M2(3, 2)
        M3(3, 3) = M1(3, 0) * M2(0, 3) + M1(3, 1) * M2(1, 3) + M1(3, 2) * M2(2, 3) + M1(3, 3) * M2(3, 3)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim i As Integer
        a = Math.Sin(teta) * Math.Sin(phi) * Math.Cos(alpha) + Math.Cos(teta) * Math.Sin(alpha) * -1
        b = Math.Cos(teta) * Math.Sin(phi) * Math.Cos(alpha) + Math.Sin(teta) * -1 * Math.Sin(alpha) * -1
        c = Math.Sin(teta) * Math.Sin(phi) * Math.Sin(alpha) + Math.Cos(teta) * Math.Cos(alpha)
        d = Math.Cos(teta) * Math.Sin(phi) * Math.Sin(alpha) + Math.Sin(teta) * -1 * Math.Cos(alpha)
        bmp = New Bitmap(PictureBox1.Width, PictureBox1.Height)
        PictureBox1.Image = bmp

        SetPoint(v(0), -1, -1, -1)
        SetPoint(v(1), -1, 1, -1)
        SetPoint(v(2), 1, -1, -1)
        SetPoint(v(3), 1, 1, -1)
        SetPoint(v(4), -1, -1, 1)
        SetPoint(v(5), -1, 1, 1)
        SetPoint(v(6), 1, -1, 1)
        SetPoint(v(7), 1, 1, 1)

        SetLine(edge(0), 4, 6)
        SetLine(edge(1), 6, 7)
        SetLine(edge(2), 7, 5)
        SetLine(edge(3), 5, 4)
        SetLine(edge(4), 0, 2)
        SetLine(edge(5), 2, 3)
        SetLine(edge(6), 3, 1)
        SetLine(edge(7), 1, 0)
        SetLine(edge(8), 0, 4)
        SetLine(edge(9), 2, 6)
        SetLine(edge(10), 3, 7)
        SetLine(edge(11), 1, 5)

        'SetColMatrix(vt, 0, 1, 0, 0, 0)
        'SetColMatrix(vt, 1, 0, 1, 0, 0)
        'SetColMatrix(vt, 2, 0, 0, 1, 0)
        'SetColMatrix(vt, 3, 0, 0, 0, 1)
        
        'on Y axis
        'SetColMatrix(vt, 0, cos30, 0, sin30, 0)
        'SetColMatrix(vt, 1, 0, 1, 0, 0)
        'SetColMatrix(vt, 2, -sin30, 0, cos30, 0)
        'SetColMatrix(vt, 3, 0, 0, 0, 1)

        SetColMatrix(dt, 0, 1, 0, 0, 0)
        SetColMatrix(dt, 1, 0, 1, 0, 0)
        SetColMatrix(dt, 2, 0, 0, 1, 0)
        SetColMatrix(dt, 3, 0, 0, 0, 1)

        SetColMatrix(dtrans, 0, 0, 0, -1, 0)
        SetColMatrix(dtrans, 1, -1, 0, 0, 0)
        SetColMatrix(dtrans, 2, 0, 1, 0, 0)
        SetColMatrix(dtrans, 3, 0, 0, 0, 1)

        SetColMatrix(st, 0, 100, 0, 0, 250)
        SetColMatrix(st, 1, 0, -100, 0, 250)
        SetColMatrix(st, 2, 0, 0, 0, 0)
        SetColMatrix(st, 3, 0, 0, 0, 1)

        SetColMatrix(vt, 0, Math.Cos(phi) * Math.Cos(alpha), a, b, 0)
        SetColMatrix(vt, 1, Math.Cos(phi) * Math.Sin(alpha), c, d, 0)
        SetColMatrix(vt, 2, Math.Sin(phi) * -1, Math.Sin(teta) * Math.Cos(phi), Math.Cos(teta) * Math.Cos(phi), 0)
        SetColMatrix(vt, 3, 0, 0, 0, 1)

        For i = 0 To 7
            rs(i) = MultiplyMatrix(v(i), wt)
            vr(i) = MultiplyMatrix(rs(i), vt)
            vs(i) = MultiplyMatrix(vr(i), st)
        Next
        DrawCube()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If RadioButton1.Checked = True Then
            Timer3.Enabled = True
            Timer1.Enabled = False
            Timer2.Enabled = False
            choice = 1
        ElseIf RadioButton2.Checked = True Then
            Timer3.Enabled = True
            Timer1.Enabled = False
            Timer2.Enabled = False
            choice = 2
        ElseIf RadioButton3.Checked = True Then
            Timer3.Enabled = True
            Timer1.Enabled = False
            Timer2.Enabled = False
            choice = 3
        Else
            MsgBox("You haven't choose the axes for object rotation")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If RadioButton4.Checked = True Then
            Timer1.Enabled = True
            Timer2.Enabled = False
            Timer3.Enabled = False
            choice = 4
        ElseIf RadioButton5.Checked = True Then
            Timer1.Enabled = True
            Timer2.Enabled = False
            Timer3.Enabled = False
            choice = 5
        ElseIf RadioButton6.Checked = True Then
            Timer1.Enabled = True
            Timer2.Enabled = False
            Timer3.Enabled = False
            choice = 6
        Else
            MsgBox("You haven't choose the axes for viewer rotation")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Timer3.Enabled = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Timer1.Enabled = False
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim length As Double

        If IsNumeric(x) = False And IsNumeric(y) = False And IsNumeric(z) = False Then
            MsgBox("Input real number")
        Else
            Timer2.Enabled = True
            Timer1.Enabled = False
            Timer3.Enabled = False
            TextBox1.Visible = False
            TextBox2.Visible = False
            TextBox3.Visible = False
            x = Convert.ToDouble(TextBox1.Text)
            y = Convert.ToDouble(TextBox2.Text)
            z = Convert.ToDouble(TextBox3.Text)
            length = Math.Sqrt(x * x + y * y + z * z)
            x = x / length
            y = y / length
            z = z / length
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Timer2.Enabled = False
        TextBox1.Visible = True
        TextBox2.Visible = True
        TextBox3.Visible = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        hidecube()
        a = Math.Sin(teta) * Math.Sin(phi) * Math.Cos(alpha) + Math.Cos(teta) * Math.Sin(alpha) * -1
        b = Math.Cos(teta) * Math.Sin(phi) * Math.Cos(alpha) + Math.Sin(teta) * -1 * Math.Sin(alpha) * -1
        c = Math.Sin(teta) * Math.Sin(phi) * Math.Sin(alpha) + Math.Cos(teta) * Math.Cos(alpha)
        d = Math.Cos(teta) * Math.Sin(phi) * Math.Sin(alpha) + Math.Sin(teta) * -1 * Math.Cos(alpha)

        If choice = 4 Then
            teta = teta + (4 * deg2rad)
            RadioButton4.Checked = False
        ElseIf choice = 5 Then
            phi = phi + (4 * deg2rad)
            RadioButton5.Checked = False
        ElseIf choice = 6 Then
            alpha = alpha + (3 * deg2rad)
            RadioButton6.Checked = False
        End If

        SetColMatrix(vt, 0, Math.Cos(phi) * Math.Cos(alpha), a, b, 0)
        SetColMatrix(vt, 1, Math.Cos(phi) * Math.Sin(alpha), c, d, 0)
        SetColMatrix(vt, 2, Math.Sin(phi) * -1, Math.Sin(teta) * Math.Cos(phi), Math.Cos(teta) * Math.Cos(phi), 0)
        SetColMatrix(vt, 3, 0, 0, 0, 1)

        'MultiMatrix(dt, wt, temp)
        'dt = temp

        For i = 0 To 7
            'rs(i) = MultiplyMatrix(v(i), vt)
            vr(i) = MultiplyMatrix(v(i), vt)
            vs(i) = MultiplyMatrix(vr(i), st)
        Next
        DrawCube()

        PictureBox1.Refresh()
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        hidecube()

        deg = deg + (4 * deg2rad)
        Dim g, f As Double

        a = x * y * (1 - Math.Cos(deg)) - z * Math.Sin(deg)
        b = z * x * (1 - Math.Cos(deg)) + y * Math.Sin(deg)
        c = y * y + Math.Cos(deg) * (1 - y * y)
        d = y * z * (1 - Math.Cos(deg)) - x * Math.Sin(deg)
        f = y * z * (1 - Math.Cos(deg)) + x * Math.Sin(deg)
        g = z * z + Math.Cos(deg) * (1 - z * z)


        SetColMatrix(wt, 0, x * x + Math.Cos(deg) * (1 - x * x), a, b, 0)
        SetColMatrix(wt, 1, x * y * (1 - Math.Cos(deg)) + z * Math.Sin(deg), c, d, 0)
        SetColMatrix(wt, 2, z * x * (1 - Math.Cos(deg)) - y * Math.Sin(deg), f, g, 0)
        SetColMatrix(wt, 3, 0, 0, 0, 1)

        For i = 0 To 7
            rs(i) = MultiplyMatrix(v(i), wt)
            vr(i) = MultiplyMatrix(rs(i), vt)
            vs(i) = MultiplyMatrix(vr(i), st)
        Next

        DrawCube()

        PictureBox1.Refresh()
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        hidecube()

        'a = Math.Sin(teta) * Math.Sin(phi) * Math.Cos(alpha) + Math.Cos(teta) * Math.Sin(alpha) * -1
        'b = Math.Cos(teta) * Math.Sin(phi) * Math.Cos(alpha) + Math.Sin(teta) * -1 * Math.Sin(alpha) * -1
        'c = Math.Sin(teta) * Math.Sin(phi) * Math.Sin(alpha) + Math.Cos(teta) * Math.Cos(alpha)
        'd = Math.Cos(teta) * Math.Sin(phi) * Math.Sin(alpha) + Math.Sin(teta) * -1 * Math.Cos(alpha)

        If choice = 1 Then
            RadioButton1.Checked = False
            SetColMatrix(rotate, 0, 1, 0, 0, 0)
            SetColMatrix(rotate, 1, 0, cos5, -sin5, 0)
            SetColMatrix(rotate, 2, 0, sin5, cos5, 0)
            SetColMatrix(rotate, 3, 0, 0, 0, 1)
        ElseIf choice = 2 Then
            RadioButton2.Checked = False
            SetColMatrix(rotate, 0, cos5, 0, sin5, 0)
            SetColMatrix(rotate, 1, 0, 1, 0, 0)
            SetColMatrix(rotate, 2, -sin5, 0, cos5, 0)
            SetColMatrix(rotate, 3, 0, 0, 0, 1)
        ElseIf choice = 3 Then
            RadioButton3.Checked = False
            SetColMatrix(rotate, 0, cos5, -sin5, 0, 0)
            SetColMatrix(rotate, 1, sin5, cos5, 0, 0)
            SetColMatrix(rotate, 2, 0, 0, 1, 0)
            SetColMatrix(rotate, 3, 0, 0, 0, 1)

        End If
        
        'SetColMatrix(wt, 0, Math.Cos(phi) * Math.Cos(alpha), a, b, 0)
        'SetColMatrix(wt, 1, Math.Cos(phi) * Math.Sin(alpha), c, d, 0)
        'SetColMatrix(wt, 2, Math.Sin(phi) * -1, Math.Sin(teta) * Math.Cos(phi), Math.Cos(teta) * Math.Cos(phi), 0)
        'SetColMatrix(wt, 3, 0, 0, 0, 1)

        MultiMatrix(rotate, dt, temp)
        For i = 0 To 3
            For j = 0 To 3
                dt(i, j) = temp(i, j)
            Next
        Next
        'MultiMatrix(temp, wt, temp2)
        'MultiMatrix(temp2, dt, temp)

        For i = 0 To 7
            rs(i) = MultiplyMatrix(v(i), dt)
            vr(i) = MultiplyMatrix(rs(i), vt)
            vs(i) = MultiplyMatrix(vr(i), st)
            'vv(i) = MultiplyMatrix(vs(i), dt)
        Next
        DrawCube()
        PictureBox1.Refresh()
    End Sub
End Class
