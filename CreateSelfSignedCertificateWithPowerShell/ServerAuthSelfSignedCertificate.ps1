$certificate = New-SelfSignedCertificate `
    -Subject localhost `
    -DnsName localhost `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -NotBefore (Get-Date) `
    -NotAfter (Get-Date).AddYears(2) `
    -FriendlyName "Ivan Yakimov Test-only Certificate For Server Authorization" `
    -HashAlgorithm SHA256 `
    -KeyUsage DigitalSignature, KeyEncipherment, DataEncipherment `
    -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.1")

$pfxPassword = ConvertTo-SecureString `
    -String "p@ssw0rd" `
    -Force `
    -AsPlainText

Export-PfxCertificate `
    -Cert $certificate `
    -FilePath "serverCert.pfx" `
    -Password $pfxPassword