using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff
{
    internal class Program
    {
        /// <summary>
        /// Метод, взаимодействующий с пользователем.
        /// </summary>
        static void Dialogue(Repository rep)
        {
            List();
            char key = 'д';
            do
            {
                Console.Write("Укажите требуемое действие (пункт): ");
                int choise = rep.TryConvertInt(Console.ReadLine());
                switch (choise)
                {
                    case 1:
                        rep.PrintWorkers(rep.GetAllWorkers());
                        break;
                    case 2:
                        Console.Write("Введите ID сотрудника для просмотра: ");
                        rep.PrintWorkers(
                            rep.GetWorkerById(
                                rep.TryConvertInt(
                                    Console.ReadLine()
                                )
                            )
                        );
                        break;
                    case 3:
                        rep.CreatingWorker();
                        break;
                    case 4:
                        Console.Write("Введите ID сотрудника для удаления: ");
                        string id = Console.ReadLine();
                        rep.DeleteWorker(
                            rep.TryConvertInt(id)
                        );
                        break;
                    case 5:
                        Console.Write("Для загрузки диапазона записей укажите начальную дату (дд.мм.гггг): ");
                        DateTime dateFrom = rep.TryConvertDate(Console.ReadLine());
                        Console.Write("И конечную (дд.мм.гггг): ");
                        DateTime dateTo = rep.TryConvertDate(Console.ReadLine());
                        Worker[] workers = rep.GetWorkersBetweenTwoDates(dateFrom, dateTo);
                        rep.PrintWorkers(workers);
                        break;
                    case 6:
                        Console.Write("Введите ID для редактирования записи: ");
                        id = Console.ReadLine();
                        rep.EditWorkerById(
                            rep.TryConvertInt(id)
                        );
                        break;
                    case 7:
                        Console.Write("Укажите сколько записей необходимо сгенерировать: ");
                        string N = Console.ReadLine();
                        rep.GeneratorOfWorkers(
                            rep.TryConvertInt(N)
                        );
                        Console.WriteLine($"Записи сгенерированы.");
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
                        int field = rep.TryConvertInt(Console.ReadLine());
                        if (field >= 0 && field <= 6)
                        {
                            OrderWorkers(field, rep);
                        }
                        else
                        {
                            Console.WriteLine($"Указанное поле не существует.");
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
                        Console.WriteLine("Указанное действие не предусмотрено");
                        break;
                }
                Console.WriteLine("\t9 - отобразить список действий.");
            } while (char.ToLower(key) == 'д' || char.ToLower(key) == 'l');
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
        static void OrderWorkers(int sortColumn, Repository rep)
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
            rep.PrintWorkers(sortedWorkers.ToArray());
        }

        static void Main(string[] args)
        {
            string path = @"staff.csv";
            Repository rep = new Repository(path);
            Console.WriteLine("Ежедневник \"Cотрудники\".");
            Dialogue(rep);
        }
    }
}
