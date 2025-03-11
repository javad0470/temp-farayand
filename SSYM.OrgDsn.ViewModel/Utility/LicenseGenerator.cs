using System;
using System.IO;
using System.Security.Cryptography; // needs a ref. to `System.Security.dll` asm.
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SSYM.OrgDsn.ViewModel.Utility
{
    public static class LicenseGenerator
    {
        public static string CreateLicense<T>(string licensePrivateKey, T licenseData) where T : class
        {
            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(licensePrivateKey);
                var xmlDocument = createXmlDocument(licenseData);
                var xmlDigitalSignature = getXmlDigitalSignature(xmlDocument, provider);
                appendDigitalSignature(xmlDocument, xmlDigitalSignature);
                return xmlDocumentToString(xmlDocument);
            }
        }

        //public static string GetDigitalSignature<T>(string licensePrivateKey, T licenseData, CodPrvOrg CodPrv) where T : class
        //{
        //    using (var provider = new RSACryptoServiceProvider())
        //    {
        //        provider.FromXmlString(licensePrivateKey);
        //        var xmlDocument = createXmlDocument(CodPrv);
        //        var xmlDocument1 = createXmlDocument(licenseData);
        //        appendDigitalSignature(xmlDocument, xmlDocument1.DocumentElement);
        //        var xmlDigitalSignature = getXmlDigitalSignature(xmlDocument, provider);
        //        return xmlDigitalSignature.InnerText;
        //    }
        //}

        public static string CreateRSAKeyPair(int dwKeySize = 1024)
        {
            using (var provider = new RSACryptoServiceProvider(dwKeySize))
            {
                return provider.ToXmlString(includePrivateParameters: true);
            }
        }

        public static T ReadLicense<T>(string licensePublicKey, string xmlFileContent) where T : class
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlFileContent);

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(licensePublicKey);

                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");

                var xml = new SignedXml(doc);
                var signatureNode = (XmlElement)doc.SelectSingleNode("//sig:Signature", nsmgr);
                if (signatureNode == null)
                    throw new InvalidOperationException("This license file is not signed.");

                xml.LoadXml(signatureNode);
                if (!xml.CheckSignature(provider))
                    throw new InvalidOperationException("This license file is not valid.");

                var ourXml = xml.GetXml();
                if (ourXml.OwnerDocument == null || ourXml.OwnerDocument.DocumentElement == null)
                    throw new InvalidOperationException("This license file is coruppted.");

                using (var reader = new XmlNodeReader(ourXml.OwnerDocument.DocumentElement))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    return (T)xmlSerializer.Deserialize(reader);
                }
            }
        }

        private static void appendDigitalSignature(XmlDocument xmlDocument, XmlNode xmlDigitalSignature)
        {
            xmlDocument.DocumentElement.AppendChild(xmlDocument.ImportNode(xmlDigitalSignature, true));
        }

        private static XmlDocument createXmlDocument<T>(T licenseData) where T : class
        {
            var serializer = new XmlSerializer(licenseData.GetType());
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                var ns = new XmlSerializerNamespaces(); ns.Add("", "");
                serializer.Serialize(writer, licenseData, ns);
                var doc = new XmlDocument();
                doc.LoadXml(sb.ToString());
                return doc;
            }
        }

        private static XmlElement getXmlDigitalSignature(XmlDocument xmlDocument, AsymmetricAlgorithm key)
        {
            var xml = new SignedXml(xmlDocument) { SigningKey = key };
            var reference = new Reference { Uri = "" };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            xml.AddReference(reference);
            xml.ComputeSignature();
            return xml.GetXml();
        }

        private static string xmlDocumentToString(XmlDocument xmlDocument)
        {
            using (var ms = new MemoryStream())
            {
                var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };
                var xmlWriter = XmlWriter.Create(ms, settings);
                xmlDocument.Save(xmlWriter);
                ms.Position = 0;
                return new StreamReader(ms).ReadToEnd();
            }
        }
    }
}
