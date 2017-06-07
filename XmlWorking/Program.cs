using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace XmlWorking
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            string fileXml = String.Format("{0}\\{1}", filePath, "users.xml");

            XPathWork(fileXml);
            //XmlAddNewElement(fileXml);
            //XmlRead(fileXml);
            //XmlIntoList(fileXml);
            //XmlDeleteElement(fileXml);
            Console.ReadLine();
            XmlLinq(fileXml);
            
            Console.WriteLine(new string('*',40));
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void XmlRead(string fileName)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(fileName);
                XmlElement xRoot = xDoc.DocumentElement; //get root element of xml document

                foreach (XmlNode xNode in xRoot)
                {
                    //retrieve attribute "Name"
                    if (xNode.Attributes.Count > 0)
                    {
                        XmlNode attr = xNode.Attributes.GetNamedItem("name");
                        if (attr != null)
                        {
                            Console.WriteLine(attr.Value);
                        }
                    }
                    //work with child nodes in user's element
                    foreach (XmlNode childNode in xNode.ChildNodes)
                    {
                        if (childNode.Name == "company")
                        {
                            Console.WriteLine("Company: {0}", childNode.InnerText);
                        }

                        if (childNode.Name == "age")
                        {
                            Console.WriteLine("Age: {0}", childNode.InnerText);
                        }
                    }
                    Console.WriteLine(new string('_',40));
                }
                Console.ReadKey();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("You met Null Exception");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void XmlIntoList(string fileName)
        {
            List<User> users= new List<User>();
            XmlDocument xDoc= new XmlDocument();
            xDoc.Load(fileName);
            //get root Element from xml-file
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlElement xnode in xRoot)
            {
                User user=new User();
                XmlNode attr= xnode.Attributes.GetNamedItem("name");

                if (attr != null)
                {
                    user.Name = attr.Value;
                }

                foreach (XmlNode chilNode in xnode.ChildNodes)
                {
                    if (chilNode.Name == "company")
                    {
                        user.Company = chilNode.InnerText;
                    }

                    if (chilNode.Name == "age")
                    {
                        user.Age = Int32.Parse(chilNode.InnerText);
                    }
                }
                users.Add(user);
            }

            foreach (User u in users)
            {
                Console.WriteLine("{0} ({1}) - {2}", u.Name, u.Company, u.Age);
            }
            Console.WriteLine("Done");
            Console.WriteLine(new string('*',40));
        }

        private static void XmlAddNewElement(string fileName)
        {
            XmlDocument xDoc= new XmlDocument();
            xDoc.Load(fileName);
            XmlElement xRoot = xDoc.DocumentElement;

            XmlElement userElement = xDoc.CreateElement("user");
            XmlAttribute nameAttr = xDoc.CreateAttribute("name");
            XmlElement companyElem = xDoc.CreateElement("company");
            XmlElement ageElem = xDoc.CreateElement("age");

            //creating text values for elements and attributes
            XmlText nameText=xDoc.CreateTextNode("Mark Zuckerberg");
            XmlText companyText = xDoc.CreateTextNode("Facebook");
            XmlText ageText = xDoc.CreateTextNode("30");

            //add nodes
            nameAttr.AppendChild(nameText);
            companyElem.AppendChild(companyText);
            ageElem.AppendChild(ageText);
            userElement.Attributes.Append(nameAttr);
            userElement.AppendChild(companyElem);
            userElement.AppendChild(ageElem);
            xRoot.AppendChild(userElement);

            xDoc.Save(fileName);
            Console.WriteLine("Creating of new element is done");
            Console.WriteLine(new string('_', 40));
        }

        private static void XmlDeleteElement(string fileName)
        {
            XmlDocument xDoc= new XmlDocument();
            xDoc.Load(fileName);
            XmlElement xRoot = xDoc.DocumentElement;

            XmlNode lastNode = xRoot.LastChild;
            xRoot.RemoveChild(lastNode);
            xDoc.Save(fileName);

            Console.WriteLine("Deleting of the last element is done");
        }

        private static void XPathWork(string fileName)
        {
            XmlDocument xDoc=new XmlDocument();
            xDoc.Load(fileName);
            XmlElement xRoot = xDoc.DocumentElement;

            //select all the children nodes
            XmlNodeList childNodes = xRoot.SelectNodes("*");
            foreach (XmlNode n in childNodes)
            {
                Console.WriteLine(n.OuterXml);
            }
            Console.WriteLine(new string('-', 40));
            childNodes = xRoot.SelectNodes("user");
            foreach (XmlNode n in childNodes)
            {
                Console.WriteLine(n.SelectSingleNode("@name").Value);
            }

            Console.WriteLine(new string('-', 40));

            XmlNode childnode = xRoot.SelectSingleNode("user[@name='Bill Gates']");
            if (childnode != null)
            {
                Console.WriteLine(childnode.OuterXml);
            }
            Console.WriteLine(new string('-', 40));
            childNodes = xRoot.SelectNodes("//user//company");
            foreach (XmlNode n in childNodes)
            {
                Console.WriteLine(n.InnerText);
            }
        }

        private static void XmlLinq(string fileName)
        {
            XDocument xDoc= XDocument.Load(fileName);
            var items = from xe in xDoc.Element("users").Elements("user")
                where xe.Element("company").Value == "Facebook"
                select new User()
                {
                    Name = xe.Attribute("name").Value,
                    Company = xe.Element("company").Value,
                    Age = Int32.Parse(xe.Element("age").Value)
                };
            foreach (var item in items)
            {
               Console.WriteLine("{0} ({1}) - {2}", item.Name, item.Age, item.Company);
            }
            Console.WriteLine(new string('_', 40));
            Console.WriteLine("Work is done!");
        }
    }
}
