using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Net;
using System.Xml.XPath;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
	public string Verify(string xmlUrl,string xsdUrl)
	{

        string output = "No Errors"; // default output 
        string xml;
        var set = new XmlSchemaSet();

        using (var wc = new WebClient())
        {
            try
            {
                xml = wc.DownloadString(xmlUrl); // Downloads the XML document using the URL
            }
            catch (WebException e)
            {
                output = "XML INVALID";
                return output;
            }
        }

        var xd = new XmlDocument(); // Loads xml string into XmlDocument object
        try
        {
            xd.Load(xmlUrl);
        }
        catch (XmlException e)
        {
            output = e.ToString();
            return output;
        }

        var xdoc = XmlDocToXDoc(xd);

        set.Add(null, xsdUrl); // Loads the schema into the schema set

        xdoc.Validate(set, (o, e) => // tries to validate the xml document
        {
            output = e.Message;
        });

        return output;
    }

    private static XDocument XmlDocToXDoc(XmlDocument xdoc)
    {
        return XDocument.Load(new XmlNodeReader(xdoc));
    }

    public string XPathSearch(string xmlUrl, string path)
    {
        XPathDocument dx = new XPathDocument(xmlUrl);

        XPathNavigator nav = dx.CreateNavigator();

        XPathNodeIterator iterator = nav.Select(path);

        string value = "";

        while (iterator.MoveNext())
        {
            value += iterator.Current.Value + ", ";
        }

        return value;
    }

}
