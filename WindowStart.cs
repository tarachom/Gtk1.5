using Gtk;
using System.Xml;

namespace GtkTest
{
    class WindowStart : Window
    {
        Контакти Контакти;

        public WindowStart() : base("")
        {
            SetDefaultSize(1300, 600);
            SetPosition(WindowPosition.Center);
            BorderWidth = 5;

            DeleteEvent += delegate { Program.Quit(); };

            VBox vbox = new VBox();
            Add(vbox);

            #region Кнопки

            //Кнопки
            HBox hBoxButton = new HBox();
            vbox.PackStart(hBoxButton, false, false, 10);

            Button bFill = new Button("Заповнити");
            bFill.Clicked += OnFill;
            hBoxButton.PackStart(bFill, false, false, 10);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += OnSave;
            hBoxButton.PackStart(bSave, false, false, 10);

            #endregion

            Контакти = new Контакти();

            vbox.PackStart(Контакти, true, true, 5);

            ShowAll();
        }

        void OnFill(object? sender, EventArgs args)
        {
            List<Запис> records = new List<Запис>();

            for (int i = 0; i < 15; i++)
            {
                Запис record = new Запис();
                records.Add(record);

                record.Актуальний = (i < 10 ? true : false);
                record.Значення = "Довільний текст " + i.ToString();
                record.Телефон = "+380991234567";
                record.Країна = "Україна";
                record.Місто = "Львів";
                record.Область = "Львівська";
                record.ЕлектроннаПошта = "test@gmail.com";
            }

            Контакти.LoadRecords(records);
        }

        void OnSave(object? sender, EventArgs args)
        {
            List<Запис> records = Контакти.SaveRecords();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", ""));

            XmlElement rootNode = xmlDocument.CreateElement("root");
            xmlDocument.AppendChild(rootNode);

            foreach (Запис record in records)
            {
                XmlElement contactsNode = xmlDocument.CreateElement("Contacts");
                rootNode.AppendChild(contactsNode);

                XmlElement nodeАктуальний = xmlDocument.CreateElement("Актуальний");
                nodeАктуальний.InnerText = record.Актуальний.ToString();
                contactsNode.AppendChild(nodeАктуальний);

                XmlElement nodeUID = xmlDocument.CreateElement("UID");
                nodeUID.InnerText = record.UID.ToString();
                contactsNode.AppendChild(nodeUID);

                XmlElement nodeТип = xmlDocument.CreateElement("Тип");
                nodeТип.InnerText = record.Тип.ToString();
                contactsNode.AppendChild(nodeТип);

                XmlElement nodeЗначення = xmlDocument.CreateElement("Значення");
                nodeЗначення.InnerText = record.Значення;
                contactsNode.AppendChild(nodeЗначення);

                XmlElement nodeТелефон = xmlDocument.CreateElement("Телефон");
                nodeТелефон.InnerText = record.Телефон;
                contactsNode.AppendChild(nodeТелефон);

                XmlElement nodeЕлектроннаПошта = xmlDocument.CreateElement("ЕлектроннаПошта");
                nodeЕлектроннаПошта.InnerText = record.ЕлектроннаПошта;
                contactsNode.AppendChild(nodeЕлектроннаПошта);

                XmlElement nodeКраїна = xmlDocument.CreateElement("Країна");
                nodeКраїна.InnerText = record.Країна;
                contactsNode.AppendChild(nodeКраїна);

                XmlElement nodeОбласть = xmlDocument.CreateElement("Область");
                nodeОбласть.InnerText = record.Область;
                contactsNode.AppendChild(nodeОбласть);

                XmlElement nodeРайон = xmlDocument.CreateElement("Район");
                nodeРайон.InnerText = record.Район;
                contactsNode.AppendChild(nodeРайон);

                XmlElement nodeМісто = xmlDocument.CreateElement("Місто");
                nodeМісто.InnerText = record.Місто;
                contactsNode.AppendChild(nodeМісто);
            }

            xmlDocument.Save(System.IO.Path.Combine(AppContext.BaseDirectory, "../../../Contacts.xml"));
        }
    }
}
