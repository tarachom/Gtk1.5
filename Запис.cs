

namespace GtkTest
{
    class Запис
    {
        public Запис()
        {
            UID = Guid.NewGuid();
            Тип = ТипиКонтактноїІнформації.Адрес;
        }

        public bool Актуальний { get; set; } = true;
        public Guid UID { get; set; }
        public ТипиКонтактноїІнформації Тип { get; set; }
        public string Значення { get; set; } = "";
        public string Телефон { get; set; } = "";
        public string ЕлектроннаПошта { get; set; } = "";
        public string Країна { get; set; } = "";
        public string Область { get; set; } = "";
        public string Район { get; set; } = "";
        public string Місто { get; set; } = "";
    }
}
