$certificate = New-SelfSignedCertificate `
      -Type Custom `
      -Subject "Ivan Yaimov" `
      -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.2") `
      -FriendlyName "Ivan Yakimov Test-only Certificate For Client Authorization" `
      -KeyUsage DigitalSignature `
      -KeyAlgorithm RSA `
      -KeyLength 2048

$pfxPassword = ConvertTo-SecureString `
    -String "p@ssw0rd" `
    -Force `
    -AsPlainText

Export-PfxCertificate `
    -Cert $certificate `
    -FilePath "clientCert.pfx" `
    -Password $pfxPassword