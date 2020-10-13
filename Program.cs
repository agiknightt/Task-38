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

                        if (storage.SearchDetail(car.Breaking) == true)
                        {
                            carservice.ReplaceCorrectDetail(details[detailNumber].TotalCost);
                        }
                        else
                        {
                            carservice.ReplaceIncorrectDetail(details[detailNumber].TotalCost, storage.SearchDetail(carservice.SelectDetail(details)));
                        }
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

        public void ReplaceIncorrectDetail(int totalCost, bool isFoundDetail)
        {
            if(isFoundDetail)
            {
                _balans -= totalCost;
                Console.WriteLine($"Вы заплатили ущерб в размере полной стоимости {totalCost}");
            }
            else
            {
                _balans -= _forfeit;
                Console.WriteLine($"Вы заплатили штраф {_forfeit}");
            }
        }
        public void ReplaceCorrectDetail(int totalCost)
        {
            _balans += totalCost;
        }
        public string SelectDetail(Detail[] details)
        {
            Console.WriteLine("1 - заменить другой деталью\n\n2 - заплатить штраф\n");

            if(Convert.ToInt32(Console.ReadLine()) == 1)
            {
                Console.WriteLine("Выберите деталь на которую вы хотите заменить : ");

                for (int i = 0; i < details.Length; i++)
                {
                    Console.WriteLine($"{i} - {details[i].NameDetail}");
                }                
                return details[Convert.ToInt32(Console.ReadLine())].NameDetail;
            }
            return "";            
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
        public bool SearchDetail(string detailName)
        {
            for (int i = 0; i < _details.Count; i++)
            {
                if (_details[i].NameDetail == detailName)
                {
                    Console.WriteLine("На складе есть деталь под замену.\n");
                    _details[i].ShowTotalCost();
                    _details.RemoveAt(i);
                    return true;
                }
            }
            Console.WriteLine("У вас нет этой детали на складе.\n");
            return false;
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
            Console.SetCursorPosition(0,50);
            Console.WriteLine($"У вас на складе осталось : {wheel} колес, {chock} тормозных колодок, {oilFilter} масляных фильтров, {airFilter} воздушных фильтров, {ball} шаровых");
            Console.SetCursorPosition(0,0);
        }
        public bool CheckDetail(int numberDetail, Detail[] details)
        {
            for (int i = 0; i < _details.Count; i++)
            {
                if(_details[i].NameDetail == details[numberDetail].NameDetail)
                {
                    _details.RemoveAt(i);
                    Console.WriteLine("На складе есть деталь под замену.\n");
                    return true;
                }
            }
            return false;
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
