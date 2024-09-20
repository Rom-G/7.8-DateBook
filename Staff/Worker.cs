using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff
{
    struct Worker
    {
        #region Конструкторы

        /// <summary>
        /// Создание сотрудника.
        /// </summary>
        /// <param name="ID">Номер сотрудника</param>
        /// <param name="DateAndTime">Дата и время добавления записи</param>
        /// <param name="Name">Ф.И.О.</param>
        /// <param name="Age">Возраст</param>
        /// <param name="Height">Рост</param>
        /// <param name="DateOfBirth">Дата рождения</param>
        /// <param name="PlaceOfBirth">Место рождения</param>
        public Worker(
            int ID, 
            DateTime DateAndTime, 
            string Name, 
            int Age, 
            int Height, 
            DateTime DateOfBirth, 
            string PlaceOfBirth)
        {
            this.iD = ID;
            this.dateAndTime = DateAndTime;
            this.name = Name;
            this.age = Age;
            this.height = Height;
            this.dateOfBirth = DateOfBirth;
            this.placeOfBirth = PlaceOfBirth;
        }

        /// <summary>
        /// Сотрудник для автозаполнения ежедневника.
        /// </summary>
        /// <param name="Name">Обозначение записи (вместо имени)</param>
        public Worker(int ID, string Name) :
            this(ID, new DateTime(1900, 1, 1, 0, 0, 0), Name, 0, 0, new DateTime(1900, 1, 1, 0, 0, 0), String.Empty)
        {
            this.DateAndTime = DateTime.Now;
        }

        /// <summary>
        /// Неизвестный сотрудник, отсутствующий в ежедневнике.
        /// </summary>
        /// <param name="Name">Обозначение записи (вместо имени)</param>
        public Worker(string Name) :
            this(0, new DateTime(1900,1,1,0,0,0), Name, 0, 0, new DateTime(1900,1,1,0,0,0), String.Empty)
        {

        }

        #endregion

        #region Свойства

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get { return this.iD; } set { this.iD = value; } }

        /// <summary>
        /// Дата и время добавления записи
        /// </summary>
        public DateTime DateAndTime { get { return this.dateAndTime; } set { this.dateAndTime = value; } }

        /// <summary>
        /// Фамилия Имя Отчество
        /// </summary>
        public string Name { get { return this.name; } set { this.name = value; } }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get { return this.age; } set { this.age = value; } }

        /// <summary>
        /// Рост
        /// </summary>
        public int Height { get { return this.height; } set { this.height = value; } }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth { get { return dateOfBirth; } set { this.dateOfBirth = value; } }

        /// <summary>
        /// Место рождения
        /// </summary>
        public string PlaceOfBirth { get { return placeOfBirth; } set { this.placeOfBirth = value; } }

        #endregion

        #region Поля

        /// <summary>
        /// Поле "ID"
        /// </summary>
        private int iD;

        /// <summary>
        /// Поле "Дата и время добавления записи"
        /// </summary>
        private DateTime dateAndTime;

        /// <summary>
        /// Поле "Фамилия Имя Отчество"
        /// </summary>
        private string name;

        /// <summary>
        /// Поле "Возраст"
        /// </summary>
        private int age;

        /// <summary>
        /// Поле "Рост"
        /// </summary>
        private int height;

        /// <summary>
        /// Поле "Дата рождения"
        /// </summary>
        private DateTime dateOfBirth;

        /// <summary>
        /// Поле "Место рождения"
        /// </summary>
        private string placeOfBirth;

        /// <summary>
        /// Формат дат (d - дд.мм.гггг).
        /// </summary>
        readonly public static string formatDate = "d";
        /// <summary>
        /// Формат дат со временем (g - дд.мм.гггг чч:мм).
        /// </summary>
        readonly public static string formatDateTime = "g";
        /// <summary>
        /// Язык и региональные параметры.
        /// </summary>
        readonly public static string formatProvider = "ru-RU";

        #endregion

        #region Методы

        public string Print()
        {
            return  $"{this.iD,3}" +
                    $"{this.dateAndTime.ToString(formatDateTime),19}" +
                    $"{this.name,32}" +
                    $"{this.age,8}" +
                    $"{this.height,5}" +
                    $"{this.dateOfBirth.ToString(formatDate),14}" +
                    $"{this.placeOfBirth,20}";
        }

        #endregion



    }
}
