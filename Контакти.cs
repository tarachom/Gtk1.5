using Gtk;

namespace GtkTest
{
    class Контакти : VBox
    {
        enum Columns
        {
            Image,
            Actual,
            UID,
            Тип,
            Значення,
            Телефон,
            ЕлектроннаПошта,
            Країна,
            Область,
            Район,
            Місто
        }

        ListStore Store = new ListStore(
            typeof(Gdk.Pixbuf),
            typeof(bool),   //Actual
            typeof(string), //UID
            typeof(string), //Тип
            typeof(string), //Значення
            typeof(string), //Телефон
            typeof(string), //ЕлектроннаПошта
            typeof(string), //Країна
            typeof(string), //Область
            typeof(string), //Район
            typeof(string)  //Місто
        );

        TreeView TreeViewGrid;

        public Контакти() : base()
        {
            new VBox();

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(Store);
            AddColumn();

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 10);

            ShowAll();
        }

        void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton upButton = new ToolButton(Stock.Add) { Label = "Додати", IsImportant = true };
            upButton.Clicked += OnAddClick;
            toolbar.Add(upButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { Label = "Видалити", IsImportant = true };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);
        }

        public void LoadRecords(List<Запис> records)
        {
            Store.Clear();

            foreach (Запис record in records)
            {
                Store.AppendValues(
                    new Gdk.Pixbuf("doc.png"),
                    record.Актуальний,
                    record.UID.ToString(),
                    record.Тип.ToString(),
                    record.Значення,
                    record.Телефон,
                    record.ЕлектроннаПошта,
                    record.Країна,
                    record.Область,
                    record.Район,
                    record.Місто
                );
            }
        }

        public List<Запис> SaveRecords()
        {
            List<Запис> records = new List<Запис>();

            TreeIter iter;
            if (Store.GetIterFirst(out iter))
                do
                {
                    Запис record = new Запис();
                    records.Add(record);

                    record.Актуальний = (bool)Store.GetValue(iter, (int)Columns.Actual);

                    string uid = (string)Store.GetValue(iter, (int)Columns.UID);

                    if (!String.IsNullOrEmpty(uid))
                        record.UID = Guid.Parse(uid);

                    string type = (string)Store.GetValue(iter, (int)Columns.Тип);
                    record.Тип = Enum.Parse<ТипиКонтактноїІнформації>(type);

                    record.Значення = Store.GetValue(iter, (int)Columns.Значення)?.ToString() ?? "";
                    record.Телефон = Store.GetValue(iter, (int)Columns.Телефон)?.ToString() ?? "";
                    record.ЕлектроннаПошта = Store.GetValue(iter, (int)Columns.ЕлектроннаПошта)?.ToString() ?? "";
                    record.Країна = Store.GetValue(iter, (int)Columns.Країна)?.ToString() ?? "";
                    record.Область = Store.GetValue(iter, (int)Columns.Область)?.ToString() ?? "";
                    record.Район = Store.GetValue(iter, (int)Columns.Район)?.ToString() ?? "";
                    record.Місто = Store.GetValue(iter, (int)Columns.Місто)?.ToString() ?? "";
                }
                while (Store.IterNext(ref iter));

            return records;
        }

        #region TreeView

        void AddColumn()
        {
            TreeViewGrid.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", (int)Columns.Image));

            CellRendererToggle actualField = new CellRendererToggle();
            actualField.Toggled += EditedActual;
            TreeViewGrid.AppendColumn(new TreeViewColumn("Актуальний", actualField, "active", (int)Columns.Actual));

            TreeViewGrid.AppendColumn(new TreeViewColumn("UID", new CellRendererText(), "text", (int)Columns.UID) { Visible = false });

            ListStore storeTypeInfo = new ListStore(typeof(string), typeof(string));

            foreach (ТипиКонтактноїІнформації field in Enum.GetValues<ТипиКонтактноїІнформації>())
                storeTypeInfo.AppendValues(field.ToString(), field.ToString());

            CellRendererCombo TypeInfo = new CellRendererCombo() { Editable = true, Model = storeTypeInfo, TextColumn = 1 };
            TypeInfo.Edited += TextChanged;
            TypeInfo.Data.Add("Column", (int)Columns.Тип);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Тип", TypeInfo, "text", (int)Columns.Тип) { MinWidth = 100 });

            //Значення
            CellRendererText Значення = new CellRendererText() { Editable = true };
            Значення.Edited += TextChanged;
            Значення.Data.Add("Column", (int)Columns.Значення);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Значення", Значення, "text", (int)Columns.Значення) { MinWidth = 200 });

            //Телефон
            CellRendererText Телефон = new CellRendererText() { Editable = true };
            Телефон.Edited += TextChanged;
            Телефон.Data.Add("Column", (int)Columns.Телефон);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Телефон", Телефон, "text", (int)Columns.Телефон) { MinWidth = 150 });

            //Email
            CellRendererText Email = new CellRendererText() { Editable = true };
            Email.Edited += TextChanged;
            Email.Data.Add("Column", (int)Columns.ЕлектроннаПошта);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Email", Email, "text", (int)Columns.ЕлектроннаПошта) { MinWidth = 150 });

            //Країна
            CellRendererText Країна = new CellRendererText() { Editable = true };
            Країна.Edited += TextChanged;
            Країна.Data.Add("Column", (int)Columns.Країна);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Країна", Країна, "text", (int)Columns.Країна) { MinWidth = 150 });

            //Область
            CellRendererText Область = new CellRendererText() { Editable = true };
            Область.Edited += TextChanged;
            Область.Data.Add("Column", (int)Columns.Область);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Область", Область, "text", (int)Columns.Область) { MinWidth = 150 });

            //Район
            CellRendererText Район = new CellRendererText() { Editable = true };
            Район.Edited += TextChanged;
            Район.Data.Add("Column", (int)Columns.Район);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Район", Район, "text", (int)Columns.Район) { MinWidth = 150 });

            //Місто
            CellRendererText Місто = new CellRendererText() { Editable = true };
            Місто.Edited += TextChanged;
            Місто.Data.Add("Column", (int)Columns.Місто);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Місто", Місто, "text", (int)Columns.Місто) { MinWidth = 150 });
        }

        private void EditedActual(object sender, ToggledArgs args)
        {
            Gtk.TreeIter iter;
            if (Store.GetIterFromString(out iter, args.Path))
            {
                bool val = (bool)Store.GetValue(iter, (int)Columns.Actual);
                Store.SetValue(iter, (int)Columns.Actual, !val);
            }
        }

        void TextChanged(object sender, EditedArgs args)
        {
            CellRenderer cellRender = (CellRenderer)sender;

            if (cellRender.Data.Contains("Column"))
            {
                int ColumnNum = (int)cellRender.Data["Column"]!;

                TreeIter iter;
                if (Store.GetIterFromString(out iter, args.Path))
                    Store.SetValue(iter, ColumnNum, args.NewText);
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            Store.AppendValues(new Gdk.Pixbuf("doc.png"), true, "", ТипиКонтактноїІнформації.Адрес.ToString());
        }

        void OnDeleteClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();
                for (int i = selectionRows.Length - 1; i >= 0; i--)
                {
                    TreePath itemPath = selectionRows[i];

                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    Store.Remove(ref iter);
                }
            }
        }

        #endregion

    }
}