using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Staff
{
    internal class Program
    {
        /// <summary>
        /// Метод, взаимодействующий с пользователем.
        /// </summary>
        /// <param name="rep">Имя текущего экземпляра класса Repository.</param> 
        static void Dialogue(Repository rep)
        {
            List();
            char key = 'д';
            do
            {
                Console.Write("Укажите требуемое действие (пункт): ");
                int choise = TryConvertInt(Console.ReadLine());
                switch (choise)
                {
                    case 1:
                        PrintWorkers(rep, rep.GetAllWorkers());
                        break;
                    case 2:
                        Console.Write("Введите ID сотрудника для просмотра: ");
                        int idForViewing = TryConvertInt(Console.ReadLine());
                        Worker desiredWorker = rep.GetWorkerById(idForViewing);
                        if (desiredWorker.ID != 0)
                        {
                            PrintWorkers(rep, desiredWorker);
                        }
                        else
                        {
                            Console.WriteLine($"Сотрудник с ID {idForViewing} не найден.");
                        }
                        break;
                    case 3:
                        char keyForCreating = 'д';
                        do
                        {
                            CreatingWorker(rep);
                            Console.WriteLine("Добавить ещё одного сотрудника? (н/д)");
                            keyForCreating = Console.ReadKey(true).KeyChar;
                        }
                        while (char.ToLower(keyForCreating) == 'д' || char.ToLower(keyForCreating) == 'l');
                        break;
                    case 4:
                        Console.Write("Введите ID сотрудника для удаления: ");
                        int id = TryConvertInt(Console.ReadLine());
                        Worker deletWorker = rep.GetWorkerById(id);
                        if (deletWorker.ID != 0)
                        {
                            rep.DeleteWorker(deletWorker);
                            Console.WriteLine($"Сотрудник с ID {id} удалён.");
                        }
                        else
                        {
                            Console.WriteLine($"Сотрудник с ID {id} не существует.");
                        }
                        break;
                    case 5:
                        Console.Write("Для загрузки диапазона записей укажите начальную дату (дд.мм.гггг): ");
                        DateTime dateFrom = TryConvertDate(Console.ReadLine());
                        Console.Write("И конечную (дд.мм.гггг): ");
                        DateTime dateTo = TryConvertDate(Console.ReadLine());
                        Worker[] workers = rep.GetWorkersBetweenTwoDates(dateFrom, dateTo);
                        PrintWorkers(rep, workers);
                        break;
                    case 6:
                        Console.Write("Введите ID для редактирования записи: ");
                        EditWorkerById(rep,
                            TryConvertInt(Console.ReadLine())
                        );
                        break;
                    case 7:
                        Console.Write("Укажите сколько записей необходимо добавить: ");
                        string N = Console.ReadLine();
                        rep.GeneratorOfWorkers(
                            TryConvertInt(N)
                        );
                        Console.WriteLine($"Записи добавлены.");
                        break;
                    case 8:
                        Console.WriteLine("Ежедневник можно отсортировать по следующим полям:\n" +
                                  "\t0 - ID сотрудника,\n" +
                                  "\t1 - дата и время добавления записи,\n" +
                                  "\t2 - Ф.И.О.,\n" +
                                  "\t3 - возраст,\n" +
                                  "\t4 - рост,\n" +
                                  "\t5 - дата рождения,\n" +
                                  "\t6 - место рождения.\n");
                        Console.Write("По какому полю выполнить сортировку: ");
                        int field = TryConvertInt(Console.ReadLine());
                        if (field >= 0 && field <= 6)
                        {
                            OrderWorkers(rep, field);
                        }
                        else
                        {
                            Console.WriteLine($"Указанное поле не существует.\n");
                            goto case 8;
                        }
                        break;
                    case 9:
                        List();
                        break;
                    case 0:
                        key = 'н';
                        break;
                    default:
                        Console.WriteLine("Указанное действие не предусмотрено.\n");
                        break;
                }
                Console.WriteLine("\n\t9 - отобразить список действий.\n");
            } while (char.ToLower(key) == 'д' || char.ToLower(key) == 'l');
        }

        /// <summary>
        /// Метод создания новой записи (сотрудника).
        /// </summary>
        /// <param name="rep">Имя текущего экземпляра класса Repository.</param>
        static void CreatingWorker(Repository rep)
        {
            Worker newWorker = Questionnaire();

            newWorker.ID = rep.nextID;
            Console.WriteLine($"ID сотрудника - {rep.nextID}");

            newWorker.DateAndTime = DateTime.Now;
            Console.WriteLine($"Дата и время записи - {newWorker.DateAndTime.ToString(Worker.formatDateTime)}");

            rep.Add(newWorker);
            rep.Save();    
        }

        /// <summary>
        /// Метод, редактирующий данные сотрудника по ID.
        /// </summary>
        /// <param name="rep">Имя текущего экземпляра класса Repository.</param>
        /// <param name="id">ID сотрудника</param>
        static void EditWorkerById(Repository rep, int id)
        {
            Worker editableWorker = rep.GetWorkerById(id);
            
            if (editableWorker.ID != id || id == 0)
            {
                Console.WriteLine($"Сотрудник с ID {id} не найден.");
            }
            else
            {
                Console.WriteLine($"\nТекущие данные сотрудника с ID {editableWorker.ID}:");
                PrintWorkers(rep, editableWorker);

                Console.WriteLine("\nЗаполните поля новыми данными.");
                Worker newData = Questionnaire();
                newData.ID = id;
                newData.DateAndTime = editableWorker.DateAndTime;

                rep.Change(newData);
                Console.WriteLine("Данные сохранены.");
            }
        }

        /// <summary>
        /// Метод для заполнения сведений о сотруднике.
        /// </summary>
        static Worker Questionnaire()
        {
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

            return new Worker(0, DateTime.Now, name, age, height, dateOfBirth, placeOfBirth);
        }

        /// <summary>
        /// Метод, печатающий список действий с ежедневником.
        /// </summary>
        static void List()
        {
            Console.WriteLine("\t1 - просмотр всех записей,\n" +
                              "\t2 - просмотр одной записи,\n" +
                              "\t3 - создание записи,\n" +
                              "\t4 - удаление записи,\n" +
                              "\t5 - загрузка записей в выбранном диапазоне дат,\n" +
                              "\t6 - редактирование записей,\n" +
                              "\t7 - генерация запрошенного количества записей,\n" +
                              "\t8 - сортировка записей,\n" +
                              "\t0 - завершить работу.");
        }

        /// <summary>
        /// Метод сортировки данных по различным полям.
        /// </summary>
        /// <param name="sortColumn">Индекс поля (столбца) для сортировки.</param>
        static void OrderWorkers(Repository rep, int sortColumn)
        {
            var workers = new List<Worker>(rep.GetAllWorkers());
            IEnumerable<Worker> sortedWorkers = Enumerable.Empty<Worker>();
            switch (sortColumn)
            {
                case 0:
                    sortedWorkers = workers.OrderBy(w => w.ID);
                    break;
                case 1:
                    sortedWorkers = workers.OrderBy(w => w.DateAndTime);
                    break;
                case 2:
                    sortedWorkers = workers.OrderBy(w => w.Name);
                    break;
                case 3:
                    sortedWorkers = workers.OrderBy(w => w.Age);
                    break;
                case 4:
                    sortedWorkers = workers.OrderBy(w => w.Height);
                    break;
                case 5:
                    sortedWorkers = workers.OrderBy(w => w.DateOfBirth);
                    break;
                case 6:
                    sortedWorkers = workers.OrderBy(w => w.PlaceOfBirth);
                    break;
            }
            PrintWorkers(rep, sortedWorkers.ToArray());
        }

        /// <summary>
        /// Метод, печатающий заголовки.
        /// </summary>
        /// <param name="rep">Имя текущего экземпляра класса Repository.</param>
        static void PrintHeaders(Repository rep)
        {
            Console.WriteLine($"{rep.headers[0],3}" +
                              $"{rep.headers[1],19}" +
                              $"{rep.headers[2],32}" +
                              $"{rep.headers[3],8}" +
                              $"{rep.headers[4],5}" +
                              $"{rep.headers[5],14}" +
                              $"{rep.headers[6],20}");
        }

        /// <summary>
        /// Метод, печатающий выбранных сотрудников.
        /// </summary>
        /// <param name="rep">Имя текущего экземпляра класса Repository.</param>
        /// <param name="workers">Массив сотрудников запрошенных пользователем через другие методы.</param>
        static void PrintWorkers(Repository rep, Worker[] workers)
        {
            PrintHeaders(rep);
            for (int i = 0; i < workers.Length; i++)
            {
                if (workers[i].ID == 0)
                {
                    continue;
                }
                Console.WriteLine(workers[i].Print());
            }
        }

        /// <summary>
        /// Метод, печатающий одного выбранного сотрудника.
        /// </summary>
        /// <param name="rep">Имя текущего экземпляра класса Repository.</param>
        /// <param name="worker">Сотрудник, запрошенный пользователем через другие методы.</param>
        static void PrintWorkers(Repository rep, Worker worker)
        {
            if (worker.ID == 0)
            {
                return;
            }
            PrintHeaders(rep);
            Console.WriteLine(worker.Print());
        }

        /// <summary>
        /// Метод запроса и проверки целочисленных значений (варианты ответов, рост).
        /// </summary>
        /// <param name="stringInt">Введёное число в строковом формате.</param>
        /// <returns>Указанное пользователем и проверенное на корректность число.</returns>
        static int TryConvertInt(string stringInt)
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
        /// <param name="stringDate">Дата в строковом формате.</param>
        /// <returns>Указанная пользователем и проверенная на корректность дата.</returns>
        static DateTime TryConvertDate(string stringDate)
        {
            var cultureInfo = new CultureInfo(Worker.formatProvider);

            if (DateTime.TryParseExact(stringDate, Worker.formatDate, cultureInfo, DateTimeStyles.AllowWhiteSpaces, out DateTime date))
            {
                return date;
            }
            else
            {
                Console.Write("\nУказано неверное значение. Поробуйте снова: ");
                stringDate = Console.ReadLine();
                return TryConvertDate(stringDate);
            }
        }

        static void Main(string[] args)
        {
            string path = @"staff.csv";
            Repository rep = new Repository(path);
            Console.WriteLine("Ежедневник \"Cотрудники\".\n");
            Dialogue(rep);
        }
    }
}
