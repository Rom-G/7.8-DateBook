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
            int iD, 
            DateTime dateAndTime, 
            string name, 
            int age, 
            int height, 
            DateTime dateOfBirth, 
            string placeOfBirth)
        {
            ID = iD;
            DateAndTime = dateAndTime;
            Name = name;
            Age = age;
            Height = height;
            DateOfBirth = dateOfBirth;
            PlaceOfBirth = placeOfBirth;
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
        public int ID { get; set; }

        /// <summary>
        /// Дата и время добавления записи
        /// </summary>
        public DateTime DateAndTime { get; set; }

        /// <summary>
        /// Фамилия Имя Отчество
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Рост
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public string PlaceOfBirth { get; set; }

        #endregion

        #region Поля

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
            return  $"{this.ID,3}" +
                    $"{this.DateAndTime.ToString(formatDateTime),19}" +
                    $"{this.Name,32}" +
                    $"{this.Age,8}" +
                    $"{this.Height,5}" +
                    $"{this.DateOfBirth.ToString(formatDate),14}" +
                    $"{this.PlaceOfBirth,20}";
        }

        #endregion



    }
}
