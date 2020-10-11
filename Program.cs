using System;
using System.Collections.Generic;

namespace Task_38
{
    class Program
    {
        static void Main(string[] args)
        {
            CarService carservice = new CarService();

            Random rand = new Random();
            bool isWork = true;
            Detail[] details = { new Wheel("колесо", 800, 500), new Chock("тормозная колотка", 1000, 800), new AirFilter("воздушный фильтр", 700, 450), new OilFilter("масляный фильтр", 650, 200), new Ball("шаровые", 2000, 800) };

            Console.WriteLine("Сколько деталей у вас на складе? : ");
            Storage storage = new Storage(Convert.ToInt32(Console.ReadLine()), details);

            Console.Clear();

            while (isWork)
            {
                carservice.ShowBalans();
                storage.ShowInfo();
                Console.WriteLine("1 - принять следующего клиента\n\n2 - закрыть автосервис и выйти из программы.\n");

                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        int detailNumber = rand.Next(0, details.Length);
                        Car car = new Car(details[detailNumber].NameDetail);
                        car.ShowBreaking();                        
                        carservice.ReplaceDetail(storage.SearchDetail(car.Breaking), details[detailNumber].TotalCost);
                        break;
                    case 2:
                        isWork = false;
                        break;
                }
                Console.WriteLine("Для продолжения нажмите любую клавишу...");
                Console.ReadKey();
                Console.Clear();
            }

        }
    }
    class CarService
    {
        private int _balans;
        private int _forfeit = 300;
        public void ShowBalans()
        {
            Console.SetCursorPosition(60, 0);
            Console.WriteLine($"Ваш баланс : {_balans}");
            Console.SetCursorPosition(0, 0);
        }
        
        public void ReplaceDetail(int numberChoiceUser, int totalCost)
        {
            switch (numberChoiceUser)
            {
                case 1:
                    _balans += totalCost;
                    break;
                case 2:
                    _balans -= totalCost;
                    Console.WriteLine("Вы заплатили ущерб в размере полной стоимости");
                    break;
                case 3:
                    _balans -= _forfeit;
                    Console.WriteLine($"Вы заплатили штраф {_forfeit}");
                    break;
            }
        }        
    }
    class Storage
    {
        private List<Detail> _details = new List<Detail>();        
        public Storage(int countDetail, Detail[] details)
        {
            Random rand = new Random();
            for (int i = 0; i < countDetail; i++)
            {
                _details.Add(details[rand.Next(0,details.Length)]);
            }
        }
        public int SearchDetail(string detailName)
        {
            Random rand = new Random();
            for (int i = 0; i < _details.Count; i++)
            {
                if(_details[i].NameDetail == detailName)
                {
                    Console.WriteLine("На складе есть деталь под замену.\n");
                    _details[i].ShowTotalCost();
                    _details.RemoveAt(i);
                    return 1;
                }
            }
            Console.WriteLine("У вас нет этой детали на складе.\n");
            int randomDetail = rand.Next(0, _details.Count);

            if (Convert.ToInt32(Console.ReadLine()) == 2)
            {
                Console.WriteLine($"Вы попробовали заменить {_details[randomDetail].NameDetail} клиент это заметил, теперь вам придется возместить ущерб в размере полной стоимости");
                _details.RemoveAt(randomDetail);
                return 2;
            }
            return 3;
        }        
        public void ShowInfo()
        {
            int wheel = 0;
            int airFilter = 0;
            int oilFilter = 0;
            int chock = 0;
            int ball = 0;

            for (int i = 0; i < _details.Count; i++)
            {
                switch (_details[i].NameDetail)
                {
                    case "колесо":
                        wheel += 1;
                        break;
                    case "тормозная колотка":
                        chock += 1;
                        break;
                    case "масляный фильтр":
                        oilFilter += 1;
                        break;
                    case "воздушный фильтр":
                        airFilter += 1;
                        break;
                    case "шаровые":
                        ball += 1;
                        break;
                }
            }
            Console.SetCursorPosition(0,15);
            Console.WriteLine($"У вас на складе осталось :\n\n{wheel} колес\n\n{chock} тормозных колодок\n\n{oilFilter} масляных фильтров\n\n{airFilter} воздушных фильтров\n\n{ball} шаровых");
            Console.SetCursorPosition(0,0);
        }
    }
    
    class Car
    {
        private string _breaking;
        public string Breaking
        {
            get
            {
                return _breaking;
            }
        }
        public Car(string breaking)
        {
            _breaking = breaking;
        }        
        public void ShowBreaking()
        {
            Console.WriteLine($"У автомобиля нужно заменить {_breaking}.");
        }
    }
    abstract class Detail
    {
        protected int Price;
        protected string Name;
        protected int PayForChange;
        private int _totalCost;
        public string NameDetail
        {
            get
            {
                return Name;
            }
        }
        public int TotalCost
        {
            get
            {
                return _totalCost;
            }
        }
        public Detail(string name, int price, int payForWork)
        {
            Name = name;
            Price = price;
            PayForChange = payForWork;
            _totalCost += Price + PayForChange;
        }
        public void ShowTotalCost()
        {
            Console.WriteLine($"Общая стоимость будет {_totalCost}");
        }
        
    }
    class Wheel : Detail
    {
        public Wheel(string name, int price, int payForWork) : base (name, price, payForWork)
        {
            Name = name;
            Price = price;
            PayForChange = payForWork;
        }
    }
    class AirFilter : Detail
    {
        public AirFilter(string name, int price, int payForWork) : base(name, price, payForWork)
        {
            Name = name;
            Price = price;
            PayForChange = payForWork;
        }
    }
    class OilFilter : Detail
    {
        public OilFilter(string name, int price, int payForWork) : base(name, price, payForWork)
        {
            Name = name;
            Price = price;
            PayForChange = payForWork;
        }
    }
    class Chock : Detail
    {
        public Chock(string name, int price, int payForWork) : base(name, price, payForWork)
        {
            Name = name;
            Price = price;
            PayForChange = payForWork;
        }
    }
    class Ball : Detail
    {
        public Ball(string name, int price, int payForWork) : base(name, price, payForWork)
        {
            Name = name;
            Price = price;
            PayForChange = payForWork;
        }
    }
}
