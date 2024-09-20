using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        int nextID;
        /// <summary>
        /// Массив заголовков столбцов.
        /// </summary>
        string[] headers;

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
        /// Метод добавления сотрудника в массив.
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
        public void Save(string Path)
        {
            using (StreamWriter sw = new StreamWriter(Path, false, Encoding.Unicode))
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
        /// Метод, печатающий заголовки.
        /// </summary>
        public void PrintHeaders()
        {
            Console.WriteLine($"{this.headers[0],3}" +
                              $"{this.headers[1],19}" +
                              $"{this.headers[2],32}" +
                              $"{this.headers[3],8}" +
                              $"{this.headers[4],5}" +
                              $"{this.headers[5],14}" +
                              $"{this.headers[6],20}");
        }

        /// <summary>
        /// Метод, печатающий выбранных сотрудников.
        /// </summary>
        /// <param name="workers">Массив сотрудников запрошенных пользователем через другие методы.</param>
        public void PrintWorkers(Worker[] workers)
        {
            PrintHeaders();
            for (int i = 0; i < workers.Length; i++)
            {
                if (workers[i].ID == 0)
                {
                    break;
                }
                Console.WriteLine(workers[i].Print());
            }
        }

        /// <summary>
        /// Метод, печатающий одного выбранного сотрудника.
        /// </summary>
        /// <param name="worker">Сотрудник, запрошенный пользователем через другие методы.</param>
        public void PrintWorkers(Worker worker)
        {
            if (worker.ID == 0)
            {
                return;
            }
            PrintHeaders();
            Console.WriteLine(worker.Print());
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
            Console.WriteLine($"Сотрудник с ID {id} не найден.");
            return new Worker("Неизвестный сотрудник");
        }

        /// <summary>
        /// Метод создания новой записи (сотрудника).
        /// </summary>
        public void CreatingWorker()
        {
            char key = 'д';
            do
            {
                Console.WriteLine($"ID сотрудника - {nextID}");
                int id = nextID;

                DateTime dateAndTime = DateTime.Now;
                Console.WriteLine($"Дата и время записи - {dateAndTime.ToString(Worker.formatDateTime)}");

                Console.Write("Введите Ф.И.О. сотрудника: ");
                string name = Console.ReadLine();

                Console.Write("Введите рост (см.): ");
                string stringHeight = Console.ReadLine();
                int height = TryConvertInt(stringHeight);

                Console.Write("Укажите дату рождения в формате дд.мм.гггг: ");
                string stringDate = Console.ReadLine();
                DateTime dateOfBirth = TryConvertDate(stringDate);

                int age = DateTime.Now.Year - dateOfBirth.Year;
                if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                {
                    age--;
                }
                Console.WriteLine($"Возраст - {age}");

                Console.Write("Укажите место рождения: ");
                string placeOfBirth = Console.ReadLine();

                Add(new Worker(id, dateAndTime, name, age, height, dateOfBirth, placeOfBirth));

                Console.Write("Добавить ещё одного сотрудника? (н/д)\n\n");
                key = Console.ReadKey(true).KeyChar;
            } while (char.ToLower(key) == 'д');

            Save(this.path);
        }

        /// <summary>
        /// Метод, удаляющий сотрудника по ID.
        /// </summary>
        /// <param name="id">ID сотрудника</param>
        public void DeleteWorker(int id)
        {
            Worker deletedWorker = GetWorkerById(id);
            if (deletedWorker.ID == 0)
            {
                return;
            }
            
            var selectedWorker = from worker in this.workers
                                 where worker.ID != id
                                 select worker;
            this.index--;
            this.workers = selectedWorker.ToArray();

            Save(this.path);
            Console.WriteLine($"Сотрудник ID {id} удалён.");
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
        /// Метод, редактирующий данные сотрудника по ID.
        /// </summary>
        /// <param name="id">ID сотрудника</param>
        public void EditWorkerById(int id)
        {
            Worker editableWorker = new Worker("Сотрудник для редактирования");
            int i = 0;
            for (int j = 0; j < index; j++)
            {
                if (this.workers[j].ID == id)
                {
                    editableWorker = this.workers[j];
                    i = j;
                }
            }
            if (editableWorker.ID != id || id == 0)
            {
                Console.WriteLine($"Сотрудник с ID {id} не найден.");
            }
            else
            {
                Console.WriteLine($"Текущие данные сотрудника с ID {editableWorker.ID}:");
                PrintWorkers(editableWorker);
                Console.WriteLine("\nЗаполните поля новыми данными.");

                Console.Write("Введите Ф.И.О. сотрудника: ");
                editableWorker.Name = Console.ReadLine();

                Console.Write("Введите рост (см.): ");
                string stringHeight = Console.ReadLine();
                editableWorker.Height = TryConvertInt(stringHeight);

                Console.Write("Укажите дату рождения в формате дд.мм.гггг: ");
                string stringDate = Console.ReadLine();
                editableWorker.DateOfBirth = TryConvertDate(stringDate);

                int age = DateTime.Now.Year - editableWorker.DateOfBirth.Year;
                if (DateTime.Now.DayOfYear < editableWorker.DateOfBirth.DayOfYear)
                {
                    age--;
                }
                editableWorker.Age = age;
                Console.WriteLine($"Возраст - {age}");

                Console.Write("Укажите место рождения: ");
                editableWorker.PlaceOfBirth = Console.ReadLine();

                this.workers[i] = editableWorker;
                Save(this.path);
                Console.WriteLine("Данные сохранены");
            }
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
            Save(this.path);
        }

        /// <summary>
        /// Метод запроса и проверки целочисленных значений (варианты ответов, рост).
        /// </summary>
        /// <param name="stringInt">Введёное число в текстовом формате.</param>
        /// <returns>Указанное пользователем и проверенное на корректность число.</returns>
        public int TryConvertInt(string stringInt)
        {
            if (int.TryParse(stringInt, out int integer))
            {
                return integer;
            }
            else
            {
                Console.Write("\nВведено неверное значение. Попробуте снова: ");
                stringInt = Console.ReadLine();
                return TryConvertInt(stringInt);
            }
        }

        /// <summary>
        /// Метод запроса и проверки даты.
        /// </summary>
        /// <param name="formatDate">Сведения о форматировании.</param>
        /// <param name="formatProvider">Язык и региональные параметры.</param>
        /// <returns>Указанная пользователем и проверенная на корректность дата.</returns>
        public DateTime TryConvertDate(string stringDate)
        {
            var cultureInfo = new CultureInfo(Worker.formatProvider);

            if (DateTime.TryParseExact(stringDate, Worker.formatDate, cultureInfo, DateTimeStyles.AllowWhiteSpaces, out DateTime dateOfBirth))
            {
                return dateOfBirth;
            }
            else
            {
                Console.Write("Указано неверное значение. Поробуйте снова: ");
                stringDate = Console.ReadLine();
                return TryConvertDate(stringDate);
            }
        }
    }
}
