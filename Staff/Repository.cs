using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Staff
{
    /// <summary>
    /// Класс для работы с экземплярами Worker
    /// </summary>
    class Repository
    {
        /// <summary>
        /// Массив для работы с данными.
        /// </summary>
        private Worker[] workers;
        /// <summary>
        /// Путь к файлу с данными.
        /// </summary>
        readonly private string path;
        /// <summary>
        /// Индекс текущей записи в базовом массиве данных.
        /// </summary>
        int index;
        /// <summary>
        /// Следующий ID для нового сотрудника.
        /// </summary>
        public int nextID;
        /// <summary>
        /// Массив заголовков столбцов.
        /// </summary>
        public string[] headers;

        /// <summary>
        /// Конструктор экземпляров Repository.
        /// </summary>
        /// <param name="Path">Путь к файлу с данными</param>
        public Repository(string Path)
        {
            this.path = Path;
            this.index = 0;
            this.nextID = 1;
            this.headers = new string[0];
            this.workers = new Worker[1];
            this.Load();
        }

        /// <summary>
        /// Метод загрузки данных из файла.
        /// </summary>
        public void Load()
        {
            if (!File.Exists(this.path))
            {
                CreateFile();
            }

            using (StreamReader sr = new StreamReader(this.path))
            {
                headers = sr.ReadLine().Split('#');

                while (!sr.EndOfStream)
                {
                    string[] args = sr.ReadLine().Split('#');
                    var cultureInfo = new CultureInfo(Worker.formatProvider);

                    Add(new Worker(int.Parse(args[0]), 
                                   DateTime.ParseExact(args[1], Worker.formatDateTime, cultureInfo), 
                                   args[2], 
                                   int.Parse(args[3]), 
                                   int.Parse(args[4]),
                                   DateTime.ParseExact(args[5], Worker.formatDate, cultureInfo), 
                                   args[6]
                                   )
                        );
                }
            }
        }

        /// <summary>
        /// Метод создания пустого файла, в случае его отсутствия.
        /// </summary>
        public void CreateFile()
        {
            using (StreamWriter sw = new StreamWriter(this.path, false, Encoding.Unicode))
            {
                sw.WriteLine("ID#" +
                             "Дата и время#" +
                             "Фамилия Имя Отчество#" +
                             "Возраст#" +
                             "Рост#" +
                             "Дата рождения#" +
                             "Место рождения");
            }
        }

        /// <summary>
        /// Метод, добавляющий сотрудника в массив.
        /// </summary>
        /// <param name="AddWorker">Данные (параметры) сотрудника</param>
        public void Add(Worker AddWorker)
        {
            this.Increase(index >= this.workers.Length);
            this.workers[index] = AddWorker;
            this.index++;
            for (int i = 0; i < index; i++)
            {
                if (this.workers[i].ID >= this.nextID)
                {
                    this.nextID = this.workers[i].ID + 1;
                }
            }
        }

        /// <summary>
        /// Метод, изменяющий данные сотрудника в массиве.
        /// </summary>
        /// <param name="newData">Новые данные (параметры) сотрудника.</param>
        public void Change(Worker newData)
        {
            int i = 0;
            for (int j = 0; j < index; j++)
            {
                if (workers[j].ID == newData.ID)
                {
                    i = j;
                    break;
                }
            }
            this.workers[i] = newData;
            Save();
        }

        /// <summary>
        /// Метод увеличения массива для работы с данными.
        /// </summary>
        /// <param name="Flag">Условие увеличения</param>
        public void Increase(bool Flag)
        {
            if (Flag)
            {
                Array.Resize(ref this.workers, this.workers.Length * 2);
            }
        }

        /// <summary>
        /// Метод сохранения данных в файл.
        /// </summary>
        /// <param name="Path">Путь к файлу с данными</param>
        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.Unicode))
            {
                string line = String.Format("{0}#{1}#{2}#{3}#{4}#{5}#{6}",
                                            this.headers[0],
                                            this.headers[1],
                                            this.headers[2],
                                            this.headers[3],
                                            this.headers[4],
                                            this.headers[5],
                                            this.headers[6]);
                sw.WriteLine(line);

                for (int i = 0; i < index; i++)
                {
                    line = String.Format("{0}#{1}#{2}#{3}#{4}#{5}#{6}",
                                            this.workers[i].ID,
                                            this.workers[i].DateAndTime.ToString(Worker.formatDateTime),
                                            this.workers[i].Name,
                                            this.workers[i].Age,
                                            this.workers[i].Height,
                                            this.workers[i].DateOfBirth.ToString(Worker.formatDate),
                                            this.workers[i].PlaceOfBirth);
                    sw.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Метод, возвращающий данные всех сотрудников.
        /// </summary>
        /// <returns>Массив, содержащий всех сотрудников.</returns>
        public Worker[] GetAllWorkers()
        {
            return this.workers;
        }

        /// <summary>
        /// Метод, возвращяющий данные сотрудника по ID.
        /// </summary>
        /// <param name="id">ID сотрудника</param>
        public Worker GetWorkerById(int id)
        {
            int i;
            for (i = 0; i <= index; i++)
            {
                if (this.workers[i].ID == id)
                {
                    return this.workers[i];
                }
            }
            return new Worker("Неизвестный сотрудник");
        }

        /// <summary>
        /// Метод, удаляющий сотрудника по ID.
        /// </summary>
        /// <param name="id">ID сотрудника</param>
        public void DeleteWorker(Worker deletWorker)
        {
            var selectedWorker = from worker in this.workers
                                 where worker.ID != deletWorker.ID
                                 select worker;
            this.index--;
            this.workers = selectedWorker.ToArray();
            Save();
        }

        /// <summary>
        /// Метод выборки записей за указанный период.
        /// </summary>
        /// <param name="dateFrom">Дата начала выборки.</param>
        /// <param name="dateTo">Дата окончания выборки.</param>
        /// <returns>Массив сотрудников, записанных в указанный период.</returns>
        public Worker[] GetWorkersBetweenTwoDates(DateTime dateFrom, DateTime dateTo)
        {
            var selectedWorker = from worker in workers
                                 where worker.DateAndTime >= dateFrom && worker.DateAndTime <= dateTo.AddDays(1)
                                 orderby(worker.DateAndTime)
                                 select worker;
            return selectedWorker.ToArray();
        }

        /// <summary>
        /// Метод для генерации запрашиваемого количества записей.
        /// </summary>
        /// <param name="N">Количество записей, которое необходимо создать.</param>
        public void GeneratorOfWorkers(int N)
        {
            for (int i = 0; i < N; i++)
            {
                string name = string.Concat("Имя", Convert.ToString(this.nextID));
                Worker worker = new Worker(this.nextID, name);
                Add(worker);
            }
            Save();
        }
    }
}
