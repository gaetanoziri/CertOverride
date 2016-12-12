Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Cryptography

Module Module1

    Private Const OID_SHA256 As String = "OID.2.16.840.1.101.3.4.2.1"

    Sub Main(ByVal sArgs() As String)

        Dim certificateFilePath As String = Nothing
        Dim url As String = Nothing
        Dim outputFilePath As String = Nothing

        If sArgs.Count() = 6 Then
            For i As Integer = 0 To 5 Step 2
                If sArgs(i) = "-c" Then
                    certificateFilePath = sArgs(i + 1)
                ElseIf sArgs(i) = "-u" Then
                    url = sArgs(i + 1)
                ElseIf sArgs(i) = "-o" Then
                    outputFilePath = sArgs(i + 1)
                End If
            Next
        Else
            Console.WriteLine("Wrong parameters")
        End If


        Console.WriteLine(certificateFilePath)
        Console.WriteLine(url)
        Console.WriteLine(outputFilePath)


        If ((Not certificateFilePath Is Nothing) AndAlso (Not url Is Nothing) AndAlso (Not outputFilePath Is Nothing) AndAlso File.Exists(certificateFilePath) AndAlso (Not url Is Nothing)) Then

            Dim certificate As New X509Certificate2(certificateFilePath)
            certificate.SignatureAlgorithm.Value = OID_SHA256
            Console.WriteLine(PrintByteArray(certificate.GetCertHash))
            Console.WriteLine(certificate.SignatureAlgorithm.Value)
            Dim mySHA256 As SHA256 = SHA256Managed.Create()
            Dim file As System.IO.StreamWriter
            Dim line As String

            file = My.Computer.FileSystem.OpenTextFileWriter(outputFilePath, True)
            line = url + vbTab + OID_SHA256 + vbTab + PrintByteArray(mySHA256.ComputeHash(certificate.Export(X509ContentType.Cert))) + vbTab + "U" + vbTab + Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(certificate.GetSerialNumberString + certificate.Issuer))
            file.WriteLine(line)
            file.Close()
        Else
            Console.WriteLine("Wrong parameters 2")
        End If

    End Sub

    Private Function PrintByteArray(ByVal array() As Byte) As String
        Dim i As Integer
        Dim j = 1
        Dim result As String = ""
        For i = 0 To array.Length - 1
            result += (String.Format("{0:X2}", array(i)))
            If (i < array.Length - 1) Then
                result += ":"
            End If
        Next i
        Return result
    End Function

End Module
