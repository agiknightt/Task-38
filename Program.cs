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
                        Car car = new Car(details[detailNumber]);
                        car.ShowBreaking();

                        if (storage.SearchDetail(car.Broken) != null)
                        {
                            carservice.ReplaceCorrectDetail(details[detailNumber]);
                            details[detailNumber].ShowTotalCost();
                            storage.DeleteDetail(car.Broken);
                        }
                        else
                        {                           
                            Detail uncorrectDetail = storage.SearchDetail(carservice.SelectDetail(details));
                            uncorrectDetail.ShowTotalCost();
                            carservice.ReplaceUncorrectDetail(uncorrectDetail);
                            storage.DeleteDetail(uncorrectDetail);
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

        public void ReplaceUncorrectDetail(Detail detail)
        {
            if(detail != null)
            {
                _balans -= detail.TotalCost;
                Console.WriteLine($"Вы заплатили ущерб в размере полной стоимости {detail.TotalCost}");
            }
            else
            {
                _balans -= _forfeit;
                Console.WriteLine($"Вы заплатили штраф {_forfeit}");
            }
        }
        public void ReplaceCorrectDetail(Detail detail)
        {
            _balans += detail.TotalCost;
        }
        public Detail SelectDetail(Detail[] details)
        {
            Console.WriteLine("1 - заменить другой деталью\n\n2 - заплатить штраф\n");

            if(Convert.ToInt32(Console.ReadLine()) == 1)
            {
                Console.WriteLine("Выберите деталь на которую вы хотите заменить : ");

                for (int i = 0; i < details.Length; i++)
                {
                    Console.WriteLine($"{i} - {details[i].NameDetail}");
                }

                return details[Convert.ToInt32(Console.ReadLine())];
            }
            return null;            
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
        public Detail SearchDetail(Detail detail)
        {
            for (int i = 0; i < _details.Count; i++)
            {
                if (_details[i].NameDetail == detail.NameDetail)
                {
                    Console.WriteLine("На складе есть деталь под замену.\n");                    
                    return _details[i];
                }
            }
            Console.WriteLine("У вас нет этой детали на складе.\n");
            return null;
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
        public bool CheckDetail(Detail detail)
        {
            for (int i = 0; i < _details.Count; i++)
            {
                if(_details[i].NameDetail == detail.NameDetail)
                {
                    Console.WriteLine("На складе есть деталь под замену.\n");
                    return true;
                }
            }
            return false;
        }
        public void DeleteDetail(Detail detail)
        {
            for (int i = 0; i < _details.Count; i++)
            {
                if(_details[i].NameDetail == detail.NameDetail)
                {
                    _details.RemoveAt(i);
                }
            }
        }
    }
    
    class Car
    {
        private Detail _broken;
        public Detail Broken
        {
            get
            {
                return _broken;
            }
        }
        public Car(Detail breakingDetail)
        {
            _broken = breakingDetail;
        }        
        public void ShowBreaking()
        {
            Console.WriteLine($"У автомобиля нужно заменить {_broken.NameDetail}.");
        }
    }
    abstract class Detail
    {
        protected int Price;
        protected string Name;
        protected int PayForChange;
        private int _totalCost = 0;
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
        public Detail(string name, int price, int payForChange)
        {
            Name = name;
            Price = price;
            PayForChange = payForChange;
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
