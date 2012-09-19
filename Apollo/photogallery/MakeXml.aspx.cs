using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Text;

public partial class photogallery_MakeXml : System.Web.UI.Page
{
    //public const string BASE_PATH = @"\\fsnyc03\PhotoGallery\g2data\albums\";
    public const string BASE_PATH = @"W:\";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MakeXmlFile();
        }
    }

    public void MakeXmlFile()
    {
        XmlDocument fileStructureDoc = new XmlDocument();
        //Create the root element and add it to the document        
        XmlElement rootNode = fileStructureDoc.CreateElement("photoGalleryFileStructure");
        fileStructureDoc.AppendChild(rootNode);

        DirectoryInfo directoryInfo = new DirectoryInfo(BASE_PATH);
        using (StreamWriter writer = new StreamWriter(@"C:\photoGalleryFileStructure.txt"))
        {
            //write header row
            writer.WriteLine("PATH\tNAME\tIS_FOLDER\tIS_PARENT_FOLDER");
            GenerateDirectoryOutput(directoryInfo, writer);
        }
        //GenerateDirectoryXml(ref fileStructureDoc, ref rootNode, directoryInfo);

        fileStructureDoc.Save("C:\\photoGalleryFileStructure.xml");
    }

    public void GenerateDirectoryOutput(DirectoryInfo directory, StreamWriter writer)
    {
        writer.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", directory.FullName.Replace(directory.Name, "").Replace(@"W:\", "\\"), directory.Name, -1, (directory.GetDirectories().Length != 0 ? -1 : 0)));
        if (directory.GetDirectories().Length != 0)
        {
            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                GenerateDirectoryOutput(subDirectory, writer);
            }
        }
        if (directory.GetFiles().Length != 0)
        {
            string extension = string.Empty;
            foreach (FileInfo file in directory.GetFiles())
            {
                extension = file.Extension.Substring(file.Extension.LastIndexOf('.') + 1);
                if (String.Compare(extension, "JPG", true) == 0)
                {
                    writer.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", file.FullName.Replace(file.Name, "").Replace(@"W:\", "\\"), file.Name, 0, 0));
                } 
            }
        }
    }

    private string GetParentText(XmlElement element)
    {
        string parentText = string.Empty;
        XmlNode parentNode = element.ParentNode;
        XmlAttribute parentName;
        while (parentNode != null)
        {
            if (parentNode.Attributes != null)
            {
                parentName = (XmlAttribute)parentNode.Attributes["name"];
                if (parentName != null && String.Compare(parentName.Value, @"W:\", true) != 0)
                {
                    parentText = string.Format(@"\{1}{0}", parentText, parentName.Value);
                }
            }
            parentNode = parentNode.ParentNode;
        }
        return parentText;
    }

    private void GenerateDirectoryXml(ref XmlDocument xmlDoc, ref XmlElement parentElement, DirectoryInfo directory)
    {
        XmlElement directoryElement = xmlDoc.CreateElement("directory");
        directoryElement.SetAttribute("name", directory.Name);
        parentElement.AppendChild(directoryElement);
        //directoryElement.SetAttribute("parent", parentElement.GetAttribute("name"));
        directoryElement.SetAttribute("parent", GetParentText(directoryElement));        
        if (directory.GetDirectories().Length != 0)
        {
            directoryElement.SetAttribute("isParent", "true");
            XmlElement subDirectoryElement = xmlDoc.CreateElement("subdirectories");
            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                GenerateDirectoryXml(ref xmlDoc, ref subDirectoryElement, subDirectory);                
            }
            directoryElement.AppendChild(subDirectoryElement);
        }
        else
        {
            directoryElement.SetAttribute("isParent", "false");
            string extension = string.Empty;
            foreach (FileInfo file in directory.GetFiles())
            {
                extension = file.Extension.Substring(file.Extension.LastIndexOf('.') + 1);
                XmlElement fileElement;
                if (String.Compare(extension, "JPG", true) == 0)
                {
                    fileElement = xmlDoc.CreateElement("file");
                    fileElement.SetAttribute("name", file.Name);
                    //fileElement.InnerText = file.Name;
                    //directoryElement.AppendChild(fileElement);
                    directoryElement.AppendChild(fileElement);
                }                
            }            
        }        
    }
}